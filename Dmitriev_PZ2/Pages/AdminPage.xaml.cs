using Dmitriev_PZ2.Models;
using System.Windows.Controls;

namespace Dmitriev_PZ2.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        Autho autho = new Autho();
        public AdminPage(User user, string role)
        {
            InitializeComponent();
            lbl_Role.Content = role;
            lbl_WelcomeMessage.Content = autho.printGreetingMessage(user);
        }
    }
}
