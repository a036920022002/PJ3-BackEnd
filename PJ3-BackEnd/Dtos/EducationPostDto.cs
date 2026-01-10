namespace PJ3_BackEnd.Dtos
{
    public class EducationPostDto
    {
        public string school { get; set; } = null!;

        public string? schoolEng { get; set; }

        public string? department { get; set; }

        public string? departmentEng { get; set; }

        public string? degree { get; set; }

        public string? degreeEng { get; set; }

        public string? periodOfStudytime { get; set; }
    }
}
