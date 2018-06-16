
using System;
using MileStoneClient.BusinessLayer;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace MileStoneClient.PresistentLayer
{
    //an object that's responsable to transfer the users' info to the database
    public class UserHandler : ConnectionHandler
    {

        private List<User> allUsersList;
        private User userExist;
        private bool connectionFail;


        //constructor
        public UserHandler()
        {
            allUsersList = new List<User>();
            userExist = null;
            connectionFail = false;
        }


        //add a new User to Users table and then to list if the user doesn't alreay exist, return true if user is added
        public bool addUser(User user)
        {
            try
            {
                //set query to add user and executes query 
                string query = "INSERT INTO Users ([Group_Id],[Nickname],[Password]) " +
                               "VALUES (@user_G_id, @user_Nickname ,@Password)";
                connect();
                SqlCommand command = new SqlCommand(null, connection);
                command.CommandText = query;
                // updates the list of parameters
                SqlParameter id_param = new SqlParameter(@"user_G_id", SqlDbType.Int, 20);
                SqlParameter name_param = new SqlParameter(@"user_Nickname", SqlDbType.Char, 8);
                SqlParameter pass_param = new SqlParameter(@"Password", SqlDbType.Char, 64);

                id_param.Value = user.G_id;
                name_param.Value = user.Nickname;
                pass_param.Value = user.Password;

                command.Parameters.Add(id_param);
                command.Parameters.Add(name_param);
                command.Parameters.Add(pass_param);

                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
                disconnect();
            }
            catch (Exception e)
            {
                connectionFail = true;
                return false;
            }

            allUsersList.Add(user);
            return true;
        }

        ////retrieve all users from database table
        public void getAllUsers()
        {
            try
            {
                // אולי לא צריך את הרשימה
                allUsersList.Clear();

                string query = "SELECT [Id],[Group_Id],[Nickname],[Password] " +
                        "FROM [MS3].[dbo].[Users]";

                connect();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader data_reader = command.ExecuteReader();

                int index = 0;
                while (data_reader.Read())
                {
                    //add users from the users table to the list
                    allUsersList.Add(new User(data_reader.GetValue(2).ToString(), data_reader.GetValue(1).ToString(), data_reader.GetValue(3).ToString()));
                    allUsersList[index].User_id = data_reader.GetValue(0).ToString();
                    index++;
                }
                data_reader.Close();
                command.Dispose();
                disconnect();
            }
            catch (Exception e)
            {
                connectionFail = true;
            }
        }

        //check if user already exists in Users table
        public bool doesExist(string nickname, string g_id)
        {
            try
            {
                //set query to find user with same details and executes query
                string query = "select top (1) [Id],[Group_Id],[Nickname],[Password] " +
                        "from [MS3].[dbo].[Users] " +
                        "where [MS3].[dbo].[Users].[Group_Id] = @g_id" +//+ g_id +
                        " and [MS3].[dbo].[Users].[Nickname] = @nickname";// + nickname + "'";


                List<User> exist = new List<User>();
                connect();
                SqlCommand command = new SqlCommand(query, connection);

                SqlParameter g_id_param = new SqlParameter(@"g_id", SqlDbType.Int, 20);
                SqlParameter name_param = new SqlParameter(@"nickname", SqlDbType.Char, 20);
                g_id_param.Value = g_id;
                name_param.Value = nickname;
                command.Parameters.Add(g_id_param);
                command.Parameters.Add(name_param);

                SqlDataReader data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    //add users from the users table to the list
                    exist.Add(new User(data_reader.GetValue(2).ToString(), data_reader.GetValue(1).ToString(), data_reader.GetValue(3).ToString()));
                    exist[0].User_id = data_reader.GetValue(0).ToString();
                }
                data_reader.Close();
                command.Dispose();
                disconnect();

                // set the user if we found one
                if (exist != null)

                    //if the list contains a user, then user already exist it table
                    if (exist.Count > 0)
                    {
                        userExist = exist[0];
                        return true;
                    }
            }
            catch (Exception e)
            {
                connectionFail = true;
                return false;
            }
            return false;
        }

        ////get members of group g_id
        public List<User> getMembers(string g_id)
        {
            List<User> members = new List<User>();
            try
            {
                //set query to find user with same details and executes query
                string query = "SELECT [Group_Id],[Nickname],[Password] " +
                    "from [MS3].[dbo].[Users] " +
                    "where [MS3].[dbo].[Users].[Group_Id] = @g_id";

                connect();
                SqlCommand command = new SqlCommand(query, connection);

                SqlParameter g_id_param = new SqlParameter(@"g_id", SqlDbType.Int, 20);
                g_id_param.Value = g_id;
                command.Parameters.Add(g_id_param);

                SqlDataReader data_reader = command.ExecuteReader();

                //  int index = 0;
                while (data_reader.Read())
                {
                    //add users from the users table to the list
                    members.Add(new User(data_reader.GetValue(1).ToString(), data_reader.GetValue(0).ToString(), data_reader.GetValue(2).ToString()));
                    //// members[index].User_id = data_reader.GetValue(0).ToString();
                    //index++;
                }
                data_reader.Close();
                command.Dispose();
                disconnect();
            }
            catch (Exception e)
            {
                connectionFail = true;
                return null;
            }
            return members;
        }

        ////get id number for user
        public User getUserById(string user_id)
        {
            User user = null;
            try
            {
                //set query to find user with same user id
                string query = "SELECT [Group_Id],[Nickname],[Password] " +
                    "from [MS3].[dbo].[Users] " +
                    "where [MS3].[dbo].[Users].[Id] = @user_id";

                connect();
                SqlCommand command = new SqlCommand(query, connection);
                SqlParameter user_id_param = new SqlParameter(@"user_id", SqlDbType.Int, 20);
                user_id_param.Value = user_id;
                command.Parameters.Add(user_id_param);

                SqlDataReader data_reader = command.ExecuteReader();
                if (data_reader.Read())
                    //creates the user
                    user = new User(data_reader.GetValue(1).ToString(), data_reader.GetValue(0).ToString(), data_reader.GetValue(2).ToString());

                data_reader.Close();
                command.Dispose();
                disconnect();

            }
            catch (Exception e)
            {
                connectionFail = true;
            }
            return user;
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
