using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MileStoneClient.BusinessLayer;

namespace MileStoneClient.PresentationLayer
{
    public abstract class Action
    {
        abstract public List<GuiMessage> action(List<GuiMessage> msgs);
    }
}
