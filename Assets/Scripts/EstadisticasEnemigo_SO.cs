using UnityEngine;

// Esta línea es la clave para poder crearlo desde el menú de Assets
[CreateAssetMenu(fileName = "NuevasEstadisticas", menuName = "Datos/Estadisticas Enemigo")]
public class EstadisticasEnemigo_SO : ScriptableObject
{
    [Header("Estadísticas Principales")]
    public int MaxHealth;
    public float Speed; // Usamos float para la velocidad, permite más precisión
    public int Damage;
    public int Defense;

    // Aquí podrías añadir más cosas en el futuro,
    // como radio de ataque, tipo de enemigo, etc.
}