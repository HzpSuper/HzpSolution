using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzpSolution
{
    public class MenuManage
    {

        public List<MenuTreeNode>? IniMenuNodes(List<MenuTreeNode> nodes)
        {
            List<MenuTreeNode>? mainNodes  = nodes.Where(x =>  string.IsNullOrEmpty(x.ParentMenuName) && x.IsShow == true).ToList();
            List<MenuTreeNode>? otherNodes = nodes.Where(x => !string.IsNullOrEmpty(x.ParentMenuName) && x.IsShow == true).ToList();
            foreach (MenuTreeNode node in mainNodes)
            {
                node.ChildMenuNodes = GetNodes(node.MenuName, otherNodes);
            }
            return mainNodes;
        }


        private List<MenuTreeNode>? GetNodes(string? menuName, List<MenuTreeNode>? nodes)
        {
            List<MenuTreeNode>? mainNodes  = nodes?.Where(x => x.ParentMenuName == menuName && x.IsShow == true).ToList();
            List<MenuTreeNode>? otherNodes = nodes?.Where(x => x.ParentMenuName != menuName && x.IsShow == true).ToList();
            if(mainNodes != null)
            {
                foreach (MenuTreeNode node in mainNodes)
                {
                    node.ChildMenuNodes = GetNodes(node.MenuName, otherNodes);
                }
            }  
            return mainNodes;
        }

    }
}
