using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    PlayerConfigurationManager pCM;

    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField] private Animator anim;

    public bool inControl = true;
    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;
    private bool startPressed = false;

    public int respawnTime { get; private set; }
    public GameObject lI;
    private Transform[] spawnPoints;
    private bool inWater;

    private void Start()
    {
        pCM = PlayerConfigurationManager.instance;

        lI = GameObject.Find("LevelInitializer");
        spawnPoints = lI.GetComponent<InitializeLevel>().playerSpawnpoints;

        controller = gameObject.GetComponent<CharacterController>();
        anim = gameObject.GetComponent<Animator>();

        respawnTime = 2;
    }

    public void OnStartPressed(InputAction.CallbackContext context)
    {
        startPressed = context.action.triggered;

        if (pCM.canStart)
        {
            Debug.Log("Start Pressed!");
            pCM.StartMatch();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!inControl) return;

        movementInput = context.ReadValue<Vector2>();

        if (inWater)
        {
            // Play animation of swimming
        }
        else
        {
            if (movementInput != Vector2.zero)
            {
                if (anim != null) anim.SetBool("isMoving", true);
            }
            else
            {
                if (anim != null) anim.SetBool("isMoving", false);
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!inControl) return;

        jumped = context.action.triggered;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        // Change Player's Speed
        playerSpeed = 15;
        // Animate Sprinting
    }

    private void Die()
    {
        inControl = false;
        jumped = false;
        movementInput = Vector2.zero;

        // Display respawn countdown
        // Display negative score pop-up
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {
        int rando = Random.Range(0, spawnPoints.Length);

        yield return new WaitForSeconds(respawnTime);

        inWater = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.transform.position = new Vector3(spawnPoints[rando].position.x, spawnPoints[rando].position.y + 0.51f, spawnPoints[rando].position.z);
        //Reset Animations to Idle

        // Reset Gravity 
        gravityValue = -9.81f * 2;

        // Give back Player Control
        inControl = true;
    }

    void Update()
    {
        //groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);

        if (inWater)
        {
            playerVelocity.y = 0f;

            controller.Move(move * Time.deltaTime * (playerSpeed / 2));

            if (move != Vector3.zero)
            {
                transform.forward = move;
            }
        }
        else
        {
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                transform.forward = move;
            }

            // Changes the height position of the player..
            if (jumped && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
        }

        controller.Move(playerVelocity * Time.deltaTime);

        playerSpeed = 5;

        if (transform.position.y < -20) Die();
        groundedPlayer = controller.isGrounded;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Shark")
        {
            Die();
        }

        if (other.gameObject.tag == "Water")
        {
            inWater = true;
        }
    }
}
