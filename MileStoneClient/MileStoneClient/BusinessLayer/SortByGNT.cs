using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.PresentationLayer;

namespace MileStoneClient.BusinessLayer
{
    class SortByGNT : PresentationLayer.Action
    {
        public SortByGNT()
        {

        }

        /// <summary>
        /// The function returns a list sorted by Group id, Users nickname and Timestemp
        /// </summary>
        /// <param name="msgs">List to sort</param>
        /// <returns></returns>
        public override List<GuiMessage> action(List<GuiMessage> msgs)
        {
            int i =0, count;
            MessageComperator comperator = new MessageComperator();
            //  sort the list by group id
            msgs.Sort(comperator);

            // sort the list by name
            if(msgs.Count > 0)
            {
                count = 1;
                // find the range of the current group in the list, so we can sort that range by name
                string tempGroup = msgs[i].G_id;
                SortByName sbn = new SortByName();
                while (i+count < msgs.Count)
                {
                    if (tempGroup.Equals(msgs[i + count].G_id))
                        count++;
                    else
                    {
                        // send to SortByName to sort the wanted range
                        if (i + count< msgs.Count)
                            sbn.sortRange(i, count, msgs);
                        tempGroup = msgs[i+count].G_id;
                        i = i + count;
                        count = 1;
                    }
                }
                sbn.sortRange(i, count, msgs);
            }     
            return msgs;
        }

        /// <summary>
        /// A class implementing interface IComparer<T> to sort between two messages by group id
        /// </summary>
        class MessageComperator : IComparer<GuiMessage>
        {
            /// <summary>
            /// Override the Compare function to compare by group id
            /// </summary>
            /// <param name="msg1"> A parameter of type Message representing message to compare </param>
            /// <param name="msg2"> A parameter of type Message representing message to compare </param>
            /// <returns> Returns a parameter of type int:
            ///                      -1 if this is smaller than other
            ///                      0 if they are equal
            ///                      1 if this is greater than other
            /// </returns>
            public int Compare(GuiMessage msg1, GuiMessage msg2)
            {
                int x1, x2;
                if (int.TryParse(msg1.G_id, out x1) == false)
                    x1 = 0;
                else
                    x1 = int.Parse(msg1.G_id);
                if (int.TryParse(msg2.G_id, out x2) == false)
                    x2 = 0;
                else 
                    x2 = int.Parse(msg2.G_id);
                return x1-x2;
            }
        }
    }
}
