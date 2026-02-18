using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    // Destination of the bullet
    private Vector3 m_TargetPosition;
    [SerializeField]
    private float m_ProjectileSpeed = 200f;

    // Where the bullet is going
    public void Setup(Vector3 targetPos){
        m_TargetPosition = targetPos;
    }

    private void Update(){
        // Calculate the direction the bullet needs to move in
        Vector3 moveDir = (m_TargetPosition - transform.position).normalized;
        // Calculate the distance before the bullet moves this frame
        float distBeforeMoving = Vector3.Distance(transform.position, m_TargetPosition);
        // Move the projectile towards the target
        transform.position += moveDir * m_ProjectileSpeed * Time.deltaTime;
        // Calculate the distance after moving this frame
        float distAfterMoving = Vector3.Distance(transform.position, m_TargetPosition);

        // Check if bullet overshot destination
        if (distBeforeMoving < distAfterMoving){
            // Destroy this bullet
            Destroy(gameObject);
        }

    }
}
