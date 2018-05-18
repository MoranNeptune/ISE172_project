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

        public RegisterWindow(MainWindow mainWindow,ChatRoom chatRoom)
        {
            InitializeComponent();
            this.chatRoom = chatRoom;
            this.mainWindow = mainWindow;
            this.BtnRegister.IsEnabled = false;
            this.LblRegError.Visibility.Equals(false);
            this.LblAddLogin.Visibility.Equals(false);
            this.BtnLogin.Visibility = Visibility.Hidden;
        }

        public ChatRoom GetChatRoom()
        {
            return this.chatRoom;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.mainWindow.Show();
            this.LblRegError.Visibility.Equals(false);
            this.LblRegError.Content = "";
            this.LblAddLogin.Content = "";
            this.LblAddLogin.Visibility.Equals(false);
            this.BtnLogin.Visibility = Visibility.Hidden;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            string g_id = this.TxtG_Id.Text;
            string NickName = this.TxtNickN.Text;

            // checks if the user is already regester or not
            if (this.chatRoom.register(NickName, g_id) == false)
            {

                this.LblRegError.Visibility.Equals(true);
                this.LblRegError.Content = "This name is already exist in this group, try another name.";
                this.TxtG_Id.Clear();
                this.TxtNickN.Clear();
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
                        this.TxtG_Id.Clear();
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
                    this.LblRegError.Visibility.Equals(false);
                    this.LblRegError.Content = "";
                    string msg = "Registered successfully!";
                    string cap = "Congratulations!";
                    if ((MessageBox.Show(msg, cap, MessageBoxButton.OK, MessageBoxImage.Exclamation) == MessageBoxResult.OK))
                    {
                        this.TxtG_Id.Clear();
                        this.TxtNickN.Clear();
                        this.LblAddLogin.Visibility.Equals(true);
                        this.LblAddLogin.Content = "Now that you are register:";
                        this.BtnLogin.Visibility = Visibility.Visible;
                    }

                }
            }
            //Log.Instance.info("New registration - User: " + NickName);//log
        }

        private void TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.BtnRegister.IsEnabled = !string.IsNullOrEmpty(this.TxtG_Id.Text) &&
                !string.IsNullOrEmpty(this.TxtNickN.Text);

            this.LblAddLogin.Visibility.Equals(false);
            this.LblAddLogin.Content = "";
            this.BtnLogin.Visibility = Visibility.Hidden;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow(this.mainWindow,this.chatRoom);
            this.Close();
            login.Show();
        }
    }
}