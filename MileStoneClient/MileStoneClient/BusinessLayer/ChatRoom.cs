using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.CommunicationLayer;
using MileStoneClient.PresistentLayer;
using MileStoneClient.PresentationLayer;

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
        private List<GuiMessage> presMsgs;
        private PresentationLayer.Action sort, filter;

        // constractors
        public ChatRoom(string url)
        {
            this.url = url;
            this.currUser = null;
            presMsgs = new List<GuiMessage>();
            sort = new SortByTime();
            filter = null;
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
        public User findUser(string nickname, string g_id)
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
        /// return list of the users in a given group
        /// </summary>
        /// <param name="g_id">gourp id</param>
        /// <returns>List of the group's members</returns>
        public List<string> getMembersOf(string g_id)
        {
            ID id = findGroupId(g_id);
            if (id != null)
                return id.Members;
            return null;
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
                // הוספתי
                presMsgs.Add(new GuiMessage(msg.Body, msg.DateTime, msg.Id, msg.User.Nickname, msg.User.G_id.idNumber));
                return true;
            }
            return false;
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

        /// <summary>
        /// the function returns a list of messages, by specific sort, filter and order 
        /// </summary>
        /// <param name="order">The wanted order of the list</param>
        /// <param name="actions">Sort and filter</param>
        /// <returns></returns>
        public List<GuiMessage> getMessages(int order, List<PresentationLayer.Action> actions)
        {
            bool update = false;
            // check if the sort or filter had changed, if they did, we update the current sort/filter
            if (actions.Count > 0 && sort != actions[0])
            {
                sort = actions[0];
                update = true;
            }
            if (actions.Count > 1 && this.filter != actions[1])
            {
                filter = actions[1];
                update = true;
            }
            // if the filter/sort is changed, update the list of the presentation messages
            if (update)
                updatePresMessages();
            // להחזיר את השורה
            // retrieveMessages();
            if (filter != null)
                filter.action(presMsgs);

            if (sort != null)
            {
                // ascending sort - defult sort
                sort.action(presMsgs);

                // if descending sort
                if (order == 1)
                {
                    presMsgs.Reverse();
                }
            }
            return presMsgs;
        }

        /// <summary>
        /// the function retrieve 10 messages from the server and saves the new messages that we got, in both lists
        /// </summary>
        public void retrieveMessages()
        {
            // retrives last 10 messages from the server
            List<IMessage> msgs = Communication.Instance.GetTenMessages(this.url);
            List<Message> msgToUpdate = new List<Message>();
            // List<GuiMessage> msgToRetrive = new List<GuiMessage>();
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
                    allUsers.updateFile(newUser);
                    newMsg = new Message(msgs[i].MessageContent, msgs[i].Date, msgs[i].Id, newUser);
                }

                // check if the message aleardy exist 
                if (!allMessages.List.Contains(newMsg))
                {
                    newUser.addMessage(newMsg);
                    msgToUpdate.Add(newMsg);
                    presMsgs.Add(new GuiMessage(newMsg.Body, newMsg.DateTime, newMsg.Id, newMsg.User.Nickname, newMsg.User.G_id.idNumber));
                }
            }
            // update messages file
            if (msgToUpdate.Count > 0)
            {
                allMessages.updateFile(msgToUpdate);
            }
        }

        /// <summary>
        /// updates all the messages from the files to the presentation messages
        /// </summary>
        private void updatePresMessages()
        {
            presMsgs.Clear();
            if (allMessages.List != null)
                for (int i = 0; i < allMessages.List.Count; i++)
                {
                    Message m = allMessages.List[i];
                    presMsgs.Add(new GuiMessage(m.Body, m.DateTime, m.Id, m.User.Nickname, m.User.G_id.idNumber));
                }
        }
        
        /// <returns>list of all the messages of the certain user</returns>
        public List<string> getNicknames()
        {
            List<string> nicknames = new List<string>();
            nicknames.Add("Users");
            for (int i = 0; i < allUsers.List.Count; i++)
                nicknames.Add(allUsers.List[i].Nickname);
            return nicknames;
        }

        /// <returns>list of all the messages of the certain user</returns>
        public List<string> getGroups()
        {
            List<string> grp = new List<string>();
            grp.Add("Groups");
            for (int i = 0; i < groupsId.List.Count; i++)
                grp.Add(groupsId.List[i].idNumber);
            return grp;
        }
    }
}