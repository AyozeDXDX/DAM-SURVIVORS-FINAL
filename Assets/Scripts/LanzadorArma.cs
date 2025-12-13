using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;   

public class LanzadorArma : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////// VARIABLES ///////////////////////////////////////////

    [Header("Configuración del Arma")]
    [Tooltip("El Prefab del proyectil que vamos a disparar")]
    public GameObject proyectilPrefab;

    [Tooltip("Tiempo en segundos entre cada disparo")]
    public float tiempoDeAtaque = 3f;
    
    public Transform spawnPoint;

    ///////////////////////////////////////////////////////////////// FUNCIONES UNITY /////////////////////////////////////////

    void Start()
    {
        // En cuanto empieza el juego, iniciamos la rutina de ataque.
        // La coroutine se encargará de su propio bucle.
        if (proyectilPrefab != null)
        {
            StartCoroutine(LanzarAtaqueRutina());
        }
        else
        {
            Debug.LogError("¡No se ha asignado un Prefab de proyectil en el LanzadorArma!");
        }
    }

    /////////////////////////////////////////////////////////////// FUNCIONES PROPIAS /////////////////////////////////////////

    IEnumerator LanzarAtaqueRutina()
    {
        // Un bucle infinito y seguro, gracias a la Coroutine.
        while (true)
        {
            // 1. Instanciamos el proyectil en la posición y rotación del jugador
            // (El jugador está en el centro, así que usamos su transform.position)
            // Usamos transform.rotation para que el proyectil se oriente como el jugador.
            Instantiate(proyectilPrefab, spawnPoint.position, transform.rotation * proyectilPrefab.transform.rotation);
            
            // 2. Pausamos la coroutine
            // Espera el tiempo de ataque antes de volver al inicio del bucle.
            yield return new WaitForSeconds(tiempoDeAtaque);
        }
    }
}
