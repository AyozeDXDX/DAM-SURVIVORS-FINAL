using UnityEngine;
using System.Linq;

public class MagicWand : MonoBehaviour
{
    [Header("Configuración Varita")]
    public GameObject projectilePrefab;
    public float fireRate = 1.0f;
    public int projectileCount = 2; 
    public float range = 15f;
    public float spawnDelay = 0.2f; 
    
    [Header("Información de Nivel")]
    public int level = 1;
    private int previousLevel = 1;
    
    // Estadísticas Base
    private float baseFireRate;
    private int baseProjectileCount;

    private float fireTimer;

    void Awake()
    {
        baseFireRate = fireRate;
        baseProjectileCount = projectileCount;
    }

    private PlayerLevel playerLevelScript;

    void Start()
    {
        playerLevelScript = FindFirstObjectByType<PlayerLevel>();
        if (playerLevelScript) previousLevel = playerLevelScript.currentLevel;
    }

    void OnDestroy()
    {
        // Nada que limpiar
    }

    void Update()
    {
        // Comprobar nivel manualmente cada frame
        if (playerLevelScript && playerLevelScript.currentLevel > level)
        {
            LevelUp();
        }
    
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            StartCoroutine(FireSequence());
        }
    }
    
    void RecalculateStats()
    {
        projectileCount = baseProjectileCount + (level - 1);
        
        float cdDiv = 1.0f + ((level - 1) * 0.1f);
        fireRate = baseFireRate / cdDiv;
        

    }

    System.Collections.IEnumerator FireSequence()
    {
        Transform[] targets = GetNearestEnemies(projectileCount);

        foreach (Transform t in targets)
        {
            if (t != null)
            {
                SpawnProjectile(t);
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }

    void SpawnProjectile(Transform target)
    {
        if (projectilePrefab != null)
        {
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            MagicProjectile mp = proj.GetComponent<MagicProjectile>();
            if (mp == null) mp = proj.AddComponent<MagicProjectile>();
            
            mp.damage = Mathf.RoundToInt(mp.damage * level);
            
            mp.SetTarget(target);
        }
    }

    Transform[] GetNearestEnemies(int count)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        
        return hits
            .Where(h => h.GetComponent<EnemyController>() != null)
            .Select(h => h.transform)
            .OrderBy(t => Vector3.SqrMagnitude(t.position - transform.position))
            .Take(count)
            .ToArray();
    }
    
    public void LevelUp()
    {
        level++;
        RecalculateStats();
        previousLevel = level;
    }
}
