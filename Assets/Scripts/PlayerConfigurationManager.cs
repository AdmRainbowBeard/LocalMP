using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField]
    private int maxPlayers = 2;

    public static PlayerConfigurationManager instance { get; private set; }

    private float timeToStart = 5.0f;
    private bool starting = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Too many Instances!");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    private void Update()
    {
        if (playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.isReady == true))
        {
            starting = true;
        }

        if (starting)
        {
            timeToStart -= Time.deltaTime;
            if (timeToStart < 0)
            {
                StartMatch();
            }
        }
    }

    public void SetPlayerCharacter(int index, GameObject characterChoice)
    {
        playerConfigs[index].playerCharacterPrefabChoice = characterChoice;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true;
    }

    public void UnReadyPlayer(int index)
    {
        playerConfigs[index].isReady = false;
        starting = false;
    }

    private void StartMatch()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void HandlePlayerJoin(PlayerInput pI)
    {
        Debug.Log("");
        if (!playerConfigs.Any(p => p.playerIndex == pI.playerIndex))
        {
            pI.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pI));
        }
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pI)
    {
        playerIndex = pI.playerIndex;
        input = pI;
    }
    public PlayerInput input { get; set; }
    public int playerIndex { get; set; }
    public string playerName { get; set; }
    public GameObject playerCharacterPrefabChoice;
    public bool isReady { get; set; }
}
