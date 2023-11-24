namespace Back_End.Models.Views
{
    public class MonthlySummary
    {
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set;}
        public double TotalIncome { get; set; }
        public double TotalExpenditure { get; set; }
        public double NetValue { get; set; }
        public string? MostCommonIncomeCategory { get; set; }
        public string? MostCommonExpenditureCategory { get; set; }
    }
}
