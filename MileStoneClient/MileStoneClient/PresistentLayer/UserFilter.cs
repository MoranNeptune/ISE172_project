using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.BusinessLayer;

namespace MileStoneClient.PresistentLayer
{
    class UserFilter : IQueryAction
    {
        private string query;
        private List<Message> msgs;

        public UserFilter(string nickname, string g_id)
        {
            // Query to filter by user nickname
            query = "SELECT [Group_Id],[Nickname],[SendTime],[Body] " +
                    "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                    "WHERE [MS3].[dbo].[Users].Nickname = '" + nickname +
                    "' AND [MS3].[dbo].[Users].[Group_Id] = " + int.Parse(g_id) +
                    " AND [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id";
        }

        public override void ExecuteQuery(string query)
        {
            List<Message> tMsgs = new List<Message>();

            //need to execute still
        }

        public List<Message> Msgs
        {
            get { return msgs; }
        }
    }
}
