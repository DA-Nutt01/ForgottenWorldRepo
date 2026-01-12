using UnityEngine;

public interface IInteractable {
  // An interface is a contract that any class implementing it must follow.
  // Here, any class that implements IInteractable must provide implementations for its functions.
  void Interact();
}
