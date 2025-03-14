using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace Dialogue.Editor
{

    public class DialogueEditor : EditorWindow
    {
        private Dialogue _selectedDialogue = null; // The currently opened Dialogue scriptable object
        private GUIStyle _nodeStyle; // Responsible for the styling of the node
        private DialogueNode _draggingNode = null; // The current dialogue node being repositioned in the editor
        private Vector2 _dragOffset; // An offset to keep the mouse in the same position relative to the node it is dragging
        

        [MenuItem("Window/Dialgoue Editor")] // An annotation to make this function called when clicking this menu item in the editor; For this to work, the function must be public, static, and return void
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor"); // Creates the Dialogue Editor window in the Unity Editor
        }

        [OnOpenAsset(1)]
        public static bool OnOpenDialogue(int instanceID, int line)
        {
            // This function auto opens the Dialogue editor when opening a Dialogue scriptable object

            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;

            if(dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }

            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            _nodeStyle.normal.textColor = Color.white;
            _nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            _nodeStyle.border = new RectOffset(12, 12, 12, 12);
            
        }

        private void OnSelectionChanged()
        {
            // Try to change the currently selcted Dialogue Scriptable object to the newly selected one
            Dialogue newDialogue = Selection.activeObject as Dialogue;

            if( newDialogue != null ) 
            {
                _selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (_selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.");
            }
            else
            {
                ProcessEvents();
                foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
                {
                    DrawNodeConnections(node);
                }

                foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
            }
        }

        private void ProcessEvents()
        {
            
            // Node Repositioning
            if (Event.current.type == EventType.MouseDown && _draggingNode == null)
            {
                _draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if (_draggingNode != null)
                {
                    _dragOffset = _draggingNode.rect.position - Event.current.mousePosition;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingNode != null)
            {
                Undo.RecordObject(_selectedDialogue, "Move Dialogue Node");
                _draggingNode.rect.position = Event.current.mousePosition + _dragOffset;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingNode != null)
            {
                _draggingNode = null;
            }
        }

        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, _nodeStyle);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node", EditorStyles.whiteLabel);
            string newUniqueID = EditorGUILayout.TextField("ID: " + node.uniqueID);
            string newText = EditorGUILayout.TextField(node.text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_selectedDialogue, "Update Dialogue Text");

                node.text = newText;
                node.uniqueID = newUniqueID;
            }

            

            GUILayout.EndArea();
        }

        private void DrawNodeConnections(DialogueNode node)
        {
            foreach (DialogueNode childNode in _selectedDialogue.GetAllChildren(node))
            {
                Vector2 startPos = new Vector2(node.rect.xMax - 8, node.rect.center.y);
                Vector2 endPos = new Vector2(childNode.rect.xMin + 8, childNode.rect.center.y);
                Vector2 controlPointOffset = endPos - startPos;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;

                Handles.DrawBezier(startPos, endPos, startPos + controlPointOffset, endPos - controlPointOffset, Color.white, null, 5f);
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 mousePosition)
        {
            DialogueNode foundNode = null;
            foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
            {
                if (node.rect.Contains(mousePosition))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }
    }
}
