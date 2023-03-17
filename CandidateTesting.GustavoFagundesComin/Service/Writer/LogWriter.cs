using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;

namespace CandidateTesting.GustavoFagundesComin.Service.Writer
{
    public class LogWriter : ILogWriter
    {
        public void Write(List<string> logs, string filename)
        {
            using StreamWriter writer = new(filename);
            writer.WriteLine("#Version: 1.0");
            writer.WriteLine($"#Date: {DateTime.Now}");
            writer.WriteLine("#Fields: provider http-method status-code uri-path time-taken response-size cache-status");
            foreach (string log in logs)
            {
                writer.WriteLine(log);
            }
        }
    }
}
