using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DebugLevel : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public GameObject magicProjectilePrefab;
    public GameObject orbitalOrbPrefab;
    public GameObject lanzadorHachaPrefab;
    public GameObject lanzadorSlashPrefab;
    public GameObject rayoPrefab;          
    
    // Lista de armas activas
    private List<MonoBehaviour> misArmas = new List<MonoBehaviour>();

    // Input System
    private Controles controles;

    private void Awake()
    {
        controles = new Controles();
    }

    private void OnEnable()
    {
        controles.Enable();
    }

    private void OnDisable()
    {
        controles.Disable();
    }

    // Para saber si el nivel ha cambiado (estilo novato)
    private int ultimoNivel;
    private PlayerLevel playerLevelScript;

    void Start()
    {
        playerLevelScript = FindFirstObjectByType<PlayerLevel>();
        if (playerLevelScript) ultimoNivel = playerLevelScript.currentLevel;
    }
    
    void Update()
    {
        // Teclas de debug
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.digit1Key.wasPressedThisFrame || keyboard.numpad1Key.wasPressedThisFrame)
            {
                AnadirArma();
            }
            
            if (keyboard.digit2Key.wasPressedThisFrame || keyboard.numpad2Key.wasPressedThisFrame)
            {
                SubirNivelArma();
            }
        }

        // Comprobar constantemente si el nivel subió
        if (playerLevelScript != null)
        {
            if (playerLevelScript.currentLevel > ultimoNivel)
            {
                ultimoNivel = playerLevelScript.currentLevel;
                SubirNivelArma(); // Subimos arma automáticamente al subir nivel
            }
        }
    }
    
    void AnadirArma()
    {
        // 1. FrostZone
        bool tieneFrost = false;
        foreach(var a in misArmas) { if(a is FrostZone) tieneFrost = true; }

        if (!tieneFrost)
        {
            GameObject fzObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            fzObj.name = "FrostZone_Weapon";
            fzObj.transform.SetParent(transform);
            fzObj.transform.localPosition = Vector3.zero;
            fzObj.transform.localRotation = Quaternion.identity;
            
            var renderer = fzObj.GetComponent<Renderer>();
            if (renderer) renderer.material.color = new Color(0, 1, 1, 0.3f);
            
            var col = fzObj.GetComponent<Collider>();
            if (col) col.isTrigger = true;

            FrostZone fz = fzObj.AddComponent<FrostZone>();
            misArmas.Add(fz);
        }
        else
        {
            // 2. OrbitalShield
            bool tieneShield = false;
            foreach(var a in misArmas) { if(a is OrbitalShield) tieneShield = true; }

            if (!tieneShield)
            {
                GameObject osObj = new GameObject("OrbitalShield_Weapon");
                osObj.transform.SetParent(transform);
                osObj.transform.localPosition = Vector3.zero;
                
                OrbitalShield os = osObj.AddComponent<OrbitalShield>();
                os.orbPrefab = orbitalOrbPrefab;
                misArmas.Add(os);
            }
            else
            {
                // 3. MagicWand
                bool tieneWand = false;
                foreach(var a in misArmas) { if(a is MagicWand) tieneWand = true; }

                if (!tieneWand)
                {
                    GameObject mwObj = new GameObject("MagicWand_Weapon");
                    mwObj.transform.SetParent(transform);
                    mwObj.transform.localPosition = Vector3.zero;
                    
                    MagicWand mw = mwObj.AddComponent<MagicWand>();
                    mw.projectilePrefab = magicProjectilePrefab;
                    misArmas.Add(mw);
                }
                else
                {
                    // 4. Hacha
                    bool tieneHacha = false;
                    foreach(var a in misArmas) 
                    { 
                        if(a is LanzadorArma la && la.proyectilPrefab == lanzadorHachaPrefab) tieneHacha = true; 
                    }

                    if (!tieneHacha)
                    {
                        GameObject laObj = new GameObject("LanzadorHacha_Weapon");
                        laObj.transform.SetParent(transform);
                        laObj.transform.localPosition = Vector3.zero;
                        
                        LanzadorArma la = laObj.AddComponent<LanzadorArma>();
                        la.proyectilPrefab = lanzadorHachaPrefab; 
                        la.tiempoDeAtaque = 2.0f;
                        misArmas.Add(la);
                    }
                    else
                    {
                        // 5. Slash
                        bool tieneSlash = false;
                        foreach(var a in misArmas) 
                        { 
                            if(a is LanzadorArma la && la.proyectilPrefab == lanzadorSlashPrefab) tieneSlash = true; 
                        }

                        if (!tieneSlash)
                        {
                            GameObject slashObj = new GameObject("Slash_Weapon");
                            slashObj.transform.SetParent(transform);
                            slashObj.transform.localPosition = Vector3.zero;
                            
                            LanzadorArma la = slashObj.AddComponent<LanzadorArma>();
                            la.proyectilPrefab = lanzadorSlashPrefab; 
                            la.tiempoDeAtaque = 1.0f;
                            misArmas.Add(la);
                        }
                        else
                        {
                            // 6. Rayo
                            bool tieneRayo = false;
                            foreach(var a in misArmas) 
                            { 
                                if(a is LanzadorArma la && la.proyectilPrefab == rayoPrefab) tieneRayo = true; 
                            }

                            if (!tieneRayo)
                            {
                                GameObject rayObj = new GameObject("RayoLuz_Weapon");
                                rayObj.transform.SetParent(transform);
                                rayObj.transform.localPosition = Vector3.zero;
                                
                                LanzadorArma la = rayObj.AddComponent<LanzadorArma>();
                                la.proyectilPrefab = rayoPrefab; 
                                la.tiempoDeAtaque = 4.0f;
                                misArmas.Add(la);
                            }
                            else
                            {
                                // Ya tenemos todo, subimos nivel
                                SubirNivelArma();
                            }
                        }
                    }
                }
            }
        }
    }
    
    void SubirNivelArma()
    {
        ActualizarListaArmas();

        if (misArmas.Count == 0) return;
        
        MonoBehaviour armaRandom = misArmas[Random.Range(0, misArmas.Count)];
        
        if (armaRandom is FrostZone fz) fz.LevelUp();
        else if (armaRandom is OrbitalShield os) os.LevelUp();
        else if (armaRandom is MagicWand mw) mw.LevelUp();
        else if (armaRandom is LanzadorArma la) la.LevelUp();
        else if (armaRandom is RayoDeEnergia re) re.LevelUp();
    }

    private void ActualizarListaArmas()
    {
        misArmas.RemoveAll(a => a == null);
    }
}