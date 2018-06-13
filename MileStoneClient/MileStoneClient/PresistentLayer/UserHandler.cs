using System;
using MileStoneClient.BusinessLayer;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace MileStoneClient.PresistentLayer
{
    //an object that's responsable to transfer the users' info to the database
    public class UserHandler : IQueryAction
    {
        private List<User> allUsersList;
        private User userExist;
        private bool exist;
        private bool connectionFail;
        private string UserId;
        private User queryUser;

        //constructor
        public UserHandler()
        {
            allUsersList = new List<User>();
            userExist = null;
            exist = false;
            connectionFail = false;
            UserId = "";
            queryUser = null;

        }

        public override void ExecuteQuery(string query)
        {
            //add new user
            if (query.Contains("INSERT"))
            {
                Instance.ExecuteUserQuery(query, queryUser); 
            }
            //update users list
            else if (query.Contains("SELECT"))
            {
                allUsersList = Instance.ExecuteUserQuery(query, queryUser);
            }
            //check if user exist
            else if (query.Contains("select"))
            {
                /////לשלוח ותחזור רשימה, צריך רק לשנות את החתימה
                //if user exist will recieve a list with one user, else will return empty list
                List<User> exist = Instance.ExecuteUserQuery(query, queryUser);
                
                if (exist != null)
                {
                    //if the list contains a user, then user already exist it table
                    if (exist.Count <= 1)
                    {
                        this.exist = true;
                        userExist = exist[0];
                    }
                }                    
            }
            /*//add new user
            if (query.Contains("INSERT"))
            {
                Instance.UpdateTable(query); 
            }
            //update users list
            else if (query.Contains("SELECT"))
            {
                allUsersList = Instance.ReadUserTable(query);
            }
            //check if user exist
            else if (query.Contains("select"))
            {
                /////לשלוח ותחזור רשימה, צריך רק לשנות את החתימה
                //if user exist will recieve a list with one user, else will return empty list
                List<User> exist = Instance.ReadUserTable(query);
                
                if (exist != null)
                {
                    //if the list contains a user, then user already exist it table
                    if (exist.Count <= 1)
                    {
                        this.exist = true;
                        userExist = exist[0];
                    }
                }                    
            }*/
        }

        //add a new User to Users table and then to list if the user doesn't alreay exist, return true if user is added
        public bool addUser(User user)
        {            
            //set query to add user and executes query 
            string query = "INSERT INTO Users ([Group_Id],[Nickname],[Password]) " +
                   //   "VALUES ('" + user.G_id + "', '" + user.Nickname + "','" + user.Password + "')";
                   "VALUES (@user_G_id, @user_Nickname,@Password)";
            
            queryUser = user;
            
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
            {
                connectionFail = true;
                return false;
            }

            allUsersList.Add(user);
            queryUser = null;
            return true;
        }

        //retrieve all users from database table
        public void getAllUsers()
        {
            string query = "SELECT [ID],[Group_Id],[Nickname],[Password] " +
                    "FROM [MS3].[dbo].[Users]";
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
            {
                connectionFail = true;
            }
        }

        //check if user already exists in Users table
        public bool doesExist(string nickname, string g_id)
        {
            //set query to find user with same details and executes query
            string query = "select top (1) [Group_Id],[Nickname] " +
                    "from [MS3].[dbo].[Users] " +
                    "where [MS3].[dbo].[Users].[Group_Id] = '" + g_id +
                    "' and [MS3].[dbo].[Users].[Nickname] = '" + nickname;
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
            {
                connectionFail = true;
                return false;
            }
            return exist;
        }

        //get members of group g_id
        public bool getMembers(string g_id)
        {
            //set query to find user with same details and executes query
            string query = "SELECT [ID][Group_Id],[Nickname],[Password] " +
                "from [MS3].[dbo].[Users] " +
                "where [MS3].[dbo].[Users].[Group_Id] = '" + g_id + "'";
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
            {
                connectionFail = true;
                return false;
            }
            return true;
        }

        //get id number for user
        public string getUserId(string g_id, string nickname)
        {
            //set query to find user with same details and executes query
            string query = "SELECT [Id] " +
                "from [MS3].[dbo].[Users] " +
                "where [MS3].[dbo].[Users].[Group_Id] = '" + g_id +
                "' and [MS3].[dbo].[Users].Nickname = '" + nickname + "'";
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
            {
                connectionFail = true;
            }
            return UserId;
        }

        public List<User> List
        {
            get { return allUsersList; }
            set { allUsersList = value; }
        }

        public bool ConnectionFail
        {
            get { return connectionFail; }
            set { connectionFail = value; }
        }

        public User UserExist
        {
            get { return userExist; }
            set { userExist = value; }
        }
    }
}