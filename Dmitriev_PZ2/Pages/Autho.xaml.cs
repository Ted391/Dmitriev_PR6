using Dmitriev_PZ2.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Threading;
using Dmitriev_PZ2.Models;
using System.Windows.Threading;

namespace Dmitriev_PZ2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        int click;
        int failedAuthorizationCount; // Счётчик неудачных попыток авторизации

        DispatcherTimer timer;
        int timerDuration;

        public Autho()
        {
            InitializeComponent();
            click = 0;
            failedAuthorizationCount = 0;

            timerDuration = 10;
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            timer.Tick += new EventHandler(timerTick);
        }

        // Метод-процедура для генерации капчи
        private void GenerateCapctcha()
        {
            txtboxCaptcha.Visibility = Visibility.Visible;
            txtBlockCaptcha.Visibility = Visibility.Visible;

            string capctchaText = CaptchaGenerator.GenerateCaptchaText(6);
            txtBlockCaptcha.Text = capctchaText;
            txtBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;
        }

        // Метод-процедура для изменения GUI при успешной авторизации
        private void successfulAuthorization()
        {
            txtBlockFailedAuthorizationCount.Visibility = Visibility.Hidden;
            failedAuthorizationCount = 0;

            txtbLogin.Text = "";
            pswbPassword.Password = "";

            txtboxCaptcha.Text = "";
            txtBlockCaptcha.Visibility = Visibility.Hidden;
            txtboxCaptcha.Visibility = Visibility.Hidden;
        }

        // Метод-процедура для изменения GUI при неудачной авторизации
        private void failedAuthorization()
        {
            timerDuration = 10;
            txtBlockFailedAuthorizationCount.Visibility = Visibility.Visible;
            failedAuthorizationCount += 1;
            txtBlockFailedAuthorizationCount.Text = $"Количество неудачных попыток: {failedAuthorizationCount}";

            GenerateCapctcha();

            pswbPassword.Password = "";
            txtboxCaptcha.Text = "";
            if (failedAuthorizationCount >= 3)
            {
                temporarilyBlockedAuthorization();
            }
        }

        // Метод-процедура для блокирования GUI на 10 секунд после трёх неудачных попыток
        private void temporarilyBlockedAuthorization()
        {
            txtbLogin.IsEnabled = false;
            pswbPassword.IsEnabled = false;
            txtboxCaptcha.IsEnabled = false;
            btnEnter.IsEnabled = false;

            txtBlockTimer.Visibility = Visibility.Visible;
            timer.Start();
        }

        // Метод-процедура для разблокирования GUI по истечении 10 секунд после его блокировки
        private void unblockAuthorization()
        {
            txtbLogin.IsEnabled = true;
            pswbPassword.IsEnabled = true;
            txtboxCaptcha.IsEnabled = true;
            btnEnter.IsEnabled = true;

            txtBlockTimer.Visibility = Visibility.Collapsed;
            timer.Stop();
        }

        // Метод-функция для вывода приветственного сообщения в формате "Доброе утро/день/вечер, <Имя> <Фамилия>"
        public string printGreetingMessage(User user)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            string greeting;
            int gender = Convert.ToInt32(user.Gender);
            string genderChoice;
            string name;

            if (now >= TimeSpan.FromHours(10) && now <= TimeSpan.FromHours(12))
            {
                greeting = "Доброе утро";
            }
            else if (now > TimeSpan.FromHours(12) && now <= TimeSpan.FromHours(17))
            {
                greeting = "Добрый день";
            }
            else if (now > TimeSpan.FromHours(17) && now <= TimeSpan.FromHours(19))
            {
                greeting = "Добрый вечер";
            }
            else
            {
                greeting = "Здравствуйте";
            }

            switch (gender)
            {
                case 1:
                    genderChoice = "Mr";
                    break;
                case 2:
                    genderChoice = "Mrs";
                    break;
                default:
                    genderChoice = "<Unknown gender>";
                    break;
            }
            name = user.FirstName.ToString();

            return $"{greeting}, {genderChoice} {name}";
        }

        // Метод-функция для определения условия нахождения в интервале рабочего времени
        public bool InWorkingTime()
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            return now >= TimeSpan.FromHours(10) && now <= TimeSpan.FromHours(19);
        }

        // Обработчики событий
        private void timerTick(object sender, EventArgs e)
        {
            if (timerDuration <= 0)
            {
                unblockAuthorization();
                txtBlockTimer.Text = $"До разблокировки осталось 10 секунд";
            }
            else
            {
                txtBlockTimer.Text = $"До разблокировки осталось {timerDuration} секунд";
            }
            timerDuration--;
        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null, null));
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            click += 1;
            string login = txtbLogin.Text.Trim();
            string password = pswbPassword.Password.Trim();
            string hashedPassword = Hash.HashPassword(password);
            bool inWorkingTime = InWorkingTime();

            var db = Helper.GetContext();

            var user = db.User.Where(x => x.Login == login && x.Password == hashedPassword).FirstOrDefault(); // после выполнения заменить password на hashedPassword, а во время выполнения - hashedPassword на password

            if (!inWorkingTime)
            {
                MessageBox.Show("Доступ запрещен. Рабочее время с 10:00 до 19:00.", "Ошибка доступа");
            }

            else
            {
                if (click == 1)
                {
                    if (user != null)
                    {
                        MessageBox.Show("Вы вошли под: " + user.UserRole.Name.ToString());
                        LoadPage(user.UserRole.Name.ToString(), user);

                        successfulAuthorization();
                    }
                    else
                    {
                        MessageBox.Show("Вы ввели логин или пароль неверно!");

                        failedAuthorization();
                    }
                }
                else if (click > 1)
                {
                    if (user != null && txtboxCaptcha.Text == txtBlockCaptcha.Text)
                    {
                        MessageBox.Show("Вы вошли под: " + user.UserRole.Name.ToString());
                        LoadPage(user.UserRole.Name.ToString(), user);

                        successfulAuthorization();
                    }
                    else
                    {
                        MessageBox.Show($"Введите данные заново!\nВы ввели: {txtboxCaptcha.Text}\nПравильный ответ: {txtBlockCaptcha.Text}");

                        failedAuthorization();
                    }
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
