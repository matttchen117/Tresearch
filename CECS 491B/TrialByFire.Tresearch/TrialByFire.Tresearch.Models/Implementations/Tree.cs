using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrialByFire.Tresearch.Models.Implementations
{
    public class Tree
    {
        public TreeNode rootNode { get; set; }
        public List<TreeNode> allChildren { get; set; }

        public Tree()
        {
            allChildren = new List<TreeNode>();
        }

        //Searches through the Tree starting at the rootNode to find the TreeNode in which its ID is equal to the one searched for 
        public TreeNode FindNode(TreeNode treeNode, long nodeID)
        {
            if (treeNode == null)
            {
                return null;
            }

            if (treeNode.NodeID == nodeID)
            {
                return treeNode;
            }

            //Depth First Search approach that iterates through the TreeNode's list of children
            foreach (TreeNode child in treeNode.Children)
            {
                TreeNode found = FindNode(child, nodeID);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        //Adds the child to the targeted TreeNode
        public string AddChild(TreeNode treeNode, TreeNode child, long nodeID)
        {
            /*if (node == null)
            {
                return null;
            }*/

            if (treeNode.NodeID == nodeID)
            {
                allChildren.Add(child);
                treeNode.Children.Add(child);
                return "Success";
            }

            //DFS Approach that iterates through the TreeNodes until it is equal to the targeted one so that 
            foreach (TreeNode n in treeNode.Children)
            {
                string added = AddChild(n, child, nodeID);
                if (added != null)
                {
                    return "Success";
                }
            }

            return null;
        }

    }
}
