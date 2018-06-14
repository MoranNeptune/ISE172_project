using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStoneClient.PresistentLayer
{
    class HandlerFactory
    {
        SqlConnection connection;

        public HandlerFactory()
        {
            string server_address = "ise172.ise.bgu.ac.il,1433\\DB_LAB";
            string database_name = "MS3";
            string user_name = "publicUser";
            string password = "isANerd";
            string connetion_string = $"Data Source={server_address};Initial Catalog={database_name };User ID={user_name};Password={password}";
            connection = new SqlConnection(connetion_string);
        }

        public UserHandler createUserHandler()
        {
            UserHandler userHandler = new UserHandler();
            userHandler.setCon(connection);
            return userHandler;
        }

        public MessageHandler createMessageHandler()
        {
            // creates a messages handler with defult filter NONE = "" 
            MessageHandler messageHandler = new MessageHandler("","");
            messageHandler.setCon(connection);
            return messageHandler;
        }
    }
}
