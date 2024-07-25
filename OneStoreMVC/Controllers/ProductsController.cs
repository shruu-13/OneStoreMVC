using Microsoft.AspNetCore.Mvc;
using OneStoreMVC.Models;
using OneStoreMVC.Services;
using System.Security.Cryptography.Xml;

namespace OneStoreMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public string newFileName { get; private set; }

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p => p.Id).ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            // Save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/IMAGES/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }

            // Save new product in the database

            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Catagory = productDto.Catagory,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageFileName = newFileName,
                CreateAt = DateTime.Now,
            };

            context.Products.Add(product);
            context.SaveChanges();




            return RedirectToAction("Index", "Products");
        }

        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            // create productDto from product

            var productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Catagory = product.Catagory,
                Price = product.Price,
                Description = product.Description,

            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageFileName;
            ViewData["CreatedAt"] = product.CreateAt.ToString("MM/dd/yyyy");

            return View(productDto);

        }


        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;
                ViewData["CreatedAt"] = product.CreateAt.ToString("MM/dd/yyyy");

                return View(productDto);
            }

            // update the image file if new image file

            string newFileIName = product.ImageFileName;
            if (productDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyy/MMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImageFile.FileName);

                string imageFullPath = environment.WebRootPath + "/IMAGES/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);
                }



                // delete the old image

                string oldImageFullPath = environment.WebRootPath + "/IMAGES/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);

            }

            // update the product in the database

            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.Catagory = productDto.Catagory;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.ImageFileName = newFileIName;

            context.SaveChanges();

            return RedirectToAction("Index", "Products");

        }

        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            string imageFullPath = environment.WebRootPath + "/IMAGES/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Products");
        }
    }
}
