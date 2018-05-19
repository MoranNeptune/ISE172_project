using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStoneClient.BusinessLayer
{
    class FilterByGroup : Action
    {
        private string groupId;

        public FilterByGroup(string g_id)
        {
            groupId = g_id;
        }
        public override List<GuiMessage> action(List<GuiMessage> msgs)
        {
            for( int i=0; i<msgs.Count; i++)
            {
                if (!msgs[i].G_id.Equals(groupId))
                {
                    msgs.RemoveAt(i);
                    i--;
                }
            }
            return msgs;
        }
    }
}
