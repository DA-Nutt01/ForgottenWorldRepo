using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Dialogue.Editor
{

    public class DialogueEditor : EditorWindow
    {
        private Dialogue _selectedDialogue = null; // The currently opened Dialogue scriptable object

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
                foreach (DialogueNode node in _selectedDialogue.GetAllNodes())
                {
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.LabelField("Node");
                    string newText = EditorGUILayout.TextField(node.text);
                    string newUniqueID = EditorGUILayout.TextField(node.uniqueID);

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(_selectedDialogue, "Update Dialogue Text");

                        node.text = newText;
                        node.uniqueID = newUniqueID;
                    }
                }
            }
        }
    }
}
