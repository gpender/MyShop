using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepository context;
        ProductCategoryRepository productCategories;
        public ProductManagerController()
        {
            context = new ProductRepository();
            productCategories = new ProductCategoryRepository();
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }
        public ActionResult Create()
        {
            ProductManagerViewModel vm = new ProductManagerViewModel();
            vm.Product = new Product();
            vm.ProductCategories = productCategories.Collection();
            return View(vm);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            context.Insert(product);
            context.Commit();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Edit(string id)
        {
            Product product = context.Find(id);
            ProductManagerViewModel vm = new ProductManagerViewModel();
            vm.Product = product;
            vm.ProductCategories = productCategories.Collection();
            return View(vm);
        }
        [HttpPost]
        public ActionResult Edit(Product product, string id)
        {
            Product productToUpdate = context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            productToUpdate.Category = product.Category;
            productToUpdate.Description = product.Description;
            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;
            productToUpdate.Image = product.Image;
            context.Commit();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(string id)
        {
            Product productToDelete = context.Find(id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            return View(productToDelete);
        }
        [HttpPost]
        [ActionName(nameof(Delete))]
        public ActionResult ConfirmDelete(string id)
        {
            context.Delete(id);
            context.Commit();
            return RedirectToAction(nameof(Index));
        }
    }
}