using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HzpSolution
{
    public class MenuTreeNode
    {
        public string  ParentMenuName { get; set; } = nameof(ParentMenuName);

        public string MenuName { get; set; } = nameof(MenuName);

        public string Icon { get; set; } = "FolderOutline";

        public bool IsShow { get; set; } = true;

        public string? ViewName { get; set; }

        public Visibility Visible { get; set; }

        public List<MenuTreeNode>? ChildMenuNodes { get; set; }

    }
}
