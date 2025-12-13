using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayoLuz : MonoBehaviour
{
    public int damage = 2;
    public float spawnLife = 3f;
    public float dotTime = 0.25f;

    private List<EnemyController> enemiesInRay = new List<EnemyController>();

    void Start()
    {
        Destroy(gameObject, spawnLife);
        StartCoroutine(DamageLoop());
    }

    // Esta es la corrutina PRINCIPAL. Solo hay UNA.
    private IEnumerator DamageLoop()
    {
        // Bucle infinito que se ejecuta mientras el rayo exista
        while (true)
        {
            // 1. Esperar el intervalo (ej. 0.25 segundos)
            yield return new WaitForSeconds(dotTime);

            // 2. Después de esperar, hacer daño a TODOS en la lista
            // Usamos un bucle 'for' al revés. Esto es una
            // técnica estándar para poder ELIMINAR de una lista
            // de forma segura mientras la recorremos.
            foreach (var enemy in enemiesInRay)
            {
                if (enemy != null)
                {
                    enemy.RecibirDano(damage);
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto que entra tiene un EnemyController...
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            // ...lo añadimos a la lista de enemigos en el rayo.
            enemiesInRay.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si el objeto que sale tiene un EnemyController...
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            // ...lo quitamos de la lista de enemigos en el rayo.
            enemiesInRay.Remove(enemy);
        }
    }
}