using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 15;
    
    private Transform target;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    void Start()
    {
        // Aseguramos Rigidbody para colisiones
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void Update()
    {
        if (target == null)
        {
            // Si el objetivo muere, lo destruimos
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.RecibirDano(damage);
            Destroy(gameObject);
        }
    }
}
