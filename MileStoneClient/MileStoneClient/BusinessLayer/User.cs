using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.PresistentLayer;
using System.IO;

namespace MileStoneClient.BusinessLayer
{

    //  need to update

    [Serializable]
    public class User
    {
        // בשביל מה צריך את ה-hendler??? !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private string nickname;
        private string g_id;
       // private MessageHandler handler;
        private bool loggedIn;
        private string password;

        /// <summary>
        /// //change constructor to get int id instead of ID 
        /// the constructor will create the ID and check if one already exists - if not create
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="g_id"></param>

        // Constructor      
        public User(string nickname, string g_id, string pass)
        {
            this.g_id = g_id; 
            this.nickname = nickname;
            this.password = pass;
            //handler = new MessageHandler(g_id + nickname);
        }

        //Getters and Setters
        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }

        public string G_id
        {
            get { return g_id; }
            set { g_id = value; }
        }
        // **************** אולי להוריד ************************************
     /*   public MessageHandler msgHandler
        {
            get { return handler; }
            set { handler = value; }
        }
*/
        public bool LoggedIn
        {
            get { return loggedIn; }
            set { loggedIn = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        // **************** אולי להוריד ************************************
        //methods
        /// <summary>
        /// Sends message to the fileSystem
        /// </summary>
        /// <param name="msg"> A parameter of type IMessage </param>
        /// <returns> Returns a parameter of type Message </returns>
   /*     public Message send(IMessage msg)
        {
            Message message = new Message(msg.MessageContent.ToString(), msg.Date, msg.Id, this);
            bool msgRegistered = this.handler.updateFile(message);

            //returns null if the message hasn't been updated into the files for any reason
            if (msgRegistered == false)
                message = null;

            return message;
        }*/
        // **************** אולי להוריד ************************************
        /// <summary>
        /// Add message to the fileSystem for this user
        /// </summary>
        /// <param name="msg"> A parameter of type Message representing the users' message </param>
   /*     public void addMessage(Message msg)
        {
            this.handler.updateFile(msg);
        }*/

        public void logout()
        {
            loggedIn = false;
        }

        /// <summary>
        /// Check if nickname is equal to another nickname
        /// </summary>
        /// <param name="nickname">A parameter of type string representing the users' nickname </param>
        /// <param name="g_id">A parameter of type string representing the users' group id </param>
        /// <returns> Returns true if this is equal to other, else return false </returns>
        public bool isEqual(string nickname, int g_id, string pass)
        {
            return this.nickname.Equals(nickname) & this.g_id.Equals(g_id) & password.Equals(pass);
        }

        public string toString()
        {
            return "Group Id: " + g_id + ", Nickname: " + nickname + ", LoggedIn: " + loggedIn;
        }
    }
}