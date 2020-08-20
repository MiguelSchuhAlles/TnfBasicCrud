using TnfBasicCrud.Application.Services.Interfaces;
using TnfBasicCrud.Domain;
using TnfBasicCrud.Domain.Entities;
using TnfBasicCrud.Common.Product;
using TnfBasicCrud.Infra.Context;
using TnfBasicCrud.API.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tnf;
using Tnf.Application.Services;
using Tnf.AspNetCore.Mvc.Response;
using Tnf.AspNetCore.TestBase;
using Tnf.Domain.Services;
using Tnf.Dto;
using Tnf.EntityFrameworkCore;
using Tnf.Notifications;
using Xunit;

namespace TnfBasicCrud.API.Tests
{
    public class ProductIntegratedTest : TnfAspNetCoreIntegratedTestBase<StartupIntegratedTest>
    {
        public ProductIntegratedTest()
        {
            var notificationHandler = new NotificationHandler(ServiceProvider);

            SetRequestCulture(CultureInfo.GetCultureInfo("pt-BR"));

            ServiceProvider.UsingDbContext<TnfBasicCrudContext>(context =>
            {
                context.Products.Add(Product.Create(notificationHandler)
                    .WithId(ProductAppServiceMock.productGuid)
                    .WithDescription("Product A")
                    .WithValue(1)
                    .Build());

                for (var i = 2; i < 21; i++)
                    context.Products.Add(Product.Create(notificationHandler)
                        .WithId(Guid.NewGuid())
                        .WithDescription($"Product {NumberToAlphabetLetter(i, true)}")
                        .WithValue(1)
                        .Build());

                context.SaveChanges();
            });
        }

