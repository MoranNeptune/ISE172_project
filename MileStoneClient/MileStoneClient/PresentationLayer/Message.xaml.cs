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

        public Message(ObservableObject obs, ChatRoom chatRoom)
        {
            InitializeComponent();
            this.obs = obs;
            DataContext = obs;
            this.chatRoom = chatRoom;

            obs.TxtEditContent = obs.ListBoxSelectedValue;
        }

        // לאתחל שוב ברגע שעדן יוצרת עריכת הודעהההההההההה
        private void BtnEditMessage_Click(object sender, RoutedEventArgs e)
        {
            //chatRoom.editMessage(obs.TxtEditContent);
        }
    }
}
