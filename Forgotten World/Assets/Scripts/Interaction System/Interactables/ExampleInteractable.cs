using UnityEngine;

public class ExampleInteractable :  BaseInteractable
{

  public override void Interact()
  {
    Debug.Log("Interacted with " + gameObject.name);
  }

}
