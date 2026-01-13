using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    private bool puedeMoverse = true;
    private float velocidadMovimiento = 5f;
    private Vector2 direccionPlana;
    private float velocidadRotacion = 10f; 

    public Controles control;
    private void Awake()
    {
        control = new Controles();
    }

    private void OnEnable()
    {
        control.Enable();
    }
    
    private void OnDisable()
    {
        control.Disable();
    }
    void Start()
    {

    }

    void Update()
    {
     if (puedeMoverse)
        {
            if (control == null || control.Player.Move == null) return;

            direccionPlana = control.Player.Move.ReadValue<Vector2>();

            Vector3 direccionMovimiento = new Vector3(direccionPlana.x, 0f, direccionPlana.y);
            direccionMovimiento.Normalize();

            transform.position += direccionMovimiento * velocidadMovimiento * Time.deltaTime;

            if (direccionMovimiento != Vector3.zero)
            {
                Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
            }
        }
    }
    
}
