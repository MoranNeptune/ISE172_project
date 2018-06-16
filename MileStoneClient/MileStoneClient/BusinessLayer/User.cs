using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.PresistentLayer;
using System.IO;

namespace MileStoneClient.BusinessLayer
{
    [Serializable]
    public class User
    {
        private string nickname;
        private string g_id;
        private bool loggedIn;
        private string password;
        private string user_id;

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
            this.nickname = nickname.Trim();
            this.password = pass;
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

        public string User_id
        {
            get { return user_id; }
            set { user_id = value; }
        }

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