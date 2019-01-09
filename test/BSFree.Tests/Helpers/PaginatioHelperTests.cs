using BSFree.Helpers;
using BSFree.Shared;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BSFree.Tests
{
    public class PaginationHelperTests
    {
        private IPaginationHelper _paginationHelper;

        public PaginationHelperTests()
        {
            _paginationHelper = new PaginationHelper();
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
            _paginationHelper.AddToken(tokenToSecond);
            var actualTokenToSecond = _paginationHelper.GetNextPageToken();

            // Go to 2nd page
            _paginationHelper.AddToken(tokenToThird);
            var actualTokenToThird = _paginationHelper.GetNextPageToken();

            // Go to 3rd page
            _paginationHelper.AddToken(tokenToFourth);
            var actualTokenToFourth = _paginationHelper.GetNextPageToken();

            // Go to 4th page
            _paginationHelper.AddToken(tokenToFifth);
            var actualTokenToFifth = _paginationHelper.GetNextPageToken();

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
            var previousToken = _paginationHelper.GetPreviousPageToken();

            // Assert
            Assert.Null(previousToken);
        }

        [Fact]
        public void GetPreviousPageToken_ReturnFromSecondToFirstPage_PreviousTokenIsNullForFirstPage()
        {
            // Arrange
            var tokenToSecond = new ContinuationToken();
            var tokenToThird = new ContinuationToken();

            _paginationHelper.AddToken(tokenToSecond);
            _paginationHelper.AddToken(tokenToThird);

            // Act
            var previousToken = _paginationHelper.GetPreviousPageToken();

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

            _paginationHelper.AddToken(tokenToSecond);
            _paginationHelper.AddToken(tokenToThird);
            _paginationHelper.AddToken(tokenToFourth);

            // Act
            var previousToken = _paginationHelper.GetPreviousPageToken();

            // Assert
            Assert.Equal(tokenToSecond, previousToken);
        }

        [Fact]
        public void AddToken_TokensAddedUpToFirstPage_LastPageIsFirstPage()
        {
            // Arrange
            var tokenToSecond = new ContinuationToken();

            // Act
            _paginationHelper.AddToken(tokenToSecond);

            // Assert
            var lastPage = _paginationHelper.ShoutsPages.Peek();

            Assert.Equal(0, lastPage.Index);
            Assert.Null(lastPage.PaginationToken);

            Assert.Equal(tokenToSecond, _paginationHelper.GetNextPageToken());
        }

        [Fact]
        public void AddToken_TokensAddedUpToSecondPage_LastPageIsSecondPage()
        {
            // Arrange
            var tokenToSecond = new ContinuationToken();
            var tokenToThird = new ContinuationToken();

            // Act
            _paginationHelper.AddToken(tokenToSecond);
            _paginationHelper.AddToken(tokenToThird);

            // Assert
            var lastPage = _paginationHelper.ShoutsPages.Peek();

            Assert.Equal(1, lastPage.Index);
            Assert.Equal(tokenToSecond, lastPage.PaginationToken);

            Assert.Equal(tokenToThird, _paginationHelper.GetNextPageToken());
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
            _paginationHelper.AddToken(tokenToSecond);
            _paginationHelper.AddToken(tokenToThird);
            _paginationHelper.AddToken(tokenToFourth);
            _paginationHelper.AddToken(tokenToFifth);
            _paginationHelper.AddToken(tokenFifth);

            // Go to 4th page
            var actualTokenToFourth1 = _paginationHelper.GetPreviousPageToken();

            // Go to 5th page
            var actualTokenToFifth = _paginationHelper.GetNextPageToken();
            _paginationHelper.AddToken(actualTokenToFifth);

            // Go to 4th page
            var actualTokenToFourth2 = _paginationHelper.GetPreviousPageToken();

            // Go to 3rd page
            var actualTokenToThird = _paginationHelper.GetPreviousPageToken();

            // Go to 2nd page
            var actualTokenToSecond = _paginationHelper.GetPreviousPageToken();

            // Go to 1st page
            var actualTokenToFirst = _paginationHelper.GetPreviousPageToken();

            // Assert
            Assert.Equal(tokenToFourth, actualTokenToFourth1);
            Assert.Equal(tokenToFifth, actualTokenToFifth);
            Assert.Equal(tokenToFourth, actualTokenToFourth2);
            Assert.Equal(tokenToThird, actualTokenToThird);
            Assert.Equal(tokenToSecond, actualTokenToSecond);
            Assert.Null(actualTokenToFirst);
        }

        [Fact]
        public void HasNextPage_ForSecondToLastPage_ReturnsTrue()
        {
            // Arrange
            var tokenOne = new ContinuationToken();
            var tokenTwo = new ContinuationToken();
            // var tokenThree = (ContinuationToken)null;

            _paginationHelper.AddToken(tokenOne);
            _paginationHelper.AddToken(tokenTwo);

            // Act
            var result = _paginationHelper.HasNextPage;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasNextPage_ForLastPage_ReturnsFalse()
        {
            // Arrange
            var tokenOne = new ContinuationToken();
            var tokenTwo = new ContinuationToken();
            var tokenThree = (ContinuationToken)null;

            _paginationHelper.AddToken(tokenOne);
            _paginationHelper.AddToken(tokenTwo);
            _paginationHelper.AddToken(tokenThree);

            // Act
            var result = _paginationHelper.HasNextPage;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasPreviousPage_ForFirstPage_ReturnsFalse()
        {
            // Arrange
            var tokenOne = new ContinuationToken();
            // var tokenTwo = new ContinuationToken();
            // var tokenThree = (ContinuationToken)null;

            _paginationHelper.AddToken(tokenOne);

            // Act
            var result = _paginationHelper.HasPreviousPage;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasPreviousPage_ForLastPage_ReturnsTrue()
        {
            // Arrange
            var tokenOne = new ContinuationToken();
            var tokenTwo = new ContinuationToken();
            var tokenThree = (ContinuationToken)null;

            _paginationHelper.AddToken(tokenOne);
            _paginationHelper.AddToken(tokenTwo);
            _paginationHelper.AddToken(tokenThree);

            // Act
            var result = _paginationHelper.HasPreviousPage;

            // Assert
            Assert.True(result);
        }
    }
}
