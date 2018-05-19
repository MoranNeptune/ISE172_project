using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStoneClient.BusinessLayer
{
    class FilterByUser : Action
    {
        private string name;
        private string groupId;
        public FilterByUser(string name, string g_id)
        {
            this.name = name;
            this.groupId = g_id;
        }

        public override List<GuiMessage> action(List<GuiMessage> msgs)
        {
            //var sortedNames = msgs.OrderBy(n => n);
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
