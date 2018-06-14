using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.PresistentLayer;
using MileStoneClient.PresentationLayer;

namespace MileStoneClient.BusinessLayer
{
    public class ChatRoom
    {
        /// <summary>
        /// change creation of user to get int id instead of creating ID
        /// </summary>
        // fields
        private string url;
        private User currUser;
        private MessageHandler allMessages;
        private UserHandler allUsers;
        private List<GuiMessage> presMsgs;
        private PresentationLayer.Action sort;
        private string filter;
        //private string NONE = "";

        // constractors
        public ChatRoom(string url)
        {
            this.url = url;
            this.currUser = null;
            presMsgs = new List<GuiMessage>();
            sort = new SortByTime();
            filter = "";
            // initialize the messages handler with a default filter - NONE
            //   allMessages = new MessageHandler(NONE, NONE);
            //   allUsers = new UserHandler();
            //this.groupsId = new IdHandler("allGroups");
            HandlerFactory handler = new HandlerFactory();
            allMessages = handler.createMessageHandler();
            allUsers = handler.createUserHandler();
            allMessages.filterByNone();
            updatePresMessages();
        }

        // methods
        /// <summary>
        /// Log in a certain user 
        /// </summary>
        /// <param name="nickname">User's nickname</param>
        /// <param name="g_id">User's group id</param>
        /// <returns>true if we foud the user and the user logged in</returns>
        public bool login(string nickname, string g_id, string pass)
        {
            // check if the group id exist and if it does, check if the user is in that group
            if (this.currUser == null)
                this.currUser = findUser(nickname, g_id);
            if (this.currUser != null && currUser.Password.Equals(pass))
            {
                currUser.Nickname = nickname;
                return true;
            }
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
            // לשנות אצל עינת שליפה ל
            if (allUsers.doesExist(nickname, g_id))
                return allUsers.UserExist;
            return null;
        }

        /// <summary>
        /// Find a specific group id
        /// </summary>
        /// <param name="g_id">gourp id to find</param>
        /// <returns>the group ID</returns>
      /*  private ID findGroupId(string g_id)
        {
            for (int i = 0; i < groupsId.List.Count; i++)
                if (groupsId.List[i].isEqual(g_id))
                    return groupsId.List[i];
            return null;
        }*/

        /// <summary>
        /// return list of the users in a given group
        /// </summary>
        /// <param name="g_id">gourp id</param>
        /// <returns>List of the group's members</returns>
        public List<string> getMembersOf(string g_id)
        {
            List<string> names = new List<string>();
            List<User> users = allUsers.getMembers(g_id);
            if (users.Count > 0)
            {
                for (int i = 0; i < users.Count; i++)
                    names.Add(users[i].Nickname);
            }
            return names;
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
        public bool register(string nickname, string g_id, string pass)
        {
            // check if the user exist int the data base
            User found = findUser(nickname, g_id);
            if (found != null)
                return false;
            User user = new User(nickname, g_id, pass);
            // add new user to the database
            allUsers.addUser(user);
            return true;
        }

        /// <returns>list of all the messages of the certain user</returns>
        public List<string> getGroups()
        {
            allUsers.getAllUsers();
            List<string> grp = new List<string>();
            grp.Add("Groups");
            // checking for all groups in the list of the users
            for (int i = 0; i < allUsers.List.Count; i++)
                // check if the group already exist on the list of groups
                if (!grp.Contains(allUsers.List[i].G_id))
                    grp.Add(allUsers.List[i].G_id);
            return grp;
        }

        // ****************************************************************************************************************************************************************************
        /// עדן צריכה לשנות !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// <summary>
        /// Sends the message to the server and saves the message in the files
        /// </summary>
        /// <param name="message">message body to send</param>
        /// <returns>true if was sent to the server succsesfully, else false</returns>
        public bool send(string message)
        {
            // changes the time from local time to UTC
            DateTime localDateTimeExample = DateTime.Now;
            DateTime UtcTime = localDateTimeExample.ToUniversalTime();

            // sends the message to the dataBase
            if (allMessages.send(new Message(message, UtcTime, Guid.NewGuid(), currUser)))
                return true;

            return false;
        }

        public bool updateMessage()
        {
            return false;
        }

        // לשנותתתתתתתתתתתתתתתתתתתתתתתתתתתתתתתתתתת
        /// <summary>
        /// the function returns a list of messages, by specific sort, filter and order 
        /// </summary>
        /// <param name="order">The wanted order of the list</param>
        /// <param name="actions">Sort and filter</param>
        /// <returns></returns>
        public List<GuiMessage> getMessages(int order, PresentationLayer.Action sortAction, List<string> filterInfo)
        {
            bool update = false;
            // check if the sort or filter had changed, if they did, we update the current sort/filter
            if (sort != sortAction)
            {
                sort = sortAction;
                update = true;
            }
            // the list filterInfo contains - [0] - filter name, [1]- group id, [2]- nickname
            if (!this.filter.Equals(filterInfo[0]))
            {
                this.filter = filterInfo[0];
                update = true;
                if (filter.Equals("NONE"))
                    allMessages.filterByNone();
                if (filter.Equals("ByGroup"))
                    allMessages.FilterByGroup(filterInfo[1]);
                if (filter.Equals("ByUser"))
                    allMessages.FilterByUser(filterInfo[2], filterInfo[1]);
            }
            // if the filter/sort is changed, update the list of the presentation messages
            if (update)
                updatePresMessages();
            retrieveMessages();
           // if (filter != null)
           //    filter.action(presMsgs);

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

        // לשנותתתתתתתתתתתתתתתתתתתתתתתתתתתתתתתתתתתתתת
        /// <summary>
        /// the function retrieve 10 messages from the server and saves the new messages that we got, in both lists
        /// </summary>
        public void retrieveMessages()
        {
            // retrives last messages from the server
            List<Message> msgToUpdate = allMessages.retrieve();

            // check which messages we should update in our files
            for (int i = 0; i < msgToUpdate.Count; i++)
            {
                if (msgToUpdate[i].User == null)
                    throw new Exception("an error with getUserByID");
                // creates a GuiMessage for the curr message
                presMsgs.Add(new GuiMessage(msgToUpdate[i].Body, msgToUpdate[i].DateTime, msgToUpdate[i].Id, msgToUpdate[i].User.Nickname, msgToUpdate[i].User.G_id));

            }
            // update messages list
            if (msgToUpdate.Count > 0)
            {
                allMessages.List.AddRange(msgToUpdate);
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
                    presMsgs.Add(new GuiMessage(m.Body, m.DateTime.ToLocalTime(), m.Id, m.User.Nickname, m.User.G_id));
                }
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