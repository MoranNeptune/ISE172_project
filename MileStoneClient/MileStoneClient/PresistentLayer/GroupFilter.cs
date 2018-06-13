using System;
using System.Collections.Generic;
using MileStoneClient.BusinessLayer;


namespace MileStoneClient.PresistentLayer
{
    class GroupFilter : IQueryAction
    {
        private bool connectionFail;
        private List<Message> msgs;
        // private string g_id;
        private List<SqlParam> param;

        public GroupFilter(string g_id)
        {
            connectionFail = false;
            //query to filter by group id
            string query = "SELECT TOP (200) [Group_Id],[Nickname],[SendTime],[Body] " +
                    "FROM [MS3].[dbo].[Users],[MS3].[dbo].[Messages] " +
                    "WHERE [MS3].[dbo].[Users].[Group_Id] = @user_G_id" +
                    " AND [MS3].[dbo].[Messages].User_Id = [MS3].[dbo].[Users].Id";
            SqlParam user_G_id = new SqlParam("@user_G_id", g_id);
            param.Add(user_G_id);
           // this.g_id = g_id;
            try
            {
                ExecuteQuery(query);
            }
            catch (Exception e)
            {
                connectionFail = true;
            }
            param.Clear();
        }

        public override void ExecuteQuery(string query)
        {
            msgs = Instance.FilterQuery(query,param);
           // g_id = "";
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
