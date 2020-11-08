using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductOrders;
using ProductOrders.Controllers;
using ProductOrders.Models;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetProduct()
        {
            var controller = new ValuesController();
            IHttpActionResult actionResult = controller.GetProduct(1);
            var contentResult = actionResult as OkNegotiatedContentResult<ProductDTO>;
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.ID);
        }
        [TestMethod]
        public void TestInvalidProduct()
        {
            var controller = new ValuesController();
            IHttpActionResult actionResult = controller.GetProduct(10);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
        }
      
        [TestMethod]
        public void TestInsertInvalidProduct()
        {
            var controller = new ValuesController();
            ProductDTO product =null;
            IHttpActionResult actionResult = controller.PostAddNewProduct(product);
            var createdResult = actionResult as BadRequestErrorMessageResult;
            Assert.IsNotNull(createdResult);
        }
        [TestMethod]
        public void TestInsertProduct()
        {
            var controller = new ValuesController();
            ProductDTO product = new ProductDTO()
            {
                Name = "Cool Drinks",
                Price = 12
            };
            IHttpActionResult actionResult = controller.PostAddNewProduct(product);
            var createdResult = actionResult as OkResult;
            Assert.IsNotNull(createdResult);
        }
        [TestMethod]
        public void TesOrderItem()
        {
            var controller = new OrderController();
            OrderDTO orderItem = new OrderDTO()
            {
                ProductID = 1,
                Quantity = 100,
                Date = DateTime.Now
            };
            IHttpActionResult actionResult = controller.PostNewOrder(orderItem);
            var createdResult = actionResult as BadRequestErrorMessageResult;
            Assert.IsNotNull(createdResult);
        }
    }
}
