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
	public class IncomeController : ControllerBase
	{
		private BudgetAppDbContext db;
        private readonly IConfiguration _configuration;

        public IncomeController(BudgetAppDbContext dbparametri, IConfiguration configuration)
		{
			db = dbparametri;
			_configuration = configuration;
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
		[HttpPost, Authorize]
		[Route("AddIncome")]
		public async Task<IActionResult> AddIncome(Income Income)
		{
			string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
			List<User> Users = db.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();

			User User = new User
			{
				UserId = Users[0].UserId,
				Username = Users[0].Username,
				Password = Users[0].Password
			};


			if (User.UserId != Income.UserId)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized. UserID mismatch!");
			}
			try
			{
				string sqlFormattedDate = Income.IncomeDate.Date.ToString("s") + ".000Z";

                await db.Database.ExecuteSqlAsync($"INSERT INTO Income (UserID, CategoryID, IncomeDate, IncomeAmount) VALUES ({Income.UserId}, {Income.CategoryId}, {@sqlFormattedDate}, {@Income.IncomeAmount});");
				await db.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}

			return StatusCode(StatusCodes.Status200OK, ("Income Added successfully!"));
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

		// EditIncome esimerkki 
		// Lisää Authorize http metodin viereen, jotta muut käyttäjät eivät voi muokata toistensa tietoja. Authorize käyttää JWT tokenia varmmenukseen!
		// Koska haluamme muokata tietyn incomen tietoja, otetaan muokatut tiedot parametrinä
		[HttpPut, Authorize]
		[Route("EditIncome")]
        public async Task<IActionResult> EditIncome(Income Income)
		{
			// Tietoturvan vuoksi suoritetaan pieni tarkistus.
			// Haetaan käyttäjän nimi lukemalla name claim tokenista ja asetetaan se muuttujaan ClaimName
            string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
            // Sitten haemme käyttäjän kaikki tiedot tietokannasta SQL komennolla jossa parametrinä on ClaimName
            // Saatuamme käyttäjän tiedot voimme verrata, ovatko käyttäjän UserID ja parametrinä saadun Income:n UserID samat
            List<User> Users = db.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();

			// Asetetaan haetun käyttäjän tiedot listasta uuteen User objektiin selvyyden vuoksi
			User User = new User
			{
				UserId = Users[0].UserId,
				Username = Users[0].Username,
				Password = Users[0].Password
			};

			// Jos parametrinä saatu muokattava income ei kuulu käyttäjälle, palauta koodi 401 (Unauthorized)
			if (User.UserId != Income.UserId)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized. UserID mismatch!");
			}
			else if (Income.IncomeAmount <= 0 || Income.IncomeDate.GetHashCode() == 0) // Tarkistetaan samalla onko parametrinä saatu tieto kelpoista. Tyhjän DateTimen hashcode on yleensä 0 jos ymmärsin oikein
			{
				return StatusCode(StatusCodes.Status204NoContent, "Empty fields or empty request!");
			}

            // Jos tieto on kelpoista ja kaikki ok, tallennetaan muutokset tietokantaan try catchilla
            try
			{
				// Ihan vitun ghetto DateTime formatointi, mutta toimii...
                string sqlFormattedDate = Income.IncomeDate.Date.ToString("s") + ".000Z";

                // Parametrisöity SQL execute tekee sql injektoinnista vaikeampaa, suosittelen käyttämään
                await db.Database.ExecuteSqlAsync($"UPDATE Income SET CategoryID = {@Income.CategoryId}, IncomeDate =  {@sqlFormattedDate}, IncomeAmount = {@Income.IncomeAmount} WHERE IncomeID = {@Income.IncomeID}");
				await db.SaveChangesAsync();
            }
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}

            return StatusCode(StatusCodes.Status200OK, $"Income ID {Income.IncomeID} edited and saved succesfully!");
		}



       

		[HttpDelete, Authorize]
		[Route("DeleteIncome")]
		public async Task<IActionResult> DeleteIncome(Income Income)
		{
			string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
			List<User> Users = db.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();

			User User = new User
			{
				UserId = Users[0].UserId,
				Username = Users[0].Username,
				Password = Users[0].Password
			};


			if (User.UserId != Income.UserId)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized. UserID mismatch!");
			}
			try
			{
				
				await db.Database.ExecuteSqlAsync($"DELETE FROM Income WHERE IncomeID = {@Income.IncomeID}");
				await db.SaveChangesAsync();
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}

			return StatusCode(StatusCodes.Status200OK, ("Income deleted successfully!"));
		}
		

	}
		

	}

