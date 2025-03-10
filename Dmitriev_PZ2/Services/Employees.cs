namespace Dmitriev_PZ2.Services
{
    internal class Employees
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string EmployeePost { get; set; }
        public string IdentificationNumber { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; } = "DefaultPhoto.png";
    }
}
