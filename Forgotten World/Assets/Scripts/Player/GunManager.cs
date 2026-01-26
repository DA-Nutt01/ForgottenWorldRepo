using UnityEngine;

public class GunManager : MonoBehaviour
{
    public static GunManager Instance { get; private set;}
    [SerializeField] private Gun m_EquippedGun;

    public Gun GetEqippedGun()
    {
        return m_EquippedGun;
    }
    
     private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            Debug.Log("Can't Have More Than One Copy!");
        }
    }
}
