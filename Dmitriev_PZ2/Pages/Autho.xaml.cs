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
using Dmitriev_PZ2.Models;

namespace Dmitriev_PZ2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        int click;
        public Autho()
        {
            InitializeComponent();
            click = 0;
        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null, null));
        }
        private void GenerateCapctcha()
        {
            txtboxCaptcha.Visibility = Visibility.Visible;
            txtBlockCaptcha.Visibility = Visibility.Visible;

            string capctchaText = CaptchaGenerator.GenerateCaptchaText(6);
            txtBlockCaptcha.Text = capctchaText;
            txtBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;
        }


        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            click += 1;
            string login = txtbLogin.Text.Trim();
            string password = pswbPassword.Password.Trim();
            string hashedPassword = Hash.HashPassword(password);

            
            var db = Helper.GetContext();

            var user = db.User.Where(x => x.Login == login && x.Password == hashedPassword).FirstOrDefault();
            if (click == 1)
            {
                if (user != null)
                {
                    MessageBox.Show("Вы вошли под: " + user.UserRole.Name.ToString());
                    LoadPage(user.UserRole.Name.ToString(), user);
                    txtbLogin.Text = "";
                    pswbPassword.Password = "";
                    txtboxCaptcha.Text = "";
                    txtBlockCaptcha.Visibility = Visibility.Hidden;
                    txtboxCaptcha.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("Вы ввели логин или пароль неверно!");
                    GenerateCapctcha();
                    pswbPassword.Password = "";
                    txtboxCaptcha.Text = "";
                }
            }
            else if (click > 1)
            {
                if (user != null && txtboxCaptcha.Text == txtBlockCaptcha.Text)
                {
                    MessageBox.Show("Вы вошли под: " + user.UserRole.Name.ToString());
                    LoadPage(user.UserRole.Name.ToString(), user);
                    txtbLogin.Text = "";
                    pswbPassword.Password = "";
                    txtboxCaptcha.Text = "";
                    txtBlockCaptcha.Visibility = Visibility.Hidden;
                    txtboxCaptcha.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show($"Введите данные заново!\nВы ввели: {txtboxCaptcha.Text}\nПравильный ответ: {txtBlockCaptcha.Text}");
                    GenerateCapctcha();
                    pswbPassword.Password = "";
                    txtboxCaptcha.Text = "";
                }
            }
        }
        private void LoadPage(string _role, User user)
        {
            click = 0;
            switch (_role)
            {
                case "Клиент":
                    NavigationService.Navigate(new Client(user, _role));
                    break;
                case "Администратор":
                    NavigationService.Navigate(new AdminPage(user, _role));
                    break;
                case "Пользователь":
                    NavigationService.Navigate(new UserPage(user, _role));
                    break;
            }
        }
    }
}
