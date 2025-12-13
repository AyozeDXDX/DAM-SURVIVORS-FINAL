using UnityEngine;

public class KeepPlayerSpawnPosition : MonoBehaviour
{
    public GameObject Player;
    private Transform spawnPositionTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        spawnPositionTransform = Player.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = spawnPositionTransform.position;
        transform.rotation = Player.transform.rotation;
    }
}
