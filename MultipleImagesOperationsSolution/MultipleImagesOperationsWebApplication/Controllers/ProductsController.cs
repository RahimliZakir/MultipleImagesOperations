using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultipleImagesOperationsWebApplication.Models.DataContext;
using MultipleImagesOperationsWebApplication.Models.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleImagesOperationsWebApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MultipleDbContext db;
        readonly IWebHostEnvironment env;
        public ProductsController(MultipleDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var multipleDbContext = db.Products.Include(p => p.Category).Include(i => i.Images);
            return View(await multipleDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products
                .Include(p => p.Category)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Files, Name,ImagePath,ShortDescription,CategoryId,Id,CreatedDate,UpdatedDate,DeletedDate")] Product product)
        {
            product.Images = new List<Images>();

            foreach (var item in product.Files)
            {
                var ext = Path.GetExtension(item.File.FileName);
                var fileName = $"prod-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                var fullPath = Path.Combine(env.WebRootPath, "uploads", "product-images", fileName);

                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    item.File.CopyTo(fs);
                }

                product.Images.Add(new Images
                {
                    IsMain = item.IsMain,
                    ImagePath = fileName
                });
            }

            if (ModelState.IsValid)
            {
                db.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lazy - loading

            var product = await db.Products
                .Include(i => i.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name, Files,ShortDescription,CategoryId,Id,CreatedDate,UpdatedDate,DeletedDate")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.Images = new List<Images>();

                    var entity = db.Products.AsNoTracking().Where(p => p.Id == id);

                    if (entity == null)
                    {
                        return NotFound();
                    }

                    var images = db.Images.Where(i => i.ProductId == id).ToList();

                    foreach (var item in images)
                    {
                        if (product.Files.Any(f => f.File == null && string.IsNullOrWhiteSpace(f.TempPath)
                        && f.Id == item.Id))
                        {
                            db.Images.Remove(item);

                            var fullPath = Path.Combine(env.WebRootPath, "uploads", "product-images", item.ImagePath);

                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                        else if (product.Files.Any(i => i.Id == item.Id && i.IsMain == true))
                        {
                            item.IsMain = true;
                        }
                        else
                        {
                            item.IsMain = false;
                        }
                    }

                    foreach (var item in product.Files.Where(f => f.File != null))
                    {
                        var ext = Path.GetExtension(item.File.FileName);
                        var fileName = $"prod-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                        var fullPath = Path.Combine(env.WebRootPath, "uploads", "product-images", fileName);

                        using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                        {
                            item.File.CopyTo(fs);
                        }

                        product.Images.Add(new Images
                        {
                            IsMain = item.IsMain,
                            ImagePath = fileName
                        });
                    }

                    product.UpdatedDate = DateTime.UtcNow.AddHours(4);

                    db.Update(product);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return db.Products.Any(e => e.Id == id);
        }
    }
}
