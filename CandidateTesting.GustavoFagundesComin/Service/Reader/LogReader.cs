using CandidateTesting.GustavoFagundesComin.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CandidateTesting.GustavoFagundesComin.Service.Reader
{
    public class LogReader : ILogReader
    {
        public async Task<List<LogEntry>> ReadAsync(HttpResponseMessage response)
        {
            List<LogEntry> logs = new();

            using Stream stream = await response.Content.ReadAsStreamAsync();
            using (StreamReader reader = new(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        string[] parts = line.Split('|');
                        logs.Add(new LogEntry
                        {
                            Provider = "MINHA CDN",
                            HttpMethod = parts[3].Split(' ')[0],
                            StatusCode = int.Parse(parts[1]),
                            UriPath = parts[3].Split(' ')[1],
                            TimeTaken = double.Parse(parts[4]),
                            ResponseSize = 0,
                            CacheStatus = parts[2] switch
                            {
                                "HIT" => "HIT",
                                "MISS" => "MISS",
                                "INVALIDATE" => "REFRESH_HIT",
                                _ => throw new InvalidOperationException($"Invalid cache status: {parts[2]}")
                            }
                        });
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Erro on read log. Error: {e.Message}. Line with error: {line}\n");
                    }
                }
            }

            var a = JsonSerializer.Serialize(logs);
            File.WriteAllText(@"C:\Users\Gustavo\Desktop\Nova pasta\teste.json", a);
            return logs;
        }
    }
}