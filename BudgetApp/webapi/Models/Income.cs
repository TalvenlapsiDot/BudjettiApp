using System;
using System.Collections.Generic;

namespace Back_End.Models;

public partial class Income
{
    public int UserId { get; set; }

    public int? CategoryId { get; set; }

    public DateTime IncomeDate { get; set; }

    public double IncomeAmount { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User User { get; set; } = null!;
}
