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
        [TestCase]
        public void RegisterValid()
        {
            ChatRoom c = new ChatRoom("http://ise172.ise.bgu.ac.il:80");
            string name = "name1";
            string group = "1";

            Assert.AreEqual(true, c.register(name, group));
        }

    }
}
