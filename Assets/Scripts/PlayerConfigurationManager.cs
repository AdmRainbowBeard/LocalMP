using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField]
    private int maxPlayers = 4;

}

public class PlayerConfiguration
{
    public PlayerInput input { get; set; }
    public int playerIndex { get; set; }
    public string playerName { get; set; }
    public GameObject playerCharacterPrefabChoice;
    public bool isReady { get; set; }
}
