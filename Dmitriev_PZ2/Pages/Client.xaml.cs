using Dmitriev_PZ2.Models;
using System.Windows.Controls;

namespace Dmitriev_PZ2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        Autho autho = new Autho();
        public Client(User user, string role)
        {
            InitializeComponent();
            lbl_Role.Content = role;
            lbl_WelcomeMessage.Content = autho.printGreetingMessage(user);
        }
    }
}
