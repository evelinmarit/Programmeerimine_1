using KooliProjekt.Services;
using KooliProjekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using KooliProjekt.Search;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class ProductServiceTests : ServiceTestBase
    {
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _service = new ProductService(DbContext);
        }

        [Fact]
        public async Task List_ShouldReturnPagedResult()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            DbContext.Products.AddRange(
                new Product { Id = 1, Name = "Test Product 1", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 10 },
                new Product { Id = 2, Name = "Another Product", Description = "Test2", PhotoUrl = "http://example.com/phone.jpg", AtStock = false, Category = category, Price = 15 },
                new Product { Id = 3, Name = "Test Product 2", Description = "Test3", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 20 }
            );
            await DbContext.SaveChangesAsync();

            var service = new ProductService(DbContext);
            var search = new ProductSearch { Keyword = "Test" };

            // Act
            var result = await service.List(1, 10, search);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Results.Count);
            Assert.All(result.Results, p => Assert.Contains("Test", p.Name));
        }

        [Fact]
        public async Task Get_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            DbContext.Products.Add(new Product { Id = 1, Name = "Test Product", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 10 });
            await DbContext.SaveChangesAsync();

            var service = new ProductService(DbContext);

            // Act
            var result = await service.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Product", result.Name);
        }

        [Fact]
        public async Task Get_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var service = new ProductService(DbContext);

            // Act
            var result = await service.Get(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ListCategories_ShouldReturnAllCategories()
        {
            // Arrange
            DbContext.Categories.AddRange(
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            );
            await DbContext.SaveChangesAsync();

            var service = new ProductService(DbContext);

            // Act
            var result = await service.ListCategories();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Save_ShouldAddNewProduct_WhenIdIsZero()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            await DbContext.SaveChangesAsync();

            var service = new ProductService(DbContext);
            var product = new Product { Id = 0, Name = "New Product", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, CategoryId = 1, Price = 25 };

            // Act
            await service.Save(product);

            // Assert
            var savedProduct = DbContext.Products.FirstOrDefault(p => p.Name == "New Product");
            Assert.NotNull(savedProduct);
            Assert.Equal("New Product", savedProduct.Name);
            Assert.Equal(25, savedProduct.Price);
        }

        [Fact]
        public async Task Save_ShouldUpdateExistingProduct_WhenIdIsNotZero()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            var existingProduct = new Product { Id = 1, Name = "Old Product", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 10 };
            DbContext.Products.Add(existingProduct);
            await DbContext.SaveChangesAsync();

            var service = new ProductService(DbContext);
            existingProduct.Name = "Updated Product";
            existingProduct.Price = 20;

            // Act
            await service.Save(existingProduct);

            // Assert
            var updatedProduct = DbContext.Products.FirstOrDefault(p => p.Id == 1);
            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Product", updatedProduct.Name);
            Assert.Equal(20, updatedProduct.Price);
        }

        [Fact]
        public async Task List_ShouldFilterProductsByAtStock_WhenAtStockIsSpecified()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            DbContext.Categories.Add(category);
            var product1 = new Product { Id = 1, Name = "Painting A", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 100 };
            var product2 = new Product { Id = 2, Name = "Painting B", Description = "Test2", PhotoUrl = "http://example.com/phone.jpg", AtStock = false, Category = category, Price = 150 };
            var product3 = new Product { Id = 3, Name = "Painting C", Description = "Test3", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 200 };

            DbContext.Products.AddRange(product1, product2, product3);
            await DbContext.SaveChangesAsync();

            var search = new ProductSearch { AtStock = true };

            // Act
            var result = await _service.List(1, 10, search);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Results.Count); // Only product1 and product3 have AtStock = true
            Assert.All(result.Results, p => Assert.True(p.AtStock));
        }

        [Fact]
        public async Task Delete_should_remove_existing_product()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category 1" };
            var product = new Product { Name = "Test", Description = "Test1", PhotoUrl = "http://example.com/phone.jpg", AtStock = true, Category = category, Price = 10 };
            DbContext.Products.Add(product);
            DbContext.SaveChanges();

            // Act
            await _service.Delete(product.Id);

            // Assert
            var count = DbContext.Products.Count();
            Assert.Equal(0, count);
        }
        [Fact]
        public async Task Delete_should_return_if_product_was_not_found()
        {
            // Arrange
            var id = -100;

            // Act
            await _service.Delete(id);

            // Assert
            var count = DbContext.Products.Count();
            Assert.Equal(0, count);
        }
    }
}
