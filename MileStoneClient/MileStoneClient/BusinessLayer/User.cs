using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.PresistentLayer;
using MileStoneClient.CommunicationLayer;
using System.IO;

namespace MileStoneClient.BusinessLayer
{

    [Serializable]
    public class User
    {
        private String nickname;
        private ID g_id;
        private MessageHandler handler;
        private bool loggedIn;

        // Constructor      
        public User(string nickname, ID g_id)
        {
            this.g_id = g_id; 
            this.nickname = nickname; 
            handler = new MessageHandler(g_id.idNumber + nickname);
        }

        //Getters and Setters
        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }

        public ID G_id
        {
            get { return g_id; }
            set { g_id = value; }
        }

        public MessageHandler msgHandler
        {
            get { return handler; }
            set { handler = value; }
        }

        public bool LoggedIn
        {
            get { return loggedIn; }
            set { loggedIn = value; }
        }

        //methods
        /// <summary>
        /// Sends message to the fileSystem
        /// </summary>
        /// <param name="msg"> A parameter of type IMessage </param>
        /// <returns> Returns a parameter of type Message </returns>
        public Message send(IMessage msg)
        {
            Message message = new Message(msg.MessageContent.ToString(), msg.Date, msg.Id, this);
            bool msgRegistered = this.handler.updateFile(message);

            //returns null if the message hasn't been updated into the files for any reason
            if (msgRegistered == false)
                message = null;

            return message;
        }

        /// <summary>
        /// Add message to the fileSystem for this user
        /// </summary>
        /// <param name="msg"> A parameter of type Message representing the users' message </param>
        public void addMessage(Message msg)
        {
            this.handler.updateFile(msg);
        }

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
        public bool isEqual(string nickname, string g_id)
        {
            return (this.nickname.Equals(nickname) & (this.g_id.isEqual(g_id)));
        }

        public string toString()
        {
            return "Group Id: " + g_id.idNumber + ", Nickname: " + nickname + ", LoggedIn: " + loggedIn;
        }
    }
}