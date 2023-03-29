using CandidateTesting.GustavoFagundesComin.Service.Writer;

namespace CandidateTesting.GustavoFagundesComin.Tests.Services.Writer
{
    [TestClass]
    public class LogWriterTests
    {
        [TestMethod]
        public void Write_ShouldWriteLogFile()
        {
            // Arrange
            var logs = new List<string>
            {
                "\"MINHA CDN\" GET 200 /path1 1.23 1024 HIT",
                "\"MINHA CDN\" POST 404 /path2 4.56 0 MISS"
            };
            string filename = "test-logs.txt";

            // Act
            var writer = new LogWriter();
            writer.Write(logs, filename);

            // Assert
            Assert.IsTrue(File.Exists(filename));
            var lines = File.ReadAllLines(filename);
            Assert.AreEqual(5, lines.Length);
            Assert.AreEqual("#Version: 1.0", lines[0]);
            Assert.IsTrue(lines[1].StartsWith("#Date: "));
            Assert.AreEqual("#Fields: provider http-method status-code uri-path time-taken response-size cache-status", lines[2]);
            Assert.AreEqual(logs[0], lines[3]);
            Assert.AreEqual(logs[1], lines[4]);

            // Cleanup
            File.Delete(filename);
        }

        [TestMethod]
        public void Write_ShouldThrowException_WhenFileIsInUse()
        {
            // Arrange
            var logs = new List<string>
            {
                "\"MINHA CDN\" GET 200 /path1 1.23 1024 HIT"
            };

            string filename = "test-logs.txt";

            // Act
            using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var writer = new LogWriter();
                Assert.ThrowsException<Exception>(() => writer.Write(logs, filename));
            }

            // Cleanup
            File.Delete(filename);
        }
    }
}
