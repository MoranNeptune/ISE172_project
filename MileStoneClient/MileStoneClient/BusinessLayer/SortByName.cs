﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.PresentationLayer;

namespace MileStoneClient.BusinessLayer
{
     class SortByName : PresentationLayer.Action
    {
        private MessageComperator comperator;
        
        public SortByName()
        {
            comperator = new MessageComperator();
        }
        public override List<GuiMessage> action(List<GuiMessage> msgs)
        {
            
            msgs.Sort(comperator);
            return msgs;
        }

        public void sortRange(int i, int range , List<GuiMessage> msgs)
        {
             msgs.Sort(i, range, comperator);
            // sort the list by Time
            int count = 1;
            int size = i + range;
            if (msgs.Count > 0)
            {
                // find the range of the current User in the list, so we can sort that range by time
                string tempGroup = msgs[i].UserName;
                SortByTime sbn = new SortByTime();
                while (i+count <= size && i + count < msgs.Count)
                {
                    if (tempGroup.Equals(msgs[i + count].UserName))
                        count++;
                    else
                    {
                        // send to SortByName to sort the wanted range
                        if (i + count < msgs.Count && count > 1)
                            sbn.sortRange(i, count, msgs);
                        tempGroup = msgs[i + count].UserName;
                        i = i + count;
                        count = 1;
                    }
                }
                // אולי להוריד את המינוס 1 או להוסיף בדיקת חריגה
               // sbn.sortRange(i, count, msgs);
            }
        }

        /// <summary>
        /// A class implementing interface IComparer<T> to sort between two messages by the users nickname
        /// </summary>
        class MessageComperator : IComparer<GuiMessage>
        {
            /// <summary>
            /// Override the Compare function to compare by User's name
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