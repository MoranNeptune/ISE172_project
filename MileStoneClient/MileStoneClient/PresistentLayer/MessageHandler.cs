
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MileStoneClient.BusinessLayer;
using System.Collections.Generic;

namespace MileStoneClient.PresistentLayer
{
    [Serializable]
    /// <summary>
    ///  an object that responsable to transfer the message's info to files
    /// </summary>
    class MessageHandler
    {
        private List<Message> list;
        private String name;

        /// <summary>
        /// a constructor for this class that get the file name ("AllMessages" or username+group ID)
        /// </summary>
        /// <param name="name"> file name </param>
        public MessageHandler(String name)
        {
            //assume name is valid
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
            }
        }

        /// <summary>
        /// add a new Message to this list and afterwards to the file
        /// </summary>
        /// <param name="msg"> new message </param>
        /// <returns> true if succsed, else false </returns>
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

        /// <summary>
        /// add a list of new Message's to this list and afterwards to the file
        /// </summary>
        /// <param name="msgList"> a list of new messages </param>
        /// <returns> true if succsed, else false </returns>
        public bool updateFile(List<Message> msgList)
        {
            int numThisList = list.Count;
            int numNewList = msgList.Count;
            list.AddRange(msgList);
            SortByTimeStamp();
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
            list.RemoveRange(numThisList + 1, numNewList);
            return false;
        }

        /// <summary>
        /// delete the file with this name
        /// </summary>
        /// <returns> true if succsed, else false </returns>
        private bool deleteFile()
        {
            //assume there is a file with this name
            File.Delete(name + ".bin");
            return !(File.Exists(name + ".bin"));
        }

        /// <summary>
        /// open a new file with this name
        /// </summary>
        /// <returns> true if succsed, else false </returns>
        private bool openNewFile()
        {
            //assume there isnt a file with this name
            Stream fileStream = File.Create(name + ".bin");
            fileStream.Close();
            return File.Exists(name + ".bin");
        }

        /// <summary>
        /// geters and seters of this list
        /// </summary>
        public List<Message> List
        {
            get { return list; }
            set { list = value; }
        }

        /// <summary>
        /// sort this list by time stamp, using the message comperator
        /// </summary>
        private void SortByTimeStamp()
        {
            MessageComperator comperator = new MessageComperator();
            this.list.Sort(comperator);
        }
    }
}