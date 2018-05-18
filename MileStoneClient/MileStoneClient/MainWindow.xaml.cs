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
using MileStoneClient.BusinessLayer;

namespace MileStoneClient.PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RegisterWindow register;
        LoginWindow login;
        public const string url = "http://ise172.ise.bgu.ac.il:80";
        private ChatRoom chatRoom = new ChatRoom(url);
        private List<Message> retMessages;

        public MainWindow()
        {
            InitializeComponent();
        }

        public RegisterWindow GetRegister()
        {
            return this.register;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            this.register = new RegisterWindow(this,this.chatRoom); 
            register.Show();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.login = new LoginWindow(this,this.chatRoom);
            login.Show();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
