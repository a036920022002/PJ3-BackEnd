using System;
using System.Collections.Generic;

namespace PJ3_BackEnd.Models;

public partial class workexperience
{
    public int id { get; set; }

    public string company { get; set; } = null!;

    public string? companyEng { get; set; }

    public string? companyType { get; set; }

    public string? location { get; set; }

    public string? yearInService { get; set; }

    public string? tenure { get; set; }

    public string? jobPosition { get; set; }

    public string? jobPositionEng { get; set; }

    public string? descript { get; set; }

    public string? descriptEng { get; set; }
}
