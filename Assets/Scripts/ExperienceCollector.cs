using UnityEngine;

public class ExperienceCollector : MonoBehaviour
{
    private PlayerLevel playerLevel;

    void Start()
    {
        playerLevel = GetComponentInParent<PlayerLevel>();
        if (playerLevel == null)
        {
            playerLevel = FindFirstObjectByType<PlayerLevel>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerLevel == null) return;

        if (other.name.Contains("Orbe") || other.name.Contains("Exp"))
        {
            ExpOrb orb = other.GetComponent<ExpOrb>();
            if (orb != null)
            {
                playerLevel.AddExperience(orb.cantidadExp);
                Destroy(other.gameObject);
            }
        }
    }
}
