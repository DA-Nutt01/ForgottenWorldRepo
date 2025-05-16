using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        public string text; [Space(15)]
        public List<string> childrenNodeIDs = new List<string>(); [Space(15)] 
        public Rect rect = new Rect(100, 100, 200, 100);
    }
}
