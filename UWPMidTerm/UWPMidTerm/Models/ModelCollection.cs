using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPMidTerm.Models
{
    public class ModelCollection<T>
    {
        public string name { get; set; }
        public ObservableCollection<T> collection { get; set; }
    }
}
