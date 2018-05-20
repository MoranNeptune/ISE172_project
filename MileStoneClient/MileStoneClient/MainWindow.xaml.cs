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
        private RegisterWindow register;
        private LoginWindow login;
        public string url;
        private ChatRoom chatRoom;
        private List<Message> retMessages;
        private ObservableObject obs;

        public MainWindow()
        {
            url = "http://ise172.ise.bgu.ac.il:80";
            obs = new ObservableObject();
            chatRoom = new ChatRoom(url);
            InitializeComponent();
        }

        public RegisterWindow GetRegister()
        {
            return this.register;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            ChatRoomWindow cr = new ChatRoomWindow(this, chatRoom, obs);
            cr.Show();
            //this.register = new RegisterWindow(this,this.chatRoom); 
            //register.Show();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.login = new LoginWindow(this,this.chatRoom, obs);
            login.Show();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
