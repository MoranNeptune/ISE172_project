using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.PresentationLayer;

namespace MileStoneClient.BusinessLayer
{
    class SortByTime : PresentationLayer.Action
    {
        private MessageComperator comperator;

        public SortByTime()
        {
            comperator = new MessageComperator();
        }
        public override List<GuiMessage> action(List<GuiMessage> msgs)
        {

            msgs.Sort(comperator);
            return msgs;
        }

        public void sortRange(int i, int count, List<GuiMessage> msgs)
        {
            msgs.Sort(i, count, comperator);
        }
        /// <summary>
        /// A class implementing interface IComparer<T> to sort between two messages by nickname
        /// </summary>
        class MessageComperator : IComparer<GuiMessage>
        {
            /// <summary>
            /// Override the Compare function to compare by DateTime 
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
                // in case both messages were sent at the same time
                if (msg1.DateTime.ToString().Equals(msg2.DateTime.ToString()))
                    return msg1.Id.ToString().CompareTo(msg2.Id.ToString());
                return msg1.DateTime.Ticks.CompareTo(msg2.DateTime.Ticks);
            }
        }
    }
}
