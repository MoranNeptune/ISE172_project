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
        private ObservableObject obs;//binding
        private Hashing hashing;
        private readonly String salt;

        public LoginWindow(MainWindow mainWindow, ChatRoom chatRoom, ObservableObject obs)
        {
            Log.Instance.info("Login window opened"); //log

            InitializeComponent();
            this.obs = obs;
            DataContext = obs;
            this.mainWindow = mainWindow;
            this.chatRoom = chatRoom;
            this.hashing = new Hashing();
            this.salt="1337";
            //initialize the buttons and messages that the user dont need to have access to with Hidden&notEnable option
            obs.BtnLoginIsEnabled = false;
            obs.LblAddRegVisibility = "Hidden";
            obs.BtnRegisterVisibility = "Hidden";
        }

        //check validity of the login, if not- let the user option to regiester
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            int number;
            // A validity check of the NickName
            if (obs.NicknameContent[0] == ' ')// if the user presses space 
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
            else if (int.TryParse(obs.GroupIdContent, out number) == false || (obs.GroupIdContent.Length > 2))
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
            // A validity check of the password
            else if (!(4 <= obs.PasswordContent.Length && obs.PasswordContent.Length <= 16))
            {
                if (MessageBox.Show("password length should be between 4 to 16 letters", "Invalid message", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {// if the length of 
                    obs.GroupIdText = "";
                    obs.NicknameText = "";
                    obs.PasswordContent = "";
                }
                else if (obs.PasswordContent.Equals("") || isPassOnlySpaces() || obs.PasswordContent[0] == 13)
                {// if the password is empty or full with spaces or enter 
                    if (MessageBox.Show("Message cannot be empty or with spaces", "Invalid message", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        obs.GroupIdText = "";
                        obs.NicknameText = "";
                        obs.PasswordContent = "";
                    }
                }
                else if (!PasswordVlidity())
                {// if the password conteins the correct letters
                    if (MessageBox.Show("Message should include only letters and numbers", "Invalid message", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        obs.GroupIdText = "";
                        obs.NicknameText = "";
                        obs.PasswordContent = "";
                    }
                }
                Log.Instance.warn("Invalid input - Invalid Password");//log
            }
            // checks if the user is already registered or not
            else if (this.chatRoom.login(obs.NicknameContent, obs.GroupIdContent , hashing.GetHashString(obs.PasswordContent + salt)) == false)
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
                //if the user is not registered give him an option to register
                obs.LblAddRegVisibility = "Visible";
                obs.LblAddRegContent = "Try to register :";
                obs.BtnRegisterVisibility = "Visible";
            }
            else
            { // if the inputs are correct
                Log.Instance.info("New log-in - User: " + obs.NicknameContent);//log

                obs.GroupIdContent = "";
                obs.NicknameContent = "";
                obs.LblAddRegVisibility = "Visible";
                this.Close();
                ChatRoomWindow chatRoomWin = new ChatRoomWindow(this.mainWindow, this.chatRoom, obs);
                chatRoomWin.Show();
            }
        }

        // if the passeord contains the correct letters
        private bool PasswordVlidity()
        {
            String str = obs.PasswordContent;
            for (int i = 0; i < str.Length; i++)
                if (!((str[i] <= 48 && str[i] <= 57) || (str[i] <= 65 && str[i] <= 90) || (str[i] <= 97 && str[i] <= 122)))
                    return false;
            return true;
        }

        // if the password contains only spaces
        private bool isPassOnlySpaces()
        {
            bool ans = true;
            string tMsg = obs.TxtSendContent;

            for (int i = 0; i < tMsg.Length; i++)
            {
                if (tMsg[i] != ' ')
                {
                    ans = false;
                }
            }
            return ans;
        }

        //click this button let the user to return to the previous window (the menu)
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Log.Instance.info("User returned from login window to main window");

            this.Close();
            this.mainWindow.Show();
            obs.LblAddRegContent = "";
            obs.LblAddRegVisibility = "Hidden";
            obs.BtnRegisterVisibility = "Hidden";
        }

        //only if the textboxs of the "username" and "group ID" aren't empty allow the user to press login
        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            obs.BtnLoginIsEnabled = !string.IsNullOrEmpty(obs.NicknameContent)
                && !string.IsNullOrEmpty(obs.GroupIdContent);
            obs.LblAddRegVisibility = "Hidden";
            obs.LblAddRegContent = "";
            obs.BtnRegisterVisibility = "Hidden";
        }

        //a button that connect between the the LoginWindow to the RegiesterWindow
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            Log.Instance.info("Login window closed");

            RegisterWindow register = new RegisterWindow(this.mainWindow, this.chatRoom, obs);
            this.Close();
            register.Show();
        }
    }
}