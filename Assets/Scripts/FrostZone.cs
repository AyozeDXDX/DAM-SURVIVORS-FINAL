using UnityEngine;
using System.Collections.Generic;

public class FrostZone : MonoBehaviour
{
    [Header("Configuración")]
    public float damageInterval = 0.5f;
    public int damagePerTick = 1;
    public float slowAmount = 0.7f; 
    public float activeDuration = 3f;
    public float cooldownDuration = 3f;
    
    [Header("Nivel")]
    public int level = 1;

    private List<EnemyController> enemiesInRange = new List<EnemyController>();
    private float damageTimer;
    
    private bool isActive = true;
    private float cycleTimer;
    private Collider zoneCollider;
    public Renderer zoneVisual;

    private PlayerLevel playerLevelScript;

    void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
           transform.SetParent(player);
           transform.localPosition = Vector3.zero;
        }
        zoneCollider = GetComponent<Collider>();
        zoneVisual = GetComponent<Renderer>();
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        
        playerLevelScript = FindFirstObjectByType<PlayerLevel>();
    }

    void Update()
    {
        // Polling
        if (playerLevelScript && playerLevelScript.currentLevel > level)
        {
            LevelUp();
        }

        HandleCycle();

        if (isActive)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                ApplyDamage();
                damageTimer = 0f;
            }
            enemiesInRange.RemoveAll(x => x == null);
        }
    }

    void LateUpdate()
    {
        // Cancelar rotación del padre
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    void HandleCycle()
    {
        cycleTimer += Time.deltaTime;
        
        if (isActive && cycleTimer >= activeDuration)
        {
            isActive = false;
            cycleTimer = 0f;
            if (zoneCollider) zoneCollider.enabled = false;
            if (zoneVisual) zoneVisual.enabled = false;
            
            foreach (var enemy in enemiesInRange)
            {
                if (enemy != null) enemy.RemoveSlow();
            }
            enemiesInRange.Clear();
        }
        else if (!isActive && cycleTimer >= cooldownDuration)
        {
            isActive = true;
            cycleTimer = 0f;
            if (zoneCollider) zoneCollider.enabled = true;
            if (zoneVisual) zoneVisual.enabled = true;
        }
    }

    private void ApplyDamage()
    {
        foreach (var enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                enemy.RecibirDano(damagePerTick);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
            enemy.ApplySlow(slowAmount);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null && enemiesInRange.Contains(enemy))
        {
            enemy.RemoveSlow();
            enemiesInRange.Remove(enemy);
        }
    }

    private void OnDestroy()
    {
        // Nada
    }

    private void OnDisable()
    {
         foreach (var enemy in enemiesInRange)
        {
            if (enemy != null) enemy.RemoveSlow();
        }
    }
    
    public void LevelUp()
    {
        level++;
        activeDuration += 1.0f; 
        
        if (slowAmount > 0.2f)
            slowAmount -= 0.1f;
            
        if (cooldownDuration > 0.5f)
            cooldownDuration -= 0.2f;
            
        damagePerTick = Mathf.RoundToInt(damagePerTick * 1.2f);

        if (isActive)
        {
            foreach (var enemy in enemiesInRange)
            {
               if (enemy != null) enemy.ApplySlow(slowAmount);
            }
        }
        

    }
}
