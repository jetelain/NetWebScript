using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Debug.Server;

namespace NetWebScript.Debug.App.ViewModel
{
    public class DataModel
    {
        private readonly ThreadModel thread;
        private readonly JSData jsData;
        private List<DataModel> children;

        public DataModel(ThreadModel thread, JSData jsData)
        {
            this.thread = thread;
            this.jsData = jsData;
            if (jsData.Children != null)
            {
                children = jsData.Children.Select(d => new DataModel(thread, d)).ToList();
            }
        }


        public string ValueTypeDisplayName { get { return jsData.ValueTypeDisplayName; } }


        public string DisplayName { get { return jsData.DisplayName; } }


        public string Value { get { return jsData.Value; } }

        public List<DataModel> Children
        {
            get
            {
                if (children == null && jsData.ShouldRetreiveChildren)
                {
                    var retreive = thread.Expand(jsData);
                    jsData.Merge(retreive);
                    if (retreive != null && retreive.Children != null)
                    {
                        children = retreive.Children.Select(d => new DataModel(thread, d)).ToList();
                    }
                    else
                    {
                        children = new List<DataModel>(0);
                    }
                }
                return children;
            } 
        }
    }
}
