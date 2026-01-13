using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("UI References")]
    public Slider healthBar;
    public Image healthFill;
    public TextMeshProUGUI levelText;
    public Slider xpBar;
    
    [Header("Player References")]
    public PlayerStats playerStats;
    public PlayerLevel playerLevel;

    void Start()
    {
        if (healthBar) healthBar.interactable = false;
        if (xpBar) xpBar.interactable = false;
    }
    
    public void UpdateHealth(int current, int max)
    {
        if (healthBar)
        {
            healthBar.maxValue = max;
            healthBar.value = current;
        }
    }

    public void UpdateExp(int current, int max)
    {
        if (xpBar)
        {
            xpBar.maxValue = max;
            xpBar.value = current;
        }
    }

    public void UpdateLevel(int level)
    {
        if (levelText)
        {
            levelText.text = "Lvl " + level;
        }
    }
}
