using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MileStoneClient.BusinessLayer;
using System.Collections.Generic;

namespace MileStoneClient.PresistentLayer
{
    //an object that responsable to transfer the user's info to files
    class UserHandler
    {
        private List<User> list;
        private String name;

        //constructor
        public UserHandler(String name)
        {
            //assume name is valid
            this.name = name;
            //check if there is already a file with this needed data, and open a new one if not
            if (!File.Exists(name + ".bin"))
            {
                Stream myFileStream = File.Create(name + ".bin");
                myFileStream.Close();
                list = new List<User>();
            }
            //deserialize the list of User's from the file with this name
            else
            {
                Stream ReadFileStream = File.OpenRead(name + ".bin");
                BinaryFormatter deserializer = new BinaryFormatter();
                if (new FileInfo(name + ".bin").Length != 0)
                    list = (List<User>)deserializer.Deserialize(ReadFileStream);
                else this.list = new List<User>();
                ReadFileStream.Close();
            }
        }

        //add a new User to this list and afterwards to the file, return true if succsed
        public bool updateFile(User user)
        {
            list.Add(user);
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
            list.Remove(user);
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
        }

        public List<User> List
        {
            get { return list; }
            set { list = value; }
        }

    }
}