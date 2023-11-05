using Back_End.Context;
using Back_End.Models;
using Back_End.Services;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        // Register new users
        // UserId is assigned AFTER creation, so don't compare to existing users with it!
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> RegisterUser(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var ExistingUsers = from u in _context.Users
                                select u.Username;

            if (ExistingUsers.Contains(user.Username))
            {
                return Problem("User already exists!");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("RegisterUser", new { id = user.UserId }, user);
        }

        // Login
        [HttpPost]
        [Route("Login")]
        public ActionResult<IEnumerable<User>> Login(User User)
        {
            if (User == null)
            {
                return BadRequest("Null request");
            }

            var ExistingUsers = from u in _context.Users
                                where u.Username == User.Username && u.Password == User.Password
                                select u;

            if (ExistingUsers.Count() <= 0)
            {
                return BadRequest("User not found");
            }

            string Result = JWT.GenerateToken(User, _configuration);

            return Ok(Result);
        }

        // Delete user & related data
        // Delete all data related to user before the deletion from Users table or you'll get errors related to keys
        [HttpDelete]
        [Route("Delete/{UserId}")]
        public async Task<IActionResult> DeleteUserAndData(int UserId)
        {
            if (UserId == 0)
            {
                return BadRequest();
            }

            var User = await _context.Users.FindAsync(UserId);

            if (User == null)
            {
                return NotFound();
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
        [HttpPut]
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
