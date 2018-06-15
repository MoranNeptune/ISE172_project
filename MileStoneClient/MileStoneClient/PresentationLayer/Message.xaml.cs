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
        private String lastMSG;
        private List<GuiMessage> msgs;

        public Message(ObservableObject obs, ChatRoom chatRoom, GuiMessage msg,List<GuiMessage> msgs)//String lastMSG)
        {
            InitializeComponent();
            this.obs = obs;
            DataContext = obs;
            this.chatRoom = chatRoom;
            this.ResizeMode = ResizeMode.NoResize;

            //this.lastMSG = lastMSG;
            this.msgs= msgs;
            obs.TxtEditContent = msgs[obs.ListBoxSelectedIndex].Body;
        }

        // if the user edit his message
        private void BtnEditMessage_Click(object sender, RoutedEventArgs e)
        {
            chatRoom.updateMessage(obs.TxtEditContent,msgs[obs.ListBoxSelectedIndex]);
        }

        // if the user don't eant to edit his message
        private void BtnIgnoreMessage_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
