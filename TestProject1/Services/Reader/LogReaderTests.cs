using CandidateTesting.GustavoFagundesComin.Application;
using CandidateTesting.GustavoFagundesComin.Domain;
using CandidateTesting.GustavoFagundesComin.Service.Reader;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CandidateTesting.GustavoFagundesComin.Tests.Services.Reader
{
    [TestClass]
    public class LogReaderTests
    {
        [TestMethod]
        public async Task ReadAsync_ShouldReadLogs()
        {
            // Arrange
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var response = await Application.Application.GetFileContentAsync("https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt");
            //using Stream stream = await response.Content.ReadAsStreamAsync();

            //mockResponse.Content = new StreamContent(stream);

            var expectedLogs = new List<LogEntry>
            {
                new LogEntry
                {
                    Provider ="MINHA CDN",
                    HttpMethod ="\u0022GET",
                    StatusCode =200,
                    UriPath ="/robots.txt",
                    TimeTaken =1002,
                    ResponseSize =0,
                    CacheStatus ="HIT"
                },
                new LogEntry
                {
                    Provider ="MINHA CDN",
                    HttpMethod ="\u0022POST",
                    StatusCode =200,
                    UriPath ="/myImages",
                    TimeTaken =3194,
                    ResponseSize =0,
                    CacheStatus ="MISS"
                },
                new LogEntry
                {
                    Provider ="MINHA CDN",
                    HttpMethod ="\u0022GET",
                    StatusCode =404,
                    UriPath ="/not-found",
                    TimeTaken =1429,
                    ResponseSize =0,
                    CacheStatus ="MISS"
                },
                new LogEntry
                {
                    Provider ="MINHA CDN",
                    HttpMethod ="\u0022GET",
                    StatusCode =200,
                    UriPath ="/robots.txt",
                    TimeTaken =2451,
                    ResponseSize =0,
                    CacheStatus ="REFRESH_HIT"
                }
            };

            var logReader = new LogReader();

            // Act
            var result = await logReader.ReadAsync(response);

            // Assert
            CollectionAssert.AreEqual(expectedLogs, result);
        }

        [TestMethod]
        public async Task ReadAsync_InvalidInput_ThrowsException()
        {
            // Arrange
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var input = "200|HIT|GET /robots.txt HTTP/1.1|100.0|-\n" +
                        "invalid-line\n" +
                        "500|INVALIDATE|GET /api/users HTTP/1.1|50.0|-\n";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            mockResponse.Content = new StreamContent(stream);

            var logReader = new LogReader();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => logReader.ReadAsync(mockResponse));
        }
    }
}