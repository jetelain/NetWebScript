using System.Windows;
using System.Windows.Controls;

namespace NetWebScript.Debug.App.Controls
{
    public class TreeListView : TreeView
    {
        protected override DependencyObject
                           GetContainerForItemOverride()
        {
            return new TreeListViewItem();
        }

        protected override bool
                           IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListViewItem;
        }

        public GridViewColumnCollection Columns
        {
            get
            {
                if (_columns == null)
                {
                    _columns = new GridViewColumnCollection();
                }

                return _columns;
            }
        }

        private GridViewColumnCollection _columns;
    }
}
