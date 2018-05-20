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

namespace MileStoneClient.PresentationLayer
{
    /// <summary>
    /// Interaction logic for ChatRoom.xaml
    /// </summary>
    public partial class ChatRoomWindow : Window
    {
        ChatRoom chatRoom;
        MainWindow mainWindow;
        ObservableObject obs = new ObservableObject();

        public ChatRoomWindow(MainWindow mainWindow,ChatRoom chatRoom)
        {
            InitializeComponent();
            this.chatRoom = chatRoom;
            this.mainWindow = mainWindow;
            this.DataContext = obs;
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            this.chatRoom.logOut();
            this.Close();
            this.mainWindow.Show();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if(obs.TxtSendContent.Equals("what is on your mind?"))
                //error
            //check if the text is by standarts and pop up error message if not
            //send the message
            //clear the txt box
            obs.TxtSendContent = "what is on your mind?";
        }

        private void ViewProfile_Click(object sender, RoutedEventArgs e)
        {
            ViewProfileWindow viewProfileWindow = new ViewProfileWindow(this.mainWindow, this.chatRoom);
            viewProfileWindow.Show();
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
