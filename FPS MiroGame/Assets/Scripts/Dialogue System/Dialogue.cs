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

#if UNITY_EDITOR
        private void Awake()
        {
            // Create a root dialogue node for this scriptable object when it is created if it does not already have one
            if (_nodes.Count == 0)
            {
                _nodes.Add(new DialogueNode());
            }
        }
#endif
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            // A getter function to return all nodes in this scriptable object
            return _nodes;
        }
    }

}
