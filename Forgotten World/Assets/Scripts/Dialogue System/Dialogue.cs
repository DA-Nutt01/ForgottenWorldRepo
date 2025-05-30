using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        List<DialogueNode> _nodes = new List<DialogueNode>(); // A list of the DialogueNode scriptable objects

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>(); // A dictionary of all dialogue nodes in this dialogue

        public void InitializeRootNode()
        {
            //Creates the root node for the dialogue
            _nodes.Clear(); // Clear the nodes list 
            DialogueNode rootNode = CreateNode(null); // Construct a new Dialogue Node
            rootNode.name = Guid.NewGuid().ToString(); // Assign the new root node a unique ID
            _nodes.Add(rootNode); // Add the root node to the nodes list
            OnValidate();
        }

        public bool IsEmpty()
        {
            return _nodes.Count == 0;
        }

        private void OnValidate() // Called when a script instance is loaded or a value is updated in the editor
        {


            nodeLookup.Clear();

            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }

#if UNITY_EDITOR
        public DialogueNode CreateNode(DialogueNode parentNode)
        {
            DialogueNode newNode = MakeNode(parentNode);
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Node Created");
            AddNode(newNode);
            return newNode;
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
             Undo.RecordObject(this, "Node Deleted");
            _nodes.Remove(nodeToDelete);
            OnValidate();
            CleanupNodeChildrenReferencesOnDeletion(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);

        }

        private void AddNode(DialogueNode newNode)
        {
            _nodes.Add(newNode);
            OnValidate();
        }

        private static DialogueNode MakeNode(DialogueNode parentNode)
        {
            DialogueNode newNode = ScriptableObject.CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            if (parentNode != null) parentNode.AddChildID(newNode.name);
            return newNode;
        }

        private void CleanupNodeChildrenReferencesOnDeletion(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChildID(nodeToDelete.name);
            }
        }
#endif

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            // A getter function to return all nodes in this scriptable object
            return _nodes;
        }

        public DialogueNode GetRootNode()
        {
            return _nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {

            foreach (string childID in parentNode.GetChildrenNodesIDs())
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
                else
                {
                    Debug.LogWarning($"Child ID {childID} not found in nodeLookup.");
                }
            }
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (_nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null);
                AddNode(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this); // Add this new node as a sub asset to this dialogue scriptable object
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }

}
  