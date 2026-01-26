using UnityEngine;

public class Gun : MonoBehaviour, IGun
{

    [SerializeField] private GunData m_GunData;
    [SerializeField] private string m_GunName; 
    // Mag size
    [SerializeField] private float m_MagSize;
    // Reload time
    [SerializeField] private float m_ReloadTime;
    // Fire Rate
    [SerializeField] private float m_FireRate;
    // Damage
    [SerializeField] private int m_Damage;
    // Weapon Prefab
    [SerializeField] private GameObject m_WeaponPrefab;

    private void Awake()
    {
        // Awake is the ideal place to initialzie your vairables
        m_GunName = m_GunData.gunName;
        m_MagSize = m_GunData.magSize;
        m_ReloadTime = m_GunData.reloadTime;
        m_FireRate = m_GunData.fireRate;
        m_Damage = m_GunData.damage;
        m_WeaponPrefab =m_GunData.weaponPrefab;
        
    }

    // Shoot Function
    public void Shoot()
    {
        Debug.Log("BANG!");
    }
        // Reload Function
    public void Reload()
    {
            
    }
}
