using Back_End.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

namespace Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginAndRegisterationController : ControllerBase
    {
        private readonly BudgetAppDbContext _context;

        public LoginAndRegisterationController(BudgetAppDbContext context)
        {
            _context = context;
        }

        // Register new users
        // UserId is assigned AFTER creation, so don't compare to existing users with it!
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> RegisterUser(User user)
        {
            if (user == null)
            {
                return BadRequest("Request was null");
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

        // Delete user & related data
        // Delete all data related to user before the deletion from Users table or you'll get errors related to keys
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteUserAndData(int UserId)
        {
            if (UserId == 0)
            {
                return BadRequest("Request was null");
            }

            var User = await _context.Users.FindAsync(UserId);

            if (User == null)
            {
                return NotFound("UserId not found");
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

            return NoContent();
        }

        // Edit user
        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> EditUser(User User)
        {
            if (User == null)
            {
                return BadRequest("Request was null");
            }

            _context.Entry(User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // Login
        //[HttpPost]
        //[Route("Login")]
        //public async Task<ActionResult<IEnumerable<User>>> Login(User User)
        //{
        //    if (User == null)
        //    {
        //        return BadRequest("Request was null");
        //    }





        //}

        // ------------------------------------------------------------ //

        // GET: api/LoginAndRegisteration/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/LoginAndRegisteration/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
