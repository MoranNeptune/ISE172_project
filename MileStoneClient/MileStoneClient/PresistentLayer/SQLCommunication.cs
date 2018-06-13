using MileStoneClient.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MileStoneClient.PresistentLayer
{
    //maybe move to a different layre - databaseLayer - see later
    //Singleton class SQLCommunication
    public sealed class SQLCommunication
    {
        private static SQLCommunication instance = null;
        private static readonly object padlock = new object();

        private string connetion_string;
        private string sql_query;
        private string server_address;
        private string database_name;
        private string user_name;
        private string password;
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader data_reader;

        //private constructor for singleton
        private SQLCommunication()
        {
            connetion_string = null;
            sql_query = null;
            server_address = "ise172.ise.bgu.ac.il,1433\\DB_LAB";
            database_name = "MS3";
            user_name = "publicUser";
            password = "isANerd";
            connetion_string = $"Data Source={server_address};Initial Catalog={database_name };User ID={user_name};Password={password}";
            connection = new SqlConnection(connetion_string);

        }

        public static SQLCommunication Instance
        {
            get
            {   //only if there is no instance lock object, otherwise return instance
                if (instance == null)
                {
                    lock (padlock) // senario: n threads in here,
                    {              //locking the first and others going to sleep till the first get new Instance
                        if (instance == null)  // rest n-1 threads no need new instance because its not null anymore.
                        {
                            instance = new SQLCommunication();
                        }
                    }
                }
                return instance;
            }
        }

     /*   //read users' details from table and return list of users
        public List<User> ReadUserTable(string query)
        {
            List<User> users = new List<User>();
            connection.Open();
            sql_query = query;

            command = new SqlCommand(sql_query, connection);
            data_reader = command.ExecuteReader();

            while (data_reader.Read())
            {
                //add users from the users table to the list
                users.Add(new User(data_reader.GetValue(1).ToString(), data_reader.GetValue(0).ToString(), data_reader.GetValue(2).ToString()));
            }
            data_reader.Close();
            command.Dispose();
            connection.Close();

            return users;

        }

        //add new user or msg to the needed table - assume valid  input in query
        //or update a msg
        public void UpdateTable(string query, List<SqlParameter> param)
        {
                //open connection and set command text to be the value of query
                connection.Open();
                command = new SqlCommand(null, connection);
                command.CommandText = query;
            //add the parameters to the query
            for (int i = 0; i < param.Count; i++)
                command.Parameters.Add(param[i]);
           
            //call Prepare after setting the Commandtext and Parameters.
            command.Prepare();
                int num_rows_changed = command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
        }

        //read msgs' details from table and return list of msgs
        public List<Message> ReadMessageTable(string query)
        {
            List<Message> messages = new List<Message>();
            connection.Open();
            sql_query = query;

            command = new SqlCommand(sql_query, connection);
            data_reader = command.ExecuteReader();

            while (data_reader.Read())
            {
                // צריך לשים לב לעשות בדיקה לזמן שמתקבל כמו שעשו בתרגול
                //add msgs from the msgs table to the list
               // messages.Add(new Message(data_reader.GetValue(3), /*dateTime*/data_reader.GetValue(2), /*guid*/, );
                ///////להחליט אם בנאי חדש להודעות או שכאן לחפש משתמש
                /////check if the msg legal
         /*   }
            data_reader.Close();
            command.Dispose();
            connection.Close();
            return messages;
        }*/

        public void InsertQuery(string query, List<SqlParam> param)
        {
            //open connection and set command text to be the value of query
            connection.Open();
            command = new SqlCommand(null, connection);
            command.CommandText = query;
             for (int i = 0; i < param.Count; i++)
                {
                    //add the parameters to the query
                    SqlParameter tParam = new SqlParameter(param[i].Name, SqlDbType.Text, 20);
                    tParam.Value = param[i].Value;
                    command.Parameters.Add(tParam);
                }
            
        }
        ///לא רלוונטי יותר- מוחלף על ידי אינסרט קוורי
        public List<User> ExecuteUserQuery(string query, User user)
        {
            List<User> users = new List<User>();
            //open connection and set command text to be the value of query
            connection.Open();
            command = new SqlCommand(null, connection);

            //////להעביר לתוך אינסרט??
            SqlParameter user_G_id_param = new SqlParameter(@"user_G_id", SqlDbType.Text, 20);
            SqlParameter user_Nickname_param = new SqlParameter(@"user_Nickname", SqlDbType.Text, 20);
            SqlParameter Password_param = new SqlParameter(@"Password", SqlDbType.Text, 20);

            user_G_id_param.Value = user.G_id;
            user_Nickname_param.Value = user.Nickname;
            Password_param.Value = user.Password;

            //add new user
            if (query.Contains("INSERT"))
            {
                command.CommandText = query;
                //add the parameters to the query
                command.Parameters.Add(user_G_id_param);
                command.Parameters.Add(user_Nickname_param);
                command.Parameters.Add(Password_param);
            }
            //update users list or check if user exist
            else if (query.Contains("SELECT") | query.Contains("select"))
            {
                command.CommandText = query;
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    //add users from the users table to the list
                    users.Add(new User(data_reader.GetValue(1).ToString(), data_reader.GetValue(0).ToString(), data_reader.GetValue(2).ToString()));
                }
                data_reader.Close();
            }
            /////////////close
            command.Prepare();
            int num_rows_changed = command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            return users;
        }
        ///לא רלוונטי- הופך לאינסרט קוורי
        public List<Message> ExecuteMessageQuery(string query, Message msg)
        {
            List<Message> msgs = new List<Message>();
            //open connection and set command text to be the value of query
            connection.Open();
            command = new SqlCommand(null, connection);

            //////להעביר לתוך אינסרט??
            SqlParameter user_G_id_param = new SqlParameter(@"user_G_id", SqlDbType.Text, 20);
            SqlParameter user_Nickname_param = new SqlParameter(@"user_Nickname", SqlDbType.Text, 20);
            SqlParameter Password_param = new SqlParameter(@"Password", SqlDbType.Text, 20);

            user_G_id_param.Value = user.G_id;
            user_Nickname_param.Value = user.Nickname;
            Password_param.Value = user.Password;

            //add new user
            if (query.Contains("INSERT"))
            {
                command.CommandText = query;
                //add the parameters to the query
                command.Parameters.Add(user_G_id_param);
                command.Parameters.Add(user_Nickname_param);
                command.Parameters.Add(Password_param);
            }
            //update users list or check if user exist
            else if (query.Contains("SELECT") | query.Contains("select"))
            {
                command.CommandText = query;
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    //add users from the users table to the list
                    msgs.Add(new User(data_reader.GetValue(1).ToString(), data_reader.GetValue(0).ToString(), data_reader.GetValue(2).ToString()));
                }
                data_reader.Close();
            }
            /////////////close
            command.Prepare();
            int num_rows_changed = command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            return msgs;
        }

        ///לא באמת מעודכן, צריך לבדוק אל מול השינויים בהנדלר
        public List<User> ExecuteSelectUserQuery(string query, string where)
        {
            List<User> users = new List<User>();
            //open connection and set command text to be the value of query
            connection.Open();
            command = new SqlCommand(null, connection);
                command.CommandText = query;
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    //add users from the users table to the list
                    users.Add(new User(data_reader.GetValue(1).ToString(), data_reader.GetValue(0).ToString(), data_reader.GetValue(2).ToString()));
                }
                data_reader.Close();
            
            /////////////close
            command.Prepare();
            int num_rows_changed = command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            return users;
        }

        public List<Message> FilterQuery (String query, string nickname, string g_id)
        {
            List<Message> msgs = new List<Message>();
            //open connection and set command text to be the value of query
            connection.Open();
            command = new SqlCommand(null, connection);

            SqlParameter user_G_id_param = new SqlParameter(@"user_G_id", SqlDbType.Text, 20);
            SqlParameter user_Nickname_param = new SqlParameter(@"user_Nickname", SqlDbType.Text, 20);

            user_G_id_param.Value = g_id;
            user_Nickname_param.Value = nickname;
          
            //update users list or check if user exist
            if (query.Contains("SELECT") | query.Contains("select"))
            {
                command.CommandText = query;
                data_reader = command.ExecuteReader();
                while (data_reader.Read())
                {
                    //add users from the users table to the list
                    msgs.Add(new User(data_reader.GetValue(1).ToString(), data_reader.GetValue(0).ToString(), data_reader.GetValue(2).ToString()));
                }
                data_reader.Close();
            }
            /////////////close
            command.Prepare();
            int num_rows_changed = command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            return msgs;
            if (!nickname.Equals(""))
            {

            }
            //query to filter by user nickname
            string query = "SELECT [Group_Id],[Nickname],[SendTime],[Body] " +
                "VALUES (@user_G_id, @user_Nickname)" +
                    "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
            "WHERE [MS3].[dbo].[Users].Nickname = @user_Nickname" +
                    " AND [MS3].[dbo].[Users].[Group_Id] = @user_G_id" + 
                    " AND [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id";
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
            {
                connectionFail = true;
            }
            return connectionFail;
        }

    }
}
