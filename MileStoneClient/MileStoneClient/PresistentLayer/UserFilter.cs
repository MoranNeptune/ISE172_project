using System;
using System.Collections.Generic;
using MileStoneClient.BusinessLayer;

namespace MileStoneClient.PresistentLayer
{
    class UserFilter : IQueryAction
    {
        private bool connectionFail;
        private List<Message> msgs;

        public UserFilter(string nickname, string g_id)
        {
            connectionFail = false;
            //query to filter by user nickname
            string query = "SELECT [Group_Id],[Nickname],[SendTime],[Body] " +
                    "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                    "WHERE [MS3].[dbo].[Users].Nickname = '" + nickname +
                    "' AND [MS3].[dbo].[Users].[Group_Id] = '" + g_id +
                    "' AND [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id";
            try
            {
                ExecuteQuery(query);
            }catch(Exception e)
            {
                connectionFail = true;
            }
        }

        public override void ExecuteQuery(string query)
        {
            msgs = Instance.ReadMessageTable(query);
        }

        public List<Message> Msgs
        {
            get { return msgs; }
        }

        public bool ConnectionFail
        {
            get { return connectionFail; }
            set { connectionFail = value; }
        }
    }
}
