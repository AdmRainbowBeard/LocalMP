using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private Animator anim;

    public bool inControl = true;
    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;

    public int respawnTime { get; private set; }
    public GameObject lI;
    private Transform[] spawnPoints;

    private void Start()
    {
        lI = GameObject.Find("LevelInitializer");
        spawnPoints = lI.GetComponent<InitializeLevel>().playerSpawnpoints;

        controller = gameObject.GetComponent<CharacterController>();
        anim = gameObject.GetComponent<Animator>();

        respawnTime = 2;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!inControl) return;

        movementInput = context.ReadValue<Vector2>();

        if (movementInput != Vector2.zero)
        {
            if (anim != null) anim.SetBool("isMoving", true);
        }
        else
        {
            if (anim != null) anim.SetBool("isMoving", false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!inControl) return;

        jumped = context.action.triggered;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        playerSpeed = 15;
    }

    private void Die()
    {
        inControl = false;
        // Display respanw countdown
        // Display negative score pop-up
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        inControl = true;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.transform.position = new Vector3(spawnPoints[1].position.x, spawnPoints[1].position.y + 1, spawnPoints[1].position.z);

        // Reset Gravity 
        gravityValue = -9.81f * 2;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
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
        controller.Move(playerVelocity * Time.deltaTime);

        playerSpeed = 5;

        if (transform.position.y < -20) Die();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            gravityValue = -4f;
            Die();
        }
    }
}
