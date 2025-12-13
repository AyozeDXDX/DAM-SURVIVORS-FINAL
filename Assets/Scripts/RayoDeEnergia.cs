using UnityEngine;
using System.Collections.Generic;

public class RayoDeEnergia : MonoBehaviour
{
    [Header("Configuración del Rayo")]
    [Tooltip("Tiempo total que el rayo permanece activo.")]
    public float duracionTotal = 3.0f;

    [Tooltip("Intervalo de tiempo entre cada tick de daño.")]
    public float intervaloDano = 0.25f;

    [Tooltip("Daño aplicado por tick.")]
    public int danoPorTick = 2;

    // Temporizador interno para controlar los ticks de daño
    private float temporizadorDano;

    // Lista para mantener referencia a los enemigos dentro del área del rayo
    private List<EnemyController> enemigosEnRango = new List<EnemyController>();

    [Tooltip("Radio de detección para buscar enemigos.")]
    public float radioDeteccion = 20f;

    private Transform playerTransform;

    void Start()
    {
        // Destruir el rayo después de su duración total
        Destroy(gameObject, duracionTotal);
        
        // Inicializar el temporizador
        temporizadorDano = 0f;

        // Buscar al jugador para seguirlo
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            // Hacemos que el rayo sea hijo del jugador
            transform.SetParent(playerTransform, true);
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            Transform target = GetNearestEnemy();
            
            if (target != null)
            {
                // Calcular dirección hacia el enemigo
                Vector3 direccion = (target.position - playerTransform.position).normalized;
                direccion.y = 0; // Mantener el rayo horizontal
                
                // Posicionar y rotar hacia el enemigo
                // Posición: Desde el jugador, extendiéndose en la dirección del enemigo
                transform.position = playerTransform.position + direccion * 5.5f;
                transform.position = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z); // Altura 0 relativa al jugador
                
                // Rotar hacia el enemigo + Offset de 90 grados en X para el Cilindro
                transform.rotation = Quaternion.LookRotation(direccion) * Quaternion.Euler(90, 0, 0);
            }
            else
            {
                // Comportamiento por defecto: Apuntar hacia adelante del jugador
                transform.localPosition = new Vector3(0, 0f, 5.5f); 
                // Offset de 90 grados en X para que el cilindro apunte hacia adelante
                transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
        }

        // Actualizar el temporizador
        temporizadorDano += Time.deltaTime;

        // Si ha pasado el intervalo de daño
        if (temporizadorDano >= intervaloDano)
        {
            AplicarDanoPeriodico();
            // Reiniciar el temporizador (restando el intervalo para mantener precisión)
            temporizadorDano = 0f; 
        }
        
        // Limpiar la lista de enemigos nulos (por si alguno murió y se destruyó)
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
            // Opcional: Aplicar daño inmediato al entrar? 
            // El requisito dice "aplica Daño por Tiempo", así que esperaremos al tick.
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
    private Transform GetNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(playerTransform.position, radioDeteccion);
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.GetComponent<EnemyController>())
            {
                Vector3 directionToTarget = hit.transform.position - playerTransform.position;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = hit.transform;
                }
            }
        }
        return bestTarget;
    }
}
