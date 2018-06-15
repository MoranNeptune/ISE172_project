using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStoneClient.PresentationLayer
{
    public class GuiMessage 
    {
        private string body;
        private string userName;
        private string g_id;
        private DateTime dateTime;
        private string id;

        //Constructor
        public GuiMessage(string body, DateTime time, Guid id, string userName, string g_id)
        {
            this.body = body;
            this.dateTime = time;
            this.id = id.ToString();
            this.userName = userName;
            this.g_id = g_id;
        }

        //getters and setters
        public String Body
        {
            get { return body; }
            set { body = value; }
        }


        public string G_id
        {
            get { return g_id; }
            set { g_id = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        public string time()
        {
            return dateTime.ToString();
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string toString()
        {
           // DateTime updateTime = dateTime.AddHours(3);
            return "Group ID: " + this.g_id + ", Nickname: " + this.userName + ", (" + dateTime.ToString() + "), Message Body: " + this.body;
        }
    }
}
