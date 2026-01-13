using UnityEngine;

public class OrbitalShield : MonoBehaviour
{
    [Header("Configuración del Escudo")]
    public GameObject orbPrefab;
    public int orbCount = 3;
    public float rotationSpeed = 100f;
    public float distance = 3f;
    public int damage = 10;
    
    [Header("Nivel")]
    public int level = 1;

    private GameObject[] orbs;
    private Transform playerTransform;
    private float currentAngle = 0f;

    // Polling
    private PlayerLevel playerLevelScript;

    void Start()
    {
        playerTransform = transform;
        SpawnOrbs();
        
        playerLevelScript = FindFirstObjectByType<PlayerLevel>();
    }

    void OnDestroy()
    {
        // Nada
    }

    void Update()
    {
        if (playerLevelScript && playerLevelScript.currentLevel > level)
        {
            LevelUp();
        }

        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;

        if (orbs != null && orbs.Length != orbCount)
        {
            SpawnOrbs();
        }

        if (orbs != null && playerTransform != null)
        {
            float angleStep = 360f / orbCount;
            for (int i = 0; i < orbs.Length; i++)
            {
                if (orbs[i] != null)
                {
                    float angle = currentAngle + (angleStep * i);
                    
                    Quaternion rotation = Quaternion.Euler(0, angle, 0);
                    Vector3 direction = rotation * Vector3.forward;
                    Vector3 targetPos = playerTransform.position + (direction * distance);
                    
                    orbs[i].transform.position = targetPos;
                }
            }
        }
    }

    void OnValidate()
    {
        if (Application.isPlaying && orbs != null && orbs.Length != orbCount)
        {
            SpawnOrbs();
        }
    }

    public void SpawnOrbs()
    {
        if (orbs != null)
        {
            foreach(var o in orbs) if(o) Destroy(o);
        }

        orbs = new GameObject[orbCount];
        for (int i = 0; i < orbCount; i++)
        {
            if (orbPrefab)
            {
                GameObject newOrb = Instantiate(orbPrefab, transform);
                
                if (newOrb.GetComponent<Rigidbody>() == null)
                {
                    Rigidbody rb = newOrb.AddComponent<Rigidbody>();
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                OrbitalProjectile op = newOrb.GetComponent<OrbitalProjectile>();
                if (op == null) op = newOrb.AddComponent<OrbitalProjectile>();
                op.damage = damage;
                orbs[i] = newOrb;
            }
            else
            {
                GameObject newOrb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                newOrb.transform.SetParent(transform);
                newOrb.transform.localScale = Vector3.one * 0.5f;
                Destroy(newOrb.GetComponent<Collider>());
                SphereCollider sc = newOrb.AddComponent<SphereCollider>();
                sc.isTrigger = true;
                
                Rigidbody rb = newOrb.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;

                OrbitalProjectile op = newOrb.AddComponent<OrbitalProjectile>();
                op.damage = damage;
                orbs[i] = newOrb;
            }
        }
    }
    
    public void LevelUp()
    {
        level++;
        rotationSpeed += 30f;
        
        if (level % 2 != 0) 
        {
            orbCount++;
            SpawnOrbs();
        }
        
        damage = Mathf.RoundToInt(damage * 1.2f); 
        
        if (level % 2 == 0 && orbs != null)
        {
            foreach(var o in orbs)
            {
                if(o)
                {
                    var op = o.GetComponent<OrbitalProjectile>();
                    if(op) op.damage = damage;
                }
            }
        }
        

    }
}
