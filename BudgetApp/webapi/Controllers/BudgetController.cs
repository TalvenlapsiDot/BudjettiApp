using Back_End.Models;
using Back_End.Models.Context;
using Back_End.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Back_End.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class BudgetController : ControllerBase
	{
		private BudgetAppDbContext db;
		private readonly IConfiguration _configuration;

		public BudgetController(BudgetAppDbContext dbparametri, IConfiguration configuration)
		{
			db = dbparametri;
			_configuration = configuration;
		}

		//Add Budget
		[HttpPost, Authorize]
		[Route("AddBudget")]
		public async Task<IActionResult> AddBudget(Budget Budget)
		{
			string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
			List<User> Users = db.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();

			User User = new User
			{
				UserId = Users[0].UserId,
				Username = Users[0].Username,
				Password = Users[0].Password
			};


			if (User.UserId != Budget.UserId)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized. UserID mismatch!");
			}
			try
			{
				string sqlFormattedDate = Budget.StartDate.Date.ToString("s") + ".000Z";

				string sqlFormattedDate2 = Budget.EndDate.Date.ToString("s") + ".000Z";

				await db.Database.ExecuteSqlAsync($"INSERT INTO Budget (BudgetAmount, StartDate, EndDate, UserID) VALUES ({Budget.BudgetAmount}, {@sqlFormattedDate}, {sqlFormattedDate2}, {Budget.UserId});");
				await db.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}

			return StatusCode(StatusCodes.Status200OK, ("Budget Added successfully!"));
		}

		//Get Budget by UserID
		[HttpGet("userid/{uid}")]
		public ActionResult GetBudgetById(int uid)
		{
			try
			{
				var bud = db.Budgets.Where(c => c.UserId == uid);
				return Ok(bud);
			}
			catch (Exception ex)
			{
				return BadRequest("Tapahtui virhe. Lue lisää: " + ex.InnerException);
			}
		}
		//Edit Budget
		[HttpPut, Authorize]
		[Route("EditBudget")]
		public async Task<IActionResult> EditBudget(Budget Budget)
		{
			
			string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
			
			List<User> Users = db.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();

			
			User User = new User
			{
				UserId = Users[0].UserId,
				Username = Users[0].Username,
				Password = Users[0].Password
			};

			
			if (User.UserId != Budget.UserId)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized. UserID mismatch!");
			}
			else if (Budget.BudgetAmount <= 0 || Budget.StartDate.GetHashCode() == 0) 
			{
				return StatusCode(StatusCodes.Status204NoContent, "Empty fields or empty request!");
			}

			
			try
			{
				
				string sqlFormattedDate = Budget.StartDate.Date.ToString("s") + ".000Z";

				string sqlFormattedDate2 = Budget.EndDate.Date.ToString("s") + ".000Z";


				await db.Database.ExecuteSqlAsync($"UPDATE Budget SET StartDate =  {@sqlFormattedDate}, EndDate = {sqlFormattedDate2}, BudgetAmount = {Budget.BudgetAmount} WHERE BudgetID = {Budget.BudgetId}");
				await db.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}

			return StatusCode(StatusCodes.Status200OK, $"Budget ID {Budget.BudgetId} edited and saved succesfully!");
		}


		[HttpDelete, Authorize]
		[Route("DeleteBudget")]
		public async Task<IActionResult> DeleteBudget(Budget Budget)
		{
			string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
			List<User> Users = db.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();

			User User = new User
			{
				UserId = Users[0].UserId,
				Username = Users[0].Username,
				Password = Users[0].Password
			};


			if (User.UserId != Budget.UserId)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized. UserID mismatch!");
			}
			try
			{

				await db.Database.ExecuteSqlAsync($"DELETE FROM Budget WHERE BudgetID = {Budget.BudgetId}");
				await db.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}

			return StatusCode(StatusCodes.Status200OK, ("Budget deleted successfully!"));
		}

	}
}