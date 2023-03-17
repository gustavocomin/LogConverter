using CandidateTesting.GustavoFagundesComin.Domain;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CandidateTesting.GustavoFagundesComin.Service.Reader
{
    public interface ILogReader
    {
        Task<List<LogEntry>> ReadAsync(HttpResponseMessage response);
    }
}