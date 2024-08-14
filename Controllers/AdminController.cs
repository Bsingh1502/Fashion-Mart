using FashionMart.EntityFramework;
using FashionMart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FashionMart.Controllers
{
    /// <summary>
    /// Controller for managing admin-related actions
    /// </summary>
    public class AdminController : Controller
    {
        private readonly ICategoryRepository categoryrepository;
        private readonly Context database;

        public IProductRepository productrepository { get; }
        private readonly IWebHostEnvironment webEnvironment;

        /// <summary>
        /// Constructor to inject dependencies
        /// </summary>
        public AdminController(Context context, IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment hostEnvironment)
        {
            this.productrepository = productRepository;
            database = context;
            this.categoryrepository = categoryRepository;
            webEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Action method to display the admin index
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var categories = await categoryrepository.FetchCategoryDetail();
            var products = await productrepository.FetchProducts();
            var prod = new Models.UI.Product();
            prod.Products.AddRange(products);
            prod.Categories.AddRange(categories);
            return View("Admin", prod);
        }

        /// <summary>
        /// Action method to toggle edit state for products
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ToggleEdit(string EditAction, int ProductId, string Name, decimal Price)
        {
            var products = await productrepository.FetchProducts();
            var categories = await categoryrepository.FetchCategoryDetail();
            var prod = new Models.UI.Product();
            prod.Products.AddRange(products);
            prod.Categories.AddRange(categories);
            if (EditAction.StartsWith("Edit-"))
            {
                ViewBag.EditId = Convert.ToInt32(EditAction.Split("-")[1]);  // Start editing this product

                return View("Admin", prod);
            }
            else if (EditAction.StartsWith("Save-"))
            {
                var productId = Convert.ToInt32(EditAction.Split("-")[1]);
                var product = await this.productrepository.FetchById(productId);
                if (product != null)
                {
                    product.Name = Name;
                    product.Price = Price;
                    await this.productrepository.ModifyProduct(product);
                }
                return RedirectToAction("Index");
            }
            else if (EditAction.StartsWith("Delete-"))
            {
                var productId = Convert.ToInt32(EditAction.Split("-")[1]);
                await productrepository.RemoveProduct(productId);
                return RedirectToAction("Index");
            }
            else if (EditAction == "Cancel")
            {
                return RedirectToAction("Index");
            }

            return View("Admin", prod);
        }

        /// <summary>
        /// Action method to save a new product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProduct([Bind("Name,Price,ImageUrl, CategoryId")] Models.UI.Product product, IFormFile ImageUrl, int CategoryId)
        {
            ModelState.Remove("CategoryId");
            ModelState.Remove("NewCategory");
            ModelState.Remove("Categories");

            if (ModelState.IsValid)
            {
                Product productObj = new Product();
                if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var imagePath = Path.Combine(webEnvironment.WebRootPath, "images", ImageUrl.FileName);
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(fileStream);
                    }
                    productObj.Name = product.Name;
                    productObj.Price = product.Price;
                    productObj.ImageUrl = Path.Combine("images", ImageUrl.FileName);
                    productObj.CategoryId = product.CategoryId;
                }

                await this.productrepository.InsertProducts(productObj);

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(database.Categories, "CategoryId", "Name", product.CategoryId);
            return View("Admin", product);
        }
        /// Action method to update an existing product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Price,ImageUrl,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    database.Update(product);
                    await database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(database.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        /// <summary>
        /// Action method to display the edit product form
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || database.Products == null)
            {
                return NotFound();
            }

            var product = await database.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(database.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }



        /// <summary>
        /// Action method to display product details
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || database.Products == null)
            {
                return NotFound();
            }

            var product = await database.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        /// <summary>
        /// Action method to display the create product form
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(database.Categories, "CategoryId", "Name");
            return View();
        }

        /// <summary>
        /// Action method to confirm product deletion
        /// </summary>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (database.Products == null)
            {
                return Problem("Entity set 'DBContext.Products'  is null.");
            }
            var product = await database.Products.FindAsync(id);
            if (product != null)
            {
                database.Products.Remove(product);
            }

            await database.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks if a product exists by its ID
        /// </summary>
        /// <returns></returns>
        private bool ProductExists(int id)
        {
            return (database.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        /// <summary>
        /// Action method to save a new category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCategory([Bind("NewCategory")] Models.UI.Product product)
        {
            ModelState.Remove("Name");
            ModelState.Remove("ImageUrl");
            ModelState.Remove("Categories");

            if (ModelState.IsValid)
            {
                Category categoryObj = new Category();
                categoryObj.Name = product.NewCategory;
                await categoryrepository.InsertCategory(categoryObj);
                return RedirectToAction(nameof(Index));
            }
            return View("Admin", product);
        }

        /// <summary>
        /// Action method to delete a product
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await productrepository.RemoveProduct(id);

            return RedirectToAction(nameof(Index));
        }

       
        /// <summary>
    

    }
}
