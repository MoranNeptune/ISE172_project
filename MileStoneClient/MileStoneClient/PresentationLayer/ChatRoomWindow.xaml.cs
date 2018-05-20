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
using System.Windows.Threading;
using MileStoneClient.BusinessLayer;

namespace MileStoneClient.PresentationLayer
{
    /// <summary>
    /// Interaction logic for ChatRoom.xaml
    /// </summary>
    public partial class ChatRoomWindow : Window
    {
        private ChatRoom chatRoom;
        private MainWindow mainWindow;
        private ObservableObject obs;
        private bool isOptionsVisible;
        private Options op;
        private string orderChoice, filterChoice, sortChoice;
        private int order;
        private List<string> nicknames, groups;

        public ChatRoomWindow(MainWindow mainWindow, ChatRoom chatRoom, ObservableObject obs)
        {
            this.obs = obs;
            InitializeComponent();
            this.chatRoom = chatRoom;
            this.mainWindow = mainWindow;
            DataContext = obs;
            nicknames = chatRoom.getNicknames();
            groups = chatRoom.getGroups();
            op = new Options(this, nicknames, groups, obs);
            isOptionsVisible = false;
            orderChoice = "ascending";
            filterChoice = "none";
            sortChoice = "time";


            //initiate timer
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            dispatcherTimer.Start();



        }

        //print messages with timer
        private void dispatcherTimer_Tick(object sender, EventArgs e)

        {
            //if another sort/filter/order was chosen
            if (op.IsChanged)
            {
                orderChoice = op.OrderChoice;
                filterChoice = op.FilterChoice;
                sortChoice = op.SortChoice;
                op.IsChanged = false;
            }
            //list == order,sort,filter
            switch (orderChoice)
            {
                case "ascending":
                    break;
                case "decending":
                    break;
            }
            // adds the choosen sort to the action list
            switch (sortChoice)
            {
                case "name":
                    break;
                case "all":
                    break;
                case "time":
                    break;
            }
            // adds the choosen filter to the action list
            switch (filterChoice)
            {
                case "none":
                    break;
                case "group":
                    break;
                case "user":
                    break;
            }
            //getActions(order, filter, sort);
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            this.chatRoom.logOut();
            Close();
            this.mainWindow.Show();
        }

        private void Send(object sender, RoutedEventArgs e)
        {
            chatRoom.send(obs.TxtSendContent);

            if (obs.TxtSendContent.Equals("what is on your mind?"))
            {
                //error
                //check if the text is by standarts and pop up error message if not
                //send the message
                //clear the txt box
                obs.TxtSendContent = "what is on your mind?";
            }
        }

        private void ViewProfile(object sender, RoutedEventArgs e)
        {
            ViewProfileWindow viewProfileWindow = new ViewProfileWindow(this.mainWindow, this.chatRoom);
            viewProfileWindow.Show();
        }

        private void Options(object sender, RoutedEventArgs e)
        {
            //close options menu
            if (isOptionsVisible)
            {
                obs.IsOptionVisible = null;
                isOptionsVisible = false;
            }
            //open options menu
            else
            {
                //op = new Options(obs);
                obs.IsOptionVisible = op;
                isOptionsVisible = true;
            }
        }
    }
}
