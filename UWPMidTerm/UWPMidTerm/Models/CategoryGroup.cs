using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPMidTerm.Models
{
    public class CategoryGroup
    {
        public string CategoryGroupName { get; set; }
        public List<Category> items { get; set; }
    }
}
