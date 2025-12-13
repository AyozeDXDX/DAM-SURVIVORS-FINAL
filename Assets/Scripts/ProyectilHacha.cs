using UnityEngine;

public class ProyectilHacha : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////// VARIABLES ///////////////////////////////////////////

    [Header("Configuración del Proyectil")]
    public float velocidad = 10f;
    public int dano = 5;
    public float tiempoDeVida = 1.5f; // Cuántos segundos vive el hacha

    ///////////////////////////////////////////////////////////////// FUNCIONES UNITY /////////////////////////////////////////

    void Start()
    {
        // Programamos la autodestrucción del hacha para que no vuele para siempre
        Destroy(this.gameObject, tiempoDeVida);
    }

    void Update()
    {
        // Movemos el hacha hacia adelante (en su eje Z local)
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    // ¡LA FUNCIÓN MÁGICA DE LOS TRIGGERS!
    // Esta función se llama AUTOMÁTICAMENTE cuando otro collider
    // entra en nuestro sensor (que marcamos como "Is Trigger").
    void OnTriggerEnter(Collider other)
    {
        // 'other' es el objeto que nos ha tocado.
        // Comprobamos si el objeto que hemos tocado tiene el script "EnemyController".
        EnemyController enemigo = other.GetComponent<EnemyController>();

        // Si 'enemigo' no es null, ¡significa que hemos chocado con un enemigo!
        if (enemigo != null)
        {
            // Le decimos al enemigo que reciba daño
            enemigo.RecibirDano(dano);
            
            // (Opcional) Destruimos el hacha al primer impacto
            Destroy(this.gameObject); 
        }
    }
}