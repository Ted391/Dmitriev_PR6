using Dmitriev_PZ2.Models;
using Dmitriev_PZ2.Services;
using System.Linq;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Documents;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.Remoting.Contexts;

namespace Dmitriev_PZ2.Pages
{
    /// <summary>
    /// Логика главной страницы администратора
    /// </summary>
    public partial class AdminPage : Page
    {
        private List<Employees> _employees;
        private List<Employees> _filteredEmployees;
        private List<string> _jobTitles;
        ProgMod_PZ4Entities db = Helper.GetContext();
        public AdminPage(User user)
        {
            InitializeComponent();
            LoadEmployees();
            LoadJobTitles();
        }

        private void LoadEmployees()
        {
            _employees = db.Employee.Select(e => new Employees
            {
                ID = e.Employee_ID.ToString(),
                FirstName = e.FirstName,
                LastName = e.LastName,
                FatherName = e.FatherName,
                Gender = e.Gender.Name,
                EmployeePost = e.EmployeePost.Name,
                IdentificationNumber = e.IdentificationNumber,
                Email = e.Email
            }).ToList();

            foreach (var employee in _employees)
            {
                employee.FullName = $"{employee.LastName} {employee.FirstName}";
                employee.PhotoUrl = "/Resources/DefaultPhoto.png";
            }
            EmployeesListView.ItemsSource = _employees;
        }

        private void LoadJobTitles()
        {
            _jobTitles = db.EmployeePost.Select(ep => ep.Name).Distinct().ToList();

            _jobTitles.Insert(0, "Все должности");

            cbJobTitle.ItemsSource = _jobTitles;
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterEmployees();
        }

        private void cbJobTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEmployees();
        }

        private void FilterEmployees()
        {
            string searchText = tbSearch.Text.ToLower();
            string selectedJobTitle = cbJobTitle.SelectedItem as string;

            _filteredEmployees = _employees.Where(emp =>
                (emp.LastName + " " + emp.FirstName + " " + emp.FatherName).ToLower().Contains(searchText) &&
                (selectedJobTitle == "Все должности" || emp.EmployeePost == selectedJobTitle))
                .ToList();

            EmployeesListView.ItemsSource = null;
            EmployeesListView.ItemsSource = _filteredEmployees;
        }

        private void EmployeesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (EmployeesListView.SelectedItem is Employees selectedEmployee)
            {
                try
                {
                    int employeeId = int.Parse(selectedEmployee.ID);

                    
                    var employeeExists = db.Employee.Find(employeeId) != null;

                    if (employeeExists)
                    {
                        NavigationService.Navigate(new EditEmployee(employeeId));
                    }
                    else
                    {
                        MessageBox.Show($"Сотрудник с ID = {employeeId} не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Ошибка формата ID: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEmployee());
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadEmployees();
        }

        private void PrintEmployee_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                FlowDocument docToPrint = new FlowDocument();
                foreach (var employee in _employees)
                {
                    var employeeBlock = new Paragraph();

                    employeeBlock.Inlines.Add(new Run($"ФИО: {employee.FullName}\n"));
                    employeeBlock.Inlines.Add(new Run($"Должность: {employee.EmployeePost}\n"));
                    employeeBlock.Inlines.Add(new Run($"Почта: {employee.Email}\n"));
                    docToPrint.Blocks.Add(employeeBlock);
                }

                IDocumentPaginatorSource idpSource = docToPrint;
                printDialog.PrintDocument(idpSource.DocumentPaginator, "Список сотрудников");
            }
        }
        private void PrintClients_Click(object sender, RoutedEventArgs e)
        {
            var _clientItems = db.Client.ToList();

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument docToPrint = new FlowDocument();
                foreach (var clients in _clientItems)
                {
                    if ((clients.AdditionalInfo == "") || (clients.AdditionalInfo == null))
                        clients.AdditionalInfo = "Отсутствует";
                    
                    var clientsBlock = new Paragraph();
                    clientsBlock.Inlines.Add(new Run($"ФИО: {clients.LastName} {clients.FirstName} {clients.FatherName}\n"));
                    clientsBlock.Inlines.Add(new Run($"Телефонный номер: {clients.PhoneNumber}\n"));
                    clientsBlock.Inlines.Add(new Run($"Дополнительная информация: {clients.AdditionalInfo}\n"));
                    docToPrint.Blocks.Add(clientsBlock);
                }

                IDocumentPaginatorSource idpSource = docToPrint;
                printDialog.PrintDocument(idpSource.DocumentPaginator, "Список клиентов");
            }
        }

        private void PrintExcelEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employees = db.Employee.ToList();

            var excelApp = new Excel.Application();
            excelApp.Visible = true;
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel._Worksheet)excelApp.ActiveSheet;

            workSheet.Cells[1, 1] = "ID";
            workSheet.Cells[1, 2] = "Должность";
            workSheet.Cells[1, 3] = "Фамилия";
            workSheet.Cells[1, 4] = "Имя";
            workSheet.Cells[1, 5] = "Отчество";
            workSheet.Cells[1, 6] = "Паспорт";
            workSheet.Cells[1, 7] = "Email";
            workSheet.Cells[1, 8] = "Пол";

            int row = 2;

            foreach (var employee in employees)
            {
                workSheet.Cells[row, 1] = employee.Employee_ID.ToString();
                workSheet.Cells[row, 2] = employee.EmployeePost.Name;
                workSheet.Cells[row, 3] = employee.LastName;
                workSheet.Cells[row, 4] = employee.FirstName;
                workSheet.Cells[row, 5] = employee.FatherName;
                workSheet.Cells[row, 6] = employee.IdentificationNumber;
                workSheet.Cells[row, 7] = employee.Email;
                workSheet.Cells[row, 8] = employee.Gender.Name;
                row++;
            }

            workSheet.Columns[1].AutoFit();
            workSheet.Columns[2].AutoFit();
            workSheet.Columns[3].AutoFit();
            workSheet.Columns[4].AutoFit();
            workSheet.Columns[5].AutoFit();
            workSheet.Columns[6].AutoFit();
            workSheet.Columns[7].AutoFit();
            workSheet.Columns[8].AutoFit();
        }
    }
}
