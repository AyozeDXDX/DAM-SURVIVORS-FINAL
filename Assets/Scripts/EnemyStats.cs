using UnityEngine;


[CreateAssetMenu(fileName = "EnemyStats", menuName = "Stats/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int health;
    public int damage;
    public int defense;
    public float speed;
   
    
}
