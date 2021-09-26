using Config.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzpSolution
{
    public class MenuManage
    {
        private static List<MenuTreeNode> GetSetting(string path)
        {
            IMenuSettings imenusettings = new ConfigurationBuilder<IMenuSettings>().UseJsonFile(path).Build();
            List<MenuTreeNode> nodes = new();
            foreach(IMenuSetting menuSetting in imenusettings.AllMenuSetting)
            {
                nodes.Add(
                new()
                {
                    ParentMenuName = menuSetting.ParentMenuName,
                    MenuName = menuSetting.MenuName,
                    Icon = menuSetting.Icon,
                    IsShow = menuSetting.IsShow,
                    ViewName = menuSetting.ViewName
                });
            }
            return nodes;
        }
    
        public List<MenuTreeNode>? IniMenuNodes(string path)
        {
            List<MenuTreeNode> nodes = GetSetting(path);
            List<MenuTreeNode>? mainNodes  = nodes.Where(x => x.ParentMenuName == nameof(x.ParentMenuName) && x.IsShow).ToList();
            List<MenuTreeNode>? otherNodes = nodes.Where(x => x.ParentMenuName != nameof(x.ParentMenuName) && x.IsShow).ToList();
            foreach (MenuTreeNode node in mainNodes)
            {
                node.ChildMenuNodes = GetNodes(node.MenuName, otherNodes);
            }
            return mainNodes;
        }


        private List<MenuTreeNode>? GetNodes(string menuName, List<MenuTreeNode>? nodes)
        {
            List<MenuTreeNode>? mainNodes  = nodes?.Where(x => x.ParentMenuName == menuName && x.IsShow).ToList();
            List<MenuTreeNode>? otherNodes = nodes?.Where(x => x.ParentMenuName != menuName && x.IsShow).ToList();
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
