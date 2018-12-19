using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPMidTerm.Models
{
    public class MenuItem
    {
        public string icon { get; set; }
        public string content { get; set; }
        public Type purpose { get; set; }

        public MenuItem(string icon, string content, Type purpose)
        {
            this.icon = icon;
            this.content = content;
            this.purpose = purpose;
        }
    }
}
