using BusinessEntities;
using Core.Services.Users;
using Moq;
using System.Net;
using System.Reflection;
using System.Web.Http;
using WebApi.Controllers;
using WebApi.Models.Users;

namespace Tests.Users
{
    [TestClass]
    public class UserControllerTests: BaseTest
    {
        private Mock<ICreateUserService> _mockCreateUserService;
        private Mock<IDeleteUserService> _mockDeleteUserService;
        private Mock<IGetUserService> _mockGetUserService;
        private IUpdateUserService _updateUserService;

        private UserController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockCreateUserService = new Mock<ICreateUserService>();
            _mockDeleteUserService = new Mock<IDeleteUserService>();
            _mockGetUserService = new Mock<IGetUserService>();
            _updateUserService = new UpdateUserService();

            _controller = new UserController(_mockCreateUserService.Object, _mockDeleteUserService.Object, _mockGetUserService.Object, _updateUserService)
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

        [TestMethod]
        public void Tasks_UpdateUser_A_Error()
        {
            var existingUserId = new Guid("9422740e-6426-4c46-8445-3f5274a62424");

            var existingUser = new BusinessEntities.User(); 
            existingUser.SetName("Mark Williams (updated)");
            existingUser.SetType(BusinessEntities.UserTypes.Employee);
            existingUser.SetAge(29);
            existingUser.SetMonthlySalary(80000 / 12);
            existingUser.SetTags(["A", "C", "D"]);

            _mockGetUserService.Setup(s => s.GetUser(existingUserId)).Returns(existingUser);            

            var model = new UserModel() {
                Name = "Mark Williams (updated)",
                Email = null, // test comments: as in the postman request
                Type = UserTypes.Employee,
                Age = 29,
                AnnualSalary = 80000,
                Tags = ["A", "C", "D"],
            };

            var response = _controller.UpdateUser(existingUserId, model);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest, $"Incorrect http code. Expected 400 (BadRequest), got {response.StatusCode}");
            var objectContent = response.Content as ObjectContent<string[]>;
            Assert.IsNotNull(objectContent, "Response must be not null");

            var errorMessages = objectContent.Value as string[];

            Assert.IsTrue(errorMessages.Any(a => a.Contains("Email was not provided")), "Response content must be an ObjectContent type.");
        }
    }
}