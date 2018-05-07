using System;
using System.Collections.Generic;
using MileStoneClient.BusinessLayer;
using MileStoneClient.Logger;

namespace MileStoneClient.PresentationLayer
{
    /// <summary>
    ///  The presentation layer represents the GUI of the program.
    ///  This is the only class in the presentation later.
    /// </summary>
    class Presentation
    {
        public const string url = "http://ise172.ise.bgu.ac.il:80";
        public const int displayNum = 20;
        private ChatRoom chatRoom = new ChatRoom(url);
        private List<Message> retMessages;
        private bool boolLogout = false;
        private bool boolExit = false;

        /// <summary>
        /// This is the main method Of the project.
        /// </summary>
        /// <param name="args"></param>
        public void Main(string[] args)
        {
            PrintOnScreenStart();
        }

        /// <summary>
        /// A private function that prints the menu on the screen at the begining of the program.
        /// </summary>
        private void PrintOnScreenStart()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Welcome to the ChatRoom!");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Choose only one option :");
            Console.ResetColor();
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("NOTE : In order to return to the menu, press '- 1'.");
            Console.ResetColor();

            string choiseStart = Console.ReadLine(); // call's to the function that sort the users' choise.
            ChooseOptionsStart(choiseStart);
        }

        /// <summary>
        /// A private function that sort the option that the user entered and deliveres it to it's specific function.
        /// </summary>
        /// <param name="choiseStart"> the option that the user entered to the program </param>
        private void ChooseOptionsStart(string choiseStart)
        {
            if (choiseStart.Length == 1) // if the length of the option that the user entered is correct
            {
                switch (choiseStart)
                {
                    case "1": // in case the user wants to register
                        Register();
                        PrintOnScreenStart();
                        break;
                    case "2": // in case that the user wants to login
                        this.boolLogout = false;
                        login();
                        break;
                    case "3": // in case that the user wants to exit
                        Exit();
                        break;
                    default: // in all other case
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please enter a valid choise from the menu!");
                            Console.ResetColor();
                            Log.Instance.warn("Invalid input - Invalid menu choice");//log 
                            choiseStart = Console.ReadLine();
                            ChooseOptionsStart(choiseStart);
                        }
                        break;
                }
            }
            else if (choiseStart.Equals("-1")) // if the user wants to go to the first menu
                PrintOnScreenStart();
            else // if the length of the option that the user entered is not correct
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid choise from the menu!");
                Console.ResetColor();
                Log.Instance.warn("Invalid input - Invalid menu choice");//log 
                choiseStart = Console.ReadLine();
                ChooseOptionsStart(choiseStart);
            }
        }

        /// <summary>
        /// A function that enables the user regester to the system.
        /// </summary>
        public void Register()
        {
            if (this.boolExit == false) // if the user is in the system
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Thank you for registering!");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("NOTE : In order to return to the menu, press '-1' at the group ID/NickName option!");
                Console.WriteLine("Warning ! Invalid group ID - ID should be 1-2 digits (from 1 to 99) !");
                Console.ResetColor();
                Console.Write("Group ID : ");
                string g_id = Console.ReadLine();
                g_id = CheckVlidityId(g_id);
                if (g_id != "-1")
                { // the user doesn't want to go to the menu.
                    Console.Write("NickName : ");
                    string NickName = Console.ReadLine();
                    NickName = CheckVlidityName(NickName);
                    if (NickName != "-1")
                    { // the user doesn't want to go to the menu.
                        if (this.chatRoom.register(NickName, g_id) == false)
                        { // checks if the user is already regester or not
                            do
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("The name : " + NickName + " already exist in this group, try another name.");
                                Console.ResetColor();
                                Log.Instance.warn("Invalid input - Nickname already exist");//log
                                Console.Write("NickName : ");
                                NickName = Console.ReadLine();
                                NickName = CheckVlidityName(NickName);
                                if (NickName == "-1")// go to the main menu
                                    PrintOnScreenStart();
                            }
                            while (this.chatRoom.register(NickName, g_id) == false || NickName == "-1");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Congratulations! Registered successfully!");
                            Console.ResetColor();
                            Log.Instance.info("New registration - User: " + NickName);//log
                        }
                        else // the user now is registered 
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Congratulations! Registered successfully!");
                            Console.ResetColor();
                            Log.Instance.info("New registration - User: " + NickName);//log
                        }
                    }
                }
            }
            else // if the user isn't in the system
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR ! You have to be logged-in and in the system.");
                Console.ResetColor();
                Log.Instance.error("Action fail - User not logged in");//log
                PrintOnScreenStart();
            }
        }


        /// <summary>
        /// A private function that checkes the validity of the g_id of the user.
        /// If the id is wrong, it calls to the function with anouter inputs of g_id's untill a correct id pressed 
        /// </summary>
        /// <param name="option"> the g_id that the user entered </param>
        /// <returns> returns a correct string for the g_id</returns>
        private string CheckVlidityId(string option)
        {
            if (((option.Length == 2) && ((option[0] >= 48 && option[0] <= 57) && (option[1] >= 48 && option[1] <= 57))) ||
                (option.Length == 1 && option[0] >= 48 && option[0] <= 57))// if the length is 2 and the number is between 01 to 99 or the length is 1 and the number is between 1 to 9
                return option;
            else
            {
                if (option == "-1") // if the user wants to go back to the menu
                    return option;
                else // if the user doesn't want to go back to the menu
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid group ID - You sould only enter numbers between 01 to 99 !");
                    Console.ResetColor();
                    Log.Instance.warn("Invalid input - Invalid ID");//log 
                    Console.Write("Group ID : ");
                    option = Console.ReadLine();
                    return CheckVlidityId(option);
                }
            }
        }


        /// <summary>
        /// A private function that checkes the validity of the nickname of the user.
        /// </summary>
        /// <param name="option"> the nickname that the user entered </param>
        /// <returns> returns a correct string for the nickname </returns>
        private string CheckVlidityName(string option)
        {
            if (option == "" || option == null)//if the user press enter or if the option is null
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid name! Nickname cannot start with spaces!");
                Console.ResetColor();
                Log.Instance.warn("Invalid input - Invalid nickname");//log
                Console.Write("NickName : ");
                option = Console.ReadLine();
                if (option == "-1") // if the user wants to go back to the menu
                    return option;
                else return CheckVlidityName(option);
            }
            else
            if (option[0] == ' ')// if the user press space 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid name! Nickname cannot start with spaces!");
                Console.ResetColor();
                Log.Instance.warn("Invalid input - Invalid nickname");//log
                Console.Write("NickName : ");
                option = Console.ReadLine();
                if (option == "-1") // if the user eants to go back to the menu.
                    return option;
                else return CheckVlidityName(option);
            }
            else return option;
        }

        /// <summary>
        /// A function that enables the user login to the system 
        /// </summary>
        public void login()
        {
            if (this.boolExit == false && this.boolLogout == false) // if the user is in the system and if he is not logined yet.
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Hi my friend! Welcome to the ChatRoom!");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("NOTE : In order to return to the menu, press '-1' at the group ID/NickName option!");
                Console.WriteLine("NOTE : ID should be 2 digits (from 01 to 99) !");
                Console.ResetColor();
                Console.Write("Group ID : ");
                string g_id = Console.ReadLine();
                g_id = CheckVlidityId(g_id);
                if (g_id != "-1")
                { // the user doesn't want to go to the menu.
                    Console.Write("NickName : ");
                    string NickName = Console.ReadLine();
                    NickName = CheckVlidityName(NickName);
                    if (NickName != "-1")
                    { // the user doesn't want to go to the menu. 
                        if (this.chatRoom.login(NickName, g_id) == false)
                        { // checks if the user is already regester or not
                            do
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("ERROR! You are not registered, please register first and then try to login again");
                                Console.ResetColor();
                                Log.Instance.error("Log-in fail - User not registered");//log
                                PrintOnScreenStart();
                            }
                            while (this.chatRoom.login(NickName, g_id) == false);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Congratulations!You are now in the ChatRoom");
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Hi my friend! Welcome to the ChatRoom!");
                            Console.ResetColor();
                            PrintOnScreenLogin();
                        }
                        else // the user is in the system
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Congratulations!You are now in the ChatRoom");
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Hi my friend! Welcome to the ChatRoom!");
                            Console.ResetColor();
                            PrintOnScreenLogin();
                        }
                        Log.Instance.info("New log-in - User: " + NickName);//log
                    }
                    else PrintOnScreenStart(); // the user doesn't want to go to the menu.
                }
                else PrintOnScreenStart(); // the user doesn't want to go to the menu.
            }
            else // if the user is not in the system
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR ! You have to be logged-in and in the system.");
                Console.ResetColor();
                Log.Instance.error("Log-in fail - User not registered");//log
                PrintOnScreenStart();
            }
        }

        /// <summary>
        /// A function that enables the user exit the program.
        /// </summary>
        public void Exit()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Bye Bye");
            Console.ResetColor();
            boolExit = true;
            this.chatRoom.exit();
        }


        /// <summary>
        /// A private function that prints the menu of options in the chatroom on the screen 
        /// </summary>
        private void PrintOnScreenLogin()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Choose only one option :");
            Console.ResetColor();
            Console.WriteLine("1. Send");
            Console.WriteLine("2. Retrieve 10 messages");
            Console.WriteLine("3. Display last 20 messages");
            Console.WriteLine("4. Display all messages of a certain user");
            Console.WriteLine("5. LogOut");
            Console.WriteLine("6. Exit");
            string choiseLogin = Console.ReadLine();
            ChooseOptionsLogin(choiseLogin); // call's to the function that sort the users' choise.
        }

        /// <summary>
        /// A private function that sort the option that the user entered and deliveres it to it's specific function.
        /// </summary>
        /// <param name="choiseStart"> the option that the user entered to the program </param>
        public void ChooseOptionsLogin(string choiseLogin)
        {
            if (choiseLogin.Length == 1)
            {
                switch (choiseLogin)
                {
                    case "1":  // in case the user wants to send a message
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("NOTE: message should consist of at most 150 letters !");
                        Console.ResetColor();
                        Console.Write("Type your message: ");
                        string message = Console.ReadLine();
                        Send(message);
                        Log.Instance.info("Message sent to server");//log
                        break;
                    case "2": // in case the user wants to retrieve 10 messages
                        retrieveMessages();
                        Log.Instance.info("Messages retrived from server");//log
                        break;
                    case "3": // in case the user wants to display las 20 messages
                        display(displayNum);
                        Log.Instance.info("Messages displayed");//log
                        break;
                    case "4":  // in case the user wants to display all the messages of a certain user 
                        displayAll();
                        break;
                    case "5": // in case the user wants to log out of the system
                        LogOut();
                        break;
                    case "6": // in case the user wants to exit from the system
                        Exit();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter a valid choise from the menu!");
                        Console.ResetColor();
                        PrintOnScreenLogin();
                        choiseLogin = Console.ReadLine();
                        ChooseOptionsLogin(choiseLogin);
                        break;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid choise from the menu!");
                Console.ResetColor();
                Log.Instance.warn("Invalid input - Invalid menu choice");//log
                PrintOnScreenLogin();
                choiseLogin = Console.ReadLine();
                ChooseOptionsLogin(choiseLogin);
            }
        }

        /// <summary>
        /// A function that enables the user to retrieve Messages (10 messages as a default)
        /// </summary>
        public void retrieveMessages()
        {
            if (boolLogout == false || boolExit == false) // if the user is in the system and login
            {
                this.retMessages = this.chatRoom.retrieveMessages();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Retrieved new messages:");
                Console.ResetColor();
                PrintOnScreenLogin();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR ! You have to be logged-in and in the system.");
                Console.ResetColor();
                Log.Instance.error("Action failed - User not logged in");//log
            }
        }

        /// <summary>
        /// A function that enables the user to send Messages 
        /// </summary>
        /// <param name="message"> the body of the message </param>
        public void Send(string message)
        {
            bool sent = false;
            if (boolLogout == false || boolExit == false) // if the user is in the system and login 
            {
                if (message == "-1") // if the user wants to go back to the menu
                    PrintOnScreenLogin();
                else// if the user doesn't want to go back to the menu
                {
                    if (CheckVlidityMSG(message))// if the message is in the correct context
                    {
                        sent = this.chatRoom.send(message);
                        if (sent) // if the message sent
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Message sent.");
                            Console.ResetColor();
                        }
                        else // if the message didn't sent
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Opps! There was a problem with the server, the message wasn't sent.");
                            Console.ResetColor();
                            Log.Instance.error("Server issue - message not sent");//log
                        }
                        PrintOnScreenLogin();
                    }
                    else // if there is a problen with the message context
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please note that a valid messege should contain 1 to 150 letters !");
                        Console.ResetColor();
                        Console.Write("Type your message: ");
                        message = Console.ReadLine();
                        Send(message);
                    }
                }
            }
            else // if the user is not in the system
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR ! You have to be logged-in and in the system.");
                Console.ResetColor();
                Log.Instance.error("Action failed - User not logged in");//log
                PrintOnScreenStart();
            }
        }

        /// <summary>
        /// A private function that cheks if the message is legal or not 
        /// </summary>
        /// <param name="MSG"></param>
        /// <returns></returns>
        private bool CheckVlidityMSG(string MSG)
        {
            if (MSG.Length <= 150 && MSG.Length > 0)// if the length of the MSG is correct.
                return true;
            else if (MSG.Length == 1 && MSG[0] == 13)// if the MSG is enter
                return false;
            else return false;
        }

        /// <summary>
        /// A function that enables the user to display some Messages (20 messages as a default) 
        /// </summary>
        /// <param name="num"> the number of messages we want to display </param>
        public void display(int num)
        {
            if (boolLogout == false || boolExit == false) // if the user is in the system and login
            {
                List<Message> l = this.chatRoom.display(num);

                if (l.Count != 0) // if he has messages
                {
                    for (int i = 0; i < l.Count; i++)
                        Console.WriteLine(l[i].ToString());
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(" Messages are displayed.");
                    Console.ResetColor();
                    Log.Instance.info("Display messages from server");//log
                    PrintOnScreenLogin();
                }
                else// if he hasn't messages
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No message exist");
                    Console.ResetColor();
                    Log.Instance.error("No such message exist");
                    PrintOnScreenLogin();
                }

            }
            else// if he is not login or in the system
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR ! You have to be logged-in and in the system.");
                Console.ResetColor();
                Log.Instance.error("Action failed - User not logged in");//log
            }
        }

        /// <summary>
        /// A function that enables the user to display all Messages of a certain user
        /// </summary>
        public void displayAll()
        {
            if (boolLogout == false || boolExit == false) // if the user is in the system and login
            {
                Console.Write("Enter user's group Id: ");
                string g_id = Console.ReadLine();
                g_id = CheckVlidityId(g_id);
                Console.Write("Enter user's nickname: ");
                string nickname = Console.ReadLine();
                nickname = CheckVlidityName(nickname);

                List<Message> msg = this.chatRoom.displayAll(nickname, g_id);

                if (msg == null)// the user doest not exist on the system
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The user does not exist on the system.");
                    Console.ResetColor();
                    Log.Instance.error("No such user exist");//log
                }
                else if (msg.Count == 0) // there are no messages to the u
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("There are no messages from this user.");
                    Console.ResetColor();
                    Log.Instance.warn("No messages to display for User" + nickname);//log
                }
                else // Prints all the messages
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The messages are:");
                    Console.ResetColor();
                    for (int i = 0; i < msg.Count; i++)
                        Console.WriteLine(msg[i].ToString());
                    Log.Instance.info("Display all messages of User:" + nickname);//log
                }
                PrintOnScreenLogin();
            }
            else// if he is not login or in the system
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR ! You have to be logged-in and in the system.");
                Console.ResetColor();
                Log.Instance.error("Action failed - User not logged in");//log
            }
        }

        /// <summary>
        /// A function that enables the user to logout the program 
        /// </summary>
        public void LogOut()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Bye Bye " + this.chatRoom.CurrUser.Nickname);
            Console.ResetColor();
            this.chatRoom.logOut();
            boolLogout = true;
            PrintOnScreenStart();
        }
    }
}