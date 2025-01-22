using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EnergyApp.src
{
    public class XmlFetcher
    {
        private const string BaseUrl = "https://www.ilportaleofferte.it/portaleOfferte/resources/opendata/csv/offerteML/";

        /// <summary>
        /// Fetches the XML data from the specified endpoint based on the provided date.
        /// </summary>
        /// <param name="year">Year of the desired file.</param>
        /// <param name="month">Month of the desired file.</param>
        /// <param name="day">Day of the desired file.</param>
        /// <returns>The XML content as a string.</returns>
        public async Task<string> GetXmlAsync(int year, int month, int day)
        {
            // Validate date components
            if (month < 1 || month > 12 || day < 1 || day > 31)
                throw new ArgumentException("Invalid month or day provided.");

            // Format the URL
            string formattedDate = $"{year:D4}_{month:D1}";
            string fileName = $"PO_Offerte_E_MLIBERO_{year:D4}{month:D2}{day:D2}.xml";
            string fullUrl = $"{BaseUrl}{formattedDate}/{fileName}";
            string outputFileName = fileName;
            string fullOutputPath = Path.Combine(FileSystem.AppDataDirectory, outputFileName);
            File.Delete(fullOutputPath);

            // Ignore SSL certificate errors
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;


            using (WebClient client = new())
            {
                try
                {
                    client.DownloadFile(fullUrl, fullOutputPath);
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"Error fetching XML data: {ex.Message}", ex);
                }
            }
            return fullOutputPath;
        }
    }

}
