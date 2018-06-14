using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStoneClient.PresistentLayer
{
    public abstract class ConnectionHandler
    {
        protected SqlConnection connection;

        public void connect()
        {
            connection.Open();
        }

        public void disconnect()
        {
            connection.Close();
        }

        public void setCon(SqlConnection con)
        {
            connection = con;
        }
    }
}
