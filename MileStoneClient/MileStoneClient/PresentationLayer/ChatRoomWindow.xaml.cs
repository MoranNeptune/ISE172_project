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
        private List<Action> actionList;
        private int order;
        private List<GuiMessage> msgs;
        private List<string> nicknames, groups;

        public ChatRoomWindow(MainWindow mainWindow, ChatRoom chatRoom, ObservableObject obs)
        {
            this.obs = obs;
            InitializeComponent();
            this.chatRoom = chatRoom;
            this.mainWindow = mainWindow;
            this.DataContext = obs;
            nicknames = chatRoom.getNicknames();
            groups = chatRoom.getGroups();
            op = new Options(this, nicknames, groups, obs);
            actionList = new List<Action>();
            actionList.Add(new SortByTime());
            actionList.Add(null);
            msgs = new List<GuiMessage>();
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
                actionList.Clear();

                //list == order,sort,filter
                switch (orderChoice)
                {
                    case "ascending":
                        order = 0;
                        break;
                    case "decending":
                        order = 1;
                        break;
                }

                // adds the choosen sort to the action list
                switch (sortChoice)
                {
                    case "name":
                        actionList.Add(new SortByName());
                        break;
                    case "all":
                        actionList.Add(new SortByGNT());
                        break;
                    case "time":
                        actionList.Add(new SortByTime());
                        break;
                }
                // adds the choosen filter to the action list
                switch (filterChoice)
                {
                    case "none":
                        actionList.Add(null);
                        break;
                    case "group":
                        actionList.Add(new FilterByGroup(op.GroupChoice));
                        break;
                    case "user":
                        actionList.Add(new FilterByUser(op.UserChoice, op.GroupChoice));
                        break;
                }
                getMessagesList();
            }
            // אולי להוריד
           else
             getMessagesList();
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
            if (!obs.TxtSendContent.Equals(""))
            {
                obs.TxtSendContent = "";
            }
        }
        
        private void ViewProfile(object sender, RoutedEventArgs e)
        {
            ViewProfileWindow viewProfileWindow = new ViewProfileWindow(this.mainWindow, this.chatRoom);
            viewProfileWindow.Show();
            // רק לבדיקת פילטרים
            
            
            {
                order = 0;
                actionList.Clear();
                actionList.Add(new SortByName());
                actionList.Add(new FilterByGroup("21"));
                getMessagesList();
                actionList.Clear();
                            }

            
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

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// return list of the users in a given group
        public List<string> getMembersOf(string g_id)
        {
            if (g_id != null)
                return chatRoom.getMembersOf(g_id);
            return null;
        }

        //display all the messages
        private void getMessagesList()
        {
           obs.Messages.Clear();
           msgs = chatRoom.getMessages(order, actionList);
            // convers all the Gui Messages to a string
            for (int i = 0; i < msgs.Count; i++)
            {
                obs.Messages.Add(msgs[i].ToString());
            }
        }

        //initiate listBox wuth the messages list
        private void updateMessages(object sender, RoutedEventArgs e)
        {
            msgs = chatRoom.getMessages(0, actionList);
            for (int i = 0; i < msgs.Count; i++)
            {
                obs.Messages.Add(msgs[i].ToString());
            }
        }
    
        // sendsa message by pressing enter
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Send(sender, e);
            }
        }
    }
}
