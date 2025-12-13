using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int cantidadExp = 10;

    public void Recoger()
    {
        // Destruir el orbe
        Destroy(gameObject);
    }
}
