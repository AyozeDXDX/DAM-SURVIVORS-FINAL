using UnityEngine;

public class SlashWarrior : MonoBehaviour
{
    public int dano = 10;
    public float tiempoDeVida = 0.2f;

    private Transform playerTransform;

    void Start()
    {
        // 1. Programar la autodestrucción
        Destroy(gameObject, tiempoDeVida);

        // Buscar al jugador para seguirlo
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            transform.SetParent(playerTransform, true);
            
            Vector3 frenteJugador = player.transform.forward;
            transform.localPosition = new Vector3(0, 1f, 1.5f);
            transform.localRotation = Quaternion.identity;
        }
        
        PlayerLevel pl = FindFirstObjectByType<PlayerLevel>();
        if (pl != null)
        {
            float scaleMod = 1f + (pl.currentLevel - 1) * 0.25f;
            transform.localScale = new Vector3(scaleMod, 1f, scaleMod); 
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.RecibirDano(dano);
        }
    }


    public void LevelUp()
    {
        // El nivel lo controla el lanzador
    }
}