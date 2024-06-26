using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using Unity.VisualScripting;
// using System.Diagnostics;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        DialogueNode draggingNode;
        GUIStyle nodeStyle;
        Vector2 draggingOffset;

        [MenuItem("Window/Dialogue Editor")]                                // Add to "Window" tab.
        public static void ShowEditorWindow()                               // Opens custom editor.
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]                                                    // Call when any asset is opened.
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;       // Cast clicked object as Dialogue.

            if(dialogue != null)                                            // If object is Dialogue, open dialogue editor.
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        private void OnEnable()                                             // Called when the Dialogue editor is created.
        {
            Selection.selectionChanged += OnSelectionChanged;               // Add to list of events to call when selection changed.

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.normal.textColor = Color.white;                       // TODO: Make more customizable.
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12 ,12);
        }

        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;      // cast selected object as Dialogue.

            if(newDialogue != null)
            {
                selectedDialogue = newDialogue;                             // Update editor with selected Dialogue.
                Repaint();
            }
        }

        private void OnGUI()
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue asset selected");
            }
            else
            {
                ProcessEvents();
                foreach(DialogueNode node in selectedDialogue.GetAllNodes())        // Separate to fix overlap issues.
                {
                    DrawNode(node);
                }
                foreach(DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
            }
        }

        private void ProcessEvents()
        {
            if(Event.current.type == EventType.MouseDown && draggingNode == null)                          // Mouse down.
            {
                draggingNode = selectedDialogue.GetNodeAtPoint(Event.current.mousePosition);
                
                if(draggingNode != null)
                    draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
            }
            else if(Event.current.type == EventType.MouseDrag && draggingNode != null)                      // Mouse dragging.
            {
                Undo.RecordObject(selectedDialogue, "Updated node position");
                draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
                GUI.changed = true;
            }
            else if(Event.current.type == EventType.MouseUp && draggingNode != null)                        // Mouse up.
            {
                draggingNode = null;
            }
        }

        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, nodeStyle);                          // Start node boundary.

            EditorGUILayout.LabelField("Node:" + node.nodeID);
            EditorGUI.BeginChangeCheck();                                           // Start change log.
            string newID = EditorGUILayout.TextField(node.nodeID);
            string newText = EditorGUILayout.TextField(node.nodeText);
                    
            if(EditorGUI.EndChangeCheck())                                         // If changes made.
            {
                Undo.RecordObject(selectedDialogue, "Update dialogue text");
                node.nodeText = newText;
                node.nodeID = newID;
            }

            GUILayout.EndArea();                                                   // End node boundary.
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPos = new Vector2(node.rect.xMax, node.rect.center.y);
            foreach(DialogueNode child in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPos = new Vector2(child.rect.xMin, child.rect.center.y);
                Vector3 cpOffset = endPos - startPos;
                cpOffset.y = 0;
                cpOffset.x *= 0.8f;
                Handles.DrawBezier(startPos, endPos, startPos + cpOffset, endPos - cpOffset, Color.white, null, 4f);
            }
        }
    }
}
