using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultipleImagesOperationsWebApplication.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleImagesOperationsWebApplication.Models.DataContext
{
    public static class DataSeeding
    {
        public static IApplicationBuilder DataSeed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MultipleDbContext>();

                db.Database.Migrate();

                //if (!db.ProductImages.Any())
                //{

                //}

                if (!db.Categories.Any())
                {
                    db.Categories.Add(new Category
                    {
                        Name = "Fruits"
                    });

                    db.Categories.Add(new Category
                    {
                        Name = "Vegetables"
                    });

                    db.Categories.Add(new Category
                    {
                        Name = "Meat and meat products"
                    });

                    db.Categories.Add(new Category
                    {
                        Name = "Dairy products"
                    });

                    db.SaveChanges();
                }

                if (!db.Products.Any())
                {
                    db.Products.Add(new Product
                    {
                        Name = "Chicken meat",
                        ShortDescription = "Chicken is the most common type of poultry in the world. Owing to the relative ease and low cost of raising them in comparison to animals such as cattle or hogs, chickens have become prevalent throughout the cuisine of cultures around the world, and their meat has been variously adapted to regional tastes.",
                        ImagePath = "chicken-meat.jpg",
                        CategoryId = 3
                    });

                    db.Products.Add(new Product
                    {
                        Name = "Pear",
                        ShortDescription = "Pears are fruits produced and consumed around the world, growing on a tree and harvested in late Summer into October. The pear tree and shrub are a species of genus Pyrus, in the family Rosaceae, bearing the pomaceous fruit of the same name. Several species of pears are valued for their edible fruit and juices, while others are cultivated as trees.",
                        ImagePath = "pear.jpg",
                        CategoryId = 1
                    });

                    db.Products.Add(new Product
                    {
                        Name = "Onion",
                        ShortDescription = "The onion, also known as the bulb onion or common onion, is a vegetable that is the most widely cultivated species of the genus Allium. Its close relatives include the garlic, scallion, shallot, leek, chive, and Chinese onion.",
                        ImagePath = "onion.jpg",
                        CategoryId = 2
                    });

                    db.Products.Add(new Product
                    {
                        Name = "Milk",
                        ShortDescription = "Milk is a nutrient-rich liquid food produced by the mammary glands of mammals. It is the primary source of nutrition for young mammals, including breastfed human infants before they are able to digest solid food. Early-lactation milk is called colostrum, which contains antibodies that strengthen the immune system and thus reduces the risk of many diseases.",
                        ImagePath = "milk.jpg",
                        CategoryId = 4
                    });

                    db.SaveChanges();
                }
            }

            return app;
        }
    }
}
