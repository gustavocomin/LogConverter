using CandidateTesting.GustavoFagundesComin.Application.Service.Converter;
using CandidateTesting.GustavoFagundesComin.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CandidateTesting.GustavoFagundesComin.Tests.Services.Converter
{
    [TestClass]
    public class LogConverterTests
    {
        private LogConverter _logConverter;

        [TestInitialize]
        public void Setup()
        {
            _logConverter = new LogConverter();
        }

        [TestMethod]
        public void Convert_Should_Return_Correct_String()
        {
            // Arrange
            var log = new LogEntry
            {
                Provider = "testProvider",
                HttpMethod = "GET",
                StatusCode = 200,
                UriPath = "/test/path",
                TimeTaken = 150,
                ResponseSize = 1024,
                CacheStatus = "HIT"
            };

            // Act
            var convertedLog = _logConverter.Convert(log);

            // Assert
            Assert.AreEqual("\"testProvider\" GET 200 /test/path 150 1024 HIT", convertedLog);
        }
    }
}
