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

        [HttpGet,Authorize]
        [Route("SummarizeSetDates")]
        public IActionResult SummarizeSetDates(DateTime StartingDate, DateTime EndingDate)
        {
            // Read users name from token nameclaim and get users data from db
            string ClaimName = JsonWebTokenService.GetUniqueName(Request, _configuration);
            List<User> UserList = _context.Users.FromSql($"SELECT * FROM USERS WHERE Username = {ClaimName}").ToList();

            // Query reqired data
            List<Income> IncomeList = _context.Incomes.FromSql($"SELECT * FROM Income WHERE IncomeDate >= {@StartingDate} AND IncomeDate <= {@EndingDate} AND UserID = {UserList[0].UserId}").ToList();
            List<Expenditure> ExpenditureList = _context.Expenditures.FromSql($"SELECT * FROM Expenditure WHERE ExpenditureDate >= {@StartingDate} AND ExpenditureDate <= {@EndingDate} AND UserID = {UserList[0].UserId}").ToList();

            double TotalIncome = 0;
            double TotalExpenditure = 0;

            for (int i = 0; i < IncomeList.Count(); i++)
            {
                TotalIncome = TotalIncome + IncomeList[i].IncomeAmount;
            }

            for (int i = 0; i < ExpenditureList.Count(); i++)
            {
                TotalExpenditure = TotalExpenditure + ExpenditureList[i].ExpenditureAmount;
            }

            MonthlySummary monthlySummary = new MonthlySummary
            {
                StartingDate = StartingDate,
                EndingDate = EndingDate,
                TotalIncome = TotalIncome,
                TotalExpenditure = TotalExpenditure,
                NetValue = TotalIncome - TotalExpenditure,
                MostCommonIncomeCategory = "",
                MostCommonExpenditureCategory = "",
            };

            return StatusCode(StatusCodes.Status418ImATeapot, monthlySummary);
        }

        
    }
}
