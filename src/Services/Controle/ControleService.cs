using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Svk.Domain;
using Svk.Domain.Exceptions;
using Svk.Domain.Files;
using Svk.Persistence;
using Svk.Shared.Controles;
using Svk.Shared.Files;
using Svk.Shared.Misc;

namespace Svk.Services.Controle;

public class ControleService : IControleService
{
    private readonly string BUCKET_PREFIX = "svk";
    private readonly SvkDbContext dbContext;
    private readonly ILogger<ControleService> logger;
    private readonly IFileService pictureService;

    public ControleService(SvkDbContext dbContext, IFileService pictureService, ILogger<ControleService> logger)
    {
        this.dbContext = dbContext;
        this.pictureService = pictureService;
        this.logger = logger;
    }

    public async Task<ControleResult.Create> CreateAsync(ControleDto.Create model)
    {
        List<string> signedUrls = new List<string>();
        List<Image> images = new();

        //TODO fix possible null reference exception

        if (model.Routenummers is null || model.Laadbonnummers is null || model.Transporteur is null ||
            model.Nummerplaat is null || model.Datum is null)
        {
            logger.LogError(
                "One of the required fields is null in the ControleDto.Create wich should not be possible because of the validator");
            throw new ArgumentNullException();
        }


        var routeNumbers = model.Routenummers.Select(routenummer => new Routenumber(routenummer));
        var loadingSlips = model.Laadbonnummers.Select(slip => new LoadingSlip(
            slip.LaadBonnummer,
            new Address(
                street: slip.Adres.Street,
                huisNr: slip.Adres.HouseNr,
                postCode: slip.Adres.Postcode,
                countryIso: slip.Adres.CountryIsoCode
            )
        ));

        var transporter = new Transporter(model.Transporteur);
        var loader = dbContext.Loaders.Select(x => x).Where(x => x.Auth0Id == model.auth0id).First();


        TransportControl controle = new(
            routeNumbers,
            loadingSlips,
            transporter,
            model.Nummerplaat,
            images,
            loader,
            model.Datum!.Value
        );


        dbContext.TransportControls.Add(controle);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Controle created with id: {id}", controle.Id);

        var bucket = BUCKET_PREFIX + controle.Id;

        await pictureService.MakeBucketAsync(bucket);
        for (int i = 0; i < model.Fotos; i++)
        {
            var guid = Guid.NewGuid().ToString();
            controle.AddImage(new Image(bucket: bucket, fileName: guid));

            var signedUrl =
                await pictureService.GetSignedUploadUrlAsync(bucket, guid);
            signedUrls.Add(signedUrl);
        }

        await dbContext.SaveChangesAsync();


        return new ControleResult.Create()
        {
            Id = controle.Id,
            SignedUrls = signedUrls
        };
    }

    public async Task<ControleResult.Edit> EditAsync(int controleId, ControleDto.Edit model)
    {
        var existingControle = await dbContext.TransportControls.FindAsync(controleId);

        if (existingControle is null)
        {
            logger.LogWarning("Controle with id: {id} not found", controleId);
            throw new EntityNotFoundException(nameof(TransportControl), controleId);
        }

        var signedUrls = new List<string>();
        var bucket = BUCKET_PREFIX + controleId;

        for (int i = 0; i < model.Fotos; i++)
        {
            var guid = Guid.NewGuid().ToString();
            existingControle.AddImage(new Image(bucket: bucket, fileName: guid));

            var signedUrl =
                await pictureService.GetSignedUploadUrlAsync(bucket, guid);
            signedUrls.Add(signedUrl);
        }

        // Save the changes to the database
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Controle edited with id: {id}", controleId);

        return new ControleResult.Edit
        {
            Id = controleId,
            SignedUrls = signedUrls
        };
    }

    public async Task<ControleDto.Detail> GetDetailAsync(int id)
    {
        TransportControl? dbresult =
            await dbContext.TransportControls.Select(x => x)
                .Include(x => x.Transporter)
                .Include(x => x.Loader)
                .Include(x => x.Images)
                .Include(x => x.LoadingSlips)
                .Include(x => x.RouteNumbers)
                .FirstOrDefaultAsync(x => x.Id == id);

        if (dbresult is null)
        {
            logger.LogWarning("Controle with id: {id} not found", id);
            throw new EntityNotFoundException(nameof(TransportControl), id);
        }

        List<string> signedUrls = new List<string>();

        foreach (var img in dbresult.Images)
        {
            var url = await pictureService.GetSignedDownloadUrlAsync(img.Bucket, img.FilePath);
            signedUrls.Add(url);
        }


        var result = new ControleDto.Detail()
        {
            Datum = dbresult.LoadingDate,
            Transporteur = dbresult.Transporter.Name,
            Nummerplaat = dbresult.LicensePlate,
            Routenummers = dbresult.RouteNumbers.Select(x => x.Number.ToString()),
            Laadbonnummers = dbresult.LoadingSlips.Select(slip => slip.LaadbonNr),
            Fotos = signedUrls,
            Id = dbresult.Id,
            CreatedAt = dbresult.CreatedAt,
            UpdatedAt = dbresult.UpdatedAt,
        };

        //TODO throw exception if result is null
        return result!;
    }

