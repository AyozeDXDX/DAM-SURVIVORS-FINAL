using UnityEngine;

public class SlashWarrior : MonoBehaviour
{
    public int dano = 10;
    public float tiempoDeVida = 0.2f;

    private Transform playerTransform;

    void Start()
    {
        // 1. Programar la autodestrucción
        Destroy(gameObject, tiempoDeVida);

        // Buscar al jugador para seguirlo
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            // Hacemos que el slash sea hijo del jugador
            transform.SetParent(playerTransform, true);
            
            // CORRECCIÓN: Forzar posición delante del jugador
            // Asumimos que el jugador mira hacia Z+ (forward)
            transform.localPosition = new Vector3(0, 1f, 1.5f); // Un poco arriba y adelante
            transform.localRotation = Quaternion.identity; // Alinear con el jugador
        }
    }

    void Update()
    {
        // Movimiento de "tajo": Avanzar hacia adelante RELATIVO al jugador
        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }

    // 2. Detectar enemigos (repaso de Lección 6)
    void OnTriggerEnter(Collider other)
    {
        // Debug para ver qué estamos tocando
        // Debug.Log("Slash tocó: " + other.name);

        // Intentamos obtener el "Cerebro" del objeto con el que chocamos
        EnemyController enemigo = other.GetComponent<EnemyController>();

        // Si tiene cerebro (es un enemigo)
        if (enemigo != null)
        {
            Debug.Log("Slash impactó a enemigo: " + enemigo.name);
            // 3. Le hacemos daño
            enemigo.RecibirDano(dano);

            // (Opcional) ¿El slash desaparece al primer golpe o sigue activo?
            // Si quisiéramos que desapareciera:
            // Destroy(gameObject);
        }
    }
}