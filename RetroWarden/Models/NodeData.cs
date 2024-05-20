using Terminal.Gui.Trees;

namespace Retrowarden.Models
{
    public class NodeData
    {
        public string? Id { get; set; }
        public NodeType NodeType { get; set; }
        public TreeNode? Parent { get; set; }
        public string? Text { get; set; }
    }    
}

