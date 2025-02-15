using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class CategoryServiceTests : ServiceTestBase
    {
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _service = new CategoryService(DbContext);
        }

        [Fact]
        public async Task List_ShouldReturnPagedResult()
        {
            // Arrange
            await DbContext.Categories.AddRangeAsync(
                new Category { Name = "Category 1" },
                new Category { Name = "Category 2" },
                new Category { Name = "Category 3" }
            );
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.List(1, 2); // Page 1, 2 items per page

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Results.Count);  // Kontrollib, kas leheküljel on 2 elementi
            Assert.Equal(3, result.RowCount);       // Kontrollib, kas kõikide elementide koguarv on 3
            Assert.Equal(2, result.PageCount);      // Kontrollib, et lehekülgede arv oleks korrektne
            Assert.Equal(1, result.CurrentPage);    // Kontrollib, et hetkelehekülg on 1
            Assert.Contains(result.Results, r => r.Name == "Category 1");
            Assert.Contains(result.Results, r => r.Name == "Category 2");
        }

        [Fact]
        public async Task Get_ShouldReturnCategoryWithProducts()
        {
            // Arrange
            var category = new Category
            {
                Name = "Electronics",
                Products = new List<Product>
        {
            new Product { Name = "Phone", Description = "Smartphone", PhotoUrl = "http://example.com/phone.jpg", Price = 699 },
            new Product { Name = "Laptop", Description = "Gaming Laptop", PhotoUrl = "http://example.com/laptop.jpg", Price = 1200 }
        }
            };

            DbContext.Categories.Add(category);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await _service.Get(category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Name, result.Name);
            Assert.Equal(2, result.Products.Count);
            Assert.Contains(result.Products, p => p.Name == "Phone");
            Assert.Contains(result.Products, p => p.Name == "Laptop");
        }


        [Fact]
        public async Task Save_ShouldAddNewCategory()
        {
            // Arrange
            var category = new Category { Name = "New Category" };

            // Act
            await _service.Save(category);
            var savedCategory = await DbContext.Categories.FirstOrDefaultAsync(c => c.Name == "New Category");

            // Assert
            Assert.NotNull(savedCategory);
            Assert.Equal("New Category", savedCategory.Name);
        }

        [Fact]
        public async Task Save_ShouldUpdateExistingCategory()
        {
            // Arrange
            var category = new Category { Name = "Old Name" };
            await DbContext.Categories.AddAsync(category);
            await DbContext.SaveChangesAsync();

            // Act
            category.Name = "Updated Name";
            await _service.Save(category);
            var updatedCategory = await DbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

            // Assert
            Assert.NotNull(updatedCategory);
            Assert.Equal("Updated Name", updatedCategory.Name);
        }

        [Fact]
        public async Task Delete_should_remove_existing_category()
        {
            // Arrange
            var category = new Category { Name = "Test" };
            DbContext.Categories.Add(category);
            DbContext.SaveChanges();

            // Act
            await _service.Delete(category.Id);

            // Assert
            var count = DbContext.Categories.Count();
            Assert.Equal(0, count);
        }
        [Fact]
        public async Task Delete_should_return_if_category_was_not_found()
        {
            // Arrange
            var id = -100;

            // Act
            await _service.Delete(id);

            // Assert
            var count = DbContext.Categories.Count();
            Assert.Equal(0, count);
        }
    }
}
