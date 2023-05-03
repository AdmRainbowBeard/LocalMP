using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerConfiguration playerConfig;
    private PlayerController control;

    [SerializeField]
    private SkinnedMeshRenderer playerMesh;

    private PlayerControls controls;

    private void Awake()
    {
        control = GetComponent<PlayerController>();
        controls = new PlayerControls();
    }

    public void InitializePlayer(PlayerConfiguration pC)
    {
        playerConfig = pC;
        playerMesh.material = pC.playerMaterial;
        playerConfig.input.onActionTriggered += Input_onActionTriggered;
    }

    private void Input_onActionTriggered(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (obj.action.name == controls.Player.Functions.name)
        {
            GameManager.instance.OnStartPressed(obj);
        }

        if (obj.action.name == controls.Player.Movement.name)
        {
            control.OnMove(obj);
        }

        if (obj.action.name == controls.Player.Jump.name)
        {
            control.OnJump(obj);
        }

        if (obj.action.name == controls.Player.Sprint.name)
        {
            control.OnSprint(obj);
        }
    }
}
