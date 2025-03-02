using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dialogue.Editor
{

    public class DialogueEditor : EditorWindow
    {
        [MenuItem("Window/Dialgoue Editor")] // An annotation to make this function called when clicking this menu item in the editor; For this to work, the function must be public, static, and return void
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor"); // Creates the Dialogue Editor window in the Unity Editor
        }
    }
}
