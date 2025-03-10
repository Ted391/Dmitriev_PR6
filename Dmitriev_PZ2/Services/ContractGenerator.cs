using Dmitriev_PZ2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using Xceed.Words.NET;

namespace Dmitriev_PZ2.Services
{
    internal class ContractGenerator
    {
        ProgMod_PZ4Entities db = Helper.GetContext();

        string templatePath = "C:\\Users\\teddy\\Desktop\\III_course\\Program_modules_development\\Теоретические_материалы\\ПЗ_14\\TrudovoiDogovor.docx";
        static string day;
        static string month;
        static string year;
        static string startDay;
        static string startMonth;
        static string startYear;

        private Dictionary<string, string> companyInfo = new Dictionary<string, string>
            {
                { "city", "Новосибирск" },
                { "employerForm", "OOO" },
                { "employerName", "Помыв Деняк" },
                { "employerINN", "38398053535" },
                { "generalDirector", "Канев Андрей Витальевич" },
                { "generalDirectorRodPad", "Канева Андрея Витальевича" },
                { "generalDirectorSokr", "Канев А.В." },
                { "companyName", "ОтдеВневедомОхр" },
                { "companyAdress", "ул. Назарбаева, 67" },
                { "testDuration", "3"},
                { "salary", "100 000" },
                { "salaryLetters", "ста тысяч" },
                { "normDoc", "Тарифные планы" },
                { "normDocName", "Бонусы и предложения" },
                { "issued", "ГУ МВД Новосибирской области" }
                
            };

        public void GenerateContract(long employeeId)
        {
            GetCurrentDate();
            var employee = db.Employee.Find(employeeId);
            if (employee != null)
            {
                using (DocX document = DocX.Load(templatePath))
                {
                    document.ReplaceText("{ID}", $"{employee.Employee_ID}");
                    document.ReplaceText("{City}", companyInfo["city"]);
                    document.ReplaceText("{Day}", $"{day}");
                    document.ReplaceText("{Month}", $"{month}");
                    document.ReplaceText("{Year}", $"{year}");
                    document.ReplaceText("{EmployerForm}", companyInfo["employerForm"]);
                    document.ReplaceText("{EmployerName}", companyInfo["employerName"]);
                    document.ReplaceText("{GeneralDirector}", companyInfo["generalDirector"]);
                    document.ReplaceText("{GeneralDirectorRodPad}", companyInfo["generalDirectorRodPad"]);
                    document.ReplaceText("{FullName}", $"{employee.LastName} {employee.FirstName}" + (string.IsNullOrEmpty(employee.FatherName) ? "" : $" {employee.FatherName}"));
                    document.ReplaceText("{CompanyName}", companyInfo["companyName"]);
                    document.ReplaceText("{EmployeePost}", $"{employee.EmployeePost.Name}");
                    document.ReplaceText("{CompanyAdress}", companyInfo["companyAdress"]);
                    document.ReplaceText("{StartDay}", $"{startDay}");
                    document.ReplaceText("{StartMonth}", $"{startMonth}");
                    document.ReplaceText("{StartYear}", $"{startYear}");
                    document.ReplaceText("{TestDuration}", companyInfo["testDuration"]);
                    document.ReplaceText("{Salary}", companyInfo["salary"]);
                    document.ReplaceText("{SalaryLetters}", companyInfo["salaryLetters"]);
                    document.ReplaceText("{NormDoc}", companyInfo["normDoc"]);
                    document.ReplaceText("{NormDocName}", companyInfo["normDocName"]);
                    document.ReplaceText("{Employee}", $"{employee.LastName} {employee.FirstName[0]}. {(string.IsNullOrEmpty(employee.FatherName) ? "" : $"{employee.FatherName[0]}.")}");
                    document.ReplaceText("{GeneralDirectorSokr}", companyInfo["generalDirectorSokr"]);
                    document.ReplaceText("{Passport}", $"{employee.IdentificationNumber}");
                    document.ReplaceText("{Issued}", companyInfo["issued"]);
                    document.ReplaceText("{EmployerINN}", companyInfo["employerINN"]);
                    
                    document.SaveAs(System.IO.Path.Combine("C:\\Users\\teddy\\Desktop\\III_course\\Program_modules_development\\Теоретические_материалы\\ПЗ_14", $"TrudovoiDogovor_{employee.LastName}_{employee.FirstName}.docx"));
                }
            }
        }

        static void GetCurrentDate()
        {
            DateTime now = DateTime.Now; //возвращает текущую дату и время
            DateTime startDate = now.AddDays(3);

            day = now.Day.ToString();
            month = now.ToString("MMMM", CultureInfo.CurrentCulture);
            year = now.Year.ToString().Substring(2);

            if (startDate.DayOfWeek == DayOfWeek.Saturday)
                startDate = startDate.AddDays(2);
            
            else if (startDate.DayOfWeek == DayOfWeek.Sunday)
                startDate = startDate.AddDays(1);

            startDay = startDate.Day.ToString();
            startMonth = startDate.ToString("MMMM", CultureInfo.CurrentCulture);
            startYear = startDate.Year.ToString().Substring(2);
        }
    }
}
