﻿using Dmitriev_PZ2.Models;
using Dmitriev_PZ2.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Dmitriev_PZ2.Pages
{
    /// <summary>
    /// Логика главной страницы пользователя за исключением администратора
    /// </summary>
    public partial class Client : Page
    {
        private List<Employees> _employees;
        private List<Employees> _filteredEmployees;
        private List<string> _jobTitles;
        ProgMod_PZ4Entities db = Helper.GetContext();
        public Client(User user)
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
            string selectedJobTitle = cbJobTitle.SelectedItem.ToString(); //as string

            _filteredEmployees = _employees.Where(emp =>
                (emp.LastName + " " + emp.FirstName + " " + emp.FatherName).ToLower().Contains(searchText) &&
                (selectedJobTitle == "Все должности" || emp.EmployeePost == selectedJobTitle))
                .ToList();

            EmployeesListView.ItemsSource = null;
            EmployeesListView.ItemsSource = _filteredEmployees;
        }
    }
}
