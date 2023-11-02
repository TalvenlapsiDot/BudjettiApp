using System;
using System.Collections.Generic;

namespace Back_End.Models;

public partial class Budget
{
    public int BudgetId { get; set; }

    public int BudgetAmount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int UserId { get; set; }

    //public virtual User User { get; set; } = null!;
}
