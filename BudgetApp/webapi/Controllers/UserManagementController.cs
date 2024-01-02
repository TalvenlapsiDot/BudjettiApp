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
        // Min length 2, max length 50, can contain letters, numbers & - _ ., atleast one @
        private readonly Regex EmailRules = new Regex(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,50}$");

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser(RegisterationForm Form)
        {
            // Get existing users with requested username. Use parameters to combat sql injection!
            List<User> ExistingUsers = _context.Users.FromSql($"SELECT * FROM Users WHERE Username = {Form.Username};").ToList();

            if (Form.Username.IsNullOrEmpty() || Form.Password.IsNullOrEmpty() || Form.Email.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status204NoContent, "Empty fields!");
            }
            else if (!UsernameRules.IsMatch(Form.Username) || !PasswordRules.IsMatch(Form.Password) || !EmailRules.IsMatch(Form.Email))
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Regex error. Check the rules!");
            }
            else if (ExistingUsers.Count() > 0)
            {
                return StatusCode(StatusCodes.Status409Conflict, "User already exists!");
            }

            // Create confirmation code
            string DtNow = DateTime.Now.ToString();
            string VerificationCode = DtNow
                .Replace(" ", "")
                .Replace("/", "")
                .Replace(":", "")
                .Replace(".", "");
            VerificationCode = VerificationCode.Substring(8);

            // Set values to user object
            User NewUser = new User
            {
                UserId = 0,
                Username = Form.Username,
                Password = Form.Password,
                Email = Form.Email,
                VerificationCode = VerificationCode,
                EmailVerified = false
            };

            try
            {
                // Save account to database
                _context.Database.ExecuteSqlRaw($"INSERT INTO Users (Username, Password, Email, VerificationCode, EmailVerified) VALUES ('{NewUser.Username}', '{NewUser.Password}', '{NewUser.Email}', '{NewUser.VerificationCode}', '{NewUser.EmailVerified}');");
                await _context.SaveChangesAsync();

                // Send verification email with confirmation code
                await EmailService.SendConfirmationCode(Form.Email, VerificationCode, _configuration);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return StatusCode(StatusCodes.Status201Created, $"User {Form.Username} creation succesful!");
        }

        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string Code)
        {
            List<User> Users = _context.Users.FromSql($"SELECT * FROM Users WHERE VerificationCode = {Code};").ToList();

            if (Users.Count() <= 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Verification code doesn't belong to any user");
            }

            // Everything ok, set EmailVerified to true and save it
            Users[0].EmailVerified = true;

            try
            {
                // Update email verification status to database
                _context.Database.ExecuteSqlRaw($"INSERT INTO Users (EmailVerified) VALUES ('{@Users[0].EmailVerified}');");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return StatusCode(StatusCodes.Status200OK, "Email verified succesfully!");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginForm Form)
        {
            // Get existing users with requested username & password. Use parameters to combat sql injection!
            List<User> ExistingUsers = _context.Users.FromSql($"SELECT * FROM Users WHERE Username = {@Form.Username} AND Password = {@Form.Password};").ToList();

            if (Form.Username.IsNullOrEmpty() || Form.Password.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status204NoContent, "Empty fields!");
            }
            else if (ExistingUsers.Count() <= 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "User not found!");
            }

            // Create user to pass onto JsonWebTokenServices
            User User = new User
            {
                Username = Form.Username,
                Password = Form.Password,
            };

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
        public async Task<IActionResult> EditUser(EditUserForm Form)
        {
            // Use tokens name claim to acquire users data
            string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
            List<User> Users = _context.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();
            List<User> Usernames = _context.Users.FromSql($"SELECT * FROM Users WHERE Username = {Form.Username};").ToList();

            if (Users.Count() <= 0) // Check if token claim name exists
            {
                return StatusCode(StatusCodes.Status404NotFound, "Token error. Name claim not found!");
            }
            else if (Usernames.Count() > 0) // Check if requested new username exists
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Username already exists!");
            }
            else if (!UsernameRules.IsMatch(Form.Username) || !PasswordRules.IsMatch(Form.Password)) // Check regex
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Regex error. Check the rules!");
            }

            User EditedUser = new User
            {
                UserId = Users[0].UserId,
                Email = Form.Email,
                Username = Form.Username,
                Password = Form.Password
            };

            try
            {
                await _context.Database.ExecuteSqlRawAsync($"UPDATE Users SET Username = '{EditedUser.Username}', Password = '{EditedUser.Password}', Email = '{EditedUser.Email}' WHERE UserID = {EditedUser.UserId}");
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
