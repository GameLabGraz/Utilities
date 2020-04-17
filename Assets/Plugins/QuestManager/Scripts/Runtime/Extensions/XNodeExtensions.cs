using System.Collections.Generic;
using System.Linq;
using XNode;

namespace GEAR.QuestManager.Extensions
{
    public static class XNodeExtensions
    {
        public static IEnumerable<Node> GetNodeByName (this List<Node> nodes, string name)
        {
            return nodes.Where (node => node.name.Contains (name));
        }

        public static IEnumerable<NodePort> GetNodePortByName(this List<NodePort> nodePorts, string name)
        {
            return nodePorts.Where(nodePort => nodePort.node.name.Contains(name));
        }
    }
}