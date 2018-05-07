using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStoneClient.BusinessLayer
{

    [Serializable]
    class ID
    {
        private String id;
        private List<String> members;

        //constructor
        public ID(String id)
        {
            this.id = id;
            members = new List<String>();
        }

        //getters and setters
        public String idNumber
        {
            get { return id; }
            set { id = value; }
        }

        public List<String> Members
        {
            get { return members; }
            set { members = value; }
        }

        //methods
        //check if group contains member already - if not add member to the member list
        public bool addMember(String nickname)
        {
            bool added = true;
            if (!members.Contains(nickname))
                members.Add(nickname);
            else
                added = false;

            return added;
        }

        //check if group contains user by nickname
        private bool contains(String nickname)
        {
            if (members == null)
                return false;

            return members.Contains(nickname);
        }

        //check if group ID is equal to another group ID 
        public bool isEqual(string g_id)
        {
            return (this.idNumber.Equals(g_id));
        }

        public String toString()
        {
            String str = "";

            foreach (String s in members)
            {
                str = str + s + " ";
            }

            return "Group ID: " + id + ", Members: [ " + str + "]";
        }

        /// <summary>
        /// Compares ID to another ID
        /// </summary>
        /// <param name="other"> A parameter of type ID representing an ID to compare to</param>
        /// <returns> Returns a parameter of type int:
        ///                      -1 if this is smaller than other
        ///                      0 if they are equal
        ///                      1 if this is greater than other
        /// </returns>
        public int CompareTo(ID other)
        {
            return int.Parse(id) - int.Parse(other.idNumber);
        }
    }
}