namespace CandidateTesting.GustavoFagundesComin.Service.Writer
{
    public class LogWriter : ILogWriter
    {
        public void Write(List<string> logs, string filename)
        {
            try
            {
                using StreamWriter writer = new(filename);
                writer.WriteLine("#Version: 1.0");
                writer.WriteLine($"#Date: {DateTime.Now}");
                writer.WriteLine("#Fields: provider http-method status-code uri-path time-taken response-size cache-status");
                logs.ForEach(log =>
                {
                    writer.WriteLine(log);
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}