        private string NumberToAlphabetLetter(int number, bool isCaps)
        {
            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));
            return c.ToString();
        }

        [Fact]
        public void Should_Resolve_All()
        {
            TnfSession.ShouldNotBeNull();
            ServiceProvider.GetService<IProductAppService>().ShouldNotBeNull();
            ServiceProvider.GetService<IDomainService<Product>>().ShouldNotBeNull();
        }


        [Fact]
        public async Task Should_GetAll_With_Paginated()
        {
            // Act
            var response = await GetResponseAsObjectAsync<ListDto<ProductDto>>("api/products?pageSize=15");

            // Assert
            Assert.True(response.HasNext);
            Assert.Equal(15, response.Items.Count);
        }

        [Fact]
        public async Task Should_GetAll_With_Paginated_All_Items()
        {
            // Act
            var response = await GetResponseAsObjectAsync<ListDto<ProductDto>>($"api/products?pageSize=30");

            // Assert
            Assert.False(response.HasNext);
            Assert.Equal(20, response.Items.Count);
        }

        [Fact]
        public async Task Should_GetAll_Sorted()
        {
            // Act
            var response = await GetResponseAsObjectAsync<ListDto<ProductDto>>(
                $"api/products?pageSize=20&order=-description"
            );

            // Assert
            Assert.Equal(20, response.Items.Count);
            Assert.Equal("Product T", response.Items[0].Description);
            Assert.Equal("Product A", response.Items.Last().Description);
        }

        [Fact]
        public async Task Should_GetAll_By_Name()
        {
            // Act
            var response = await GetResponseAsObjectAsync<ListDto<ProductDto>>($"api/products?pageSize=20&name=product%20");

            // Assert
            Assert.Equal(20, response.Items.Count);
            Assert.All(response.Items, p => p.Description.Contains("product "));
        }

        [Fact]
        public async Task Should_Get_By_Name()
        {
            // Act
            var response = await GetResponseAsObjectAsync<ListDto<ProductDto>>(
                $"api/products?description=Product%20C"
            );

            // Assert
            Assert.Equal(1, response.Items.Count);
            Assert.Equal("Product C", response.Items[0].Description);
        }


        [Fact]
        public async Task Should_Get_product()
        {
            // Act
            var product = await GetResponseAsObjectAsync<ProductDto>(
                $"api/products/{ProductAppServiceMock.productGuid}"
            );

            // Assert
            Assert.Equal(product.Id, ProductAppServiceMock.productGuid);
            Assert.Equal("Product A", product.Description);
        }

        [Fact]
        public async Task Should_Get_product_Select_Fields()
        {
            // Act
            var product = await GetResponseAsObjectAsync<ProductDto>(
                $"api/products/{ProductAppServiceMock.productGuid}?fields=description"
            );

            // Assert
            Assert.Equal(product.Id, Guid.Empty);
            Assert.Equal("Product A", product.Description);
        }

        [Fact]
        public async Task Should_Raise_Notification_On_Get_Null()
        {
            // Act
            var response = await GetResponseAsObjectAsync<ErrorResponse>(
                $"api/products/{Guid.Empty}",
                HttpStatusCode.BadRequest
            );

            // Assert
            Assert.NotNull(response);
            Assert.Equal(1, response.Details.Count);

            var message = string.Format(GetLocalizedString(TnfConsts.LocalizationSourceName, TnfController.Error.AspNetCoreOnGetError), "Product");
            Assert.Equal(message, response.Message);

            message = string.Format(GetLocalizedString(TnfConsts.LocalizationSourceName, ApplicationService.Error.ApplicationServiceOnInvalidIdError), "id");
            Assert.Contains(response.Details, n => n.Message == message);
        }


        [Fact]
        public async Task Should_Create_product()
        {
            // Act
            var product = await PostResponseAsObjectAsync<ProductDto, ProductDto>(
                "api/products",
                new ProductDto() { Description = "Product @", Value = 1 }
            );

            var response = await GetResponseAsObjectAsync<ListDto<ProductDto>>(
                $"api/products?pageSize=30"
            );

            // Assert
            Assert.NotNull(product);
            Assert.Equal(21, response.Items.Count);
        }

        [Fact]
        public async Task Should_Raise_Notification_On_Create_With_Specifications()
        {
            // Act
            var response = await PostResponseAsObjectAsync<ProductDto, ErrorResponse>(
                $"api/products",
                new ProductDto(),
                HttpStatusCode.BadRequest
            );

            // Assert
            Assert.NotNull(response);
            Assert.Equal(2, response.Details.Count);

            var message = string.Format(GetLocalizedString(TnfConsts.LocalizationSourceName, TnfController.Error.AspNetCoreOnPostError), "Product");
            Assert.Equal(message, response.Message);

            message = GetLocalizedString(Constants.LocalizationSourceName, Product.Error.ProductShouldHaveDescription);
            Assert.Contains(response.Details, n => n.Message == message);
        }


        [Fact]
        public async Task Should_Update_product()
        {
            // Act
            var result = await PutResponseAsObjectAsync<ProductDto, ProductDto>(
                $"api/products/{ProductAppServiceMock.productGuid}",
                new ProductDto() { Description = "Product @", Value = 1 }
            );

            var product = await GetResponseAsObjectAsync<ProductDto>(
                $"api/products/{ProductAppServiceMock.productGuid}"
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, ProductAppServiceMock.productGuid);
            Assert.Equal("Product @", product.Description);
        }

        [Fact]
        public async Task Should_Raise_Notification_On_Update_With_Specifications()
        {
            // Act
            var response = await PutResponseAsObjectAsync<ProductDto, ErrorResponse>(
                $"api/products/{ProductAppServiceMock.productGuid}",
                new ProductDto(),
                HttpStatusCode.BadRequest
            );

            // Assert
            Assert.NotNull(response);
            Assert.Equal(2, response.Details.Count);

            var message = string.Format(GetLocalizedString(TnfConsts.LocalizationSourceName, TnfController.Error.AspNetCoreOnPutError), "Product");
            Assert.Equal(message, response.Message);

            message = GetLocalizedString(Constants.LocalizationSourceName, Product.Error.ProductShouldHaveDescription);
            Assert.Contains(response.Details, n => n.Message == message);
        }


        [Fact]
        public async Task Should_Delete_product()
        {
            // Act
            await DeleteResponseAsync(
                $"api/products/{ProductAppServiceMock.productGuid}"
            );

            var response = await GetResponseAsObjectAsync<ListDto<ProductDto>>(
                $"api/products?pageSize=30"
            );

            // Assert
            Assert.Equal(19, response.Items.Count);
        }

        [Fact]
        public async Task Should_Raise_Notification_On_Delete()
        {
            // Act
            var response = await DeleteResponseAsObjectAsync<ErrorResponse>(
                $"api/products/{Guid.Empty}",
                HttpStatusCode.BadRequest
            );

            // Assert
            Assert.NotNull(response);
            Assert.Equal(1, response.Details.Count);

            var message = string.Format(GetLocalizedString(TnfConsts.LocalizationSourceName, TnfController.Error.AspNetCoreOnDeleteError), "Product");
            Assert.Equal(message, response.Message);

            message = string.Format(GetLocalizedString(TnfConsts.LocalizationSourceName, ApplicationService.Error.ApplicationServiceOnInvalidIdError), "id");
            Assert.Contains(response.Details, n => n.Message == message);
        }

        [Fact]
        public async Task Should_Return_Null_On_Delete_NotFound()
        {
            // Act
            var response = await DeleteResponseAsObjectAsync<ProductDto>(
                $"api/products/{Guid.NewGuid()}"
            );

            // Assert
            Assert.Null(response);
        }
    }
}
