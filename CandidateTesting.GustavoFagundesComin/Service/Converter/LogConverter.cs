using CandidateTesting.GustavoFagundesComin.Domain;

namespace CandidateTesting.GustavoFagundesComin.Application.Service.Converter
{
    public class LogConverter : ILogConverter
    {
        public string Convert(LogEntry log)
        {
            return $"\"{log.Provider}\" {log.HttpMethod} {log.StatusCode} {log.UriPath} {log.TimeTaken} {log.ResponseSize} {log.CacheStatus}";
        }
    }
}