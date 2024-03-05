using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace BulkyWeb.Controllers
{
	public class CategoryController : Controller
	{
		// Whatever implementation we get here, we will assign that to our local variable _db
		private readonly ApplicationDbContext _db;
		public CategoryController(ApplicationDbContext db)
		{
			_db = db;
		}
        // We want to retrieve all the categories here in Index
        public IActionResult Index()
		{
			// Go to db, run the command, select star from categories,
			// retrieve that and assign it to the object right here
			// List of categories will be retrieved here 
			List<Category> objCategoryList = _db.Categories.ToList();
			return View(objCategoryList);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(Category obj)
		{
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("name", "The DisplayOrder cannot excatly match the Name.");
			}

			// if ModelState (Category obj) is valid, it will go to Category.cs and examine all the validation
			if (ModelState.IsValid)
			{
				// Create Category
				_db.Categories.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Category created successfully";
				return RedirectToAction("Index");
			}
 			return View();
		}
		//Calling Edit action method(GET), passing id
		public IActionResult Edit(int? id)
		{
			if(id == null || id==0) 
			{
				return NotFound();
			}
			//Multiple ways to retrieve one of the category from db (Edit)
			Category? categoryFromDb = _db.Categories.Find(id);
			//Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
			//Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id == id).FirstOrDefault();	

			if (categoryFromDb == null)
			{
				return NotFound();
			}
			return View(categoryFromDb);
		}
		[HttpPost]
		public IActionResult Edit(Category obj)
		{

			// if ModelState (Category obj) is valid, it will go to Category.cs and examine all the validation
			if (ModelState.IsValid)
			{
				// Create Category
				_db.Categories.Update(obj);
				_db.SaveChanges();
				TempData["success"] = "Category updated successfully";
				return RedirectToAction("Index");
			}
			return View();
		}

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {

			Category? obj = _db.Categories.Find(id);
			if (obj == null) 
			{
				return NotFound();
			}
			_db.Categories.Remove(obj);
            _db.SaveChanges();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index");
        }
    }
}
