using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MileStoneClient.BusinessLayer
{
    class SortByName : Action
    {
       /* private int order;
        public SortByName(int order)
        {
            this.order = order;
        }*/
        public override List<GuiMessage> action(List<GuiMessage> msgs)
        {
            MessageComperator comperator = new MessageComperator();

            msgs.Sort(comperator);
            return msgs;
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
                return msg1.UserName.CompareTo(msg2.UserName) ;
            }
        }
    }
}
