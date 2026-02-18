using UnityEngine;

public interface IDamagable 
{
    public void OnDeath(); // Function called when this object reaches 0 HP
    public void Damage(int dmg); // Function called when object takes damage
    public void Heal(int heal); //Function called when objects are repaired
}
