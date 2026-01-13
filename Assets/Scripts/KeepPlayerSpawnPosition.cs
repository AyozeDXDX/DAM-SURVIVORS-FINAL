using UnityEngine;

public class KeepPlayerSpawnPosition : MonoBehaviour
{
    public GameObject Player;
    private Transform spawnPositionTransform;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        spawnPositionTransform = Player.transform.GetChild(0);
    }

    void Update()
    {
        transform.position = spawnPositionTransform.position;
        transform.rotation = Player.transform.rotation;
    }
}
