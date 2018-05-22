using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStoneClient.BusinessLayer
{

    [Serializable]
    public class ID
    {
        private string id;
        private List<string> members;

        //constructor
        public ID(string id)
        {
            this.id = id;
            members = new List<string>();
        }

        //getters and setters
        public string idNumber
        {
            get { return id; }
            set { id = value; }
        }

        public List<string> Members
        {
            get { return members; }
            set { members = value; }
        }

        /// <summary>
        /// Check if group contains member already - if not add member to the member list
        /// </summary>
        /// <param name="nickname"> A parameter of type string representing the users' nickname </param>
        /// <returns> Returns true if the member was added, false if the nickname already exist in the group </returns>
        public bool addMember(string nickname)
        {
            bool added = true;
            if (!members.Contains(nickname))
                members.Add(nickname);
            else
                added = false;
            return added;
        }

        /// <summary>
        /// Check if group contains user by nickname
        /// </summary>
        /// <param name="nickname"> A parameter of type string representing the users' nickname </param>
        /// <returns> Returns true if the group contains the user nickname, else returns false </returns>
        private bool contains(string nickname)
        {
            if (members == null)
                return false;

            return members.Contains(nickname);
        }

        /// <summary>
        /// Check if group ID is equal to another group ID 
        /// </summary>
        /// <param name="g_id"> A parameter of type string representing the users' group id </param>
        /// <returns> Returns true if this is equal to other, else return false </returns>
        public bool isEqual(string g_id)
        {
            return (this.idNumber.Equals(g_id));
        }

        public string toString()
        {
            string str = "";

            foreach (string s in members)
            {
                str = str + s + " ";
            }

            return "Group ID: " + id + ", Members: [ " + str + "]";
        }
    }
}