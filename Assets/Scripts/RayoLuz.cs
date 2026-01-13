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

    private IEnumerator DamageLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(dotTime);

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
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemiesInRay.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemiesInRay.Remove(enemy);
        }
    }
}