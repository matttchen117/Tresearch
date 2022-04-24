using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Models.Contracts;
using TrialByFire.Tresearch.Models.Implementations;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Services.Implementations
{
    public class TreeManagementService : ITreeManagementService
    {
        private ISqlDAO _sqlDAO { get; }
        private ILogService _logService { get; }
        private IMessageBank _messageBank { get; }

        public TreeManagementService(ISqlDAO sqlDAO, ILogService logService, IMessageBank messageBank)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _messageBank = messageBank;
        }

        public async Task<Tuple<Tree, string>> GetNodesAsync(string userHash, string accountHash, CancellationToken cancellationToken = default)
        {
            List<INode> nullNodes = new List<INode>();
            Tree nullTree = null;
            Tree tree = new Tree();
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                //Store the result of the DAO method as a tuple of the List of Nodes retrieved as well as the completion result
                Tuple<List<INode>, string> nodesResult = await _sqlDAO.GetNodesAsync(userHash, accountHash, cancellationToken).ConfigureAwait(false);
                
                //The root node of the tree should have the lowest ID as it should be created before any additional children are
                //Therefore find the ID of that rootNode
                //long rootID = nodesResult.Item1.Min(Node => Node.NodeID);//check if the nodeID is the ParentID
                //INode r = nodesResult.Item1.Where(INode => INode.NodeID == INode.ParentNodeID).FirstOrDefault();
                //Assign that node as the rootNode of the Tree object
                //tree.rootNode = new TreeNode(r);
                //Iterate through the list of Nodes finding where the NodeParentID is equal to that of a TreeNode's NodeID 
                    foreach (INode n in nodesResult.Item1)
                    {
                        if(n.NodeID == n.ParentNodeID)
                        {
                            tree.rootNode = new TreeNode(n);
                            continue;
                        }
                        //Create a TreeNode from the current Node
                        TreeNode temp = new TreeNode(n);
                        /*if (n.NodeParentID == tree.rootNode.NodeID)
                        {
                            tree.rootNode.Children.Add(n);
                        }*/
                        //Performs a DFS of the Tree to find the TreeNode that is the parent of the current INode "n"
                        TreeNode findResult = new TreeNode();

                        findResult = tree.FindNode(tree.rootNode, temp.ParentNodeID);
                        //If the result of the search is not null, then there exists a TreeNode that is the parent of the current INode "n"
                        if (findResult != null)
                        {
                            string addResult = "";
                            addResult = tree.AddChild(tree.rootNode, temp, findResult.NodeID);
                        }
                    }
                return Tuple.Create(tree, "200: Server: Get Nodes Success");
            }
            catch(OperationCanceledException ex)
            {
                return Tuple.Create(nullTree, _messageBank.GetMessage(IMessageBank.Responses.cancellationRequested).Result);
            }
            catch(Exception ex)
            {
                return Tuple.Create(nullTree, "500: Server: " + ex);
            }

        }
    }
}
