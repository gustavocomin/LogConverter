using CandidateTesting.GustavoFagundesComin.Domain;

namespace CandidateTesting.GustavoFagundesComin.Service.Reader
{
    public interface ILogReader
    {
        Task<List<LogEntry>> ReadAsync(HttpResponseMessage response);
    }
}