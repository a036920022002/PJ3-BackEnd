using System;
using System.Collections.Generic;

namespace PJ3_BackEnd.Models;

public partial class certificate
{
    public int id { get; set; }

    public string? name { get; set; }

    public string? issuing_authority { get; set; }

    public string? photo { get; set; }
}
