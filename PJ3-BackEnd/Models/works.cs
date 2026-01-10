using System;
using System.Collections.Generic;

namespace PJ3_BackEnd.Models;

public partial class works
{
    public int id { get; set; }

    public string? name { get; set; }

    public string? descript { get; set; }

    public string? image { get; set; }

    public string? frontEnd { get; set; }

    public string? backEnd { get; set; }

    public string? database_name { get; set; }

    public string? tool { get; set; }

    public string? function_name { get; set; }

    public string? gitHub_link { get; set; }

    public string? page_link { get; set; }

    public string? item_label { get; set; }
}
