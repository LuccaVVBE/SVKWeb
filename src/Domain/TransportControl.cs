using Ardalis.GuardClauses;
using Svk.Domain.Common;
using Svk.Domain.Files;

namespace Svk.Domain
{
    public class TransportControl : Entity
    {
        // PRIVATE FIELDS

        private readonly List<Image> _images = new();

        private readonly List<LoadingSlip> _loadingSlips = new();

        private readonly List<Routenumber> _routeNumbers = new();

        private String _licensePlate = default!;

        private Loader _loader = default!;

        private DateTime _loadingDate = default!;

        private Transporter _transporter = default!;

        private TransportControl()
        {
        }

        public TransportControl(IEnumerable<Routenumber> routeNumbers, IEnumerable<LoadingSlip> loadingSlips,
            Transporter transporter,
            string licensePlate, IEnumerable<Image>? images, Loader loader, DateTime loadingDate)

        {
            Loader = loader;
            LoadingDate = loadingDate;
            Transporter = transporter;
            LicensePlate = licensePlate;

            Guard.Against.NullOrEmpty(routeNumbers, nameof(routeNumbers));
            foreach (var routeNumber in routeNumbers)
            {
                _routeNumbers.Add(routeNumber);
            }

            Guard.Against.NullOrEmpty(loadingSlips, nameof(loadingSlips));
            foreach (var slip in loadingSlips)
            {
                _loadingSlips.Add(slip);
            }

            if (images != null)
            {
                foreach (var image in images)
                {
                    //Guard.Against.NullOrEmpty(image, nameof(image));
                    _images.Add(image);
                }
            }
        }


        // PUBLIC PROPERTIES
        public Loader Loader
        {
            get => _loader;
            set => _loader = Guard.Against.Null(value, nameof(Loader));
        }

        public IReadOnlyCollection<Image> Images => _images.AsReadOnly();

        public IReadOnlyCollection<LoadingSlip> LoadingSlips => _loadingSlips.AsReadOnly();

        public IReadOnlyCollection<Routenumber> RouteNumbers => _routeNumbers.AsReadOnly();


        public Transporter Transporter
        {
            get => _transporter;
            set => _transporter = Guard.Against.Null(value, nameof(Transporter));
        }

        public String LicensePlate
        {
            get => _licensePlate;
            set => _licensePlate = Guard.Against.NullOrWhiteSpace(value, nameof(LicensePlate));
        }

        public DateTime LoadingDate
        {
            get => _loadingDate;
            set => _loadingDate = Guard.Against.Default(value, nameof(LoadingDate));
        }

		public void AddImage(Image image)
		{
			Guard.Against.Null(image, nameof(image));
			_images.Add(image);
		}
	}
}