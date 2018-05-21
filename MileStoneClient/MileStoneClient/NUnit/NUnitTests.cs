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
        private const String url = "http://ise172.ise.bgu.ac.il:80";

        /// <summary>
        /// test the registeretion of a new user
        /// </summary>
        [TestCase]
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
        [TestCase]
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
        [TestCase]
        public void Login()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("name", "1");
            bool observedResult = c.login("name", "1");
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test login of a not regisstered user
        /// </summary>
        /*[TestCase]
        public void LoginValid()
        {
            ChatRoom c = new ChatRoom(url);
             g_id = 1;
            while(c.findUser())
            bool observedResult = c.login("name", "1");
            bool expectedResult = false;
            Assert.AreEqual(expectedResult, observedResult);
        }
        */
      
        //one case that work one that doesnt
        //test exit program
        //test send- is succsed, max. Length 150 characters, empty message
        //test filter- send a message from the signin user and try filter on another group, on another user
        //test getMembersOf(chatroom)- init new group and check if the list is the same
        //to save the tests in file? to add a message?

    }
}
