using BackendShop.Core.Interfaces;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.Data.DataSeeder
{
    public static class DataSeederExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
                var imageHulk = scope.ServiceProvider.GetRequiredService<IImageHulk>();
                dbContext.Database.Migrate();

                // ✅ **Seed Categories**
                if (!dbContext.Categories.Any())
                {
                    int number = 5;
                    var faker = new Faker("uk");
                    var categories = new List<Category>();

                    for (int i = 0; i < number; i++)
                    {
                        string image = await imageHulk.Save("https://picsum.photos/1200/800?category");
                        var category = new Category
                        {
                            Name = faker.Commerce.Categories(1).First(),
                            Description = faker.Commerce.ProductDescription(),
                            ImageCategoryPath = image
                        };
                        category.GenerateSlug(); // 🟢 Додаємо генерацію slug
                        categories.Add(category);
                    }

                    dbContext.Categories.AddRange(categories);
                    await dbContext.SaveChangesAsync();
                }

                // ✅ **Seed SubCategories**
                if (!dbContext.SubCategories.Any())
                {
                    var categories = dbContext.Categories.ToList();
                    if (!categories.Any()) return;

                    var faker = new Faker("uk");
                    var subCategories = new List<SubCategory>();

                    for (int i = 0; i < 5; i++)
                    {
                        string image = await imageHulk.Save("https://picsum.photos/1200/800?subcategory");
                        var randomCategory = faker.PickRandom(categories);

                        var subCategory = new SubCategory
                        {
                            Name = faker.Commerce.Categories(1).First(),
                            Description = faker.Commerce.ProductDescription(),
                            ImageSubCategoryPath = image,
                            CategoryId = randomCategory.CategoryId
                        };
                        subCategory.GenerateSlug(); // 🟢 Додаємо генерацію slug
                        subCategories.Add(subCategory);
                    }

                    dbContext.SubCategories.AddRange(subCategories);
                    await dbContext.SaveChangesAsync();
                }

                // ✅ **Seed Products**
                if (!dbContext.Products.Any())
                {
                    var subcategories = dbContext.SubCategories.ToList();
                    if (!subcategories.Any()) return;

                    var faker = new Faker("uk");
                    var products = new List<ProductEntity>();

                    for (int i = 0; i < 5; i++)
                    {
                        var subCategory = faker.PickRandom(subcategories);

                        var product = new ProductEntity
                        {
                            Code = faker.Commerce.Ean13(),
                            Name = faker.Commerce.ProductName(),
                            Description = faker.Lorem.Paragraph(),
                            Manufacturer = faker.Company.CompanyName(),
                            Size = faker.Commerce.ProductAdjective(),
                            Color = faker.Commerce.Color(),
                            Type = faker.Commerce.Department(),
                            Form = faker.Commerce.ProductMaterial(),
                            Price = decimal.Parse(faker.Commerce.Price()),
                            QuantityInPack = faker.Random.Int(1, 10),
                            QuantityInStock = faker.Random.Int(0, 100),
                            Modeles = faker.Commerce.ProductMaterial(),
                            SubCategoryId = subCategory.SubCategoryId
                        };
                        product.GenerateSlug(); // 🟢 Додаємо генерацію slug
                        products.Add(product);
                    }

                    dbContext.Products.AddRange(products);
                    await dbContext.SaveChangesAsync();

                    // ✅ Додаємо зображення для кожного продукту
                    foreach (var product in products)
                    {
                        var images = new List<ProductImageEntity>();
                        for (int j = 0; j < 3; j++)
                        {
                            string imageUrl = $"https://picsum.photos/200/300?random={Guid.NewGuid()}";
                            images.Add(new ProductImageEntity
                            {
                                ProductId = product.Id,
                                Image = await imageHulk.Save(imageUrl),
                                Priority = j + 1
                            });
                        }

                        dbContext.ProductImages.AddRange(images);

                        // ✅ Додаємо описові зображення
                        for (int j = 0; j < 2; j++)
                        {
                            string descImageUrl = $"https://picsum.photos/400/300?random={Guid.NewGuid()}";
                            dbContext.ProductDescImages.Add(new ProductDescImageEntity
                            {
                                ProductId = product.Id,
                                Image = await imageHulk.Save(descImageUrl)
                            });
                        }
                    }

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
