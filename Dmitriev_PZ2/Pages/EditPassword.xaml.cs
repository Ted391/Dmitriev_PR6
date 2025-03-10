using Dmitriev_PZ2.Models;
using Dmitriev_PZ2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Dmitriev_PZ2.Pages
{
    /// <summary>
    /// Логика страницы редактирования пароля
    /// </summary>
    public partial class EditPassword : Page
    {
        ProgMod_PZ4Entities db = Helper.GetContext();
        private User _user;
        private string _email;
        private string _confirmationCode;

        public EditPassword(string login)
        {
            InitializeComponent();
            FindUser(login);
        }

        private void FindUser(string login)
        {
            _user = db.User.FirstOrDefault(u => u.Login == login);

            if (_user != null)
            {
                _email = _user.Employee.Email;
            }
            else
            {
                MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //NavigationService.Navigate(new Autho());
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (_email != null)
            {
                CodeConfirmation confCode = new CodeConfirmation();
                _confirmationCode = confCode.SendEmail(_email);
                btnSend.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Email сотрудника не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var existingUser = db.User.Find(_user.User_ID);
            string password;

            if (existingUser != null)
            {
                if (txtbNewPassword.Text == txtbConfirmPassword.Text)
                {
                    password = txtbConfirmPassword.Text;
                    string hashPassw = Hash.HashPassword(password);
                    if (hashPassw == existingUser.Password)
                    {
                        MessageBox.Show("Новый пароль не может быть таким же как старый", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        existingUser.Password = hashPassw;
                    }
                }
                else
                {
                    MessageBox.Show("Введите одинаковый пароль в оба поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    db.SaveChanges();
                    MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при смене пароля: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            NavigationService.GoBack();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (txtbConfirmCode.Text == _confirmationCode)
            {
                txtbConfirmCode.IsEnabled = false;
                btnContinue.IsEnabled = false;
                txtbNewPassword.IsEnabled = true;
                txtbConfirmPassword.IsEnabled = true;
                btnSave.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Неверный код подтверждения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
