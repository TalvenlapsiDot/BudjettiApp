using Back_End.Models;
using Back_End.Models.Context;
using Back_End.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

namespace Back_End.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly BudgetAppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserManagementController(BudgetAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Min length 5, max length 25, only allow -_ special characters
        private readonly Regex UsernameRules = new Regex(@"^[a-zA-Z0-9_-]{5,25}$");
        // Min length 10, max length 50, contains atleast one digit, upper & lowercase character
        private readonly Regex PasswordRules = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{10,50}$");

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser(User User)
        {
            // Get existing users with requested username. Use parameters to combat sql injection!
            List<User> ExistingUsers = _context.Users.FromSql($"SELECT * FROM Users WHERE Username = {@User.Username};").ToList();

            if (User.Username.IsNullOrEmpty() || User.Password.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status204NoContent, "Empty fields!");
            }
            else if (!UsernameRules.IsMatch(User.Username) || !PasswordRules.IsMatch(User.Password))
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Regex error. Check the rules!");
            }
            else if (ExistingUsers.Count() > 0)
            {
                return StatusCode(StatusCodes.Status409Conflict, "User already exists!");
            }

            try
            {
                _context.Database.ExecuteSqlRaw($"INSERT INTO Users VALUES ('{@User.Username}', '{@User.Password}');");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return StatusCode(StatusCodes.Status201Created, $"User {User.Username} creation succesful!");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(User User)
        {
            // Get existing users with requested username & password. Use parameters to combat sql injection!
            List<User> ExistingUsers = _context.Users.FromSql($"SELECT * FROM Users WHERE Username = {@User.Username} AND Password = {@User.Password};").ToList();

            if (User.Username.IsNullOrEmpty() || User.Password.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status204NoContent, "Empty fields!");
            }
            else if (ExistingUsers.Count() <= 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User not found!");
            }

            // Generate a Jwt
            string Token = JsonWebTokenService.GenerateToken(User, _configuration);

            return StatusCode(StatusCodes.Status200OK, Token);
        }

        // Logout
        [HttpPost, Authorize]
        [Route("Logout")]
        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> LogOut()
        {
            // Use tokens name claim to acquire users data


            return StatusCode(StatusCodes.Status418ImATeapot, "Not ready yet :(");
        }

        [HttpDelete, Authorize]
        [Route("Delete")]
        public async Task<IActionResult> DeleteUserAndData()
        {
            // Use tokens name claim to query users data
            string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
            List<User> Users = _context.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();

            if (Users.Count() <= 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User not found!");
            }

            User User = Users[0];

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

            // Delete user
            try
            {
                _context.Users.Remove(User);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            // Need invalidate token function here!

            return StatusCode(StatusCodes.Status200OK, $"Deleted user {ClaimName} succesfully!");
        }

        [HttpPut, Authorize]
        [Route("Edit")]
        public async Task<IActionResult> EditUser(User User)
        {
            // Use tokens name claim to acquire users data
            string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
            List<User> Users = _context.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();
            List<User> Usernames = _context.Users.FromSql($"SELECT * FROM Users WHERE Username = {User.Username};").ToList();

            if (Users.Count() <= 0) // Check if token claim name exists
            {
                return StatusCode(StatusCodes.Status404NotFound, "Token error. Name claim not found!");
            }
            else if (Usernames.Count() > 0) // Check if requested new username exists
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Username already exists!");
            }
            else if (!UsernameRules.IsMatch(User.Username) || !PasswordRules.IsMatch(User.Password)) // Check regex
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Regex error. Check the rules!");
            }

            User EditedUser = new User
            {
                UserId = Users[0].UserId,
                Username = User.Username,
                Password = User.Password
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync($"UPDATE Users SET Username = '{EditedUser.Username}', Password = '{EditedUser.Password}' WHERE UserID = {EditedUser.UserId}");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            string Token = JsonWebTokenService.GenerateToken(EditedUser, _configuration);

            return StatusCode(StatusCodes.Status200OK, Token);
        }

    }
}
