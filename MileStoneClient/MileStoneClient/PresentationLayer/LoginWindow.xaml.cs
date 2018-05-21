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
using MileStoneClient.Logger;

namespace MileStoneClient.PresentationLayer
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private MainWindow mainWindow;
        private ChatRoom chatRoom;
        private ObservableObject obs;
        private string nickname;
        private string groupId;

        public LoginWindow(MainWindow mainWindow, ChatRoom chatRoom, ObservableObject obs)
        {
            Log.Instance.info("Login window opened"); //log

            InitializeComponent();
            this.obs = obs;
            DataContext = obs;
            this.mainWindow = mainWindow;
            this.chatRoom = chatRoom;
            obs.BtnLoginIsEnabled = false;
            obs.LblAddRegVisibility = "Hidden";
            obs.BtnRegisterVisibility = "Hidden";
        }

        //add check for group string validity
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            int number;
            // A validity check of the NickName
            if (nickname[0] == ' ')// if the user presses space 
            {
                Log.Instance.warn("Invalid input - Invalid nickname");//log

                string message = "Nickname cannot start with spaces!";
                string caption = "Invalid name";
                if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                {
                    obs.GroupIdContent = "";
                    obs.NicknameContent = "";
                }
            }
            // A validity check of the group id
            else if (int.TryParse(groupId, out number) == false || (groupId.Length > 2))
            {
                Log.Instance.warn("Invalid input - Invalid group number");//log 

                string message = "You sould only enter numbers between 1 to 99!";
                string caption = "Invalid group ID";
                if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                {
                    obs.GroupIdContent = "";
                    obs.NicknameContent = "";
                }
            }
            // checks if the user is already registered or not
            else if (this.chatRoom.login(nickname, groupId) == false)
            {
                Log.Instance.error("Log-in fail - User not registered");//log

                string message = "please register first and then try to login again";
                string caption = "You are not registered";
                if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                {
                    this.obs.GroupIdContent = "";
                    this.obs.NicknameContent = "";
                }
                obs.GroupIdContent = "";
                obs.NicknameContent = "";
                obs.LblAddRegVisibility = "Visible";
                obs.LblAddRegContent = "Try to register :";
                obs.BtnRegisterVisibility = "Visible";
            }
            else
            { // if the inputs are correct
                Log.Instance.info("New log-in - User: " + nickname);//log

                obs.GroupIdContent = "";
                obs.NicknameContent = "";
                obs.LblAddRegVisibility = "Visible";
                this.Close();
                ChatRoomWindow chatRoomWin = new ChatRoomWindow(this.mainWindow, this.chatRoom, obs);
                chatRoomWin.Show();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Log.Instance.info("User returned from login window to main window");

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
            Log.Instance.info("Login window closed");

            RegisterWindow register = new RegisterWindow(this.mainWindow, this.chatRoom, obs);
            this.Close();
            register.Show();
        }
    }
}