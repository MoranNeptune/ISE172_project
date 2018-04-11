using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStoneClient.BusinessLayer
{
    [Serializable]
    class Message : IEquatable<Message>
    {
        private string body;
        private User user;
        private DateTime dateTime;
        private Guid id;

        //Constructor
        public Message(string body, DateTime time, Guid id, User user)
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

        //Methods
        /// <summary>
        /// Check if messege is equal to another messege by Guid
        /// </summary>
        /// <param name="other"> A parameter of type Message representing the other object to compare to </param>
        /// <returns> Returns true if both messages are equal</returns>
        bool IEquatable<Message>.Equals(Message other)
        {
            if (other == null) return false;
            return id.Equals(other.Id);
        }

        /// <summary>
        /// Compares message to another messege by DateTime
        /// </summary>
        /// <param name="other"> A parameter of type Object representing message to compare to</param>
        /// <returns> Returns a parameter of type int:
        ///                      -1 if this is smaller than other
        ///                      0 if they are equal
        ///                      1 if this is greater than other
        /// </returns>
        public int CompareTo(Object other)
        {
            return dateTime.CompareTo(((Message)other).DateTime);
        }

        public string ToString()
        {
            return "Group ID: " + this.user.G_id.idNumber + ", Nickname: " + this.user.Nickname + ", (" + this.dateTime.ToString() + "), Message Body: " + this.body +'\n' + "GUID: " + this.id;
        }
    }

    /// <summary>
    /// A class implementing interface IComparer<T> to between two messages by dateTime
    /// </summary>
    class MessageComperator : IComparer<Message>
    {
        /// <summary>
        /// Override the Compare function to compare by DateTime 
        /// </summary>
        /// <param name="msg1"> A parameter of type Message representing message to compare </param>
        /// <param name="msg2"> A parameter of type Message representing message to compare </param>
        /// <returns> Returns a parameter of type int:
        ///                      -1 if this is smaller than other
        ///                      0 if they are equal
        ///                      1 if this is greater than other
        /// </returns>
        public int Compare(Message msg1, Message msg2)
        {
            return msg1.CompareTo(msg2);
        }
    }
}