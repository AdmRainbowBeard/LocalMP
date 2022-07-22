using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawnpoints;
    [SerializeField]
    private GameObject playerPrefab;

    private void Start()
    {
        var playerConfigs = PlayerConfigurationManager.instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawnpoints[i].position, playerSpawnpoints[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);
        }
    }
}
