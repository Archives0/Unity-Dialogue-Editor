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

#if UNITY_EDITOR                                        // If in Unity editor, not runtime.
        private void Awake()
        {
            if(nodes.Count == 0)                        // If no nodes, add default node.
            {
                nodes.Add(new DialogueNode());
            }
        }
#endif

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }
    }
}