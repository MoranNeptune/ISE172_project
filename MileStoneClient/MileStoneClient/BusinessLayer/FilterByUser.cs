using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.PresentationLayer;

namespace MileStoneClient.BusinessLayer
{
    class FilterByUser : PresentationLayer.Action
    {
        private string name;
        private string groupId;
        public FilterByUser(string name, string g_id)
        {
            this.name = name;
            this.groupId = g_id;
        }

        /// <summary>
        /// The function returns a list of messages of a specific User
        /// </summary>
        /// <param name="msgs">List to filter</param>
        /// <returns></returns>
        public override List<GuiMessage> action(List<GuiMessage> msgs)
        {
            for (int i = 0; i < msgs.Count; i++)
            {
                if (!msgs[i].G_id.Equals(groupId) || !msgs[i].UserName.Equals(name))
                {
                    msgs.RemoveAt(i);
                    i--;
                }
            }
            return msgs;
        }
    }
}
