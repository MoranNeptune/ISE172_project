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
    public class ChatRoom
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
        /// <summary>
        /// Log in a certain user 
        /// </summary>
        /// <param name="nickname">User's nickname</param>
        /// <param name="g_id">User's group id</param>
        /// <returns>true if we foud the user and the user logged in</returns>
        public bool login(string nickname, string g_id)
        {
            // check if the group id exist and if it does, check if the user is in that group
            if (this.currUser == null)
                this.currUser = findUser(nickname, g_id);
            if (this.currUser != null)
                return true;
            return false;
        }


        /// <summary>
        /// Find a specific user
        /// </summary>
        /// <param name="nickname">User's nickname</param>
        /// <param name="g_id">User's group id</param>
        /// <returns>The user</returns>
        private User findUser(string nickname, string g_id)
        {
            for (int i = 0; i < allUsers.List.Count; i++)
            {
                if (allUsers.List[i].isEqual(nickname, g_id))
                    return allUsers.List[i];
            }
            return null;
        }

        /// <summary>
        /// Find a specific group id
        /// </summary>
        /// <param name="g_id">gourp id to find</param>
        /// <returns>the group ID</returns>
        private ID findGroupId(string g_id)
        {
            for (int i = 0; i < groupsId.List.Count; i++)
                if (groupsId.List[i].isEqual(g_id))
                    return groupsId.List[i];
            return null;
        }

        /// <summary>
        /// the function retrieve 10 messages from the server and saves the new messages that we got.
        /// </summary>
        /// <returns>List of the last 10 messages that were retrived</returns>
        public List<Message> retrieveMessages()
        {
            // retrives last 10 messages from the server
            List<IMessage> msgs = Communication.Instance.GetTenMessages(this.url);
            List<Message> msgToUpdate = new List<Message>();
            // check which messages we should update in our files
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

        /// <summary>
        /// Sends the message to the server and saves the message in the files
        /// </summary>
        /// <param name="message">message body to send</param>
        /// <returns>true if was sent to the server succsesfully, else false</returns>
        public bool send(string message)
        {
            IMessage Imsg = Communication.Instance.Send(url, currUser.G_id.idNumber, currUser.Nickname, message);
            Message msg = currUser.send(Imsg);
            // check if the 
            if (msg != null)
            {
                allMessages.updateFile(msg);
                return true;
            }
            return false;
        }

        /// <summary>
        /// returns all messages of a certain user
        /// </summary>
        /// <param name="nickname">the user's nickname</param>
        /// <param name="g_id">the user's group id</param>
        /// <returns>list of all the messages of the certain user</returns>
        public List<Message> displayAll(string nickname, string g_id)
        {
            // Find the specific user
            User user = findUser(nickname, g_id);
            if (user != null)
                return user.msgHandler.List;
            return null;
        }

        /// <summary>
        /// retrive new messages from the server
        /// </summary>
        /// <param name="num">number of messages the user would like to display</param>
        /// <returns>returns a specific number of messages</returns>
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

        /// <summary>
        /// register a user to the system
        /// if the group id doest not exist, it creats a new group and add the user
        /// else, the group exist, check if the user's nickname exist on the group.
        /// if it doest exist then return false
        /// else, the user's nickname doest not exist in the group, add him to the group
        /// </summary>
        /// <param name="nickname">User's nickname to register</param>
        /// <param name="g_id">Group id to register to</param>
        /// <returns></returns>
        public bool register(string nickname, string g_id)
        {
            // In case the group number exist
            if (this.groupsId != null && this.groupsId.List != null)
            {
                for (int i = 0; i < this.groupsId.List.Count; i++)
                {
                    ID gId = (this.groupsId.List)[i];
                    //if found the corect group
                    if (gId.isEqual(g_id))
                    {
                        // if we added successfuly the user to the current group
                        if (gId.addMember(nickname))
                        {
                            //update of users file
                            this.allUsers.updateFile(new User(nickname, gId));
                            return true;
                        }
                        else
                            return false;
                    }
                }

                // In case the group doesn't exist, creats a new group
                ID newG_ID = new ID(g_id);
                newG_ID.addMember(nickname);
                this.groupsId.updateFile(newG_ID);
                this.allUsers.updateFile(new User(nickname, newG_ID));
                return true;
            }
            return false;
        }
        /// <summary>
        /// updates the user's status and logout from the system. 
        /// </summary>
        public void logOut()
        {
            this.currUser.logout();
            this.currUser = null;
        }

        public User CurrUser
        {
            get { return currUser; }
            set { currUser = value; }
        }
    }
}
