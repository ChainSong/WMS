using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class CheckedMenu : Menu
    {
        [EntityPropertyExtension("IsChecked", "IsChecked")]
        public bool IsChecked { get; set; }

        [EntityPropertyExtension("IsOpen", "IsOpen")]
        public bool IsOpen { get; set; }
    }
}