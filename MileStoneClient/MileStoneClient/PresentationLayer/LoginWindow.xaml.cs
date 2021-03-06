﻿using System;
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
        private String HashedPassword;
        private bool correctPass;
        private readonly String salt;

        public LoginWindow(MainWindow mainWindow, ChatRoom chatRoom, ObservableObject obs)
        {
            Log.Instance.info("Login window opened"); //log

            InitializeComponent();
            this.obs = obs;
            this.HashedPassword = "";
            DataContext = obs;
            this.mainWindow = mainWindow;
            this.chatRoom = chatRoom;
            this.ResizeMode = ResizeMode.NoResize;

            this.hashing = new Hashing();
            this.salt = "1337";
            this.correctPass = false;

            //initialize the buttons and messages that the user dont need to have access to with Hidden&notEnable option
            obs.BtnLoginIsEnabled = false;
            obs.LblAddRegVisibility = "Hidden";
            obs.BtnRegisterVisibility = "Hidden";
        }

        /// <summary>
        ///  check validity of the login, if not- let the user option to regiester
        /// </summary>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            int number;
            // A validity check of the NickName
            if (obs.NicknameContent[0] == ' ' || (obs.NicknameContent.Length > 8))// if the user presses space 
            {
                Log.Instance.warn("Invalid input - Invalid nickname");//log

                string message = "Nickname cannot start with spaces or be longer than 8 digits!";
                string caption = "Invalid name";
                if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                {
                    this.obs.GroupIdContent = "";
                    this.obs.NicknameContent = "";
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
                    this.obs.GroupIdContent = "";
                    this.obs.NicknameContent = "";
                }
            }
            // A validity check of the password
            else if (!correctPass)
            {
                if (MessageBox.Show("Password mast contain 4 to 16 letters", "Invalid message", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    this.obs.GroupIdContent = "";
                    this.obs.NicknameContent = "";
                }
                Log.Instance.warn("Invalid input - Invalid Password");//log
            }
            // A validity check of the password and the BL
            // checks if the user is already registered or not
            else if (correctPass & this.chatRoom.login(obs.NicknameContent, obs.GroupIdContent, this.HashedPassword) == false)
            {
                Log.Instance.error("Log-in fail - User not registered");//log

                string message = "please register first and then try to login again";
                string caption = "You are not registered";
                if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                {
                    this.obs.GroupIdContent = "";
                    this.obs.NicknameContent = "";

                }
                this.obs.GroupIdContent = "";
                this.obs.NicknameContent = "";
                //if the user is not registered give him an option to register
                obs.LblAddRegVisibility = "Visible";
                obs.LblAddRegContent = "Try to register :";
                obs.BtnRegisterVisibility = "Visible";
            }
            // if the inputs are correct
            else
            {
                Log.Instance.info("New log-in - User: " + obs.NicknameContent);//log

                this.obs.GroupIdContent = "";
                this.obs.NicknameContent = "";
                HashedPassword = "";
                obs.LblAddRegVisibility = "Visible";
                this.Close();
                ChatRoomWindow chatRoomWin = new ChatRoomWindow(this.mainWindow, this.chatRoom, obs);
                chatRoomWin.Show();
            }
        }

        /// <summary>
        /// click this button let the user to return to the previous window (the menu)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Log.Instance.info("User returned from login window to main window");

            this.Close();
            this.mainWindow.Show();
            obs.LblAddRegContent = "";
            obs.LblAddRegVisibility = "Hidden";
            obs.BtnRegisterVisibility = "Hidden";
        }

        /// <summary>
        /// only if the textboxs of the "username" and "group ID" aren't empty allow the user to press login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            obs.BtnLoginIsEnabled = !string.IsNullOrEmpty(obs.NicknameContent)
                && !string.IsNullOrEmpty(obs.GroupIdContent);
            obs.LblAddRegVisibility = "Hidden";
            obs.LblAddRegContent = "";
            obs.BtnRegisterVisibility = "Hidden";
        }

        /// <summary>
        /// a button that connect between the the LoginWindow to the RegiesterWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            Log.Instance.info("Login window closed");

            RegisterWindow register = new RegisterWindow(this.mainWindow, this.chatRoom, obs);
            this.Close();
            register.Show();
        }

        /// <summary>
        /// when the password change we check the validity of the password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = sender as PasswordBox;
            if (4 <= pb.Password.Length)
            {
                // A validity check of the password
                if (!(pb.Password.Length <= 16))
                {// if the length of the password is not correct
                    if (MessageBox.Show("password length should be between 4 to 16 letters", "Invalid message", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        this.obs.GroupIdContent = "";
                        this.obs.NicknameContent = "";
                        pb.Password = "";
                    }
                    Log.Instance.warn("Invalid input - Invalid Password");//log
                    this.correctPass = false;
                }
                else if (pb.Password.Equals("") || isPassOnlySpaces(pb.Password) || pb.Password[0] == 13)
                {// if the password is empty or full with spaces or enter 
                    if (MessageBox.Show("Message cannot be empty or with spaces", "Invalid message", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        this.obs.GroupIdContent = "";
                        this.obs.NicknameContent = "";
                        pb.Password = "";
                    }
                    Log.Instance.warn("Invalid input - Invalid Password");//log
                    this.correctPass = false;
                }
                else if (!PasswordVlidity(pb.Password))
                {// if the password conteins the correct letters
                    if (MessageBox.Show("Message should include only letters and numbers", "Invalid message", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        this.obs.GroupIdContent = "";
                        this.obs.NicknameContent = "";
                        pb.Password = "";
                    }
                    Log.Instance.warn("Invalid input - Invalid Password");//log
                    this.correctPass = false;
                }
                else correctPass = true;

                if (correctPass)
                {
                    this.HashedPassword = hashing.GetHashString(pb.Password + salt);
                }
            }
            else this.correctPass = false;
        }

        /// <summary>
        /// if the passeord contains the correct letters
        /// </summary>
        /// <param name="pb"></param>
        /// <returns></returns>        
        private bool PasswordVlidity(String pb)
        {
            for (int i = 0; i < pb.Length; i++)
                if (!((pb[i] >= 48 && pb[i] <= 57) || (pb[i] >= 65 && pb[i] <= 90) || (pb[i] >= 97 && pb[i] <= 122)))
                    return false;
            return true;
        }

        /// <summary>
        /// if the password contains only spaces
        /// </summary>
        /// <param name="pb"></param>
        /// <returns></returns>        
        private bool isPassOnlySpaces(String pb)
        {
            bool ans = true;

            for (int i = 0; i < pb.Length; i++)
                if (pb[i] != ' ')
                    ans = false;
            return ans;
        }
    }
}
