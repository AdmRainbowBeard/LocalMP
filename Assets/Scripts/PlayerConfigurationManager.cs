using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    public bool canStart = false;
    private bool hasStarted = false;

    [SerializeField]
    private int minPlayers = 2;

    [SerializeField]
    private int maxPlayers = 3;

    public static PlayerConfigurationManager instance { get; private set; }

    #region Singleton
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
    #endregion

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void SetPlayerCharacter(int index, Material color)
    {
        playerConfigs[index].playerMaterial = color;
    }

    public void SetPlayerCharacter(int index, GameObject characterChoice)
    {
        playerConfigs[index].playerCharacterPrefabChoice = characterChoice;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true;

        if (playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.isReady == true))
        {
            StartMatch();
            canStart = true;
        }
    }

    public void UnReadyPlayer(int index)
    {
        playerConfigs[index].isReady = false;
    }

    private void Update()
    {
        if (playerConfigs.Any(p => p.isReady))
        {
            if (!hasStarted)
            {
                StartCoroutine(CountdownToStart());
                hasStarted = true;
            }
        }


        if (playerConfigs.Any(p => p.pressedStart)) 
        {
            StartMatch();
        } 
    }

    public IEnumerator CountdownToStart()
    {
        yield return new WaitForSeconds(3);
        StartMatch();
    }

    public void StartMatch(string levelName = "SampleScene")
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void HandlePlayerJoin(PlayerInput pI)
    {
        Debug.Log("Player Joined!");
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
    public Material playerMaterial { get; set; }
    public bool isReady { get; set; }
    public bool pressedStart { get; set; }
}
