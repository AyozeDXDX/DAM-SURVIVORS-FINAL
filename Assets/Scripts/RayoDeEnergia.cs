
using UnityEngine;
using System.Collections.Generic;

public class RayoDeEnergia : MonoBehaviour
{
    [Header("Configuración")]
    public float duracionTotal = 3.0f;
    public float intervaloDano = 0.25f;
    public int danoPorTick = 2;
    public int level = 1;

    private float temporizadorDano;
    private List<EnemyController> enemigosEnRango = new List<EnemyController>();

    [Header("Visuales")]
    public Vector3 offsetPos = new Vector3(0, 0f, 6f);
    public Vector3 offsetRot = new Vector3(90f, 0f, 0f);

    private Transform playerTransform;
    private Vector3 escalaOriginal;

    void Awake()
    {
        if (transform.localScale.x < 0.1f) transform.localScale = Vector3.one;
        escalaOriginal = transform.localScale;
    }

    void Start()
    {
        Destroy(gameObject, duracionTotal);        
        temporizadorDano = 0f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            transform.SetParent(playerTransform);
            
            transform.localPosition = offsetPos; 
            transform.localRotation = Quaternion.Euler(offsetRot);
            
            RecalcularStats();
        }
    }
    
    public void Inicializar(int nivelActual)
    {
        level = nivelActual;
        RecalcularStats();
    }

    void RecalcularStats()
    {
        // Intervalo minimo 0.05s
        intervaloDano = Mathf.Max(0.05f, 0.25f - ((level - 1) * 0.02f));

        // Daño base * 1.2^nivel
        danoPorTick = Mathf.RoundToInt(2 * Mathf.Pow(1.2f, level - 1));

        // Ancho base * (3 + 0.5 * nivel)
        float multiplicadorAncho = 3f + ((level - 1) * 0.5f);
        
        if (escalaOriginal == Vector3.zero) escalaOriginal = Vector3.one;

        transform.localScale = new Vector3(
            escalaOriginal.x * multiplicadorAncho, 
            escalaOriginal.y,
            escalaOriginal.z * multiplicadorAncho
        );
    }

    void OnDestroy()
    {
        
    }

    void OnPlayerLevelChanged(int newLevel)
    {
        if (newLevel > level)
        {
            level = newLevel;
            RecalcularStats();
        }
    }

    void Update()
    {
        temporizadorDano += Time.deltaTime;

        if (temporizadorDano >= intervaloDano)
        {
            AplicarDanoPeriodico();
            temporizadorDano = 0f; 
        }
        
        enemigosEnRango.RemoveAll(item => item == null);
    }

    private void AplicarDanoPeriodico()
    {
        foreach (EnemyController enemigo in enemigosEnRango)
        {
            if (enemigo != null)
            {
                enemigo.RecibirDano(danoPorTick);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemigo = other.GetComponent<EnemyController>();
        if (enemigo != null && !enemigosEnRango.Contains(enemigo))
        {
            enemigosEnRango.Add(enemigo);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyController enemigo = other.GetComponent<EnemyController>();
        if (enemigo != null && enemigosEnRango.Contains(enemigo))
        {
            enemigosEnRango.Remove(enemigo);
        }
    }

    public void LevelUp()
    {
        level++;
        
        // Reducir intervalo de daño
        if (intervaloDano > 0.1f)
            intervaloDano -= 0.05f;
        RecalcularStats();
    }
}
