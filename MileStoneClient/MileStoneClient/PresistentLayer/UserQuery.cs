using MileStoneClient.BusinessLayer;
using System;
using System.Collections.Generic;


namespace MileStoneClient.PresistentLayer
{
    class UserQuery : IQueryAction
    {
        private List<User> users;
        //these may be deleted later
        private bool exist;
        private bool connectionFail;

        public UserQuery()
        {
            users = new List<User>();
            exist = false;
            connectionFail = false;
            //if connection fail - need to return bool and then return false in add
        }

        public override void ExecuteQuery(string query)
        {
            if (query.Contains("INSERT"))
            {
                Instance.addToUserTable(query);
            }
            else if (query.Contains("select"))
            {
                //if user exist will recieve a list with one user, else will return empty list
                users = Instance.ReadUserTable(query);
                if(users != null)
                {
                    if (users.Count <= 1)
                        exist = true;
                }
            }

        }

        // Set query to add user and executes query 
        public bool addUser(User u)
        {
            string query = "INSERT INTO Users ([Group_Id],[Nickname],[Password]) " +
                           "VALUES (" + u.G_id.idNumber + ", '" + u.Nickname + "','" + u.Password + "')";

            // Check if user already exist in the table
            if (doesExist(u))
                return false;
            
            try
            {
                ExecuteQuery(query);
            }catch(Exception e)
            {
                connectionFail = true;
                return false;
            }
            return true;
        }

        // Set query to find user with same details and executes query
        public bool doesExist(User u)
        {            
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
            return true;
        }


    }
}
