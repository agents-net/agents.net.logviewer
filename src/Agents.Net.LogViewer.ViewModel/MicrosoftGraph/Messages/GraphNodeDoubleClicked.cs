using System.Collections.Generic;
using Agents.Net;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.LogViewer.ViewModel.MicrosoftGraph.Messages
{
    public class GraphNodeDoubleClicked : Message
    {
        public GraphNodeDoubleClicked(DrawingObject doubleClickedItem, Message predecessorMessage)
			: base(predecessorMessage)
        {
            DoubleClickedItem = doubleClickedItem;
        }

        public GraphNodeDoubleClicked(DrawingObject doubleClickedItem, IEnumerable<Message> predecessorMessages)
			: base(predecessorMessages)
        {
            DoubleClickedItem = doubleClickedItem;
        }
        
        public DrawingObject DoubleClickedItem { get; }

        protected override string DataToString()
        {
            return $"{nameof(DoubleClickedItem)}: {DoubleClickedItem}";
        }
    }
}
