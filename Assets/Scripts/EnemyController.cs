using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// ////////////////////////////////// Variables ///////////////////////
    /// </summary>
    private GameObject player;
    private Transform objetivo;

    [Header("Estadísticas del Enemigo")]
    public EnemyStats estadisticas;
    private int currentHealth;
    private int damage;
    private int defense;
    private float speed;
    private float currentSpeed; // Velocidad actual modificable
    
    private Renderer rend;
    private Color originalColor;

    [Header("Botín (Orbes XP)")]
    public GameObject orbVerdePrefab; // 10 XP
    public GameObject orbAzulPrefab;  // 50 XP
    public GameObject orbOroPrefab;   // 100 XP

    private NavMeshAgent agente;

    /// <summary>
    /// //////////////////////////////// Funciones Unity //////////////////////
    /// </summary>
    private void Awake()
    {
        // Inicializamos las estadísticas del enemigo desde el ScriptableObject
        currentHealth = estadisticas.health;
        damage = estadisticas.damage;
        defense = estadisticas.defense;
        speed = estadisticas.speed;

        // Obtenemos el componente del Agente
        agente = GetComponent<NavMeshAgent>();
        currentSpeed = speed; // Inicialmente la velocidad actual es la base
        
        // Buscar renderer en este objeto o en hijos
        rend = GetComponent<Renderer>();
        if (rend == null) rend = GetComponentInChildren<Renderer>();
        
        if (rend && rend.material) 
        {
            originalColor = rend.material.color;
        }
    }

    [Header("Enjambre Behaviour")]
    public GameObject minionToSpawn;
    public int minionCount = 0;

    // Start se llama antes del primer frame
    void Start()
    {
        if (minionToSpawn != null && minionCount > 0)
        {
            for (int i = 0; i < minionCount; i++)
            {
                // Spawnear alrededor ligeramente dispersos
                Vector3 randomOffset = Random.insideUnitSphere * 2f;
                randomOffset.y = 0; 
                Instantiate(minionToSpawn, transform.position + randomOffset, Quaternion.identity);
            }
            // Después de spawnear, el Enjambre actúa como un enemigo normal (débil)
        }

        if (estadisticas == null) return;

        // Encuentra al jugador en la escena
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) objetivo = player.transform;
        
        // CONFIGURAMOS AL AGENTE si existe
        if (agente != null && estadisticas != null)
        {
            // Solo usar NavMesh si el agente está en un NavMesh válido
            if (agente.isOnNavMesh)
            {
                agente.speed = currentSpeed;
            }
            else
            {
                // Si no hay NavMesh, desactivar el agente y usar movimiento simple
                agente.enabled = false;
            }
        }
    }

    // Update se llama una vez por frame
    void Update()
    {
        if (isDead) return;

        // Solo si tenemos objetivo
        if (objetivo != null)
        {
            // Intentar usar NavMesh si está disponible
            if (agente != null && agente.enabled && agente.isOnNavMesh)
            {
                agente.SetDestination(objetivo.position);
            }
            else
            {
                // Movimiento simple sin NavMesh
                Vector3 direction = (objetivo.position - transform.position).normalized;
                transform.position += direction * currentSpeed * Time.deltaTime;
                
                // Rotar hacia el objetivo
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
            }
        }        
    }

    /////////////////////////////////////////////////// Funciones Propias ////////////////////
    public void RecibirDano(int cantidadDano)
    {
        currentHealth -= cantidadDano;
        
        StartCoroutine(FlashRoutine());

        // Comprobamos si el enemigo ha muerto
        if (currentHealth <= 0)
        {
            Morir();
        }
    }
    
    private System.Collections.IEnumerator FlashRoutine()
    {
        if (rend && rend.material)
        {
            rend.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            rend.material.color = originalColor;
        }
    }

    private bool isDead = false;

    private void Morir()
    {
        if (isDead) return;
        isDead = true;

        // Desactivar collider y movimiento inmediatamente
        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;
        if (agente) agente.enabled = false;

        // Lógica experiencia
        // 60% Verde, 30% Azul, 10% Dorado
        float roll = Random.Range(0f, 100f);
        GameObject prefabAInstanciar = orbVerdePrefab; // Por defecto verde

        if (roll < 60f) 
        {
           prefabAInstanciar = orbVerdePrefab;
        }
        else if (roll < 90f) 
        {
           prefabAInstanciar = orbAzulPrefab;
        }
        else 
        {
           prefabAInstanciar = orbOroPrefab;
        }

        if (prefabAInstanciar != null)
        {
            Instantiate(prefabAInstanciar, transform.position, Quaternion.identity);
        }
        
        // Retrasar destrucción para permitir que termine el Flash/Feedback
        Destroy(this.gameObject, 0.2f);
    }

    public void ApplySlow(float slowCurrent)
    {
        currentSpeed = speed * slowCurrent;
        if (agente != null) agente.speed = currentSpeed;
    }

    public void RemoveSlow()
    {
        currentSpeed = speed;
        if (agente != null) agente.speed = currentSpeed;
    }

    // LÓGICA DE ATAQUE AL JUGADOR
    private float tiempoSiguienteAtaque = 0f;
    private float intervaloAtaque = 0.5f;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AtacarJugador(collision.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AtacarJugador(other.gameObject);
        }
    }

    private void AtacarJugador(GameObject jugador)
    {
        // Solo atacamos cada X tiempo
        if (Time.time >= tiempoSiguienteAtaque)
        {
            PlayerStats stats = jugador.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.RecibirDmg(damage);
                tiempoSiguienteAtaque = Time.time + intervaloAtaque;
            }
        }
    }
}
