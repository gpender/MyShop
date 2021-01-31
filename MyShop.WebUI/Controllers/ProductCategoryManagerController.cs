using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        IRepository<ProductCategory> context;
        public ProductCategoryManagerController(IRepository<ProductCategory> productCategoryContext)
        {
            context = productCategoryContext;
        }
        // GET: ProductCategoryManager
        public ActionResult Index()
        {
            List<ProductCategory> productCategorys = context.Collection().ToList();
            return View(productCategorys);
        }
        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }
        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            context.Insert(productCategory);
            context.Commit();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Edit(string id)
        {
            ProductCategory productCategory = context.Find(id);
            return View(productCategory);
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string id)
        {
            ProductCategory productCategoryToUpdate = context.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            productCategoryToUpdate.Category = productCategory.Category;
            context.Commit();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(string id)
        {
            ProductCategory productCategoryToDelete = context.Find(id);
            if (productCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            return View(productCategoryToDelete);
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