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
using System.Windows.Threading;
using MileStoneClient.PresentationLayer;

namespace MileStoneClient.PresentationLayer
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        MainWindow mainWindow;
        public ChatRoom chatRoom;
        public ObservableObject obs = new ObservableObject();
        String nickname;
        String groupId;
        public LoginWindow(MainWindow mainWindow, ChatRoom chatRoom)
        {
            InitializeComponent();
            this.DataContext = obs;
            this.mainWindow = mainWindow;
            this.chatRoom = chatRoom;
            obs.BtnLoginIsEnabled = false;
            obs.LblAddRegVisibility="Hidden";
            obs.BtnRegisterVisibility = "Hidden";
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // checks if the user is already regester or not
            if (this.chatRoom.login(nickname, groupId) == false)
            {
                string message = "please register first and then try to login again";
                string caption = "You are not registered";
                if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                {
                    this.obs.GroupIdContent = "";
                    this.obs.NicknameContent = "";
                }
                //Log.Instance.error("Log-in fail - User not registered");//log
                obs.GroupIdContent = "";
                obs.NicknameContent = "";
                obs.LblAddRegVisibility = "Visible";
                obs.LblAddRegContent = "Try to register :";
                obs.BtnRegisterVisibility = "Visible";

            }
            // if the user is resister 
            else
            {
                int number;
                // A validity check of the NickName
                if (nickname[0] == ' ')// if the user press space 
                {
                    string message = "Nickname cannot start with spaces!";
                    string caption = "Invalid name";
                    if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                    {
                        obs.GroupIdContent = "";
                        obs.NicknameContent = "";
                    }
                    //Log.Instance.warn("Invalid input - Invalid nickname");//log
                }
                // A validity check of the group id
                else if (int.TryParse(groupId, out number) == false || (!(int.Parse(groupId) < 100 && int.Parse(groupId) > 0)))
                {// if the group Id is not between 1-99
                    string message = "You sould only enter numbers between 1 to 99!";
                    string caption = "Invalid group ID";
                    if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                    {
                        obs.GroupIdContent = "";
                        obs.NicknameContent = "";
                    }
                    //Log.Instance.warn("Invalid input - Invalid ID");//log 
                }
                else // if the inputs are correct
                {
                    /*string msg = "Logged in successfully!";
                    string cap = "Congratulations!";
                    if ((MessageBox.Show(msg, cap, MessageBoxButton.OK, MessageBoxImage.Exclamation) == MessageBoxResult.OK))
                    {
                        obs.GroupIdContent = "";
                        obs.NicknameContent = "";
                        obs.LblAddRegVisibility = "Visible";
                    }*/
                    obs.GroupIdContent = "";
                    obs.NicknameContent = "";
                    obs.LblAddRegVisibility = "Visible";
                    //Log.Instance.info("New log-in - User: " + NickName);//log
                    this.Close();
                    ChatRoomWindow chatRoomWin = new ChatRoomWindow(this.mainWindow, this.chatRoom);
                    chatRoomWin.Show();
                }
            }
            //Log.Instance.info("New registration - User: " + NickName);//log
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.mainWindow.Show();
            obs.LblAddRegContent = "";
            obs.LblAddRegVisibility = "Hidden";
            obs.BtnRegisterVisibility = "Hidden";
        }

       private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            obs.BtnLoginIsEnabled = !string.IsNullOrEmpty(obs.NicknameContent) 
                && !string.IsNullOrEmpty(obs.GroupIdContent);
            obs.LblAddRegVisibility = "Hidden";
            obs.LblAddRegContent = "";
            obs.BtnRegisterVisibility = "Hidden";
            this.nickname = obs.NicknameContent;
            this.groupId = obs.GroupIdContent;
        }
   
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
             RegisterWindow register = new RegisterWindow(this.mainWindow, this.chatRoom);
             this.Close();
             register.Show();
   
        }
    }
}