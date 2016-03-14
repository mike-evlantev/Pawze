using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pawze.API;
using Pawze.API.Controllers;
using Pawze.Core.Domain;
using Pawze.Core.Infrastructure;
using Pawze.Core.Models;
using Pawze.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;


namespace Pawze.Test.Controllers
{
    [TestClass]
    public class BoxesControllerTests
    {
        private Mock<IBoxRepository> _boxRepository = null;
        private Mock<IBoxItemRepository> _boxItemRepository = null;
        private Mock<IUnitOfWork> _unitOfWork = null;
        private Mock<IPawzeUserRepository> _pawzeUserRepository = null;
        private Mock<IPawzeConfigurationRepository> _pawzeConfigRepository = null;

        BoxesController controller = null;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            WebApiConfig.CreateMaps();

            _boxRepository = new Mock<IBoxRepository>();
            _boxRepository.Setup(b => b.GetWhere(It.IsAny<Expression<Func<Box, bool>>>())).Returns(new List<Box>
            {
                new Box { BoxId = 1, PawzeUserId = "test1", SubscriptionId = 1 },
                new Box { BoxId = 2, PawzeUserId = "test2", SubscriptionId = 2 }
            });

            // RESEARCH HOW TO DO THIS
            // Possible google terms
            // complex lambda expressions moq
            // testing lambda expressions in moq

            _pawzeConfigRepository = new Mock<IPawzeConfigurationRepository>();
            _boxItemRepository = new Mock<IBoxItemRepository>();

            _unitOfWork = new Mock<IUnitOfWork>();
            _pawzeUserRepository = new Mock<IPawzeUserRepository>();

            controller = new BoxesController(_boxRepository.Object, _pawzeConfigRepository.Object, _boxItemRepository.Object, _unitOfWork.Object, _pawzeUserRepository.Object);
        }

        [TestMethod]
        public void GetAllShouldReturnAll()
        {
            // Act
            var result = controller.GetBoxes();

            // Assert
            Assert.IsTrue(result.Count() == 2);
        }

        [TestMethod]
        public void GetBoxByIdShouldReturnSingleBox()
        {
            // Arrange
            _boxRepository.Setup(b => b.GetFirstOrDefault(It.IsAny<Expression<Func<Box, bool>>>()))
                         .Returns(new Box { BoxId = 1, PawzeUserId = "test1", SubscriptionId = 1 });

            BoxesModel expectedBox = new BoxesModel { BoxId = 1, PawzeUserId = "test1", SubscriptionId = 1 };

            // Act
            IHttpActionResult result = controller.GetBox(1);

            //Assert
            OkNegotiatedContentResult<BoxesModel> okResult = (OkNegotiatedContentResult<BoxesModel>)result;

            var actualBox = okResult.Content;

            Assert.IsTrue(actualBox.BoxId == expectedBox.BoxId);
        }

        [TestMethod]
        public void GetBoxByIdShouldFail()
        {
            // Act
            IHttpActionResult result = controller.GetBox(1);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetBoxItemsForBoxShouldReturnBoxItems()
        {
            // Arrange
            _boxItemRepository.Setup(b => b.GetWhere(It.IsAny<Expression<Func<BoxItem, bool>>>())).Returns(new List<BoxItem>
            {
                new BoxItem { BoxId = 1, BoxItemId = 1, InventoryId = 1},
                new BoxItem { BoxId = 1, BoxItemId = 2, InventoryId = 1},
                new BoxItem { BoxId = 1, BoxItemId = 3, InventoryId = 1},
                new BoxItem { BoxId = 1, BoxItemId = 4, InventoryId = 1}
               
            });

            // Act
            var boxItems = controller.GetBoxItemsForBox(1);

            // Assert 
            Assert.IsTrue(boxItems.Count() == 4);
        }

        [TestMethod()]
        public void PutBoxTest()
        {
            //Arrange
         
            BoxesModel box = new BoxesModel() { BoxId = 1, PawzeUserId = "test1", SubscriptionId = 1 };
             
            //Act
            //BoxesModel box = new BoxesModel();
            box.BoxId = 1;
            box.PawzeUserId = "test1";

            IHttpActionResult response = controller.PutBox(box.BoxId, box);
            StatusCodeResult statuscodeResult = response as StatusCodeResult;



            //Assert
            //Assert.IsNotNull(contentResult);
            //Assert.IsNotNull(contentResult.Content);
            //Assert.AreEqual(HttpStatusCode.NoContent, typeof(response.StatusCode));
            //Assert.IsInstanceOfType(response, typeof());
            //Assert.AreEqual<HttpStatusCode>(HttpStatusCode.NoContent, statusCodeResult.StatusCode);

        }

        [TestMethod]
        public void PostBoxShouldReturnSingleBox()
        {

            //Arrange
            BoxesModel box = new BoxesModel { BoxId = 1, PawzeUserId = "test1", SubscriptionId = 1 };

            //Act
            IHttpActionResult result = controller.PostBox(box);

            //Assert
            CreatedAtRouteNegotiatedContentResult<BoxesModel> okResult = (CreatedAtRouteNegotiatedContentResult<BoxesModel>)result;

            var actualBox = okResult.Content;

            // TODO: Cameron's last thought on testing
            // _boxRepository.Verify(e => e.)

            //Assert
            BoxesModel expected = new BoxesModel { BoxId = 1, PawzeUserId = "test1", SubscriptionId = 1 };
            Assert.AreEqual(expected.BoxId, actualBox.BoxId);
        }
    }
}
