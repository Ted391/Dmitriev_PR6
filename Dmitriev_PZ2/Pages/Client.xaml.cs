using System.Windows.Controls;
using Dmitriev_PZ2.Models;

namespace Dmitriev_PZ2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        public Client(User user, string role)
        {
            InitializeComponent();
            lbl_Role.Content = role;
            lbl_WelcomeMessage.Content = $"Добро пожаловать, {user.Login}";
        }
    }
}
