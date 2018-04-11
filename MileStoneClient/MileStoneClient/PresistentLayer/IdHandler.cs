using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MileStoneClient.BusinessLayer;
using System.Collections.Generic;

namespace MileStoneClient.PresistentLayer
{
    //an object that responsable to transfer the ID's info to files
    class IdHandler
    {
        private List<ID> list = null;
        private String name;

        //constructor
        public IdHandler(String name)
        {
            //assume name is valid
            this.name = name;
            //check if there is already a file with this needed data, and open a new one if not
            if (!File.Exists(name + ".bin"))
            {
                Stream myFileStream = File.Create(name + ".bin");
                myFileStream.Close();
                this.list = new List<ID>();
            }
            //if the file exists
            else
            {
                Stream ReadFileStream = File.OpenRead(name + ".bin");
                BinaryFormatter deserializer = new BinaryFormatter();
                // check if the file is empty or there is info in it- if its not empty deserialize the list of Id's from the file with this name
                if (new FileInfo(name + ".bin").Length != 0)
                    this.list = (List<ID>)deserializer.Deserialize(ReadFileStream);
                //if the file is empty- initialize new list
                else
                    this.list = new List<ID>();
                ReadFileStream.Close();
            }
        }

        //add a new Id to this list and afterwards to the file, return true if succsed
        public bool updateFile(ID id)
        {
            list.Add(id);
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
            else
                //if the update failed- dont change this list and return false
                list.Remove(id);
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

        public List<ID> List
        {
            get { return list; }
            set { list = value; }
        }
    }
}