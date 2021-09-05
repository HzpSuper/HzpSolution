using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzpSolution
{
    public class MenuTreeNode
    {
        public string? ParentMenuName { get; set; }

        public string? MenuName { get; set; }

        public string? Icon { get; set; } = "FolderOutline";

        public bool IsShow { get; set; } = true;

        public string? ViewName { get; set; }

        public List<MenuTreeNode>? ChildMenuNodes { get; set; }

    }
}
