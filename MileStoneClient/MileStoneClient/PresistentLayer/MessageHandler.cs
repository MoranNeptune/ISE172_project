using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MileStoneClient.BusinessLayer;
using System.Collections.Generic;

namespace MileStoneClient.PresistentLayer
{
  //  [Serializable]
    //an object that responsable to transfer the message's info to files
    public class MessageHandler : IQueryAction
    {
        private List<Message> list; //filtered & sorted list
        private String _name; // "" or nickname
        private string _id; //"" or g_id
        private bool connectionFail;
        private Message queryMessage;

        //constructor
        public MessageHandler(String name, string id)
        {
            _name = name;
            _id = id;
            connectionFail = false;
            queryMessage = null;

            ///////////////////////////////להוסיף קליר לפארם בכל מקום!!!
            //no filter
            if (_id.Equals("") && !filterByNone()) //init the list
                throw new Exception("connection problem"); /////להחליף את הקונקשיין פייל לטרו?
            //ID filter
            else if (_name.Equals("") && !filterById(_id)) //init the list
                throw new Exception("connection problem");
            //user filter
            else if (!filterByUser(_name, _id)) //init the list
                throw new Exception("connection problem");

            /*//assume name is valid
            this.name = name;
            //check if there is already a file with this needed data, and open a new one if not
            if (!File.Exists(name + ".bin"))
            {
                Stream myFileStream = File.Create(name + ".bin");
                myFileStream.Close();
                list = new List<Message>();
            }
            //deserialize the list of Message's from the file with this name
            else
            {
                Stream ReadFileStream = File.OpenRead(name + ".bin");
                BinaryFormatter deserializer = new BinaryFormatter();
                if (new FileInfo(name + ".bin").Length != 0)
                    list = (List<Message>)deserializer.Deserialize(ReadFileStream);
                else this.list = new List<Message>();
                ReadFileStream.Close();
            }*/

            /*  if (filter.Equals("user"))
              {
                  list=UserFilter()
              }*/
        }

        //check if the user to filter by is new and change it if so
        //update the list as needed, return if sucssed
        private bool filterByUser(string name, string id)
        {
            if (!id.Equals(_id))
                _id = id;
            if (!name.Equals(_name))
                _name = name;
            UserFilter uf = new UserFilter(name, id);
            if(!uf.ConnectionFail)
                list = uf.Msgs;
            return !uf.ConnectionFail;
        }

        //update the list as needed, return if sucssed
        private bool filterByNone()
        {
            connectionFail = false;
            //query to filter by user nickname
            string query = "SELECT TOP (200) [Group_Id],[Nickname],[SendTime],[Body] " +
                    "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                    "WHERE [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id";
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connectionFail = true;
                return false;
            }
            return true;
        }

        //check if the id to filter by is new and change it if so
        //update the list as needed, return if sucssed
        private bool filterById(String id)
        {
            if (!id.Equals( _id))
                _id = id;
            GroupFilter gf = new GroupFilter(_id);
            if(!gf.ConnectionFail)
                list = gf.Msgs;
            return !gf.ConnectionFail;
        }

        //add a new message to the database
        public bool send(Message msg)
        {
            //set query to add msg and executes query 
            /////guid- ours or sqls?
            /////user ID- need to get it from the usersTable or from the user.ID if possible
            string query = "INSERT INTO Messages ([User_Id],[SendTime],[Body]) " +
                                   "VALUES (@user_id, @msg_DateTime,@msg_Body)";

          //  "VALUES (" + /*userID +*/ ", '" + msg.DateTime + "','" + msg.Body + "')";
            //queryMessage = msg;
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connectionFail = true;
                return false;
            }

            list.Add(msg);
            //queryMessage = null;
            return true;
        }

       /* //add a list of new message to the database
        public bool send(List<Message> msgs)
        {
            //bool? list?
        }*/

        /*//delete a message from the database
        public bool delete (Message msg)
        {
            //check if this msg exist in the database- return false if not
            //delete it if it is and return true
            //else return false
            queryMessage = msg;
            string query= "DELETE FROM Messages" + "WHERE Guid =@msg_Id";
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connectionFail = true;
                return false;
            }
            if (list.Contains(msg)) //update the list
                list.Remove(msg);
            queryMessage = null;
            return true;
        }*/

