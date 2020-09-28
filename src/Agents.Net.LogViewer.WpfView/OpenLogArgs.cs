using System;

namespace Agents.Net.LogViewer.WpfView
{
    public class OpenLogArgs : EventArgs
    {
        public OpenLogArgs(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }
    }
}
