using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// ///////////////////// Variables /////////////////////////
    /// </summary>
    [Header("Configuración de Spawn")]
    [SerializeField]
    private float spawnRadius = 10f;

    [Header("GameObjects")]
    [SerializeField]
    private Transform player;

    [SerializeField]
    private List<DataOleada> oleadas;


    
    ////////////////////////////////////// Funciones Unity//////////////////
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GenerarOleadas());
    }

    // Update is called once per frame
    void Update()
    {

    }

    ///////////////////////////// Funciones Propias ////////////////////

     private IEnumerator spawn(DataOleada oleada)
    {
        // ¡CAMBIO 1: Hemos cambiado 'while(true)' por un bucle 'for'!
        // Ahora el bucle respeta la 'CantidadDeEnemigos' que has definido en el Scriptable Object.
        for (int i = 0; i < oleada.CantidadDeEnemigos; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = player.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
            Instantiate(oleada.EnemyPrefab, spawnPosition, Quaternion.identity);
            
            // Esta pausa es perfecta.
            yield return new WaitForSeconds(oleada.SpawnRate);
        }
        // Cuando el bucle 'for' termina, la coroutine finaliza por sí misma.
        // ¡Ya no es infinita!
    }
    
    
    public IEnumerator GenerarOleadas()
    {
        foreach(DataOleada oleadaActual in oleadas)
        {
            yield return new WaitForSeconds(oleadaActual.TiempoEntreOleadas);
            StartCoroutine(spawn(oleadaActual));
        }
    }
}
