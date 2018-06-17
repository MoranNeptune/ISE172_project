using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MileStoneClient.BusinessLayer;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace MileStoneClient.PresistentLayer
{
    //an object that responsable to transfer the message's info to database
    public class MessageHandler : ConnectionHandler
    {
        private List<Message> list; //filtered list
        private String _name; // "" or nickname
        private string _id; //"" or g_id
        private bool connectionFail;
        private DateTime _currTime;

        //constructor
        public MessageHandler(String name, string id)
        {
            _name = name;
            _id = id;
            connectionFail = false;
            list = new List<Message>();
        }

        //update the list to be 200 messages with no filter, return if sucssed
        public bool filterByNone()
        {
            if (!_name.Equals(""))
                _name = "";
            if (!_id.Equals(""))
                _id = "";
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connect();
                SqlCommand command = new SqlCommand(null, connection);
                //query to have no filter
                string query = "SELECT TOP (200) [Group_Id],[Nickname],[Guid],[SendTime],[Body] " +
                    "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                    "WHERE [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id " +
                    "AND [MS3].[dbo].[Messages].[SendTime] <= @curr_time " +
                    //in order not to get empty messages
                    "AND [MS3].[dbo].[Messages].[Body] != ''";

                _currTime = DateTime.UtcNow;
                SqlParameter curr_time_param = new SqlParameter(@"curr_time", SqlDbType.DateTime, 20);
                curr_time_param.Value = _currTime;
                command.Parameters.Add(curr_time_param);

                //set and executes query         
                command.CommandText = query;
                SqlDataReader data_reader = command.ExecuteReader();
                list.Clear();
                while (data_reader.Read())   //add msg from the msgs table to the list
                {
                    DateTime dateFacturation = new DateTime();
                    if (!data_reader.IsDBNull(3))
                        dateFacturation = data_reader.GetDateTime(3);

                    Guid newGuid = new Guid();
                    if (Guid.TryParse(data_reader.GetValue(2).ToString(), out Guid result))
                        newGuid = new Guid(data_reader.GetValue(2).ToString());
                    list.Add(new Message(data_reader.GetValue(4).ToString(), dateFacturation,
                        newGuid, data_reader.GetValue(0).ToString(), data_reader.GetValue(1).ToString()));
                }
                data_reader.Close();
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
            return true;
        }

        //add a new message to the database
        public bool send(Message msg)
        {
            try
            {
                string query = "INSERT INTO Messages ([Guid],[User_Id],[SendTime],[Body]) " +
                                    "VALUES (@msg_guid, @user_id,@msg_DateTime, @msg_Body)";
                //open connection and set command text to be the value of query
                if (connection.State == ConnectionState.Closed)
                    connect();
                SqlCommand command = new SqlCommand(null, connection);
                command.CommandText = query;

                //add the parameters to the query
                SqlParameter msg_guid_param = new SqlParameter(@"msg_guid", SqlDbType.Text, 68);
                SqlParameter msg_DateTime_param = new SqlParameter(@"msg_DateTime", SqlDbType.DateTime, 20);
                SqlParameter msg_Body_param = new SqlParameter(@"msg_Body", SqlDbType.Text, 100);
                SqlParameter user_id_param = new SqlParameter(@"user_id", SqlDbType.Int, 20);

                msg_guid_param.Value = msg.Id.ToString();
                msg_DateTime_param.Value = msg.DateTime;
                msg_Body_param.Value = msg.Body;
                user_id_param.Value = msg.User.User_id;

                command.Parameters.Add(msg_guid_param);
                command.Parameters.Add(msg_DateTime_param);
                command.Parameters.Add(msg_Body_param);
                command.Parameters.Add(user_id_param);

                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
                disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connectionFail = true;
                return false;
            }

            return true;
        }

        //input: message's guid, body to replace at this message, and current DateTime
        //       the function will update it's body and dateTime on the database 
        //output: true for succsess, false for fail
        //assume valid input
        public bool updateMessage(string guid, string body, DateTime time)
        {
            try
            {
                string query = "UPDATE Messages " +
                    "SET [MS3].[dbo].[Messages].[Body] = @msg_body, " +
                    "[MS3].[dbo].[Messages].[SendTime] = @msg_DateTime " +
                    "WHERE [MS3].[dbo].[Messages].[Guid] = @msg_guid";
                if (connection.State == ConnectionState.Closed)
                    connect();
                SqlCommand command = new SqlCommand(null, connection);
                //set query to update message and executes query 
                command.CommandText = query;

                //add the parameters to the query
                SqlParameter msg_Body_param = new SqlParameter(@"msg_Body", SqlDbType.Text, 100);
                SqlParameter msg_DateTime_param = new SqlParameter(@"msg_DateTime", SqlDbType.DateTime, 20);
                SqlParameter msg_guid_param = new SqlParameter(@"msg_guid", SqlDbType.Char, 68);

                msg_Body_param.Value = body;
                msg_DateTime_param.Value = time;
                msg_guid_param.Value = guid;

                command.Parameters.Add(msg_Body_param);
                command.Parameters.Add(msg_DateTime_param);
                command.Parameters.Add(msg_guid_param);

                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
                disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connectionFail = true;
                return false;
            }
            return true;
        }

        //returns a list of the last mesasge's since last message's DateTime and retrieve only those who sent after it 
        //the list will include no more than 200 new messages
        public List<Message> retrieve(DateTime time)
        {
            List<Message> tempList = new List<Message>();
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connect();

                SqlCommand command = new SqlCommand(null, connection);
                SqlParameter user_g_id_param;
                SqlParameter user_name_param;

                string query = "SELECT TOP (200) [Group_Id],[Nickname],[Guid],[SendTime],[Body] " +
                        "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                        "WHERE [MS3].[dbo].[Messages].[SendTime] > @msg_time " +
                        "AND [MS3].[dbo].[Messages].[SendTime] <= @curr_time " +
                        "AND [MS3].[dbo].[Messages].[User_Id] = [MS3].[dbo].[Users].Id " +
                        //in order not to get empty messages
                        "AND [MS3].[dbo].[Messages].[Body] != '' ";

                _currTime = DateTime.UtcNow;
                SqlParameter msg_time_param = new SqlParameter(@"msg_time", SqlDbType.DateTime, 20);
                SqlParameter curr_time_param = new SqlParameter(@"curr_time", SqlDbType.DateTime, 20);

                msg_time_param.Value = time;
                curr_time_param.Value = _currTime;

                command.Parameters.Add(msg_time_param);
                command.Parameters.Add(curr_time_param);

                //the retrieve will be by filter
                //ID filter & user filter
                if (!_id.Equals(""))
                {
                    query = query + " AND [MS3].[dbo].[Users].[Group_Id] = @user_g_id ";
                    user_g_id_param = new SqlParameter("@user_g_id", SqlDbType.Int, 20);
                    user_g_id_param.Value = _id;
                    command.Parameters.Add(user_g_id_param);
                }
                //user filter
                if (!_name.Equals(""))
                {
                    query = query + " AND [MS3].[dbo].[Users].[Nickname] = @user_name";
                    user_name_param = new SqlParameter("@user_name", SqlDbType.Char, 8);
                    user_name_param.Value = _name;
                    command.Parameters.Add(user_name_param);
                }
                //set query to update message and executes query 
                command.CommandText = query;
                SqlDataReader data_reader = command.ExecuteReader();
                while (data_reader.Read())   //add msg from the msgs table to the list
                {
                    DateTime dateFacturation = new DateTime();
                    if (!data_reader.IsDBNull(3))
                        dateFacturation = data_reader.GetDateTime(3);

                    Guid newGuid = new Guid();
                    if (Guid.TryParse(data_reader.GetValue(2).ToString(), out Guid result))
                        newGuid = new Guid(data_reader.GetValue(2).ToString());
                    tempList.Add(new Message(data_reader.GetValue(4).ToString(), dateFacturation,
                        newGuid, data_reader.GetValue(0).ToString(), data_reader.GetValue(1).ToString()));
                }
                data_reader.Close();
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
                disconnect();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connectionFail = true;
            }
            return tempList;
        }

        //update the list with filter by group ID, return if sucssed
        public bool FilterByGroup(string g_id)
        {
            //check if the id to filter by is new and change it if so
            if (!g_id.Equals(_id))
                _id = g_id;
            if (!_name.Equals(""))
                _name = "";
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connect();
                SqlCommand command = new SqlCommand(null, connection);
                //query to filter by group id
                string query = "SELECT TOP (200) [Group_Id],[Nickname],[Guid],[SendTime],[Body] " +
                        "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                        "WHERE [MS3].[dbo].[Users].[Group_Id] = @user_G_id" +
                        " AND [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id " +
                        "AND [MS3].[dbo].[Messages].[SendTime] <= @curr_time " +
                        //in order not to get empty messages
                        "and [MS3].[dbo].[Messages].[Body] != ''";
                SqlParameter user_G_id_param = new SqlParameter(@"user_G_id", SqlDbType.Int, 20);
                SqlParameter curr_time_param = new SqlParameter(@"curr_time", SqlDbType.DateTime, 20);

                _currTime = DateTime.UtcNow;
                user_G_id_param.Value = g_id;
                curr_time_param.Value = _currTime;

                command.Parameters.Add(user_G_id_param);
                command.Parameters.Add(curr_time_param);

                //set query to update message and executes query 
                command.CommandText = query;
                SqlDataReader data_reader = command.ExecuteReader();
                list.Clear();
                while (data_reader.Read())   //add msg from the msgs table to the list
                {
                    DateTime dateFacturation = new DateTime();
                    if (!data_reader.IsDBNull(3))
                        dateFacturation = data_reader.GetDateTime(3);

                    Guid newGuid = new Guid();
                    if (Guid.TryParse(data_reader.GetValue(2).ToString(), out Guid result))
                        newGuid = new Guid(data_reader.GetValue(2).ToString());
                    list.Add(new Message(data_reader.GetValue(4).ToString(), dateFacturation,
                        newGuid, data_reader.GetValue(0).ToString(), data_reader.GetValue(1).ToString()));
                }
                data_reader.Close();
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
                disconnect();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        //update the list with filter by user, return if sucssed
        public bool FilterByUser(string nickname, string g_id)
        {
            //check if the user to filter by is new and change it if so
            if (!g_id.Equals(_id))
                _id = g_id;
            if (!nickname.Equals(_name))
                _name = nickname;
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connect();
                SqlCommand command = new SqlCommand(null, connection);
                //query to filter by user nickname
                string query = "SELECT TOP (200) [Group_Id],[Nickname], [Guid],[SendTime],[Body] " +
                        "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                        "WHERE [MS3].[dbo].[Users].Nickname = @user_Nickname" +
                        " AND [MS3].[dbo].[Users].[Group_Id] = @user_G_id" +
                        " AND [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id " +
                        "AND [MS3].[dbo].[Messages].[SendTime] <= @curr_time " +
                        //in order not to get empty messages
                        "and [MS3].[dbo].[Messages].[Body] != ''";

                _currTime = DateTime.UtcNow;
                SqlParameter user_Nickname_param = new SqlParameter(@"user_Nickname", SqlDbType.Char, 8);
                SqlParameter user_G_id_param = new SqlParameter(@"user_G_id", SqlDbType.Int, 20);
                SqlParameter curr_time_param = new SqlParameter(@"curr_time", SqlDbType.DateTime, 20);

                user_Nickname_param.Value = nickname;
                user_G_id_param.Value = g_id;
                curr_time_param.Value = _currTime;

                command.Parameters.Add(user_Nickname_param);
                command.Parameters.Add(user_G_id_param);
                command.Parameters.Add(curr_time_param);

                //set query to update message and executes query 
                command.CommandText = query;
                list.Clear();
                SqlDataReader data_reader = command.ExecuteReader();
                while (data_reader.Read())   //add msg from the msgs table to the list
                {
                    DateTime dateFacturation = new DateTime();
                    if (!data_reader.IsDBNull(3))
                        dateFacturation = data_reader.GetDateTime(3);

                    Guid newGuid = new Guid();
                    if (Guid.TryParse(data_reader.GetValue(2).ToString(), out Guid result))
                        newGuid = new Guid(data_reader.GetValue(2).ToString());
                    list.Add(new Message(data_reader.GetValue(4).ToString(), dateFacturation,
                        newGuid, data_reader.GetValue(0).ToString(), data_reader.GetValue(1).ToString()));
                }
                data_reader.Close();
                command.Prepare();
                command.ExecuteNonQuery();
                command.Dispose();
                disconnect();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public List<Message> List
        {
            get { return list; }
            set { list = value; }
        }

    }
}