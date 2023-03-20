using System.Collections.Generic;

namespace CandidateTesting.GustavoFagundesComin.Service.Writer
{
    public interface ILogWriter
    {
        public void Write(List<string> logs, string filename);
    }
}