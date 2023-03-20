using CandidateTesting.GustavoFagundesComin.Domain;
using CandidateTesting.GustavoFagundesComin.Service.Converter;
using CandidateTesting.GustavoFagundesComin.Service.Reader;
using CandidateTesting.GustavoFagundesComin.Service.Writer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CandidateTesting.GustavoFagundesComin.Application
{
    public class Application
    {
        static async Task Main(string[] args)
        {
            //https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt
            //C:\Users\Gustavo\Desktop\Nova pasta\input-01.txt


            Console.Write("Enter the URL to download: ");
            string url = Console.ReadLine();
            Console.Write("\nEnter the file path to save the downloaded file: ");
            string filePath = Console.ReadLine();
            string fileName = Path.GetFileName(url);

            HttpResponseMessage response = await GetFileContentAsync(url);
            List<LogEntry> logList = await new LogReader().ReadAsync(response);
            List<string> convertedLogs = PopulateLogList(logList);

            if (!Path.GetFileName(filePath).EndsWith(".txt"))
                filePath += $@"\{fileName}";

            if (File.Exists(filePath))
                filePath = AdaptFileName(fileName, filePath);

            new LogWriter().Write(convertedLogs, filePath);

            Console.WriteLine("Press any key to exit...");
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

        private static List<string> PopulateLogList(List<LogEntry> logList)
        {
            string errorList = "";
            List<string> convertedLogs = new();
            logList.ForEach(log =>
            {
                try
                {
                    convertedLogs.Add(new LogConverter().Convert(log));
                }
                catch (Exception e)
                {
                    errorList += $"Error on converting log. Error: {e.Message}.\n Unconverted log: {log}\n\n";
                }
            });

            if (errorList.Any())
                throw new Exception(errorList);

            return convertedLogs;
        }

        private static string AdaptFileName(string fileName, string filePath)
        {
            string file;
            int fileCount = 1;
            string newFileName = CreateFileName(fileName, fileCount);

            string path = filePath.Replace($@"\{fileName}", "");

            while (File.Exists(Path.Combine(path, newFileName)))
            {
                fileCount++;
                newFileName = CreateFileName(fileName, fileCount);
            }

            file = Path.Combine(path, newFileName);

            return file;
        }

        private static string CreateFileName(string fileName, int fileCount)
        {
            string newFileName = Path.GetFileNameWithoutExtension(fileName) + " (" + fileCount.ToString() + ")" + Path.GetExtension(fileName);

            return newFileName;
        }
    }
}