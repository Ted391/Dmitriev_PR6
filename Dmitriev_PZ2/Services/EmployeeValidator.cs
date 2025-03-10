using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dmitriev_PZ2.Models;

namespace Dmitriev_PZ2.Services
{
    internal class EmployeeValidator
    {
        public List<ValidationResult> Validate(Employee employee)
        {
            var context = new ValidationContext(employee);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(employee, context, results, true);
            return results;
        }
    }
}
