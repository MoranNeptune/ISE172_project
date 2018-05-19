using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MileStoneClient.BusinessLayer
{
    abstract class Action
    {
        abstract public List<GuiMessage> action(List<GuiMessage> msgs);
    }
}
