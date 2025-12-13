using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////// VARIABLES ///////////////////////////////////////////

    [Header("Estado del Jugador")]
    public int currentLevel = 1;
    public int currentExperience = 0;
    public int experienceToNextLevel = 100;

    ///////////////////////////////////////////////////////////////// FUNCIONES UNITY /////////////////////////////////////////

     void Start()
    {
        // (Opcional) Inicializar UI aquí en el futuro
    }

    // ESTA ES LA NUEVA FUNCIÓN MÁGICA
    // Se ejecuta automáticamente cuando un objeto entra en nuestro Sphere Collider (Trigger)
    private void OnTriggerEnter(Collider other)
    {
        // 1. ¿Es lo que hemos tocado un Orbe de Experiencia?
        // Usamos el Tag que configuramos en la Parte 1.
        if (other.CompareTag("OrbeEXP"))
        {
            // 2. Intentamos leer el script del orbe para saber cuánta exp da
            ExpOrb orbeScript = other.GetComponent<ExpOrb>();

            if (orbeScript != null)
            {
                // 3. Sumamos la experiencia
                GainExperience(orbeScript.cantidadExp);

                // 4. Destruimos el orbe (llamando a su función Recoger)
                orbeScript.Recoger();
            }
        }
    }

    /////////////////////////////////////////////////////////////// FUNCIONES PROPIAS /////////////////////////////////////////

    // Esta función será llamada cuando recojamos un orbe
    public void GainExperience(int amount)
    {
        currentExperience += amount;
        Debug.Log("Experiencia ganada: " + amount + ". Total: " + currentExperience + "/" + experienceToNextLevel);

        // Comprobamos si hemos alcanzado el umbral para subir de nivel
        if (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        
        // La experiencia sobrante se mantiene para el siguiente nivel
        currentExperience -= experienceToNextLevel;

        // Aumentamos la dificultad para el siguiente nivel (ej. necesitas un 20% más de exp)
        // Usamos Mathf.RoundToInt para mantener números enteros limpios.
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.2f);

        Debug.Log("<color=yellow>¡SUBIDA DE NIVEL! Nivel actual: " + currentLevel + "</color>");
        
        // (Aquí en el futuro pausaremos el juego y mostraremos el menú de mejoras)
    }
}