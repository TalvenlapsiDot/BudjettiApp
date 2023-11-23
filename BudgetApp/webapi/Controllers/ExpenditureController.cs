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
	public class ExpenditureController : ControllerBase
	{
		private BudgetAppDbContext db;
		private readonly IConfiguration _configuration;

		public ExpenditureController(BudgetAppDbContext dbparametri, IConfiguration configuration)
		{
			db = dbparametri;
			_configuration = configuration;
		}

		//Add Expenditure
		[HttpPost, Authorize]
		[Route("AddExpenditure")]
		public async Task<IActionResult> AddExpenditure(Expenditure Expenditure)
		{
			string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
			List<User> Users = db.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();

			User User = new User
			{
				UserId = Users[0].UserId,
				Username = Users[0].Username,
				Password = Users[0].Password
			};


			if (User.UserId != Expenditure.UserId)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized. UserID mismatch!");
			}
			try
			{
				string sqlFormattedDate = Expenditure.ExpenditureDate.Date.ToString("s") + ".000Z";

				await db.Database.ExecuteSqlAsync($"INSERT INTO Expenditure (UserID, CategoryID, ExpenditureDate, ExpenditureAmount) VALUES ({Expenditure.UserId}, {Expenditure.CategoryId}, {@sqlFormattedDate}, {Expenditure.ExpenditureAmount});");
				await db.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}

			return StatusCode(StatusCodes.Status200OK, ("Expenditure Added successfully!"));
		}



	}
}