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
            
            // CORRECCIÓN: Forzar posición delante del jugador
            transform.localPosition = new Vector3(0, 0f, 1.5f); 
            transform.localRotation = Quaternion.identity; 
        }
    }

    void Update()
    {
        // Movimiento de "tajo": Avanzar hacia adelante RELATIVO al jugador
        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug para ver qué estamos tocando
        // Debug.Log("Slash tocó: " + other.name);

        // Intentar obtener el componente EnemyController del objeto colisionado
        EnemyController enemigo = other.GetComponent<EnemyController>();

        if (enemigo != null)
        {
            Debug.Log("Slash impactó a enemigo: " + enemigo.name);
            // Aplicar daño al enemigo
            enemigo.RecibirDano(dano);
        }
    }
}
