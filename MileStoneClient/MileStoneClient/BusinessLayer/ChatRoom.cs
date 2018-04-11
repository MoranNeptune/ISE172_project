using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.CommunicationLayer;
using MileStoneClient.PresistentLayer;

namespace MileStoneClient.BusinessLayer
{
    class ChatRoom
    {
        // fields
        private string url;
        private User currUser;
        private MessageHandler allMessages;
        private UserHandler allUsers;
        private IdHandler groupsId;

        // constractors
        public ChatRoom(string url)
        {
            this.url = url;
            this.currUser = null;
            allMessages = new MessageHandler("allMessages");
            allUsers = new UserHandler("allUsers");
            for (int i = 0; i < allUsers.List.Count; i++)
                allUsers.List[i].msgHandler = new MessageHandler(allUsers.List[i].G_id.idNumber + allUsers.List[i].Nickname);
            this.groupsId = new IdHandler("allGroups");
        }

        // methods
        public bool login(string nickname, string g_id)
        {
            // check if the group id exist, and if it does, check if the user is in that group
            if (this.currUser == null)
                this.currUser = findUser(nickname, g_id);
            if (this.currUser != null)
                return true;
            return false;
        }

        // find a specific user in the users list
        private User findUser(string nickname, string g_id)
        {
            for (int i = 0; i < allUsers.List.Count; i++)
            {
                if (allUsers.List[i].isEqual(nickname, g_id))
                    return allUsers.List[i];
            }
            return null;
        }

        // find a specific user in the users list
        private ID findGroupId(string g_id)
        {
            for (int i = 0; i < groupsId.List.Count; i++)
                if (groupsId.List[i].isEqual(g_id))
                    return groupsId.List[i];
            return null;
        }

        // the function retrieve 10 messages from the server and saves the new messages that we got.
        public List<Message> retrieveMessages()
        {
            // retrives last 10 messages from the server
            List<IMessage> msgs = Communication.Instance.GetTenMessages(this.url);
            List<Message> msgToUpdate = new List<Message>();
            for (int i = 0; i < msgs.Count; i++)
            {
                Message newMsg;
                User newUser = findUser(msgs[i].UserName, msgs[i].GroupID);
                // if the user already exist in the list
                if (newUser != null)
                {
                    newMsg = new Message(msgs[i].MessageContent, msgs[i].Date, msgs[i].Id, newUser);
                }
                // if the user doest not exist on our lists.
                // and updates the lists with new group id, users and messages
                else
                {
                    ID newId = findGroupId(msgs[i].GroupID);
                    // if the group id already exist on the system
                    if (newId == null)
                    {
                        newId = new ID(msgs[i].GroupID);
                        this.groupsId.updateFile(newId);
                    }
                    newUser = new User(msgs[i].UserName, newId);
                    newId.addMember(newUser.Nickname);
                    this.allUsers.updateFile(newUser);
                    newMsg = new Message(msgs[i].MessageContent, msgs[i].Date, msgs[i].Id, newUser);
                }

                // check if the message aleardy exist 
                if (!this.allMessages.List.Contains(newMsg))
                {
                    newUser.addMessage(newMsg);
                    msgToUpdate.Add(newMsg);
                }
            }
            // update messages file
            if (msgToUpdate.Count > 0)
                this.allMessages.updateFile(msgToUpdate);
            int index = this.allMessages.List.Count;
            return this.allMessages.List.GetRange(index - 10, 10);
        }
        // Sends the message to the server and saves the message in the files/
        public bool send(string message)
        {
            IMessage Imsg = Communication.Instance.Send(url, currUser.G_id.idNumber, currUser.Nickname, message);
            //Console.WriteLine("MessageTime:{0} , Guid:{1}\n", Imsg.Date.ToString(), Imsg.Id);
            Message msg = currUser.send(Imsg);
            // check if the 
            if (msg != null)
            {
                allMessages.updateFile(msg);
                return true;
            }
            return false;
        }
        // returns all messages of a certain user
        public List<Message> displayAll(string nickname, string g_id)
        {
            // Find the specific user
            User user = findUser(nickname, g_id);
            if (user != null)
                return user.msgHandler.List;
            return null;
        }
        /* retrive new messages from the server
         * returns a specific number of messages
         */
        public List<Message> display(int num)
        {
            if (num > this.allMessages.List.Count)
                return this.allMessages.List;
            else
            {
                List<Message> msgDisplay = new List<Message>(num);
                msgDisplay = this.allMessages.List.GetRange(allMessages.List.Count - num, num);
                //  msgDisplay.AddRange(this.allMessages.List.GetRange((this.allMessages.List).Count - num, (this.allMessages.List).Count - 1));
                return msgDisplay;
            }
        }
        // register a user to the system
        /* the function tries to add new user to a specific group.
         * if the group id doest not exist, it creats a new group and add the user
         * else, the group exist, check if the user's nickname exist on the group.
         * if it doest exist then return false
         * else, the user's nickname doest not exist in the group, add him to the group
         * and return false
         */
        public bool register(string nickname, string g_id)
        {
            // In case the group number exist

            if (this.groupsId != null && this.groupsId.List != null)
            {
                for (int i = 0; i < this.groupsId.List.Count; i++)
                {
                    ID gId = (this.groupsId.List)[i];
                    //if found the corect group, and if we can add the user to the current group
                    if (gId.isEqual(g_id))
                    {
                        if (gId.addMember(nickname))
                        {
                            //updates
                            this.allUsers.updateFile(new User(nickname, gId));
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }
            // In case its a new group
            ID newG_ID = new ID(g_id);
            newG_ID.addMember(nickname);
            this.groupsId.updateFile(newG_ID);
            this.allUsers.updateFile(new User(nickname, newG_ID));
            return true;
        }

        public void logOut()
        {
            //this.currUser.logout();
            this.currUser = null;
        }
        public void exit()
        {
            this.currUser = null;
            this.allMessages = null;
            this.allUsers = null;
            this.groupsId = null;
        }

        public User CurrUser
        {
            get { return currUser; }
            set { currUser = value; }
        }
    }
}
