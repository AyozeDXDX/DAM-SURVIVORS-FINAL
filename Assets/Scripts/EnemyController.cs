using UnityEngine;
using UnityEngine.AI; // ¡Necesario para usar NavMeshAgent!

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

    public GameObject ExpOrbPrefab;

    // NUEVA VARIABLE
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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Se asegura de que las estadísticas estén asignadas
        if (estadisticas == null)
        {
            Debug.LogError("¡Error! No se han asignado las estadísticas del enemigo en " + this.gameObject.name);
        }

        // Encuentra al jugador en la escena
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) objetivo = player.transform;
        
        // CONFIGURAMOS AL AGENTE CON NUESTROS DATOS
        if (agente != null && estadisticas != null)
        {
            agente.speed = estadisticas.speed; // ¡El agente usará la velocidad de la ficha!
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Solo si tenemos objetivo y agente
        if (objetivo != null && agente != null)
        {
            // Le decimos al Agente: "Ve a donde está el jugador ahora mismo"
            if (agente.isOnNavMesh)
            {
                agente.SetDestination(objetivo.position);
            }
            else
            {
                // Fallback: Movimiento simple si el agente no está en el NavMesh
                Vector3 direction = (objetivo.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
        else if (objetivo != null)
        {
            // Fallback: Si no hay componente NavMeshAgent
            Vector3 direction = (objetivo.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        
        // ¡BORRA TODO EL CÓDIGO VIEJO DE MOVIMIENTO MANUAL!
        // (Vector3 direccion = ... transform.position += ...)
    }

    /////////////////////////////////////////////////// Funciones Propias ////////////////////
    public void RecibirDano(int cantidadDano)
    {
        Debug.Log(gameObject.name + " RECIBIÓ DAÑO: " + cantidadDano + ". Vida actual: " + currentHealth);
        
        // (En el futuro, aquí podríamos usar 'estadisticas.defensa' para reducir el daño)
        currentHealth -= cantidadDano;

        // Comprobamos si el enemigo ha muerto
        if (currentHealth <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        if (ExpOrbPrefab != null)
        {
            // Creamos el orbe en la misma posición donde está el enemigo ahora mismo.
            // Quaternion.identity porque los orbes no necesitan rotación especial.
            Instantiate(ExpOrbPrefab, transform.position, Quaternion.identity);
        }

        // De momento, solo avisamos y destruimos el objeto
        Debug.Log("¡Enemigo " + this.gameObject.name + " derrotado!");
        Destroy(this.gameObject);
    }
}
