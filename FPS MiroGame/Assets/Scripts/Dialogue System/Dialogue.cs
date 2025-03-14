using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] 
        List<DialogueNode> _nodes; // A list of the DialogueNode scriptable objects

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>(); 

#if UNITY_EDITOR
        private void Awake()
        {
            // Create a root dialogue node for this scriptable object when it is created if it does not already have one
            if (_nodes.Count == 0)
            {
                _nodes.Add(new DialogueNode());
            }

            OnValidate();
        }
#endif

        private void OnValidate() // Called when a script instance is loaded or a value is updated in the editor
        {
            nodeLookup.Clear();

            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.uniqueID] = node;
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
            foreach (string childID in parentNode.childrenID) 
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
  