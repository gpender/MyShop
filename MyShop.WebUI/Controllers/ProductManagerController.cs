using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;
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
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if(file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
                context.Insert(product);
                context.Commit();
                return RedirectToAction(nameof(Index));
            }
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
        public ActionResult Edit(Product product, string id, HttpPostedFileBase file)
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
            if (file != null)
            {
                product.Image = product.Id + Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
            }

            productToUpdate.Category = product.Category;
            productToUpdate.Description = product.Description;
            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;
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