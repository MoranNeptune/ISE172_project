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

        public ConnectionHandler()
        {
            string server_address = "ise172.ise.bgu.ac.il,1433\\DB_LAB";
            string database_name = "MS3";
            string user_name = "publicUser";
            string password = "isANerd";
            string connetion_string = $"Data Source={server_address};Initial Catalog={database_name };User ID={user_name};Password={password}";
            connection = new SqlConnection(connetion_string);
        }

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

        protected List<Object> Select() { return null; }
        protected List<Object> Insert() { return null; }
        protected List<Object> Update() { return null; }
    }
}
