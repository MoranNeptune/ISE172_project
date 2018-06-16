using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MileStoneClient.BusinessLayer;
using MileStoneClient.PresentationLayer;

namespace MileStoneClient.NUnit
{
    [TestFixture]
    class NUnitTests
    {

        private const String url = "http://ise172.ise.bgu.ac.il:80";

        /// <summary>
        /// test the registeretion of a new user
        /// </summary>
        [Test]
        public void RegisterValid()
        {
            ChatRoom c = new ChatRoom(url);
            Random rnd = new Random();
            int num = rnd.Next(1, 100);
            bool observedResult = c.register("name" + num, "81", "1234");
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test the registeretion of a new user with the same name & group id as a registered user
        /// </summary>
        [Test]
        public void RegisterInvalid()
        {
            ChatRoom c = new ChatRoom(url);
            Random rnd = new Random();
            int num = rnd.Next(1, 100);
            c.register("name" + num, "1", "1234");
            bool observedResult = c.register("name" + num, "1", "1234");
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
            Random rnd = new Random();
            int num = rnd.Next(1, 100);
            c.register("name"+num, ""+num, "F0FBDF664ABBF1CA7292E68BE9E38C147CFA5310CC952C35EC8748E9F6C95C01"); 
            bool observedResult = c.login("name" + num, ""+num, "F0FBDF664ABBF1CA7292E68BE9E38C147CFA5310CC952C35EC8748E9F6C95C01");
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test send message 
        /// </summary>
        [Test]
        public void Send()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("Batman", "21", "1234");
            c.login("Batman", "21", "1234");
            bool observedResult = c.send("Hi");
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test the function getMembersOf
        /// </summary>
        [Test]
        public void GetMembersOf()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("Micky", "21", "1234");
            bool observedResult = c.getMembersOf("21").Contains("Micky");
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
            bool v = c.register("Micky", "21", "D4A273A742947B244DA433C4A8D39BA28A8EC0C6F3445AFC891312A6046E2612");
            bool observedResult = c.findUser("Micky", "21") != null;
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test filterByUser  
        /// </summary>
        [Test]
        public void filterByUser()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("Batman", "21", "1234");
            c.login("Batman", "21", "1234");
            PresentationLayer.Action action = new SortByTime();
            string[] filterInfo = new string[3];
            filterInfo[0] = "ByUser";
            filterInfo[1] = "21";
            filterInfo[2] = "Batman"; //the user
            List<GuiMessage> tempList = c.getMessages(0, action, filterInfo);
            List<GuiMessage> msg1 = new List<GuiMessage>();
            for (int i = 0; i < tempList.Count; i++)
                msg1.Add(tempList[i]);
            c.send("Hello!");
            List<GuiMessage> msg2 = c.getMessages(0, action, filterInfo);
            bool observedResult = msg1.Equals(msg2);
            bool expectedResult = false;
            Assert.AreEqual(expectedResult, observedResult);
        }

        /// <summary>
        /// test filterByGroupID  
        /// </summary>
        [Test]
        public void filterByGroupID()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("Batman", "21", "1234");
            c.login("Batman", "21", "1234");
            PresentationLayer.Action action = new SortByTime();
            string[] filterInfo = new string[3];
            filterInfo[0] = "ByGroup";
            filterInfo[1] = "20"; //another g_id
            filterInfo[2] = "";
            List<GuiMessage> msg1 = c.getMessages(0, action, filterInfo);
            c.send("Hi");
            List<GuiMessage> msg2 = c.getMessages(0, action, filterInfo);
            bool observedResult = msg1.Equals(msg2);
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }

    }
}