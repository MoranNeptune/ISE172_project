using System;
using System.Collections.Generic;
using MileStoneClient.BusinessLayer;


namespace MileStoneClient.PresistentLayer
{
    class GroupFilter : IQueryAction
    {
        private bool connectionFail;
        private List<Message> msgs;

        public GroupFilter(string g_id)
        {
            connectionFail = false;
            //query to filter by group id
            string query = "SELECT [Group_Id],[Nickname],[SendTime],[Body] " +
                    "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                    "WHERE [MS3].[dbo].[Users].[Group_Id] = " + int.Parse(g_id) +
                    " AND [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id";
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
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
