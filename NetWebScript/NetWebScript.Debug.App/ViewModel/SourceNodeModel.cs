using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NetWebScript.Debug.App.ViewModel
{
    public class SourceNodeModel
    {
        private readonly string name;
        private readonly string fullname;

        public SourceNodeModel(string fullname)
        {
            this.name = Path.GetFileName(fullname);
            this.fullname = fullname;
            if (string.IsNullOrEmpty(name))
            {
                name = fullname;
            }
        }

        public string Name
        {
            get { return name; }
        }

        public string Fullname
        {
            get { return fullname; }
        }
    }
}
