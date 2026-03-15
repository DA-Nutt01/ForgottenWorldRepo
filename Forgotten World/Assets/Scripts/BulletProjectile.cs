using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    // Destination of the bullet
    private Vector3 m_TravelDirection;
    [SerializeField]
    private float m_ProjectileSpeed = 200f;
    [SerializeField] private float m_LifeTime = 1f;

    private void Start(){
        // Destroy the bullet after its lifetime
        Destroy(gameObject, m_LifeTime);
    }

    // Where the bullet is going
    public void Setup(Vector3 direction){
        m_TravelDirection = direction.normalized;
    }

    private void Update(){
        // Make the bullet travel in the shoot direction
        transform.position += m_TravelDirection * m_ProjectileSpeed * Time.deltaTime;
        // Handle Despawn
    }
}
