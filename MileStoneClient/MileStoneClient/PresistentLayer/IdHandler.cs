using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MileStoneClient.BusinessLayer;
using System.Collections.Generic;

namespace MileStoneClient.PresistentLayer
{
    /// <summary>
    /// an object that responsable to transfer the ID's info to files
    /// </summary>
    class IdHandler
    {
        private List<ID> list = null;
        private String name;

        /// <summary>
        /// a constructor for this class that get the file name
        /// </summary>
        /// <param name="name">file name</param>
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

        /// <summary>
        /// add a new Id to this list and afterwards to the file
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if succsed, else false</returns>
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

        /// <summary>
        /// delete the file with this name
        /// </summary>
        /// <returns>true if succsed, else false</returns>
        private bool deleteFile()
        {
            //assume there is a file with this name
            File.Delete(name + ".bin");
            return !(File.Exists(name + ".bin"));

        }

        /// <summary>
        /// open a new file with this name
        /// </summary>
        /// <returns>true if succsed, else false</returns>
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
        public List<ID> List
        {
            get { return list; }
            set { list = value; }
        }
    }
}