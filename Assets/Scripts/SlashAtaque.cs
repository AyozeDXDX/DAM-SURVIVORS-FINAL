using UnityEngine;

public class SlashAtaque : MonoBehaviour
{
    [Header("Configuración del Ataque")]
    [Tooltip("Tiempo de vida del hitbox antes de destruirse.")]
    public float tiempoDeVida = 0.2f;
    
    [Tooltip("Daño que inflige el ataque.")]
    public int dano = 10;

    private Transform playerTransform;

    void Start()
    {
        // Destruir el objeto automáticamente después del tiempo definido
        Destroy(gameObject, tiempoDeVida);

        // Buscar al jugador para seguirlo
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            // Hacemos que el slash sea hijo del jugador
            transform.SetParent(playerTransform, true);
            
            Vector3 frenteJugador = player.transform.forward;
            transform.position = player.transform.position + frenteJugador * 1.5f + Vector3.up * 1f;
            transform.rotation = player.transform.rotation; 
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void Update()
    {
        // Movimiento de "tajo": Avanzar hacia adelante RELATIVO al jugador
        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Intentar obtener el componente EnemyController del objeto colisionado
        EnemyController enemigo = other.GetComponent<EnemyController>();

        if (enemigo != null)
        {
            // Aplicar daño al enemigo
            enemigo.RecibirDano(dano);
        }
    }
}
