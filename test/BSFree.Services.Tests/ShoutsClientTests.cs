using BSFree.Services.Interfaces;
using BSFree.Shared;
using Moq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BSFree.Services.Tests
{
    public class ShoutsClientTests
    {
        private Mock<IHttpClient> _httpClientMock;

        private IShoutsClient _shoutsClient;

        public ShoutsClientTests()
        {
            _httpClientMock = new Mock<IHttpClient>();

            _shoutsClient = new ShoutsClient(_httpClientMock.Object, It.IsAny<string>());
        }

        [Fact]
        public async Task GetLatestsShouts_WithContinuationToken_ReturnsResults()
        {
            // Arrange
            var jsonResponse = "{ " +
                "\"shouts\": [ " +
                    "{ " +
                        "\"shoutId\": \"shoutone\", " +
                        "\"nick\": \"Nick one\", " +
                        "\"body\": \"Body one\", " +
                        "\"imageUri\": \"http://image.one\", " +
                    "}, " +
                    "{ " +
                        "\"shoutId\": \"shouttwo\", " +
                        "\"nick\": \"Nick two\", " +
                        "\"body\": \"Body two\", " +
                        "\"imageUri\": \"http://image.two\", " +
                    "}, " +
                "]," +
                "\"continuationToken\": null" +
            "}";

            var continuationToken = new ContinuationToken();
            _httpClientMock
                .Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var result = await _shoutsClient.GetLatestShouts(continuationToken);

            // Assert
            Assert.Equal("shoutone", result.Shouts.ElementAt(0).ShoutId);
            Assert.Equal("Nick one", result.Shouts.ElementAt(0).Nick);
            Assert.Equal("Body one", result.Shouts.ElementAt(0).Body);
            Assert.Equal("http://image.one", result.Shouts.ElementAt(0).ImageUri);

            Assert.Equal("shouttwo", result.Shouts.ElementAt(1).ShoutId);
            Assert.Equal("Nick two", result.Shouts.ElementAt(1).Nick);
            Assert.Equal("Body two", result.Shouts.ElementAt(1).Body);
            Assert.Equal("http://image.two", result.Shouts.ElementAt(1).ImageUri);

            Assert.Null(result.ContinuationToken);
        }

        [Fact]
        public async Task GetLatestsShouts_WithoutContinuationToken_ReturnsResults()
        {
            // Arrange
            var jsonResponse = "{ " +
                "\"shouts\": [ " +
                    "{ " +
                        "\"shoutId\": \"shoutone\", " +
                        "\"nick\": \"Nick one\", " +
                        "\"body\": \"Body one\", " +
                        "\"imageUri\": \"http://image.one\", " +
                    "}, " +
                    "{ " +
                        "\"shoutId\": \"shouttwo\", " +
                        "\"nick\": \"Nick two\", " +
                        "\"body\": \"Body two\", " +
                        "\"imageUri\": \"http://image.two\", " +
                    "}, " +
                "]," +
                "\"continuationToken\": null" +
            "}";

            _httpClientMock
                .Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var result = await _shoutsClient.GetLatestShouts();

            // Assert
            Assert.Equal("shoutone", result.Shouts.ElementAt(0).ShoutId);
            Assert.Equal("Nick one", result.Shouts.ElementAt(0).Nick);
            Assert.Equal("Body one", result.Shouts.ElementAt(0).Body);
            Assert.Equal("http://image.one", result.Shouts.ElementAt(0).ImageUri);

            Assert.Equal("shouttwo", result.Shouts.ElementAt(1).ShoutId);
            Assert.Equal("Nick two", result.Shouts.ElementAt(1).Nick);
            Assert.Equal("Body two", result.Shouts.ElementAt(1).Body);
            Assert.Equal("http://image.two", result.Shouts.ElementAt(1).ImageUri);

            Assert.Null(result.ContinuationToken);
        }
    }
}
