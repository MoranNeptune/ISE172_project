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
using System.Windows.Shapes;
using MileStoneClient.BusinessLayer;

namespace MileStoneClient
{
    /// <summary>
    /// Interaction logic for ChatRoom.xaml
    /// </summary>
    public partial class ChatRoomWindow : Window
    {
        ChatRoom chatRoom;
        MainWindow mainWindow;

        public ChatRoomWindow(MainWindow mainWindow,ChatRoom chatRoom)
        {
            InitializeComponent();
            this.chatRoom = chatRoom;
            this.mainWindow = mainWindow;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            this.chatRoom.logOut();
            this.mainWindow.Show();
        }
    }
}
