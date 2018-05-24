using AllianceIntranet.Controllers;
using System;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace XUnitTestIntranet
{
    public class HomeControllerTests
    {
        public HomeControllerTests()
        {

        }

        [Fact(DisplayName = "Index should return index")]
        public void Index_should_return_default()
        {
            var controller = new HomeController();
            var viewResult = (ViewResult)controller.Index();
            var viewName = viewResult.ViewName;

            Assert.True(string.IsNullOrEmpty(viewName) || viewName == "Index");
        }

        [Fact(DisplayName = "AddClass should return class page")]
        public void Class_Should_Return_Class ()
        {
            var controller = new HomeController();
            var viewResult = (ViewResult)controller.AddClass();
            var viewName = viewResult.ViewName;

            Assert.True(viewName == "AddClass");
        }
    }
}
