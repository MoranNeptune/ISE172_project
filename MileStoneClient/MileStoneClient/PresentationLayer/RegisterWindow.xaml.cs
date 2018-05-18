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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        ChatRoom chatRoom;
        MainWindow mainWindow;
        ObservableObject obs = new ObservableObject();
        String nickname;
        String groupId;

        public RegisterWindow(MainWindow mainWindow, ChatRoom chatRoom)
        {
            InitializeComponent();
            this.chatRoom = chatRoom;
            this.mainWindow = mainWindow;
            this.DataContext = obs;
            obs.BtnRegIsEnabled = false;
            obs.LblRegErrorVisibility = "Hidden";
            obs.LblAddLoginVisibility = "Hidden";
            obs.BtnLoginVisibility = "Hidden";
        }

        public ChatRoom GetChatRoom()
        {
            return this.chatRoom;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.mainWindow.Show();
            obs.LblRegErrorVisibility = "Hidden";
            obs.LblRegErrorContent = "";
            obs.LblAddLoginContent = "";
            obs.LblAddLoginVisibility = "Hidden";
            obs.BtnLoginVisibility = "Hidden";
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        { 
            // checks if the user is already regester or not
            if (this.chatRoom.register(this.nickname, this.groupId) == false)
            {

                obs.LblRegErrorVisibility = "Hidden";
                obs.LblRegErrorContent = "This name is already exist in this group, try another name.";
                obs.GroupIdText = "";
                obs.NicknameText = "";
            }
            // if the user is resister 
            else
            {
                int number;
                // A validity check of the NickName
                if (this.nickname[0] == ' ')// if the user press space 
                {
                    string message = "Nickname cannot start with spaces!";
                    string caption = "Invalid name";
                    if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                    {
                        obs.GroupIdText = "";
                        obs.NicknameText = "";
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
                        obs.GroupIdText = "";
                        obs.NicknameText = "";
                    }
                    //Log.Instance.warn("Invalid input - Invalid ID");//log 
                }
                else // if the inputs are correct
                {
                    /*obs.LblRegErrorVisibility = false;
                    obs.LblRegErrorContent = "";
                    string msg = "Registered successfully!";
                    string cap = "Congratulations!";
                    if ((MessageBox.Show(msg, cap, MessageBoxButton.OK, MessageBoxImage.Exclamation) == MessageBoxResult.OK))
                    {
                        obs.GroupIdText = "";
                        obs.NicknameText = "";
                        obs.LblAddLoginVisibility = true;
                        obs.LblAddLoginContent = "Now that you are register:";
                        obs.BtnLoginVisibility = "Visible";
                    }*/
                    obs.GroupIdText = "";
                    obs.NicknameText = "";
                    obs.LblAddLoginVisibility = "Visible";
                    obs.LblAddLoginContent = "Now that you are register:";
                    obs.BtnLoginVisibility = "Visible";
                }
            }
            //Log.Instance.info("New registration - User: " + nickname);//log

        }

        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            obs.BtnRegIsEnabled = !string.IsNullOrEmpty(obs.NicknameContent)
                && !string.IsNullOrEmpty(obs.GroupIdContent);
            obs.LblAddLoginVisibility = "Hidden";
            obs.LblAddLoginContent = "";
            obs.BtnLoginVisibility = "Hidden";
            this.nickname = obs.NicknameContent;
            this.groupId = obs.GroupIdContent;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow(this.mainWindow, this.chatRoom);
            this.Close();
            login.Show();
        }

    }
}