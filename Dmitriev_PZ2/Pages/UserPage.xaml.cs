using Dmitriev_PZ2.Models;
using System.Windows.Controls;

namespace Dmitriev_PZ2.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        Autho autho = new Autho();
        public UserPage(User user, string role)
        {
            InitializeComponent();
            lbl_Role.Content = role;
            lbl_WelcomeMessage.Content = autho.printGreetingMessage(user);
        }
    }
}
