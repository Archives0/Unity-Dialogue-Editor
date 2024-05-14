using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]                   // Cannot reference unless serialized.
    public class DialogueNode
    {
        public string nodeID;
        public string nodeText;
        public Color textColor = Color.white;
        public string[] childNodes;
        public Rect rect = new Rect(0, 0, 200, 100);
    }
}