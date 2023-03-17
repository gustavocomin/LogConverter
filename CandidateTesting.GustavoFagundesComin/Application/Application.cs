using CandidateTesting.GustavoFagundesComin.Domain;
using CandidateTesting.GustavoFagundesComin.Service.Converter;
using CandidateTesting.GustavoFagundesComin.Service.Reader;
using CandidateTesting.GustavoFagundesComin.Service.Writer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace CandidateTesting.GustavoFagundesComin.Application
{
    public class Application
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter the URL to download: ");
            string url = @"https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt";// Console.ReadLine();
            Console.Write("\nEnter the file path to save the downloaded file: ");
            string filePath = @"C:\Users\Gustavo\Desktop\Nova pasta\teste.txt";//Console.ReadLine();


            HttpResponseMessage response = await GetFileContentAsync(url);
            List<LogEntry> logList = await new LogReader().ReadAsync(response);
            var convertedLogs = new List<string>();


            logList.ForEach(log =>
            {
                convertedLogs.Add(new LogConverter().Convert(log));
            });

            new LogWriter().Write(convertedLogs, filePath);

            //Console.WriteLine("Press any key to exit...");
            //Console.
            //Console.ReadKey();
        }

        private static async Task<HttpResponseMessage> GetFileContentAsync(string url)
        {
            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
                return response;
            else
                throw new Exception($"Failed to get file content from {url}: {response.StatusCode}");
        }
    }
}