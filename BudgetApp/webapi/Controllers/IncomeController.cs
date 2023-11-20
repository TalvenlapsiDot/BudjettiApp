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
	public class IncomeController : ControllerBase
	{
		private BudgetAppDbContext db;

		public IncomeController(BudgetAppDbContext dbparametri)
		{
			db = dbparametri;
		}

		//Get all incomes
		[HttpGet]
		public ActionResult GetAllIncomes()
		{
			try
			{
				var inc= db.Incomes.ToList();
				return Ok(inc);
			}
			catch (Exception ex)
			{
				return BadRequest("Tapahtui virhe. Lue lisää: " + ex.InnerException);
			}
		}

		//Add income
		[HttpPost]
		public ActionResult AddNew([FromBody]  Income inc)
		{
			try
			{
				db.Incomes.Add(inc);
				db.SaveChanges();
				return Ok("Added new income");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException);
			}
		}


		//Get Incomes by user id
		[HttpGet]
		[Route("{id}")]
		public ActionResult GetIncomeByid(int id)
		{
			try
			{
				Income inc = db.Incomes.Find(id);
				if (inc != null)
				{
					return Ok(inc);
				}
				else
				{
					return NotFound("Cannot find income with user id of " + id);
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException);
			}
		}

		//Get/Find by category name
		[HttpGet("categoryid/{cid}")]
		public ActionResult GetByName(int cid)
		{
			try
			{
				var cate = db.Incomes.Where(c => c.CategoryId == cid);
				return Ok(cate);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		//Find Income with id and Update
		[HttpPut("{id}")]
		public ActionResult EditIncome(int id, [FromBody] Income incs)
		{
			var inc = db.Incomes.Find(id);
			if (inc != null)
			{
				inc.UserId = incs.UserId;
				inc.CategoryId = incs.CategoryId;
				inc.IncomeDate= incs.IncomeDate;
				inc.IncomeAmount = incs.IncomeAmount;

				db.SaveChanges();
				return Ok("Updated Income ");
			}
			else
			{
				return NotFound("Cannot find Income with the user id of " + id);
			}
		}
		//Delete income
		[HttpDelete("{id}")]
		public ActionResult Delete(int id)
		{
			try
			{
				var inc= db.Incomes.Find(id);

				if (inc != null)
				{
					db.Incomes.Remove(inc);
					db.SaveChanges();
					return Ok("Removed income");
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
