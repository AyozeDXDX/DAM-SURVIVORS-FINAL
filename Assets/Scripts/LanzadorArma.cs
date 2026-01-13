using UnityEngine;
using System.Collections;

public class LanzadorArma : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject proyectilPrefab;

    public float tiempoDeAtaque = 2f; 
    public int projectilesPerBurst = 1;
    
    [Header("Estado")]
    public int level = 1;

    private float timer;
    private float baseCooldown;

    private PlayerLevel playerLevelScript;

    void Start()
    {
        baseCooldown = tiempoDeAtaque;
        timer = tiempoDeAtaque; 
        
        if (proyectilPrefab == null) return;

        playerLevelScript = FindFirstObjectByType<PlayerLevel>();
    }

    void OnDestroy()
    {
        
    }

    void Update()
    {
        if (playerLevelScript && playerLevelScript.currentLevel > level)
        {
            LevelUp();
        }

        timer += Time.deltaTime;
        if (timer >= tiempoDeAtaque)
        {
            LanzarAtaque();
            timer = 0f;
        }
    }

    void RecalculateStats()
    {
        tiempoDeAtaque = Mathf.Max(0.2f, baseCooldown * Mathf.Pow(0.9f, level - 1));
    }

    void LanzarAtaque()
    {
        StartCoroutine(LanzarAtaqueRutina());
    }

    System.Collections.IEnumerator LanzarAtaqueRutina()
    {
        for (int i = 0; i < projectilesPerBurst; i++)
        {
            if (proyectilPrefab != null)
            {
                GameObject instance = Instantiate(proyectilPrefab, transform.position, Quaternion.identity);
                
                var hacha = instance.GetComponent<ProyectilHacha>();
                if (hacha != null)
                {
                    hacha.dano = Mathf.RoundToInt(hacha.dano * (1 + (level - 1) * 0.2f));
                }
                
                var slash = instance.GetComponent<SlashWarrior>();
                if (slash != null)
                {
                    slash.dano = Mathf.RoundToInt(slash.dano * (1 + (level - 1) * 0.2f));
                }
                
                var rayo = instance.GetComponent<RayoDeEnergia>();
                if (rayo != null)
                {
                    rayo.Inicializar(level);
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void LevelUp()
    {
        level++;
        if (level % 3 == 0) projectilesPerBurst++;
        RecalculateStats();
    }
}