using UnityEngine;

public class HitPoints : MonoBehaviour, IDamagable
{
    // VARIABLES
    [SerializeField, Tooltip(" This variable stores the number of hit points the object has")] 
    private int HP = 100; 
    
    public void OnDeath(){
        //Object gets destroyed
        Destroy(gameObject);
    }

    public void Damage(int dmg){
        //Decrease health
        HP -= dmg;

        Debug.Log($"{gameObject.name} took {dmg} damage!");

        //If health reahes 0 or less, object is destroyed
        if (HP <= 0){
            OnDeath();
        }
    }

    public void Heal(int heal){
        //Health goes up
        HP += heal;
    }
}
