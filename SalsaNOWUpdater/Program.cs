using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SalsaNOWUpdater
{
    internal class Program
    {
        private static string currentPath = Directory.GetCurrentDirectory();

        private static async Task Main(string[] args)
        {
            string jsonUrl = "https://salsanowfiles.work/jsons/update.json";

            // Making sure no SSL/TLS issues occur
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, errors) => true;

            using (WebClient webClient = new WebClient())
            {
                string json = await webClient.DownloadStringTaskAsync(jsonUrl);
                List<SalsaNow> directory = JsonConvert.DeserializeObject<List<SalsaNow>>(json);
                var downloadLink = directory[0];

                await webClient.DownloadFileTaskAsync(new Uri(downloadLink.salsaNOWLink), $"{currentPath}\\SalsaNOW.exe");

                var startSteam = new ProcessStartInfo
                {
                    FileName = $"{currentPath}\\SalsaNOW.exe",
                    UseShellExecute = true,
                };

                Process.Start(startSteam);

                return;
            }
        }

        public class SalsaNow
        {
            public string salsaNOWLink { get; set; }
        }
    }
}