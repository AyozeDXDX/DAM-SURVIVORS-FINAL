using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;
    public float velocidadSuavizado = 5f;

    private Vector3 offset;
    public Controles control;

    [Header("Zoom")]
    private float zoom = 1f;
    public float sensibilidadZoom = 0.1f;
    public float zoomMinimo = 0.5f;
    public float zoomMaximo = 2f;


    void Awake()
    {
        control = new Controles();
    }

    void OnEnable()
    {
        control.Enable();
    }

    void OnDisable()
    {
        control.Disable();
    }

    private Quaternion rotacionInicial;

    void Start()
    {
        // Comprobación de seguridad
        if (objetivo == null)
        {
            return;
        }

        offset = transform.position - objetivo.position;
    }

    void Update()
    {
        SetZoom();
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        Vector3 posicionDeseada = objetivo.position + offset * zoom;

        // Mover la cámara suavemente.        
        Vector3 shakeOffset = Vector3.zero;
        if (CameraShake.Instance != null) shakeOffset = CameraShake.Instance.currentShakeOffset;

        transform.position = Vector3.Lerp(transform.position, posicionDeseada, velocidadSuavizado * Time.deltaTime) + shakeOffset;
    }
    
    
    public void SetZoom()
    {
       if (control == null || control.Camara.Zoom == null) return;
       float valorRueda = control.Camara.Zoom.ReadValue<float>();
       zoom -= valorRueda * sensibilidadZoom;
       zoom = Mathf.Clamp(zoom, zoomMinimo, zoomMaximo);
    }
}
