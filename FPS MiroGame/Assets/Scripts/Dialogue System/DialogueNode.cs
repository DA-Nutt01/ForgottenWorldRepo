using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [System.Serializable] 
    public class DialogueNode
    {
        public string uniqueID; 
        public string text; [Space(15)]
        public string[] childrenID; [Space(15)] 
        public Rect rect = new Rect(0, 0, 200, 100);
    }
}
