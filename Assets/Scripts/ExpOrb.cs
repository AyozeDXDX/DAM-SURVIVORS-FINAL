using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int cantidadExp = 10;
    
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void Configurar(int exp, Color color)
    {
        cantidadExp = exp;
        if (rend != null)
        {
            rend.material.color = color;
        }
    }

    public void Recoger()
    {
        Destroy(gameObject);
    }
}
