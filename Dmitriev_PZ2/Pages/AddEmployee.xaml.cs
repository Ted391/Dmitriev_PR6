using Dmitriev_PZ2.Models;
using Dmitriev_PZ2.Services;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Dmitriev_PZ2.Pages
{
    /// <summary>
    /// Логика страницы добавления нового сотрудника
    /// </summary>
    public partial class AddEmployee : Page
    {
        ProgMod_PZ4Entities db = Helper.GetContext();
        public AddEmployee()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            var genders = db.Gender.ToList();
            if (genders.Any())
            {
                cbGender.ItemsSource = genders;
                cbGender.DisplayMemberPath = "Name";
                cbGender.SelectedValuePath = "ID";
                cbGender.SelectedItem = db.Gender.FirstOrDefault();
            }
            else
            {
                MessageBox.Show("Полы не найдены");
            }

            var employeePosts = db.EmployeePost.ToList();
            if (employeePosts.Any())
            {
                cbEmployeePost.ItemsSource = employeePosts;
                cbEmployeePost.DisplayMemberPath = "Name";
                cbEmployeePost.SelectedValuePath = "ID";
                cbEmployeePost.SelectedItem = db.EmployeePost.FirstOrDefault();
            }
            else
            {
                MessageBox.Show("Должности не найдены");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedGender = cbGender.SelectedItem as Gender;
                var selectedEmployeePost = cbEmployeePost.SelectedItem as EmployeePost;

                var newEmployee = new Employee
                {
                    FirstName = tbFirstName.Text,
                    LastName = tbLastName.Text,
                    FatherName = tbMiddleName.Text,
                    Gender_ID = selectedGender.Gender_ID,
                    EmployeePost_ID = selectedEmployeePost.EmployeePost_ID,
                    IdentificationNumber = tbPassportNumber.Text,
                    Email = tbEmail.Text
                };

                var validator = new EmployeeValidator();
                var validationResults = validator.Validate(newEmployee);

                if (validationResults.Any())
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var result in validationResults)
                    {
                        stringBuilder.AppendLine(result.ToString());
                    }
                    MessageBox.Show(stringBuilder.ToString());
                    return;
                }

                db.Employee.Add(newEmployee);
                db.SaveChanges();
                
                // Формирование трудового договора
                ContractGenerator laborContractGenerator = new ContractGenerator();
                laborContractGenerator.GenerateContract(newEmployee.Employee_ID);

                MessageBox.Show("Сотрудник успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
    }
}
