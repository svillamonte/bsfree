using BSFree.Helpers;
using BSFree.Shared;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BSFree.Tests.Helpers
{
    public class ContinuationTokenHelperTests
    {
        private IContinuationTokenHelper _continuationTokenHelper;

        public ContinuationTokenHelperTests()
        {
            _continuationTokenHelper = new ContinuationTokenHelper();
        }

        [Fact]
        public void GetNextPageToken_TokensAddedUpToFourthPage_NextPageTokenIsNewestTokenAdded()
        {
            // Arrange
            var tokenToSecond = new ContinuationToken();
            var tokenToThird = new ContinuationToken();
            var tokenToFourth = new ContinuationToken();
            var tokenToFifth = new ContinuationToken();

            // Act

            // Go to 1st page
            _continuationTokenHelper.AddToken(tokenToSecond);
            var actualTokenToSecond = _continuationTokenHelper.GetNextPageToken();

            // Go to 2nd page
            _continuationTokenHelper.AddToken(tokenToThird);
            var actualTokenToThird = _continuationTokenHelper.GetNextPageToken();

            // Go to 3rd page
            _continuationTokenHelper.AddToken(tokenToFourth);
            var actualTokenToFourth = _continuationTokenHelper.GetNextPageToken();

            // Go to 4th page
            _continuationTokenHelper.AddToken(tokenToFifth);
            var actualTokenToFifth = _continuationTokenHelper.GetNextPageToken();

            // Assert
            Assert.Equal(tokenToSecond, actualTokenToSecond);
            Assert.Equal(tokenToThird, actualTokenToThird);
            Assert.Equal(tokenToFourth, actualTokenToFourth);
            Assert.Equal(tokenToFifth, actualTokenToFifth);
        }

        [Fact]
        public void GetPreviousPageToken_EmptyPages_ReturnsNull()
        {
            // Act
            var previousToken = _continuationTokenHelper.GetPreviousPageToken();

            // Assert
            Assert.Null(previousToken);
        }

        [Fact]
        public void GetPreviousPageToken_ReturnFromSecondToFirstPage_PreviousTokenIsNullForFirstPage()
        {
            // Arrange
            var tokenToSecond = new ContinuationToken();
            var tokenToThird = new ContinuationToken();

            _continuationTokenHelper.AddToken(tokenToSecond);
            _continuationTokenHelper.AddToken(tokenToThird);

            // Act
            var previousToken = _continuationTokenHelper.GetPreviousPageToken();

            // Assert
            Assert.Null(previousToken);
        }

        [Fact]
        public void GetPreviousPageToken_ReturnFromThirdToSecondPage_PreviousTokenIsSecondForSecondPage()
        {
            // Arrange
            var tokenToSecond = new ContinuationToken();
            var tokenToThird = new ContinuationToken();
            var tokenToFourth = new ContinuationToken();

            _continuationTokenHelper.AddToken(tokenToSecond);
            _continuationTokenHelper.AddToken(tokenToThird);
            _continuationTokenHelper.AddToken(tokenToFourth);

            // Act
            var previousToken = _continuationTokenHelper.GetPreviousPageToken();

            // Assert
            Assert.Equal(tokenToSecond, previousToken);
        }

        [Fact]
        public void AddToken_TokensAddedUpToFirstPage_LastPageIsFirstPage()
        {
            // Arrange
            var tokenToSecond = new ContinuationToken();

            // Act
            _continuationTokenHelper.AddToken(tokenToSecond);

            // Assert
            var lastPage = _continuationTokenHelper.ShoutsPages.Peek();

            Assert.Equal(0, lastPage.Index);
            Assert.Null(lastPage.PaginationToken);

            Assert.Equal(tokenToSecond, _continuationTokenHelper.GetNextPageToken());
        }

        [Fact]
        public void AddToken_TokensAddedUpToSecondPage_LastPageIsSecondPage()
        {
            // Arrange
            var tokenToSecond = new ContinuationToken();
            var tokenToThird = new ContinuationToken();

            // Act
            _continuationTokenHelper.AddToken(tokenToSecond);
            _continuationTokenHelper.AddToken(tokenToThird);

            // Assert
            var lastPage = _continuationTokenHelper.ShoutsPages.Peek();

            Assert.Equal(1, lastPage.Index);
            Assert.Equal(tokenToSecond, lastPage.PaginationToken);

            Assert.Equal(tokenToThird, _continuationTokenHelper.GetNextPageToken());
        }

        [Fact]
        public void GetPreviousPageTokenAndGetNextPageTokenWorkingTogether()
        {
            // Arrange
            var tokenToSecond = new ContinuationToken();
            var tokenToThird = new ContinuationToken();
            var tokenToFourth = new ContinuationToken();
            var tokenToFifth = new ContinuationToken();
            var tokenFifth = (ContinuationToken)null;

            // Act

            // Go from 1st to 5th page
            _continuationTokenHelper.AddToken(tokenToSecond);
            _continuationTokenHelper.AddToken(tokenToThird);
            _continuationTokenHelper.AddToken(tokenToFourth);
            _continuationTokenHelper.AddToken(tokenToFifth);
            _continuationTokenHelper.AddToken(tokenFifth);

            // Go to 4th page
            var actualTokenToFourth1 = _continuationTokenHelper.GetPreviousPageToken();

            // Go to 5th page
            var actualTokenToFifth = _continuationTokenHelper.GetNextPageToken();
            _continuationTokenHelper.AddToken(actualTokenToFifth);

            // Go to 4th page
            var actualTokenToFourth2 = _continuationTokenHelper.GetPreviousPageToken();

            // Go to 3rd page
            var actualTokenToThird = _continuationTokenHelper.GetPreviousPageToken();

            // Go to 2nd page
            var actualTokenToSecond = _continuationTokenHelper.GetPreviousPageToken();

            // Go to 1st page
            var actualTokenToFirst = _continuationTokenHelper.GetPreviousPageToken();

            // Assert
            Assert.Equal(tokenToFourth, actualTokenToFourth1);
            Assert.Equal(tokenToFifth, actualTokenToFifth);
            Assert.Equal(tokenToFourth, actualTokenToFourth2);
            Assert.Equal(tokenToThird, actualTokenToThird);
            Assert.Equal(tokenToSecond, actualTokenToSecond);
            Assert.Null(actualTokenToFirst);
        }
    }
}
