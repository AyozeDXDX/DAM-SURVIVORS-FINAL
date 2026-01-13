using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class WaveConfig
    {
        public string waveName;
        public float startTime; // In seconds
        public GameObject[] enemyPrefabs; // Zangano, Corredor, Tanque
        public int count;
        public float spawnRate;
    }

    [Header("Configuración de Oleadas")]
    public GameObject zanganoPrefab;
    public GameObject corredorPrefab;
    public GameObject tanquePrefab;
    public GameObject enjambrePrefab;
    
    public Transform player;
    public float spawnRadius = 15f;
    
    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
        StartCoroutine(GameLoop());
    }
    
    IEnumerator GameLoop()
    {
        // Oleada 1: 12 Zánganos, 1 cada 5s
        StartCoroutine(SpawnRoutine(zanganoPrefab, 12, 5f));
        yield return new WaitForSeconds(60f); 

        // Oleada 2: 24 Zánganos, 2 cada 5s
        StartCoroutine(SpawnRoutine(zanganoPrefab, 12, 5f));
        StartCoroutine(SpawnRoutine(zanganoPrefab, 12, 5f));
        yield return new WaitForSeconds(60f);

        // Oleada 3: 60 Zánganos, 4 cada 4s
        StartCoroutine(SpawnBurstRoutine(zanganoPrefab, 60, 4, 4f));
        yield return new WaitForSeconds(60f);

        // Oleada 4: Zánganos + Corredores
        StartCoroutine(SpawnRoutine(zanganoPrefab, 60, 2f)); 
        StartCoroutine(SpawnRoutine(corredorPrefab, 12, 10f));
        yield return new WaitForSeconds(120f);

        // Oleada 5: Corredores + Enjambre
        SpawnEnjambre(1);
        StartCoroutine(SpawnRoutine(corredorPrefab, 24, 5f));
        yield return new WaitForSeconds(120f); 

        // Oleada 6: Corredores + Tanques
        StartCoroutine(SpawnRoutine(corredorPrefab, 40, 3f)); 
        StartCoroutine(SpawnRoutine(tanquePrefab, 24, 5f));
        yield return new WaitForSeconds(120f); 

        // Oleada 7: Lluvia de Tanques
        StartCoroutine(SpawnBurstRoutine(tanquePrefab, 80, 4, 3f));
        yield return new WaitForSeconds(60f); 


        // FIN
        if (GameManager.Instance) GameManager.Instance.Victory();
    }
    
    void SpawnEnjambre(int count)
    {
        if (enjambrePrefab == null) return;
        
        for(int i=0; i<count; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPosition = player.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
            GameObject enjambre = Instantiate(enjambrePrefab, spawnPosition, Quaternion.identity);
            
            EnemyController ec = enjambre.GetComponent<EnemyController>();
            if (ec != null)
            {
                ec.minionToSpawn = zanganoPrefab;
                ec.minionCount = 10;
            }
        }
    }

    IEnumerator SpawnRoutine(GameObject prefab, int total, float rate)
    {
        if (prefab == null) yield break;
        for (int i = 0; i < total; i++)
        {
            SpawnEnemy(prefab);
            yield return new WaitForSeconds(rate);
        }
    }

    IEnumerator SpawnBurstRoutine(GameObject prefab, int total, int burstSize, float rate)
    {
        if (prefab == null) yield break;
        int spawned = 0;
        while (spawned < total)
        {
            for (int k = 0; k < burstSize && spawned < total; k++)
            {
               SpawnEnemy(prefab);
               spawned++;
            }
            yield return new WaitForSeconds(rate);
        }
    }
    
    void SpawnEnemy(GameObject prefab)
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = player.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
    
    void InstantiateEnemies(GameObject prefab, int count)
    {
        for(int i=0; i<count; i++) SpawnEnemy(prefab);
    }
    

}
