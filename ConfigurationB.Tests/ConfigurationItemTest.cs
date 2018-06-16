using ConfigurationB.Management.Entities;
using ConfigurationB.Management.Repositories;
using ConfigurationB.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConfigurationB.Tests
{
    public class ConfigurationItemTest
    {
        private async Task<List<ConfigurationItem>> GetTestConfigureItemCollection()
        {
            return new List<ConfigurationItem>(){
                new ConfigurationItem() {ApplicationName="SERVICE-A"}
            };
        }

        [Fact]
        public async Task ReturnsDinnersInViewModel()
        {
            var mockRepository = new Mock<IAsyncRepository<ConfigurationItem>>();
            mockRepository.Setup(r =>
              r.ListAllAsync()).Returns(GetTestConfigureItemCollection());

            var controller = new HomeController(mockRepository.Object);
            var result = controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<List<ConfigurationItem>>(viewResult.ViewData.Model).ToList();


            Assert.Equal("SERVICE-A", viewModel.First().ApplicationName);
            Assert.Equal(2, viewModel.Count);
        }
    }
}
