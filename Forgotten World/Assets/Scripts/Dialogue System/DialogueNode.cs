using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        string text; [Space(15)]
        [SerializeField]
        List<string> childrenNodeIDs = new List<string>(); [Space(15)]
        [SerializeField]
        Rect rect = new Rect(100, 100, 200, 100);

        public Rect GetRect()
        {
            return rect;
        }

        public string GetText()
        {
            return text;
        }

        public List<string> GetChildrenNodesIDs()
        {
            return childrenNodeIDs;
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            Undo.RecordObject(this, "Update Dialogue Text");
            if (newText != text) text = newText;
            EditorUtility.SetDirty(this);
        }

        public void AddChildID(string childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            childrenNodeIDs.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChildID (string childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            childrenNodeIDs.Remove(childID);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
