using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MileStoneClient.BusinessLayer;

namespace MileStoneClient.NUnit
{
    [TestFixture]
    class NUnitTests
    {
        ////////// save the tests in file? to add a message?

        private const String url = "http://ise172.ise.bgu.ac.il:80";

        /// <summary>
        /// test the registeretion of a new user
        /// </summary>
        [Test]
        public void Register()
        {
            ChatRoom c = new ChatRoom(url);
            bool observedResult = c.register("name_", "1");
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test the registeretion of a new user with the same name & group id as a registered user
        /// </summary>
        [Test]
        public void RegisterValid()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("name", "1");
            bool observedResult = c.register("name", "1");
            bool expectedResult = false;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test login of a registered user
        /// </summary>
        [Test]
        public void Login()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("name", "1");
            bool observedResult = c.login("name", "1");
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test if the program sends a message with more than 150 chars
        /// </summary>
        [Test]
        public void MessageLength()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("Batman", "21");
            c.login("Batman", "21");
            bool observedResult = c.send
                ("Hiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");
            bool expectedResult = false;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test send message 
        /// </summary>
        [Test]
        public void Send()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("Batman", "21");
            c.login("Batman", "21");
             bool observedResult = c.send("Hi");
            bool expectedResult = true;
             Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test the function getMembersOf, and the ID's functionality
        /// </summary>
        [Test]
        public void GetMembersOf()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("MickyMouse", "21");
            bool observedResult = c.getMembersOf("21").Contains("MickyMouse");
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test the function findUser, and the User's functionality
        /// </summary>
        [Test]
        public void FindUser()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("MickyMouse", "21");
            bool observedResult = c.findUser("MickyMouse", "21")!=null;
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test if the program sends an empty message 
        /// </summary>
        /*   [Test]
           public void EmptyMessage()
           {
               ChatRoom c = new ChatRoom(url);
                c.register("name", "1");
                bool observedResult = c.login("name", "1");
                bool expectedResult = true;
                Assert.AreEqual(expectedResult, observedResult);
           }*/

        /// <summary>
        /// test log out option 
        /// </summary>
        /*   [Test]
            public void Logout()
            {
                ChatRoom c = new ChatRoom(url);
                 c.register("Spiderman", "21");
             c.login("Spiderman", "21");
             c.logOut("Spiderman", "21");
             bool observedResult = 
                 bool expectedResult = true;
                 Assert.AreEqual(expectedResult, observedResult);
            }*/


        /// <summary>
        /// test login of a not registered user
        /// </summary>
        /*[Test]
        public void LoginValid()
        {
            ChatRoom c = new ChatRoom(url);
            
            bool observedResult = c.login("name", "1");
            bool expectedResult = false;
            Assert.AreEqual(expectedResult, observedResult);
        }
        */


        //anoter test filter- send a message from the signin user and see if it shows when chose filterByUser (this user)
        /// <summary>
        /// test filterByGroupID  
        /// </summary>
        /* [Test]
         public void filterByGroupID()
         {
             ChatRoom c = new ChatRoom(url);
             c.register("Batman", "21");
             c.login("Batman", "21");
             c.send("Hi");
             bool observedResult = //(list of messages with filter of another g_id).contains(the message above)
             bool expectedResult = false;
             Assert.AreEqual(expectedResult, observedResult);
         }*/
    }
}
