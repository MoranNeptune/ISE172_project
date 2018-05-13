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
    public partial class ViewProfileWindow : Window
    {
        MainWindow mainWindow;
        ChatRoom chatRoom;

        public ViewProfileWindow(MainWindow mainWindow, ChatRoom chatRoom)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.chatRoom = chatRoom;
            //this.BtnLogin.IsEnabled = false;
            //this.LblAddReg.Visibility.Equals(false);
            //this.BtnRegister.Visibility = Visibility.Hidden;
            
        }

        
        
    }
}