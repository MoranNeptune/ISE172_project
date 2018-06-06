using System;
using MileStoneClient.BusinessLayer;
using System.Collections.Generic;

namespace MileStoneClient.PresistentLayer
{
    //an object that's responsable to transfer the users' info to the database
    public class UserHandler : IQueryAction
    {
        private List<User> allUsersList;
        private List<User> userExist;
        private bool exist;
        private bool connectionFail;

        //constructor
        public UserHandler()
        {
            allUsersList = new List<User>();
            userExist = new List<User>();
            exist = false;
            connectionFail = false;
        }

        public override void ExecuteQuery(string query)
        {
            //add new user
            if (query.Contains("INSERT"))
            {
                Instance.addToUserTable(query);
            }
            //update users list
            else if (query.Contains("SELECT"))
            {
                allUsersList = Instance.ReadUserTable(query);
            }
            //check if user exist
            else if (query.Contains("select"))
            {
                //if user exist will recieve a list with one user, else will return empty list
                userExist = Instance.ReadUserTable(query);
                if (userExist != null)
                {
                    //if the list contains a user, then user already exist it table
                    if (userExist.Count <= 1)
                        exist = true;
                }
            }
        }

        //add a new User to Users table and then to list if the user doesn't alreay exist, return true if user is added
        public bool updateFile(User user)
        {
            //set query to add user and executes query 
            string query = "INSERT INTO Users ([Group_Id],[Nickname],[Password]) " +
                           "VALUES (" + user.G_id.idNumber + ", '" + user.Nickname + "','" + user.Password + "')";

            //כנראה ימחק בסוף
            //check if user already exist in the table
            //if (doesExist(user) | connectionFail)
              //  return false;

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
            return true;
        }

        //retrieve all users from database table
        public void updateUsers()
        {
            string query = "SELECT [Group_Id],[Nickname],[Password] " +
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
        public bool doesExist(User u)
        {
            //set query to find user with same details and executes query
            string query = "select top (1) [Group_Id],[Nickname],[Password] " +
                    "from [MS3].[dbo].[Users] " +
                    "where [MS3].[dbo].[Users].[Group_Id] = " + u.G_id.idNumber +
                    " and [MS3].[dbo].[Users].[Nickname] = '" + u.Nickname +
                    "' and [MS3].[dbo].[Users].[Password] = '" + u.Password + "'";
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
    }
}