        //input: the message we want to update and the new body. the function will update it's body and dateTime on the database and update the list
        //output: true for succsess, false for fail
        //assume valid input
        public bool updateMessage(Message msg, String body)
        {
            //DateTime time = new DateTime(); ////איך מגדירים שיהיה לעכשיו? //////להעביר לקומיוניקשן
            string query = "UPDATE Messages" + "SET body =" + body + ", time = @msg_DateTime"+"WHERE guid = @msg_guid";
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                connectionFail = true;
                return false;
            }
            //if the update sucsed and the msg relevant to this filter, update also the list of msgs (delete it from the list and add it in the end of it)
            if (list.Contains(msg))
            {
                list.Remove(msg);
                msg.DateTime =time;
                msg.Body = body;
                list.Add(msg);
            }
            return true;
        }

        //update the list with the new messages since the last message on the list
        public bool retrieve()
        {
            //check for the last mesasge's dateTime and retrieve only those who sent after it and also less then 200 new messages
            DateTime time=list[list.Count - 1].DateTime;
            //the retrieve will be by filter
            //add it to a new temp list, add this temp list to the end of this list and return the temp list
            List<Message> tempList = new List<Message>();
            List<SqlParam> param = new List<SqlParam>();
            string query = "SELECT TOP (200) [Group_Id],[Nickname],[SendTime],[Body] " +
                    "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                    "WHERE [MS3].[dbo].[Messages].[SendTime] > @msg_time" + time.ToString() +
                    "AND [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id";
            SqlParam msg_time = new SqlParam("@msg_time", time.ToString());
            param.Add(msg_time);
            //ID filter & user filter
            if (!_id.Equals(""))
            {
                query = query + " AND [MS3].[dbo].[Users].[Group_Id] = @user_g_id" ;
                SqlParam user_g_id = new SqlParam("@user_g_id", _id);
                param.Add(user_g_id);
            }
            //user filter
            else if (!_name.Equals(""))
            {
                query = query + "[MS3].[dbo].[Users].Nickname = @user_name";
                SqlParam user_name = new SqlParam("@user_name", _name);
                param.Add(user_name);
            }
            else
                try
                {
                    tempList = Instance.FilterQuery(query, param);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    connectionFail = true;
                    return false;
                }
            list.AddRange(tempList);
            return true;
        }

        public override void ExecuteQuery(string query)
        {
            //add new msg
            if (query.Contains("SELECT"))
            {
                list = Instance.FilterQuery(query, param);
            }
            ////////לשנות
            //update msgs list
            else
            {
                Instance.ExecuteMessageQuery(query, queryMessage);
            }
        }

        /*  //add a new Message to this list and afterwards to the file, return true if succsed
          public bool updateFile(Message msg)
          {
              list.Add(msg);
              if (File.Exists(name + ".bin"))
              {
                  if (deleteFile())
                  {
                      if (openNewFile())
                      {
                          Stream fileStream = File.OpenWrite(name + ".bin");
                          BinaryFormatter serializer = new BinaryFormatter();
                          serializer.Serialize(fileStream, list);
                          fileStream.Close();
                          return true;
                      }
                  }
              }
              //if the update failed- dont change this list and return false
              list.Remove(msg);
              return false;
          }

          //add a list of new Message's to this list and afterwards to the file, return true if succsed
          public bool updateFile(List<Message> msgList)
          {
              int numThisList = list.Count;
              int numNewList = msgList.Count;
              list.AddRange(msgList);
              if (File.Exists(name + ".bin"))
              {
                  if (deleteFile())
                  {
                      if (openNewFile())
                      {
                          Stream fileStream = File.OpenWrite(name + ".bin");
                          BinaryFormatter serializer = new BinaryFormatter();
                          serializer.Serialize(fileStream, list);
                          fileStream.Close();
                          //if (checkSuccess())
                          return true;
                      }
                  }
              }
              //if the update failed- dont change this list and return false
              list.RemoveRange(numThisList + 1, numNewList);
              return false;
          }

          //delete the file with this name
          private bool deleteFile()
          {
              //assume there is a file with this name
              File.Delete(name + ".bin");
              return !(File.Exists(name + ".bin"));
          }

          //open a new file with this name
          private bool openNewFile()
          {
              //assume there isnt a file with this name
              Stream fileStream = File.Create(name + ".bin");
              fileStream.Close();
              return File.Exists(name + ".bin");
          }*/

        public List<Message> List
        {
            get { return list; }
            set { list = value; }
        }

        public List<SqlParam> param { get; private set; }
    }
}