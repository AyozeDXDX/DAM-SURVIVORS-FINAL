using UnityEngine;

[CreateAssetMenu(fileName = "OleadaNueva", menuName="Oleadas")]
public class DataOleada : ScriptableObject
{
    [Header("Enemigos")]
    public GameObject EnemyPrefab;

    [Header("Configuracion Oleada")]
    [Tooltip("Tiempo entre spawn de enemigos en segundos")]
    public float SpawnRate;
    public float TiempoEntreOleadas;
    public int CantidadDeEnemigos;
    
}
