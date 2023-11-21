using System;
using System.Collections.Generic;

namespace Back_End.Models;

public partial class Expenditure
{
    public int UserId { get; set; }

    public int? CategoryId { get; set; }

    public DateTime ExpenditureDate { get; set; }

    public double ExpenditureAmount { get; set; }
    public int ExpenditureID { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User User { get; set; } = null!;
}
