namespace Back_End.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string VerificationCode { get; set; } = string.Empty;

    public bool EmailVerified { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    //public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
