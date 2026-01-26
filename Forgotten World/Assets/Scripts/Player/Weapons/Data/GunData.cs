using UnityEngine;

[CreateAssetMenu(fileName="NewGun", menuName = "Inventory/Gun")]
public class GunData : ScriptableObject
{
    // Sricptable objects are a place to define data an object of a certain type can hold
    public string gunName; 
    // Mag size
    public float magSize;
    // Reload time
    public float reloadTime;
    // Fire Rate
    public float fireRate;
    // Damage
    public int damage;
    // Weapon Prefab
    public GameObject weaponPrefab;
}
