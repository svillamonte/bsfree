using BSFree.Helpers;
using BSFree.Shared;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BSFree.Tests.Helpers
{
    public class PaginationServiceTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IContinuationTokenHelper> _continuationTokenHelperMock;

        private readonly IPaginationService _paginationService;

        public PaginationServiceTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _continuationTokenHelperMock = new Mock<IContinuationTokenHelper>();

            _paginationService = new PaginationService(_apiClientMock.Object, _continuationTokenHelperMock.Object);
        }

        [Fact]
        public void HasPreviousPage_WithNoShoutsPages_ReturnsFalse()
        {
            // Arrange
            var shoutsPages = new Stack<ShoutsPage>();

            _continuationTokenHelperMock
                .SetupGet(x => x.ShoutsPages)
                .Returns(shoutsPages);

            // Act
            var result = _paginationService.HasPreviousPage;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasPreviousPage_WithOneShoutsPages_ReturnsFalse()
        {
            // Arrange
            var shoutsPages = new Stack<ShoutsPage>();
            shoutsPages.Push(new ShoutsPage());

            _continuationTokenHelperMock
                .SetupGet(x => x.ShoutsPages)
                .Returns(shoutsPages);

            // Act
            var result = _paginationService.HasPreviousPage;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasPreviousPage_WithTwoShoutsPages_ReturnsTrue()
        {
            // Arrange
            var shoutsPages = new Stack<ShoutsPage>();
            shoutsPages.Push(new ShoutsPage());
            shoutsPages.Push(new ShoutsPage());

            _continuationTokenHelperMock
                .SetupGet(x => x.ShoutsPages)
                .Returns(shoutsPages);

            // Act
            var result = _paginationService.HasPreviousPage;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasNextPage_WithoutNextPageToken_ReturnsFalse()
        {
            // Arrange
            _continuationTokenHelperMock
                .Setup(x => x.GetNextPageToken())
                .Returns((ContinuationToken)null);

            // Act
            var result = _paginationService.HasNextPage;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasNextPage_WithNextPageToken_ReturnsTrue()
        {
            // Arrange
            _continuationTokenHelperMock
                .Setup(x => x.GetNextPageToken())
                .Returns(new ContinuationToken());

            // Act
            var result = _paginationService.HasNextPage;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetNextShoutsPage_CallsAllExpectedServices()
        {
            // Arrange
            var shouts = new Shout[]
            {
                new Shout(),
                new Shout(),
                new Shout()
            };
            var shoutsResponse = new ShoutsResponse
            {
                ContinuationToken = new ContinuationToken(),
                Shouts = shouts
            };

            _continuationTokenHelperMock
                .Setup(x => x.GetNextPageToken())
                .Returns(new ContinuationToken());

            _apiClientMock
                .Setup(x => x.GetShoutsResponse(It.IsAny<ContinuationToken>()))
                .ReturnsAsync(shoutsResponse);

            // Act
            var result = await _paginationService.GetNextShoutsPage();

            // Assert
            Assert.Equal(shouts, result);
            _continuationTokenHelperMock.Verify(x => x.AddToken(shoutsResponse.ContinuationToken), Times.Once);
        }

        [Fact]
        public async Task GetPreviousShoutsPage_CallsAllExpectedServices()
        {
            // Arrange
            var shouts = new Shout[]
            {
                new Shout(),
                new Shout(),
                new Shout()
            };
            var shoutsResponse = new ShoutsResponse
            {
                ContinuationToken = new ContinuationToken(),
                Shouts = shouts
            };

            _continuationTokenHelperMock
                .Setup(x => x.GetPreviousPageToken())
                .Returns(new ContinuationToken());

            _apiClientMock
                .Setup(x => x.GetShoutsResponse(It.IsAny<ContinuationToken>()))
                .ReturnsAsync(shoutsResponse);

            // Act
            var result = await _paginationService.GetPreviousShoutsPage();

            // Assert
            Assert.Equal(shouts, result);
            _continuationTokenHelperMock.Verify(x => x.AddToken(It.IsAny<ContinuationToken>()), Times.Never);
        }
    }
}