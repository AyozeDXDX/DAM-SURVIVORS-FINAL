using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int currentHealth;
    private int maxHealth = 100;
    private int defensa = 0;
    
    private bool estaVivo;

    public PlayerHUD hud;

    private Renderer rend;
    private Color originalColor;

    private void Awake() 
    {
        currentHealth = maxHealth;
        estaVivo = true;
        
        rend = GetComponentInChildren<Renderer>(); 
        if (rend) originalColor = rend.material.color;
    }
    void Start()
    {
        // Buscar el HUD si no está asignado
        if (hud == null) hud = FindFirstObjectByType<PlayerHUD>();

        // Actualizar UI al inicio
        if (hud != null) hud.UpdateHealth(currentHealth, maxHealth);
    }

    // Update se llama una vez por frame
    void Update()
    {

    }
    

    //////////////////////////////// Funciones propias /////////////////////////
    
    public void RecibirDmg(int dmg)
    {
        if (!estaVivo) return;

        // Solo recibe daño si el daño es mayor que la defensa.
        if (dmg > defensa)
        {
            //Le quitamos el daño menos la defensa
            currentHealth -= dmg - defensa;
            
            if (hud != null) hud.UpdateHealth(currentHealth, maxHealth);
            
            // Sacudir la cámara
            if (CameraShake.Instance != null)
                CameraShake.Instance.Shake(0.2f, 0.3f);
            
            // Flash Blanco
            StartCoroutine(FlashRoutine());

            //Si la vida es menor que 0 lo matamos
            if (currentHealth <= 0)
            {
                estaVivo = false;
                if (GameManager.Instance != null) GameManager.Instance.Defeat();
            }
        }
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        if (rend)
        {
            rend.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            rend.material.color = originalColor;
        }
    }
}
