using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> nodeRegistry = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR                                        // If in Unity editor, not runtime.
        private void Awake()
        {
            if(nodes.Count == 0)                        // If no nodes, add default node.
            {
                nodes.Add(new DialogueNode());
            }

            OnValidate();
        }
#endif

        private void OnValidate()
        {
            nodeRegistry.Clear();

            foreach(DialogueNode node in GetAllNodes())
            {
                nodeRegistry[node.nodeID] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public DialogueNode GetNodeAtPoint(Vector2 mousePos)
        {
            DialogueNode pointNode = null;

            foreach(DialogueNode node in nodes)
            {
                if(node.rect.Contains(mousePos))
                    pointNode = node;
            }

            return pointNode;
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            List<DialogueNode> childNodes = new List<DialogueNode>();

            foreach(string child in parentNode.childNodes)
            {
                if(nodeRegistry.ContainsKey(child))
                    childNodes.Add(nodeRegistry[child]);
            }

            return childNodes;
        }
    }
}