using System;
using System.Collections.Generic;

namespace PJ3_BackEnd.Models;

public partial class auth
{
    public int id { get; set; }

    public string email { get; set; } = null!;

    public string password { get; set; } = null!;

    public string? name { get; set; }

    public string? role { get; set; }
}
