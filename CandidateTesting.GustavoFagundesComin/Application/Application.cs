using CandidateTesting.GustavoFagundesComin.Application.Service.Converter;
using CandidateTesting.GustavoFagundesComin.Domain;
using CandidateTesting.GustavoFagundesComin.Service.Converter;
using CandidateTesting.GustavoFagundesComin.Service.Reader;
using CandidateTesting.GustavoFagundesComin.Service.Writer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CandidateTesting.GustavoFagundesComin.Application
{
    public class Application
    {
        private readonly ILogReader _logReader;
        private readonly ILogWriter _logWriter;
        private readonly ILogConverter _logConverter;

        public Application(ILogReader logReader, ILogWriter logWriter, ILogConverter logConverter)
        {
            _logReader = logReader;
            _logWriter = logWriter;
            _logConverter = logConverter;
        }

        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddScoped<Application>()
                .AddScoped<ILogReader, LogReader>()
                .AddScoped<ILogWriter, LogWriter>()
                .AddScoped<ILogConverter, LogConverter>()
                .BuildServiceProvider();

            await serviceProvider.GetService<Application>().RunAsync(args);
        }

        public async Task RunAsync(string[] args)
        {
            string url;
            string filePath;
            if (args.Length != 2)
            {
                Console.Write("Enter the URL to download: ");
                url = Console.ReadLine();
                Console.Write("\nEnter the file path to save the downloaded file: ");
                filePath = Console.ReadLine();
            }
            else
            {
                url = args[0];
                filePath = args[1];
            }

            string fileName = Path.GetFileName(url);

            HttpResponseMessage response = await GetFileContentAsync(url);
            List<LogEntry> logList = await _logReader.ReadAsync(response);
            List<string> convertedLogs = PopulateLogList(logList);

            if (!Path.GetFileName(filePath).EndsWith(".txt"))
                filePath += $@"\{fileName}";

            if (File.Exists(filePath))
                filePath = AdaptFileName(fileName, filePath);

            _logWriter.Write(convertedLogs, filePath);

            Console.WriteLine("Press any key to exit...");
        }

        public static async Task<HttpResponseMessage> GetFileContentAsync(string url)
        {
            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(url);

            if (response != null && response.IsSuccessStatusCode)
                return response;
            else
                throw new Exception($"Failed to get file content from {url}: {response?.StatusCode}");
        }

        private List<string> PopulateLogList(List<LogEntry> logList)
        {
            List<string> logs = new();
            foreach (var log in logList)
                logs.Add(_logConverter.Convert(log));
            return logs;
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