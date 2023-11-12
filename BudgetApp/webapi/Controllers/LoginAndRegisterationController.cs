using Back_End.Context;
using Back_End.Models;
using Back_End.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

namespace Back_End.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class LoginAndRegisterationController : ControllerBase
    {
        private readonly BudgetAppDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginAndRegisterationController(BudgetAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // UserId is assigned AFTER creation, so don't compare to existing users with it!
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> RegisterUser(User User)
        {
            // Min length 5, max length 25, only allow -_ special characters
            Regex UsernameRules = new Regex(@"^[a-zA-Z0-9_-]{5,25}$");
            // Min length 10, max length 50, contains atleast one upper & lowercase character, one digit
            Regex PasswordRules = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{10,50}$");

            // Get existing users with requested username. Use parameters to combat sql injection
            List<User> ExistingUsers = _context.Users.FromSql($"Select * FROM Users WHERE Username = {@User.Username};").ToList();

            if (User.Username.IsNullOrEmpty() || User.Password.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status204NoContent, "Empty request!");
            }
            else if (!UsernameRules.IsMatch(User.Username) || !PasswordRules.IsMatch(User.Password))
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Regex error!");
            }
            else if (ExistingUsers.Count() > 0)
            {
                return StatusCode(StatusCodes.Status409Conflict, "User already exists");
            }

            // Add user to DB
            _context.Database.ExecuteSqlRaw($"INSERT INTO Users VALUES ('{@User.Username}', '{@User.Password}');");
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, "User creation succesful!");
        }

        // Login
        [HttpPost]
        [Route("Login")]
        public ActionResult Login(User User)
        {
            // Get existing users with requested username & password. Use parameters to combat sql injection
            List<User> ExistingUsers = _context.Users.FromSql($"Select * FROM Users WHERE Username = {@User.Username} AND Password = {User.Password};").ToList();

            if (User.Username.IsNullOrEmpty() || User.Password.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status204NoContent, "Empty request!");
            }
            else if (ExistingUsers.Count() <= 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User not found!");
            }

            // Generate a Json web token and return it
            string Token = JWT.GenerateTokenUser(User, _configuration);

            return StatusCode(StatusCodes.Status200OK, Token);
        }

        // Delete user & related data
        // Delete all data related to user before the deletion from Users table or you'll get errors related to keys
        [HttpDelete, Authorize]
        [Route("Delete")]
        public async Task<IActionResult> DeleteUserAndData(User user)
        {
            if (user.UserId == 0)
            {
                return BadRequest();
            }

            var User = await _context.Users.FindAsync(user.UserId);

            if (User == null)
            {
                return NotFound("User not found");
            }

            // Delete budget data
            var ExistingBudgets = from b in _context.Budgets
                                  where User.UserId == b.UserId
                                  select b;

            foreach (var b in ExistingBudgets)
            {
                _context.Budgets.Remove(b);
            }

            // Delete Income data
            var ExistingIncomes = from i in _context.Incomes
                                  where User.UserId == i.UserId
                                  select i;

            foreach (var i in ExistingIncomes)
            {
                _context.Incomes.Remove(i);
            }

            // Delete Expenditure data
            var ExistingExpenditures = from e in _context.Expenditures
                                       where User.UserId == e.UserId
                                       select e;

            foreach (var e in ExistingExpenditures)
            {
                _context.Expenditures.Remove(e);
            }

            // Delete user & save changes
            _context.Users.Remove(User);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Edit user
        [HttpPut, Authorize(Roles = "User")]
        [Route("Edit/{UserId}")]
        public async Task<IActionResult> EditUser(int UserId, User User)
        {
            if (User == null || UserId != User.UserId)
            {
                return BadRequest();
            }

            _context.Entry(User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok("Succesful");
        }

    }
}
