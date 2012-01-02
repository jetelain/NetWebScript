using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace NetWebScript.Debug.App.ViewModel
{
    public class SourceFolderModel : SourceNodeModel
    {
        private readonly Dispatcher dispatcher;
        private readonly SourceFolderModel parent;
        private readonly List<SourceFolderModel> folders = new List<SourceFolderModel>();
        private readonly List<SourceFileModel> files = new List<SourceFileModel>();
        private readonly DispatcherObservableCollection<SourceNodeModel> children;
        
        public SourceFolderModel(Dispatcher dispatcher, SourceFolderModel parent, string filename)
            : base(filename)
        {
            children = new DispatcherObservableCollection<SourceNodeModel>(dispatcher);
        }

        public ObservableCollection<SourceNodeModel> Children
        {
            get { return children; }
        }

        internal SourceFolderModel GetFolderByFullname(string fullname)
        {
            var folder = folders.FirstOrDefault(r => string.Equals(r.Fullname, fullname, StringComparison.OrdinalIgnoreCase));
            if (folder == null)
            {
                folder = new SourceFolderModel(dispatcher, this, fullname);
                folders.Add(folder);
                children.Add(folder);
            }
            return folder;
        }

        internal SourceFileModel GetFileByFullname(string fullname)
        {
            var file = files.FirstOrDefault(r => string.Equals(r.Fullname, fullname, StringComparison.OrdinalIgnoreCase));
            if (file == null)
            {
                file = new SourceFileModel(fullname);
                files.Add(file);
                children.Add(file);
            }
            return file;
        }


        internal void RemoveFileByFullname(string fullname)
        {
            var file = files.FirstOrDefault(r => string.Equals(r.Fullname, fullname, StringComparison.OrdinalIgnoreCase));
            if (file != null)
            {
                files.Remove(file);
                children.Remove(file);
                RemoveIfEmpty();
            }
        }

        private void RemoveIfEmpty()
        {
            if (files.Count == 0 && folders.Count == 0 && parent != null)
            {
                parent.RemoveFolderByFullname(Fullname);
            }
        }

        private void RemoveFolderByFullname(string fullname)
        {
            var folder = folders.FirstOrDefault(r => string.Equals(r.Fullname, fullname, StringComparison.OrdinalIgnoreCase));
            if (folder != null)
            {
                folders.Remove(folder);
                children.Remove(folder);
                RemoveIfEmpty();
            }
        }
    }
}
