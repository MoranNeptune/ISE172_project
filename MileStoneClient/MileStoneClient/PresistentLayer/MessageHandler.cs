using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MileStoneClient.BusinessLayer;
using System.Collections.Generic;

namespace MileStoneClient.PresistentLayer
{
  //  [Serializable]
    //an object that responsable to transfer the message's info to files
    public class MessageHandler
    {
        private List<Message> list; //filtered & sorted list
        private String _name; // "" or nickname
        private string _id; //"" or g_id

        //constructor
        public MessageHandler(String name, string id)
        {
            _name = name;
            _id = id;

            //no filter
            if (id.Equals("") && !filterByNone()) //init the list
                    throw new Exception("connection problem");
            //ID filter
            else if (name.Equals("") && !filterById(id)) //init the list
                    throw new Exception("connection problem");
            //user filter
            else if (!filterByUser(name, id)) //init the list
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
            throw new NotImplementedException();
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
            
        }

       /* //add a list of new message to the database
        public bool send(List<Message> msgs)
        {
            //bool? list?
        }*/

        //delete a message from the database
        public bool delete (Message msg)
        {
            //check if this msg exist in the database- return false if not
            //delete it if it is and return true
            //else return false
            return false;
        }

        //input: already changed message. the function will update it's body and dateTime on the database
        //output: true for succsess, false for fail
        public bool updateMessage(Message msg)
        {
            //find this message on the database and change it's time and body
            //update the list as needed- call to retrieve?
        }

        //update the list with the new messages
        public List<Message> retrieve()
        {
            //check for the last mesasge's dateTime and retrieve only those who sent after it and also less then 200 new messages
            //the retrieve will be by filter and sort
            //add it to a new temp list, add this temp list to the end of this list and return the temp list
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

    }
}