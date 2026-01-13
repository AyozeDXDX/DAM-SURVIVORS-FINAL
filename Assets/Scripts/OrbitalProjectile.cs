using UnityEngine;

public class OrbitalProjectile : MonoBehaviour
{
    public int damage = 10;
    public float knockbackForce = 0f;

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.RecibirDano(damage);
        }
    }
}
