using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HzpSolution
{
    public interface IMenuSetting
    {
        [DefaultValue(nameof(ParentMenuName))]
        public string ParentMenuName { get; set; }

        [DefaultValue(nameof(MenuName))]
        public string MenuName { get; set; }

        [DefaultValue("FolderOutline")]
        public string Icon { get; set; }

        [DefaultValue(true)]
        public bool IsShow { get; set; }

        public string? ViewName { get; set; }
    }

    public interface IMenuSettings
    {
        public IEnumerable<IMenuSetting> AllMenuSetting { get; }
    }

}
