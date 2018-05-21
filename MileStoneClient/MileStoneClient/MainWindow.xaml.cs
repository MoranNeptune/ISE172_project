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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MileStoneClient.BusinessLayer;
using MileStoneClient.Logger;

namespace MileStoneClient.PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RegisterWindow register;
        private LoginWindow login;
        public string url;
        private ChatRoom chatRoom;
        private List<Message> retMessages;
        private ObservableObject obs;

        public MainWindow()
        {
            Log.Instance.info("Program debugged and started successfully");// log

            url = "http://ise172.ise.bgu.ac.il:80";
            obs = new ObservableObject();
            chatRoom = new ChatRoom(url);
            InitializeComponent();
        }

        public RegisterWindow GetRegister()
        {
            return this.register;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            //ChatRoomWindow cr = new ChatRoomWindow(this, chatRoom, obs);
            //cr.Show();

            Log.Instance.info("Registration window opened"); //log

            this.register = new RegisterWindow(this,this.chatRoom, obs); 
            register.Show();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Log.Instance.info("Login window opened"); //log

            this.login = new LoginWindow(this,this.chatRoom, obs);
            login.Show();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Log.Instance.info("Program exited by user");// log
            this.Close();
        }
    }
}
