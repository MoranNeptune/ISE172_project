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
    class User
    {
        private String nickname;
        private ID g_id;
        private MessageHandler handler;
        private bool loggedIn;

        //constructor 
        public User(String nickname, ID g_id)
        {
            this.g_id = g_id; //assume ID is legit
            this.nickname = nickname; //assume nickname is legit
            handler = new MessageHandler(g_id.idNumber + nickname);
        }

        //getters and setters
        public String Nickname
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
        //send message to fileSystem
        public Message send(IMessage msg)
        {
            Message message = new Message(msg.MessageContent.ToString(), msg.Date, msg.Id, this);
            bool msgRegistered = this.handler.updateFile(message);

            //returns null if the message hasn't been updated into the files for any reason
            if (msgRegistered == false)
                message = null;

            return message;
        }

        public void addMessage(Message msg)
        {
            this.handler.updateFile(msg);
        }

        public void logout()
        {
            loggedIn = false;
        }

        //check if nickname is equal to another nickname 
        public bool isEqual(string nickname, string g_id)
        {
            return (this.nickname.Equals(nickname) & (this.g_id.isEqual(g_id)));
        }

        public String toString()
        {
            return "Group Id: " + g_id.idNumber + ", Nickname: " + nickname + ", LoggedIn: " + loggedIn;
        }
    }
}