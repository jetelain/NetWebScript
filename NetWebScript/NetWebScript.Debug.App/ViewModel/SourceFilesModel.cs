using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace NetWebScript.Debug.App.ViewModel
{
    public class SourceFilesModel
    {
        private HashSet<string> currentFiles;
        private readonly DispatcherObservableCollection<SourceFolderModel> roots;
        private readonly Dispatcher dispatcher;

        public SourceFilesModel(Dispatcher dispatcher, List<string> files)
        {
            this.dispatcher = dispatcher;
            this.roots = new DispatcherObservableCollection<SourceFolderModel>(dispatcher);
            currentFiles = new HashSet<string>(files);
            foreach (var file in files)
            {
                GetFile(file);
            }
        }

        public ObservableCollection<SourceFolderModel> Roots
        {
            get { return roots; }
        }

        private SourceFolderModel GetRoot(string fullname)
        {
            var root = roots.FirstOrDefault(r => string.Equals(r.Fullname, fullname, StringComparison.OrdinalIgnoreCase));
            if (root == null)
            {
                root = new SourceFolderModel(dispatcher, null, fullname);
                roots.Add(root);
            }
            return root;
        }

        private SourceFolderModel GetFolder(string fullname)
        {
            var directory = Path.GetDirectoryName(fullname);
            if (string.IsNullOrEmpty(directory))
            {
                return GetRoot(fullname);
            }
            var parent = GetFolder(directory);
            var name = Path.GetFileName(fullname);

            return parent.GetFolderByFullname(fullname);
        }

        private SourceFileModel GetFile(string fullname)
        {
            var directory = GetFolder(Path.GetDirectoryName(fullname));
            return directory.GetFileByFullname(fullname);
        }

        private void RemoveFile(string fullname)
        {
            var directory = GetFolder(Path.GetDirectoryName(fullname));
            directory.RemoveFileByFullname(fullname);
        }

        private void SetFiles(List<string> files)
        {
            var remainFiles = currentFiles;
            currentFiles = null;
            foreach (var file in files)
            {
                GetFile(file);
                remainFiles.Remove(file);
            }
            if (remainFiles.Count > 0)
            {
                foreach (var file in remainFiles)
                {
                    RemoveFile(file);
                }
            }
            currentFiles = new HashSet<string>(files);
        }

        public void SetFilesAsync(List<string> files)
        {
            dispatcher.BeginInvoke(new Action(() => SetFiles(files)));
        }
        
    }
}
