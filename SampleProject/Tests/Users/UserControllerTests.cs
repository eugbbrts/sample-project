using Core.Services.Users;
using Moq;
using System.Web.Http;
using WebApi.Controllers;
using WebApi.Models.Users;

namespace Tests.Users
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<ICreateUserService> _mockCreateUserService;
        private Mock<IDeleteUserService> _mockDeleteUserService;
        private Mock<IGetUserService> _mockGetUserService;
        private Mock<IUpdateUserService> _mockUpdateUserService;

        private UserController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockCreateUserService = new Mock<ICreateUserService>();
            _mockDeleteUserService = new Mock<IDeleteUserService>();
            _mockGetUserService = new Mock<IGetUserService>();
            _mockUpdateUserService = new Mock<IUpdateUserService>();

            _controller = new UserController(_mockCreateUserService.Object, _mockDeleteUserService.Object, _mockGetUserService.Object, _mockUpdateUserService.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
            };
        }

        [TestMethod]
        public void Tasks_CreateUser_ExistingUser_Error()
        {
            var existingUserId = new Guid("1422740e-6426-4c46-8445-3f5274a62424");
            var existingUser = new BusinessEntities.User();
            existingUser.SetName("mary@companya.com");

            _mockGetUserService.Setup(s => s.GetUser(existingUserId)).Returns(existingUser);

            var model = new UserModel() { };
            var response = _controller.CreateUser(existingUserId, model);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.Conflict, "Incorrect http code");
            
            var objectContent = response.Content as ObjectContent<string>;
            Assert.IsTrue(objectContent?.Value?.ToString().Contains("already exists"), "Response content must be an ObjectContent type.");

        }
    }
}