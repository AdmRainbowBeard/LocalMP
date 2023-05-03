using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

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

            pCM = PlayerConfigurationManager.instance;
        }
    }
    #endregion

    PlayerConfigurationManager pCM;

    private bool startPressed = false;

    public void Update()
    {
        if (startPressed && pCM.canStart)
        {
            pCM.StartMatch();
        }
    }

    public void OnStartPressed(InputAction.CallbackContext context)
    {
        startPressed = context.action.triggered;
    }
}
