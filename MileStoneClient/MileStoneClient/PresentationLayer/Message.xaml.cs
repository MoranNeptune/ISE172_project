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
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class Message : Window
    {
        private ObservableObject obs;//binding
        private ChatRoom chatRoom;
        private GuiMessage msg;

        public Message(ObservableObject obs, ChatRoom chatRoom, GuiMessage msg)//String lastMSG)
        {
            InitializeComponent();
            this.obs = obs;
            DataContext = obs;
            this.chatRoom = chatRoom;
            this.ResizeMode = ResizeMode.NoResize;
            this.msg = msg;
            obs.TxtEditContent = msg.Body;
        }

        /// <summary>
        /// allows the user to edit his own message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditMessage_Click(object sender, RoutedEventArgs e)
        {
            if (obs.TxtEditContent.Length > 100)
                MessageBox.Show("The message length can't be longer then 100 characters");
            else
            {
                chatRoom.updateMessage(obs.TxtEditContent, msg);
                Close();
            }
            
        }

        /// <summary>
        /// if the user don't eant to edit his message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void BtnIgnoreMessage_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
