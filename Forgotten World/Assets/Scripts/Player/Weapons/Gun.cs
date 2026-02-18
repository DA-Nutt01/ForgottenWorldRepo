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
    // Gun Barrel 
    [SerializeField] private Transform m_Barrel;
    [SerializeField] private Camera m_Cam;
    //Weapon Range
    [SerializeField] private float m_WeaponRange;

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
        // Shoot a ray from the gun barrel forward the weapon range
        Ray bulletRay = new Ray(m_Cam.transform.position, m_Cam.transform.forward);
        // If the bullet ray hits anything in range
        if (Physics.Raycast(bulletRay, out RaycastHit hit, m_WeaponRange))
        {
            //Deals damage
            Debug.Log($"Bullet hit {hit.collider.name}");
            // Try to find the target's HP component
            HitPoints targetHP = hit.collider.GetComponent<HitPoints>();
            // Check if the target is damageable
            if (targetHP != null) {
                // Deal Damage to it
                targetHP.Damage(m_Damage);
                Debug.Log($"{hit.collider.name} DOES have HP");
            } else {
                Debug.Log($"{hit.collider.name} does NOT have HP");
            }
        }
    }
    
        // Reload Function
    public void Reload()
    {
            
    }
}
