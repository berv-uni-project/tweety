using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TweetyCore.Controllers;
using TweetyCore.Models;
using TweetyCore.Utils.Twitter;
using Xunit;

namespace TweetyCore.Test.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void CheckReturnTypeIndex()
        {
            using var mock = AutoMock.GetLoose();
            var homeController = mock.Create<HomeController>();
            var returnData = homeController.Index();
            Assert.IsType<ViewResult>(returnData);
        }

        [Fact]
        public void CheckReturnTypeAbout()
        {
            using var mock = AutoMock.GetLoose();
            var homeController = mock.Create<HomeController>();
            var returnData = homeController.About();
            Assert.IsType<ViewResult>(returnData);
        }

        [Fact]
        public async Task BadRequestIndexPostTest()
        {
            using var mock = AutoMock.GetLoose();
            var twitterConnect = mock.Mock<ITwitterConnect>();
            var homeController = mock.Create<HomeController>();
            var request = new Models.Tags()
            {
                Name = null,
                IsKMP = null
            };
            homeController.ModelState.AddModelError("Name", "Name is Required");
            var response = await homeController.Index(twitterConnect.Object, request);
            var viewResult = Assert.IsType<ViewResult>(response);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async Task CheckReturnTypeIndexPostNotGettingResult()
        {
            using var mock = AutoMock.GetLoose();
            var mockTwitter = mock.Mock<ITwitterConnect>();
            var tags = new Models.Tags
            {
                IsKMP = true,
                Name = "Yup"
            };
            var resultData = new TweetResponse
            {
                Count = 0
            };
            mockTwitter.Setup(x => x.ProcessTag(tags)).Returns(Task.FromResult(resultData));
            var homeController = mock.Create<HomeController>();
            var returnData = await homeController.Index(mockTwitter.Object, tags);
            var resultView = Assert.IsType<ViewResult>(returnData);
            Assert.Equal("NoResult", resultView.ViewName);
        }

        [Fact]
        public async Task CheckReturnTypeIndexPostGettingSomeResult()
        {
            using var mock = AutoMock.GetLoose();
            var mockTwitter = mock.Mock<ITwitterConnect>();
            var tags = new Models.Tags
            {
                IsKMP = true,
                Name = "Yup"
            };
            var resultData = new TweetResponse
            {
                Count = 1,
                Data = new TweetResult
                {
                    Query = new List<IQueryCategory>()
                }
            };
            mockTwitter.Setup(x => x.ProcessTag(tags)).Returns(Task.FromResult(resultData));
            var homeController = mock.Create<HomeController>();
            var returnData = await homeController.Index(mockTwitter.Object, tags);
            var resultView = Assert.IsType<ViewResult>(returnData);
            Assert.Equal("ShowResult", resultView.ViewName);
            Assert.Equal(resultData.Data, resultView.Model);
        }
    }
}
