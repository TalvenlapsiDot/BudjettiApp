using Back_End.Models;
using Back_End.Models.Context;
using Back_End.Models.Views;
using Back_End.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummariesController : ControllerBase
    {
        private readonly BudgetAppDbContext _context;
        private readonly IConfiguration _configuration;

        public SummariesController(BudgetAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet, Authorize]
        [Route("SummarizeSetDates/{StartingDate};{EndingDate}")]
        public IActionResult SummarizeSetDates(DateTime StartingDate, DateTime EndingDate)
        {
            // Read username from token nameclaim and get users data
            string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
            List<User> UserList = _context.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();

            if (StartingDate.GetHashCode() == 0 || EndingDate.GetHashCode() == 0)
            {
                return StatusCode(StatusCodes.Status204NoContent, "Invalid or empty dates!");
            }

            // Query reqired data
            List<Income> IncomeList = _context.Incomes.FromSql($"SELECT * FROM Income WHERE IncomeDate >= {@StartingDate} AND IncomeDate <= {@EndingDate} AND UserID = {UserList[0].UserId}").ToList();
            List<Expenditure> ExpenditureList = _context.Expenditures.FromSql($"SELECT * FROM Expenditure WHERE ExpenditureDate >= {@StartingDate} AND ExpenditureDate <= {@EndingDate} AND UserID = {UserList[0].UserId}").ToList();

            // Calculate total income and expenditure
            double TotalIncome = 0, TotalExpenditure = 0;

            for (int i = 0; i < IncomeList.Count(); i++)
            {
                TotalIncome = TotalIncome + IncomeList[i].IncomeAmount;
            }

            for (int i = 0; i < ExpenditureList.Count(); i++)
            {
                TotalExpenditure = TotalExpenditure + ExpenditureList[i].ExpenditureAmount;
            }

            // Figure out the most common category for income and expenditure
            var IncomeCategoryID = IncomeList.GroupBy(i => i.CategoryId)
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .FirstOrDefault();

            var IncomeCategory = from c in _context.Categories
                                 where c.CategoryId == IncomeCategoryID
                                 select c.CategoryName;

            var ExpenditureCategoryID = ExpenditureList.GroupBy(i => i.CategoryId)
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .FirstOrDefault();

            var ExpenditureCategory = from c in _context.Categories
                                      where c.CategoryId == ExpenditureCategoryID
                                      select c.CategoryName;

            // Create summary
            MonthlySummary monthlySummary = new MonthlySummary
            {
                StartingDate = StartingDate,
                EndingDate = EndingDate,
                TotalIncome = TotalIncome,
                TotalExpenditure = TotalExpenditure,
                NetValue = TotalIncome - TotalExpenditure,
                MostCommonIncomeCategory = IncomeCategory.FirstOrDefault(),
                MostCommonExpenditureCategory = ExpenditureCategory.FirstOrDefault(),
            };

            return StatusCode(StatusCodes.Status200OK, monthlySummary);
        }

        [HttpGet, Authorize]
        [Route("SummarizeThreeMonths")]
        public IActionResult SummarizeThreeMonths()
        {




            return StatusCode(StatusCodes.Status200OK);
        }


    }
}
