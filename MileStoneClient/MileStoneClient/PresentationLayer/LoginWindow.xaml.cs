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

namespace MileStoneClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        MainWindow mainWindow;
        ChatRoom chatRoom;

        public LoginWindow(MainWindow mainWindow, ChatRoom chatRoom)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.chatRoom = chatRoom;
            this.BtnLogin.IsEnabled = false;
            this.LblAddReg.Visibility.Equals(false);
            this.BtnRegister.Visibility = Visibility.Hidden;
            
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            string g_id = this.TxtG_Id.Text;
            string NickName = this.TxtNickN.Text;

            // checks if the user is already regester or not
            if (this.chatRoom.login(NickName, g_id) == false)
            {
                string message = "please register first and then try to login again";
                string caption = "You are not registered";
                if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                {
                    this.TxtG_Id.Clear();
                    this.TxtNickN.Clear();
                }
                //Log.Instance.error("Log-in fail - User not registered");//log
                this.TxtG_Id.Clear();
                this.TxtNickN.Clear();
                this.LblAddReg.Content = "Try to register :";
                this.BtnRegister.Visibility = Visibility.Visible;
            }
            // if the user is resister 
            else
            {
                int number;
                // A validity check of the NickName
                if (NickName[0] == ' ')// if the user press space 
                {
                    string message = "Nickname cannot start with spaces!";
                    string caption = "Invalid name";
                    if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                    {
                        this.TxtNickN.Clear();
                        this.TxtNickN.Clear();
                    }
                    //Log.Instance.warn("Invalid input - Invalid nickname");//log
                }
                // A validity check of the group id
                else if (int.TryParse(g_id, out number) == false || (!(int.Parse(g_id) < 100 && int.Parse(g_id) > 0)))
                {// if the group Id is not between 1-99
                    string message = "You sould only enter numbers between 1 to 99!";
                    string caption = "Invalid group ID";
                    if ((MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK))
                    {
                        this.TxtG_Id.Clear();
                        this.TxtNickN.Clear();
                    }
                    //Log.Instance.warn("Invalid input - Invalid ID");//log 
                }
                else // if the inputs are correct
                {
                    string msg = "Logged in successfully!";
                    string cap = "Congratulations!";
                    if ((MessageBox.Show(msg, cap, MessageBoxButton.OK, MessageBoxImage.Exclamation) == MessageBoxResult.OK))
                    {
                        this.TxtG_Id.Clear();
                        this.TxtNickN.Clear();
                        this.LblAddReg.Visibility.Equals(true);
                    }
                    //Log.Instance.info("New log-in - User: " + NickName);//log
                    this.Close();
                    ChatRoomWindow chatRoomWin = new ChatRoomWindow(this.mainWindow,this.chatRoom);
                    chatRoomWin.Show();
                }
            }
            //Log.Instance.info("New registration - User: " + NickName);//log
        }



        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.mainWindow.Show();
            this.LblAddReg.Content = "";
            this.LblAddReg.Visibility.Equals(false);
            this.BtnRegister.Visibility = Visibility.Hidden;
        }

        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.BtnLogin.IsEnabled = !string.IsNullOrEmpty(this.TxtG_Id.Text) &&
                !string.IsNullOrEmpty(this.TxtNickN.Text);

            this.LblAddReg.Visibility.Equals(false);
            this.LblAddReg.Content = "";
            this.BtnRegister.Visibility = Visibility.Hidden;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow(this.mainWindow, this.chatRoom);
            this.Close();
            register.Show();
        }
    }
}