using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] 
        List<DialogueNode> _nodes = new List<DialogueNode>(); // A list of the DialogueNode scriptable objects

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>(); // A dictionary of all dialogue nodes in this dialogue

        public void InitializeRootNode()
        {
            //Creates the root node for the dialogue
            _nodes.Clear(); // Clear the nodes list 
            DialogueNode rootNode = new DialogueNode(); // Construct a new Dialogue Node
            rootNode.uniqueID = Guid.NewGuid().ToString(); // Assign the new root node a unique ID
            _nodes.Add(rootNode); // Add the root node to the nodes list
            OnValidate();
        }

        public bool IsEmpty()
        {
            return _nodes.Count == 0;
        }

        private void OnValidate() // Called when a script instance is loaded or a value is updated in the editor
        {
            if (_nodes.Count == 0)
            {
                Debug.Log("Count is 0");
                DialogueNode rootNode = new DialogueNode();
                rootNode.uniqueID = Guid.NewGuid().ToString();
                _nodes.Add(rootNode);
            }

            nodeLookup.Clear();

            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.uniqueID] = node;
            }
        }

        public void CreateNode(DialogueNode parentNode)
        {
            DialogueNode newNode = new DialogueNode();
            newNode.uniqueID = Guid.NewGuid().ToString();
            parentNode.childrenNodeIDs.Add(newNode.uniqueID);
            _nodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            _nodes.Remove(nodeToDelete);
            CleanupNodeChildrenReferencesOnDeletion(nodeToDelete);
            OnValidate();
        }

        private void CleanupNodeChildrenReferencesOnDeletion(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.childrenNodeIDs.Remove(nodeToDelete.uniqueID);
            }
        }

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

            foreach (string childID in parentNode.childrenNodeIDs) 
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
    }

}
  