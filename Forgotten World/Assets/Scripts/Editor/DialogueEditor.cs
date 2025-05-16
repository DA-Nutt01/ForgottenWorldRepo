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
        
        [NonSerialized] private GUIStyle _nodeStyle; // Responsible for the styling of the node
        [NonSerialized] private DialogueNode _draggingNode = null; // The current dialogue node being repositioned in the editor
        [NonSerialized] private Vector2 _dragOffset; // An offset to keep the mouse in the same position relative to the node it is dragging
        [NonSerialized] private DialogueNode _nodeToCreate = null; // A reference to a new dialogue node we want to create when clicking the add node button
        [NonSerialized] private DialogueNode _nodeToDelete = null; // A reference to an existing dialogue node we want to delete when clicking the delete node button
        [NonSerialized] private DialogueNode _linkignNode = null; // A ref the currently selected node that wants to link to another existing node as its parent node
        private Vector2 _scrollPosition; // 
        [NonSerialized] private bool _draggingCanvas = false; // A flag telling wether we are dragging on the editor canvas to scroll the view in the editor window
        [NonSerialized] private Vector2 _draggingCanvasOffset;

        const float CANVASSIZE = 4000;
        const float BACKGROUNDSIZE = 50;

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
                if (_selectedDialogue.IsEmpty()) _selectedDialogue.InitializeRootNode(); // Checks if the selected dialogue has any nodes and creates one if it doesnt
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

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

                Rect editorCanvas = GUILayoutUtility.GetRect(CANVASSIZE, CANVASSIZE); // Create & store a rectangle for the dimensions of the editor window 
                Texture2D backgroundTexture = Resources.Load("background") as Texture2D; // from the Resourcs folder in the editor folder, load the background texture and cache it

                Rect textCoords = new Rect(0, 0, CANVASSIZE / BACKGROUNDSIZE, CANVASSIZE / BACKGROUNDSIZE);
                GUI.DrawTextureWithTexCoords(editorCanvas, backgroundTexture, textCoords);

                foreach (DialogueNode node in _selectedDialogue.GetAllNodes()) // For each DialogueNOde in the selected Dialogue
                {
                    DrawNodeConnections(node); // Draw all connections between nodes
                }

                foreach (DialogueNode node in _selectedDialogue.GetAllNodes()) // For each DialogueNode in the selected Dialogue
                {
                    DrawNode(node); // Draw that node
                }

                EditorGUILayout.EndScrollView();

                if (_nodeToCreate != null)
                {
                    Undo.RecordObject(_selectedDialogue, "Node Created");
                    _selectedDialogue.CreateNode(_nodeToCreate);
                    _nodeToCreate = null;
                }

                if (_nodeToDelete != null)
                {
                    Undo.RecordObject(_selectedDialogue, "Node Deleted");
                    _selectedDialogue.DeleteNode(_nodeToDelete);
                    _nodeToDelete = null;
                }
            }
        }

        private void ProcessEvents()
        {
            
            // Node Repositioning
            if (Event.current.type == EventType.MouseDown && _draggingNode == null) // if we click mouse down while not dragging a node
            {
                _draggingNode = GetNodeAtPoint(Event.current.mousePosition + _scrollPosition); // We have clicked on a dialogue node
                if (_draggingNode != null)
                {
                    _dragOffset = _draggingNode.rect.position - Event.current.mousePosition;
                    Selection.activeObject = _draggingNode; // Show selected dialogueNode in inspector
                }
                else // we have not clicked on a node but instead, the canvas
                {
                    _draggingCanvas = true;
                    _draggingCanvasOffset = Event.current.mousePosition + _scrollPosition; // Record Offset for dragging the view by selected point on canvas
                    Selection.activeObject = _selectedDialogue; // Show the whole dialogue object in the inspector
                }
                
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingNode != null)
            {
                Undo.RecordObject(_selectedDialogue, "Move Dialogue Node");
                _draggingNode.rect.position = Event.current.mousePosition + _dragOffset;

                // Update Scroll position
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingCanvas)
            {
                _scrollPosition = _draggingCanvasOffset - Event.current.mousePosition; // Update teh scroll position so the scroll position remains the same while dragging

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingNode != null)
            {
                _draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingCanvas) // when we unclick the mouse while  dragging the canvas
            {
                _draggingCanvas = false;
            }
        }

        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.rect, _nodeStyle);
            EditorGUI.BeginChangeCheck();

            string newText = EditorGUILayout.TextField(node.text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_selectedDialogue, "Update Dialogue Text");

                node.text = newText;
            }

            GUILayout.BeginHorizontal(); // Any GUI elements created after this call will be aligned horizontally until EndHorizontal is called

            if (GUILayout.Button("Add"))
            {
                _nodeToCreate = node;
            }

            DrawLinkButtons(node);

            if (GUILayout.Button("Delete"))
            {
                _nodeToDelete = node;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (_linkignNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    _linkignNode = node; 
                }
            } 
            else if (_linkignNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    _linkignNode = null;
                }
            }
            else if (_linkignNode.childrenNodeIDs.Contains(node.name))
            {
                if (GUILayout.Button("Unlink"))
                {
                    Undo.RecordObject(_selectedDialogue, "Remove Dialogue Link"); 
                    _linkignNode.childrenNodeIDs.Remove(node.name); 
                    _linkignNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    Undo.RecordObject(_selectedDialogue, "Add Dialogue Link"); 
                    _linkignNode.childrenNodeIDs.Add(node.name); 
                    _linkignNode = null;
                }
            }
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
