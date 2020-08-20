using TnfBasicCrud.API.Controllers;
using Shouldly;
using System;
using System.Threading.Tasks;
using Tnf.AspNetCore.TestBase;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using TnfBasicCrud.Application.Services.Interfaces;
using Tnf.Dto;
using TnfBasicCrud.Common.Customer;
using TnfBasicCrud.API.Tests.Mocks;
using System.Net;

namespace TnfBasicCrud.API.Tests
{
    public class CustomerControllerTests : TnfAspNetCoreIntegratedTestBase<StartupControllerTest>
    {
        [Fact]
        public void Should_Resolve_All()
        {
            TnfSession.ShouldNotBeNull();
            ServiceProvider.GetService<ICustomerAppService>().ShouldNotBeNull();
        }


        [Fact]
        public async Task Should_GetAll()
        {
            // Act
            var response = await GetResponseAsObjectAsync<ListDto<CustomerDto>>(
                WebConstants.CustomerRouteName
            );

            // Assert
            Assert.False(response.HasNext);
            Assert.Equal(3, response.Items.Count);
        }


        [Fact]
        public async Task Should_Get_Customer()
        {
            // Act
            var customer = await GetResponseAsObjectAsync<CustomerDto>(
                $"api/customers/{CustomerAppServiceMock.customerGuid}"
            );

            // Assert
            Assert.Equal(customer.Id, CustomerAppServiceMock.customerGuid);
            Assert.Equal("Customer A", customer.Name);
        }


        [Fact]
        public async Task Should_Create_Customer()
        {
            // Act
            var customer = await PostResponseAsObjectAsync<CustomerDto, CustomerDto>(
                "api/customers",
                new CustomerDto() { Name = "Customer @" }
            );

            // Assert
            Assert.NotNull(customer);
        }


        [Fact]
        public async Task Should_Update_Customer()
        {
            // Act
            var customer = await PutResponseAsObjectAsync<CustomerDto, CustomerDto>(
                $"api/customers/{CustomerAppServiceMock.customerGuid}",
                new CustomerDto() { Name = "Customer @" }
            );

            // Assert
            Assert.Equal(CustomerAppServiceMock.customerGuid, customer.Id);
            Assert.Equal("Customer @", customer.Name);
        }


        [Fact]
        public Task Should_Delete_Customer()
        {
            // Act
            return DeleteResponseAsync(
                $"api/customers/{CustomerAppServiceMock.customerGuid}"
            );
        }
    }
}
