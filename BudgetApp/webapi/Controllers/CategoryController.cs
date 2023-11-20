using Back_End.Models;
using Back_End.Models.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Back_End.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private BudgetAppDbContext db;

		public CategoryController(BudgetAppDbContext dbparametri)
		{
		db = dbparametri;
		}

		
		
		
		//Add new category
		[HttpPost]
		public ActionResult AddNew([FromBody] Category cate)
		{
			try
			{
				db.Categories.Add(cate);
				db.SaveChanges();
				return Ok("Added new category");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException);
			}
		}

		//Get all categories
		[HttpGet]
		public ActionResult GetAllCategories()
		{
			try
			{
				var cate = db.Categories.ToList();
				return Ok(cate);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException);
			}
		}







		//Find Category by id
		[HttpGet]
		[Route("{id}")]
		public ActionResult GetCategoryByName(int id)
		{
			try
			{
				Category cate = db.Categories.Find(id);
				if (cate != null)
				{
					return Ok(cate);
				}
				else
				{
					return NotFound("Cannot find category with the id of " + id);
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException);
			}
		}

		//Find Category with id and Update
		[HttpPut("{id}")]
		public ActionResult EditCategory(int id, [FromBody] Category cate)
		{
			var cat = db.Categories.Find(id);
			if (cat != null)
			{
				cat.CategoryId = cate.CategoryId;
				cat.CategoryName = cate.CategoryName;
				cat.CategoryDescription = cate.CategoryDescription;

				db.SaveChanges();
				return Ok("Updated Category " + cate.CategoryName);
			}
			else
			{
				return NotFound("Cannot find Category with the id of " + id);
			}
		}

		//Get/Find by catefory name
		[HttpGet("categoryname/{cname}")]
		public ActionResult GetByName(string cname)
		{
			try
			{
				var cate = db.Categories.Where(c => c.CategoryName.Contains(cname));
				return Ok(cate);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		//Delete category by id
		[HttpDelete("{id}")]
		public ActionResult Delete(int id)
		{
			try
			{
				var cate = db.Categories.Find(id);

				if (cate != null)
				{ 
					db.Categories.Remove(cate);
					db.SaveChanges();
					return Ok("Removed " + cate.CategoryName);
				}
				else
				{
					return NotFound();
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException);
			}
		}

	}
}
