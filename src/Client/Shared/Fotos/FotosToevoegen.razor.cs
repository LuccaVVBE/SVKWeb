using global::Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Svk.Shared.Controles;
using System;
using System.Net.Http.Json;

namespace Svk.Client.Shared.Fotos
{
    public partial class FotosToevoegen
    {
        [Inject]
        public  NavigationManager NavigationManager { get; set; }
        [Inject]
        public IControleService ControleService { get; set; } = default !;
        [Parameter]
        public int ControleId { get; set; }

        private static string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full z-10";
        private string DragClass = DefaultDragClass;
        private List<string> fileNames = new List<string>();
        private List<IBrowserFile> files = new List<IBrowserFile>();
        private bool alert;
        private List<string> errors = new List<string>();


        private long maxFileSize = 1024L * 1024L *64L; //64MB filesize
        private void OnInputFileChanged(InputFileChangeEventArgs e)
        {
            ClearDragClass();
            var allFiles = e.GetMultipleFiles();
            foreach (var file in allFiles)
            {
                if (file.ContentType.Contains("jpeg"))
                {
                    if (file.Size > maxFileSize)
                    {
                        alert = true;
                        errors.Add(e.File.Name);
                        continue;
                    }
                    fileNames.Add(file.Name);
                    files.Add(file);
                }
            }
        }

        private void clearAlert()
        {
            alert = false;
            errors = new List<string>();
        }

        private async Task Clear()
        {
            fileNames.Clear();
            files.Clear();
            ClearDragClass();
            await Task.Delay(100);
        }

        private async Task Upload()
        {


            //Upload the files here
            HttpClient client = new();
            int counter = 0;
            var result = await ControleService.EditAsync(ControleId, new ControleDto.Edit() { Fotos = fileNames.Count });
            foreach(var url in result.SignedUrls)
            {
                using (var fileStream = files.ElementAt(counter).OpenReadStream(maxFileSize))
                using (var streamContent = new StreamContent(fileStream))
                {
                    // Set the Content-Type header based on the file type
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                    // Create the PUT request
                    var request = new HttpRequestMessage(HttpMethod.Put, url)
                    {
                        Content = streamContent
                    };

                    // Send the request
                    var response = await client.SendAsync(request);
                }

                counter++;
            }
            NavigationManager.NavigateTo($"/controles/{ControleId}", true);
        }

        private void SetDragClass()
        {
            DragClass = $"{DefaultDragClass} mud-border-primary";
        }

        private void ClearDragClass()
        {
            DragClass = DefaultDragClass;
        }
    }
}