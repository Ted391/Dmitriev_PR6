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
    /// Логика страницы редактирования сотрудника
    /// </summary>
    public partial class EditEmployee : Page
    {
        private Employee _employee;
        ProgMod_PZ4Entities db = Helper.GetContext();

        public EditEmployee(int employeeId)
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadEmployeeData(employeeId);
        }

        private void LoadComboBoxes()
        {
            var genders = db.Gender.ToList();
            if (genders.Any())
            {
                cbGender.ItemsSource = genders;
                cbGender.DisplayMemberPath = "Name";
                cbGender.SelectedValuePath = "ID";
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
            }
            else
            {
                MessageBox.Show("Должности не найдены");
            }
        }

        private void LoadEmployeeData(int employeeId)
        {
            _employee = db.Employee.Find(employeeId);

            if (_employee != null)
            {
                tbFirstName.Text = _employee.FirstName.ToString();
                tbLastName.Text = _employee.LastName.ToString();
                tbMiddleName.Text = _employee.FatherName.ToString();
                cbGender.SelectedValue = _employee.Gender_ID;
                cbGender.Text = _employee.Gender.Name;
                cbEmployeePost.SelectedValue = _employee.EmployeePost_ID;
                cbEmployeePost.Text = _employee.EmployeePost.Name;
                tbPassportNumber.Text = _employee.IdentificationNumber.ToString();
                tbEmail.Text = _employee.Email.ToString();
            }
            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var existingEmployee = db.Employee.Find(_employee.Employee_ID);

                var selectedGender = cbGender.SelectedItem as Gender;
                var selectedEmployeePost = cbEmployeePost.SelectedItem as EmployeePost;

                if (existingEmployee != null)
                {
                    existingEmployee.FirstName = tbFirstName.Text;
                    existingEmployee.LastName = tbLastName.Text;
                    existingEmployee.FatherName = tbMiddleName.Text;
                    existingEmployee.Gender_ID = selectedGender.Gender_ID;
                    existingEmployee.EmployeePost_ID = selectedEmployeePost.EmployeePost_ID;
                    existingEmployee.IdentificationNumber = tbPassportNumber.Text;
                    existingEmployee.Email = tbEmail.Text;

                    var validator = new EmployeeValidator();
                    var validationResults = validator.Validate(existingEmployee);

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

                    db.SaveChanges();
                    MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Сотрудник не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого сотрудника?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var employeeToDelete = db.Employee.Find(_employee.Employee_ID);

                    if (employeeToDelete != null)
                    {
                        db.Employee.Remove(employeeToDelete);
                        db.SaveChanges();

                        MessageBox.Show("Сотрудник успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("Сотрудник не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