    public async Task<Result.GetItemsPaginated<ControleDto.Index>> GetIndexAsync(ControleRequest.Index req)
    {
        var query = dbContext.TransportControls.AsQueryable();

        if (req.Laadbonnummer is not null)
        {
            var laadbonString = req.Laadbonnummer.Value.ToString();
            query = query.Where(control =>
                control.LoadingSlips.Any(slip => slip.LaadbonNr.ToString().Contains(laadbonString)));
        }

        if (req.Lader is not null)
        {
            query = query.Where(control =>
                control.Loader.Name.Contains(req.Lader));
        }

        if (req.Routenummer is not null)
        {
            var routenummerString = req.Routenummer.Value.ToString();
            query = query.Where(control =>
                control.RouteNumbers.Any(routenumber => routenumber.Number.Contains(routenummerString)));
        }

        if (req.Transporteur is not null)
        {
            query = query.Where(control =>
                control.Transporter.Name.Contains(req.Transporteur));
        }

        if (req.Nummerplaat is not null)
        {
            query = query.Where(control =>
                control.LicensePlate.Contains(req.Nummerplaat));
        }

        if (req.StartDateRange is not null)
        {
            //Compare datetime with dateonly
            query = query.Where(control => control.LoadingDate.Date >= req.StartDateRange.Value.Date);
        }

        if (req.EndDateRange is not null)
        {
            //Compare datetime with dateonly
            query = query.Where(control => control.LoadingDate.Date <= req.EndDateRange.Value.Date.AddDays(1));
        }

        int TotalItems = await query.CountAsync();

        if (req.SortBy is not null)
        {
            switch (req.SortBy)
            {
                case sortableFields.Id:
                    query = req.SortDescending
                        ? query.OrderByDescending(control => control.Id)
                        : query.OrderBy(control => control.Id);
                    break;
                // case sortableFields.Laadbonnummer:
                //     query = req.SortDescending
                //         ? query.OrderByDescending(control => control.LoadingSlips.Select(slip => slip.LaadbonNr))
                //         : query.OrderBy(control => control.LoadingSlips.Select(slip => slip.LaadbonNr));
                //     break;
                case sortableFields.Transporteur:
                    if (req.SortDescending)
                    {
                        query = query.OrderByDescending(control => control.Transporter.Name);
                    }
                    else
                    {
                        query = query.OrderBy(control => control.Transporter.Name);
                    }

                    break;
                case sortableFields.Nummerplaat:
                    if (req.SortDescending)
                    {
                        query = query.OrderByDescending(control => control.LicensePlate);
                    }
                    else
                    {
                        query = query.OrderBy(control => control.LicensePlate);
                    }

                    break;
                case sortableFields.Datum:
                    if (req.SortDescending)
                    {
                        query = query.OrderByDescending(control => control.LoadingDate);
                    }
                    else
                    {
                        query = query.OrderBy(control => control.LoadingDate);
                    }

                    break;
                case sortableFields.Routenummer:
                    if (req.SortDescending)
                    {
                        query = query.OrderByDescending(control =>
                            control.RouteNumbers.Min(routenumber => routenumber.Number));
                    }
                    else
                    {
                        query = query.OrderBy(control => control.RouteNumbers.Min(routenumber => routenumber.Number));
                    }

                    break;
                case null:
                    break;
                default:
                    //TODO chek if this is the right exception
                    throw new ArgumentOutOfRangeException();
            }
        }

        var items = query
            .Skip(req.Page * req.PageSize)
            .Take(req.PageSize)
            .Select(control =>
                new ControleDto.Index
                {
                    Id = control.Id,
                    Transporteur = control.Transporter.Name,
                    Laadbonnen = control.LoadingSlips.Select(slip => new LaadBonDto.Index
                    {
                        Adres = new AddresDto
                        {
                            Street = slip.LeveringsAddress.Street,
                            HouseNr = slip.LeveringsAddress.HouseNr,
                            Postcode = slip.LeveringsAddress.Postcode,
                            CountryIsoCode = slip.LeveringsAddress.CountryIsoCode
                        },
                        LaadBonnummer = slip.LaadbonNr
                    }),
                    //sort route from small to big
                    Routenummers = control.RouteNumbers.Select(routenumber => routenumber.Number),
                    Nummerplaat = control.LicensePlate,
                    Datum = control.LoadingDate
                }
            );
        //TODO: ask if this is the right way to sort
        //sort routenumbers list inside items from small to big

        foreach (var item in items)
        {
            if (item.Routenummers != null) item.Routenummers = item.Routenummers.OrderByDescending(x => x).ToList();
        }


        var result = new Result.GetItemsPaginated<ControleDto.Index>
        {
            Items = await items.ToListAsync(),
            TotalItems = TotalItems
        };

        return result;
    }
}