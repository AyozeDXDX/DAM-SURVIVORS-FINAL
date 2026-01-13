using UnityEngine;
using System.Collections.Generic;

public class PlayerLevel : MonoBehaviour
{
    [Header("Configuración de Nivel")]
    public int currentLevel = 1;
    public int currentExperience = 0;
    public int experienceToNextLevel = 100;
    public float levelMultiplier = 1.2f; 

    // Referencias directas
    public PlayerHUD hud;

    private void Start()
    {
        if (hud == null) hud = FindFirstObjectByType<PlayerHUD>();
        NotifyUI();
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        
        CheckLevelUp();
        NotifyUI();
    }

    private void CheckLevelUp()
    {
        while (currentExperience >= experienceToNextLevel)
        {
            currentExperience -= experienceToNextLevel;
            currentLevel++;
            experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * levelMultiplier);
            
            // Ya no lanzamos eventos, el DebugLevel tendrá que comprobar si el nivel cambió
            if (hud != null) hud.UpdateLevel(currentLevel);
        }
    }
    
    private void NotifyUI()
    {
        if (hud != null) hud.UpdateExp(currentExperience, experienceToNextLevel);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detección directa de Orb
        if (other.name.Contains("Orbe") || other.name.Contains("Exp"))
        {
            ExpOrb orb = other.GetComponent<ExpOrb>();
            if (orb != null)
            {
                AddExperience(orb.cantidadExp);
                Destroy(other.gameObject);
            }
        }
    }
}