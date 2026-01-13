using UnityEngine;

public class ProyectilHacha : MonoBehaviour
{
    [Header("Configuración del Proyectil")]
    public float velocidad = 10f;
    public int dano = 5;
    public float tiempoDeVida = 1.5f; // Cuántos segundos vive el hacha

    ///////////////////////////////////////////////////////////////// FUNCIONES UNITY /////////////////////////////////////////

    void Start()
    {
        // Programamos la autodestrucción del hacha para que no vuele para siempre
        Destroy(this.gameObject, tiempoDeVida);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        EnemyController enemigo = other.GetComponent<EnemyController>();

        if (enemigo != null)
        {
            // Le decimos al enemigo que reciba daño
            enemigo.RecibirDano(dano);
            
            Destroy(this.gameObject); 
        }
    }
}