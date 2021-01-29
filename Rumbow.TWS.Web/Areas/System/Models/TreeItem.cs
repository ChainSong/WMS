using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class TreeItem
    {
        public int id { get; set; }

        public int pId { get; set; }

        public string name { get; set; }

        public bool open { get; set; }

        public bool isChecked { get; set; }

        public bool isParent { get; set; }

        public TreeItem()
        {
        }

        public TreeItem(CheckedMenu menu)
        {
            this.id = menu.ID;
            this.pId = menu.SuperID;
            this.name = menu.Name;
            this.open = menu.IsOpen;
            this.isChecked = menu.IsChecked;
        }
    }
}