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
    public class ApiControllerTest
    {

        [Fact]
        public async Task BadRequestTest()
        {
            using var mock = AutoMock.GetLoose();
            var twitterConnect = mock.Mock<ITwitterConnect>();
            var apiController = mock.Create<ApiController>();
            var request = new Models.Tags()
            {
                Name = null,
                IsKMP = null
            };
            apiController.ModelState.AddModelError("Name", "Name is Required");
            var response = await apiController.Index(request);
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async Task NoDataReturn()
        {
            using var mock = AutoMock.GetLoose();
            var twitterConnect = mock.Mock<ITwitterConnect>();
            var apiController = mock.Create<ApiController>();
            var request = new Models.Tags()
            {
                Name = "Hello",
                IsKMP = true
            };
            var responseData = new TweetResponse()
            {
                Count = 0
            };
            twitterConnect.Setup(x => x.ProcessTag(request)).Returns(Task.FromResult(responseData));
            var response = await apiController.Index(request);
            var okResult = Assert.IsType<OkObjectResult>(response);
            var emptyResult = new
            {
                status = 200,
                message = "Empty Result",
                result = new List<string>()
            };
            Assert.NotStrictEqual(emptyResult, okResult.Value);
        }

        [Fact]
        public async Task HaveDataReturn()
        {
            using var mock = AutoMock.GetLoose();
            var twitterConnect = mock.Mock<ITwitterConnect>();
            var apiController = mock.Create<ApiController>();
            var request = new Models.Tags()
            {
                Name = "Hello",
                IsKMP = true
            };
            var responseData = new TweetResponse()
            {
                Count = 1,
                Data = new TweetResult
                {
                    Query = new List<IQueryCategory>()
                }
            };
            twitterConnect.Setup(x => x.ProcessTag(request)).Returns(Task.FromResult(responseData));
            var response = await apiController.Index(request);
            var okResult = Assert.IsType<OkObjectResult>(response);
            var emptyResult = new
            {
                status = 200,
                message = "Success",
                result = responseData
            };
            Assert.NotStrictEqual(emptyResult, okResult.Value);
        }
    }
}
