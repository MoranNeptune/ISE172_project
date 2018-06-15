using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MileStoneClient.BusinessLayer;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace MileStoneClient.PresistentLayer
{
  //  [Serializable]
    //an object that responsable to transfer the message's info to files
    public class MessageHandler : ConnectionHandler
    {
        private List<Message> list; //filtered & sorted list
        private String _name; // "" or nickname
        private string _id; //"" or g_id
        private bool connectionFail;
       // private Message queryMessage;

        //constructor
        public MessageHandler(String name, string id)
        {
            _name = name;
            _id = id;
            connectionFail = false;
            list=new List<Message>();
        }

        //update the list as needed, return if sucssed
        public bool filterByNone()
        {
            if (!_name.Equals(""))
                _name = "";
            if (!_id.Equals(""))
                _id = "";
            try
            {
                if(connection.State == ConnectionState.Closed)
                    connect();
                SqlCommand command = new SqlCommand(null, connection);
                //query to filter by user nickname
                string query = "SELECT TOP (200) [Group_Id],[Nickname],[Guid],[SendTime],[Body] " +
                    "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                    "WHERE [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id";
                ///////execute
                command.CommandText = query;
                SqlDataReader data_reader = command.ExecuteReader();
                list.Clear();
                while (data_reader.Read())   //add msg from the msgs table to the list
                {
                    list.Add(new Message(data_reader.GetValue(4).ToString(), (System.DateTime)data_reader.GetValue(3),
                        data_reader.GetValue(2).ToString(), data_reader.GetValue(0).ToString(), data_reader.GetValue(1).ToString()));
                }
                data_reader.Close();
                ///////////close
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
                SqlParameter msg_guid_param = new SqlParameter(@"msg_guid", SqlDbType.Text, 20);
                SqlParameter msg_DateTime_param = new SqlParameter(@"msg_DateTime", SqlDbType.DateTime, 20);
                SqlParameter msg_Body_param = new SqlParameter(@"msg_Body", SqlDbType.Text, 20);
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

        //input: the message we want to update and the new body. the function will update it's body and dateTime on the database and update the list
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
                command.CommandText = query;

                //add the parameters to the query
                SqlParameter msg_Body_param = new SqlParameter(@"msg_Body", SqlDbType.Text, 20);
                SqlParameter msg_DateTime_param = new SqlParameter(@"msg_DateTime", SqlDbType.DateTime, 20);
                SqlParameter msg_guid_param = new SqlParameter(@"msg_guid", SqlDbType.Text, 20);

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
            //if the update sucsed and the msg relevant to this filter, update also the list of msgs (delete it from the list and add it in the end of it)
            /////////////עדן צריכה להוסיף אצלה
            /*if (list.Contains(msg))
            {
                list.Remove(msg);
                msg.DateTime =time;
                msg.Body = body;
                list.Add(msg);
            }*/
            return true;
        }

        //update the list with the new messages since the last message on the list
        public List<Message> retrieve(DateTime time)
        {
            List<Message> tempList = new List<Message>();
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connect();
                SqlCommand command = new SqlCommand(null, connection);
                //check for the last mesasge's dateTime and retrieve only those who sent after it and also less then 200 new messages
            //    DateTime time=list[list.Count - 1].DateTime;
                //the retrieve will be by filter
                //add it to a new temp list, add this temp list to the end of this list and return the temp list
                SqlParameter user_g_id_param;
                SqlParameter user_name_param;
                string query = "SELECT TOP (200) [Group_Id],[Nickname],[Guid],[SendTime],[Body] " +
                        "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                        "WHERE [MS3].[dbo].[Messages].[SendTime] > @msg_time " +  
                        "AND [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id";
                

                //ID filter & user filter
                if (!_id.Equals(""))
                {
                    query = query + " AND [MS3].[dbo].[Users].[Group_Id] = @user_g_id" ;
                    user_g_id_param = new SqlParameter("@user_g_id", _id);
                    command.Parameters.Add(user_g_id_param);
                }
                //user filter
                if (!_name.Equals(""))
                {
                    query = query + "[MS3].[dbo].[Users].Nickname = @user_name";
                    user_name_param = new SqlParameter("@user_name", _name);
                    command.Parameters.Add(user_name_param);
                }

                SqlParameter msg_time_param = new SqlParameter(@"msg_time", SqlDbType.DateTime, 20);
                msg_time_param.Value = time;
                command.Parameters.Add(msg_time_param);

                ///////execute
                command.CommandText = query;
                SqlDataReader data_reader = command.ExecuteReader();
                while (data_reader.Read())   //add msg from the msgs table to the list
                {
                    tempList.Add(new Message(data_reader.GetValue(4).ToString(), (System.DateTime)data_reader.GetValue(3),
                        data_reader.GetValue(2).ToString(), data_reader.GetValue(0).ToString(), data_reader.GetValue(1).ToString()));
                }
                data_reader.Close();
                ///////////close
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

        //check if the id to filter by is new and change it if so
        //update the list as needed, return if sucssed
        public bool FilterByGroup(string g_id)
        {
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
                        " AND [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id";
                SqlParameter user_G_id_param = new SqlParameter(@"user_G_id", SqlDbType.Int, 20);
                user_G_id_param.Value = g_id;
                command.Parameters.Add(user_G_id_param);
                ///////execute
                command.CommandText = query;
                SqlDataReader data_reader = command.ExecuteReader();
                list.Clear();
                while (data_reader.Read())   //add msg from the msgs table to the list
                {        
                    list.Add(new Message(data_reader.GetValue(4).ToString(), (System.DateTime) data_reader.GetValue(3),
                        data_reader.GetValue(2).ToString(), data_reader.GetValue(0).ToString(), data_reader.GetValue(1).ToString()));
                }
                data_reader.Close();
                ///////////close
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

        //check if the user to filter by is new and change it if so
        //update the list as needed, return true if sucssed
        public bool FilterByUser(string nickname, string g_id)
        {
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
                        " AND [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id";
      
                SqlParameter user_Nickname_param = new SqlParameter(@"user_Nickname", SqlDbType.Char, 20);
                SqlParameter user_G_id_param = new SqlParameter(@"user_G_id", SqlDbType.Int, 20);

                user_Nickname_param.Value = nickname;
                user_G_id_param.Value = g_id;

                command.Parameters.Add(user_Nickname_param);
                command.Parameters.Add(user_G_id_param);

                ///////execute
                command.CommandText = query;
                list.Clear();
                SqlDataReader data_reader = command.ExecuteReader();
                while (data_reader.Read())   //add msg from the msgs table to the list
                {         
                    list.Add(new Message(data_reader.GetValue(4).ToString(), (System.DateTime)data_reader.GetValue(3),
                        data_reader.GetValue(2).ToString(), data_reader.GetValue(0).ToString(), data_reader.GetValue(1).ToString()));
                }
                data_reader.Close();
                ///////////close
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