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
            // הוספתי להכל את הערך של הסיסמה, צריך לבדוק האם זה תקין
            //bool observedResult = c.register("name_", "1"); הקודם 
            Random rnd = new Random();
            int num = rnd.Next(1, 100);
            bool observedResult = c.register("name"+num, "81", "1234");// העדכון
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
            c.register("name"+num,"1","1234"); 
            bool observedResult = c.register("name"+num, "1", "1234");
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
            c.register("someName"+num, ""+num, "1234");
            bool observedResult = c.login("someName" + num, ""+num, "1234");
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
        /// test the function getMembersOf, and the ID's functionality
        /// </summary>
        [Test]
        public void GetMembersOf()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("MickyMouse", "21", "1234");
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
            c.register("MickyMouse", "21", "1234");
            bool observedResult = c.findUser("MickyMouse", "21") !=null;
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
            List<string> filterInfo = new List<string>();
            filterInfo.Add("ByUser");
            filterInfo.Add("21");
            filterInfo.Add("Batman"); //the user
            List<GuiMessage> msg1 = c.getMessages(0, action, filterInfo);
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
            List<string> filterInfo = new List<string>();
            filterInfo.Add("ByGroup");
            filterInfo.Add("20"); //another g_id
            List<GuiMessage> msg1 = c.getMessages(0, action, filterInfo);
            c.send("Hi");
            List<GuiMessage> msg2 = c.getMessages(0, action, filterInfo);
            bool observedResult = msg1.Equals(msg2);
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }
        

        /// <summary>
        /// test the function getMembersOf, and the ID's functionality
        /// </summary>
     /*   [Test]
        public void UpdateMessage()
        {
            ChatRoom c = new ChatRoom(url);
            c.register("MickyMouse", "21", "1234");
            c.send("check");
            bool observedResult = c.updateMessage("checked");
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, observedResult);
        }*/
    }
}
