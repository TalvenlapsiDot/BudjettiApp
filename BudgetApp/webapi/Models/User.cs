using System;
using System.Collections.Generic;
using System.Reflection;

namespace Back_End.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    //public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}

public partial class UserDTO
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

}
