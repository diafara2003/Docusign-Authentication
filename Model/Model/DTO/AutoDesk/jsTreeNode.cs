using System.Collections.Generic;

namespace Model.DTO.Autodesk
{
    public class jsTreeNode
    {
        public jsTreeNode(string id, string text, string type, bool children)
        {
            this.id = id;
            this.text = text;
            this.type = type;
            this.children = children;
        }

        public jsTreeNode()
        {
            this.type = "string";
            this.children = false;
        }
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public bool children { get; set; }
    }


    public class jsTreeNodeB
    {

        public jsTreeNodeB()
        {
            this.type = "string";
            this.children = null;
        }
        public string id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string urn { get; set; }
        public List<jsTreeNodeB> children { get; set; }
    }

    public class jsMetaData {
        public IList<jsTreeNodeB> jstreenode { get; set; }
        public List<jsTreeNode> jsSingleNode { get; set; }
    }

    public class FolferInfo {
        public string name { get; set; }
    }
}
