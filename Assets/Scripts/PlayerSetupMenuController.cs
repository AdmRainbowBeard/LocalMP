using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int playerIndex;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private Button readyButton;

    private float ignoreInputTime = 1.5f;
    [SerializeField]
    private bool inputEnabled = false;

    public void SetPlayerIndex(int pI)
    {
        playerIndex = pI;
        titleText.SetText("Player " + (pI + 1).ToString());
        ignoreInputTime = Time.time - ignoreInputTime;
    }

    private void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SetColor(Material color)
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.instance.SetPlayerCharacter(playerIndex, color);
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.instance.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
