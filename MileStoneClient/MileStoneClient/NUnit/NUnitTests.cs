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
        /// test filterByUser  
        /// </summary>
        [Test]
        public void filterByUser()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("Batman", "21");
            c.login("Batman", "21");
            List<PresentationLayer.Action> action = new List<PresentationLayer.Action>();
            FilterByUser filter = new FilterByUser("Batman", "21");//the user
            SortByTime sort = new SortByTime();
            action.Add(sort);
            action.Add(filter);
            List<GuiMessage> msg1 = new List<GuiMessage>();
            List<GuiMessage> tmp = c.getMessages(0, action);
            for (int i = 0; i < tmp.Count; i++)
                msg1.Add(tmp[i]);
            c.send("Hello!");
            List<GuiMessage> msg2 = c.getMessages(0, action);
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
             c.register("Batman", "21");
             c.login("Batman", "21");
            List<PresentationLayer.Action> action = new List<PresentationLayer.Action>();
            FilterByGroup filter = new FilterByGroup("20");//another g_id
            SortByTime sort = new SortByTime();
            action.Add(sort);
            action.Add(filter);
            List<GuiMessage> msg1 = c.getMessages(0, action);
            c.send("Hi");
            List<GuiMessage> msg2 = c.getMessages(0, action);
            bool observedResult = msg1.Equals(msg2);
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }
    }
}
