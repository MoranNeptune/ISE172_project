using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStoneClient.BusinessLayer
{
    [Serializable]
    class Message : IEquatable<Message>, IComparable<Message>
    {
        private String body;
        private User user;
        private DateTime dateTime;
        private Guid id;

        //constructor
        public Message(String body, DateTime time, Guid id, User user)
        {
            this.body = body;
            this.dateTime = time;
            this.id = id;
            this.user = user;
        }

        //getters and setters
        public String Body
        {
            get { return body; }
            set { body = value; }
        }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        //methods
        //check if messege is equal to another messege by Guid
        bool IEquatable<Message>.Equals(Message other)
        {
            if (other == null) return false;
            return id.Equals(other.Id);
        }

        //Compare between two messages by dateTime
        /* return: 
         *        -1 if this > other
         *        0 if equals
         *        1 if this < other    
         */
        int IComparable<Message>.CompareTo(Message other)
        {
            //if (other == null) return null;
            return dateTime.CompareTo(other.dateTime);
        }

        public override string ToString() 
        {
            return "Group ID: " + this.user.G_id.idNumber + ", Nickname: " + this.user.Nickname + ", (" + this.dateTime.ToString() + "), Message Body: " + this.body +'\n' + "GUID: " + this.id;
        }

        /// <summary>
        /// Compares message to another messege by group id
        /// </summary>
        /// <param name="other"> A parameter of type Object representing message to compare to</param>
        /// <returns> Returns a parameter of type int:
        ///                      -1 if this is smaller than other
        ///                      0 if they are equal
        ///                      1 if this is greater than other
        /// </returns>
        public int CompareById(Message other)
        {
            return user.G_id.CompareTo(other.User.G_id);
        }

        /// <summary>
        /// Compares message to another messege by user nickname 
        /// </summary>
        /// <param name="other"> A parameter of type Object representing message to compare to</param>
        /// <returns> Returns a parameter of type int:
        ///                      -1 if the name of this is smaller than other
        ///                      0 if they are equal
        ///                      1 if this is greater than other
        /// </returns>
        public int CompareByName(Message other)
        {
            return user.Nickname.CompareTo(other.User.Nickname);
        }
    }
}