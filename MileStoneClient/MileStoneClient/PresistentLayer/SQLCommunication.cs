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

        // Read users' details from table and return list of users
        public List<User> ReadUserTable(string query)
        {
            List<User> users = new List<User>();
            connection.Open();
            sql_query = query;

            command = new SqlCommand(sql_query, connection);
            data_reader = command.ExecuteReader();

            while (data_reader.Read())
            {
                // Add users from the users table to the list
                users.Add(new User(data_reader.GetValue(2).ToString(), new ID(data_reader.GetValue(1).ToString())));
                ///////need to add password to all users some time
            }
            data_reader.Close();
            command.Dispose();
            connection.Close();

            return users;
        }

        // Add new user to Users table - assume valid user inputed in query
        public void addToUserTable(string query)
        {
            try
            {
                // Open connection and set command text to be the value of query
                connection.Open();
                command = new SqlCommand(null, connection);
                command.CommandText = query;

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();
                int num_rows_changed = command.ExecuteNonQuery();
                 command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());

            }
        }
    }
}
