using Terminal.Gui.Trees;

namespace Retrowarden.Models
{
    public class NodeData
    {
        public string? Id { get; set; }
        public NodeType NodeType { get; set; }
        
        public NodeItemGroupType ItemGroupType { get; set; }
        public ITreeNode? Parent { get; set; }
        public string? Text { get; set; }
    }    
}

