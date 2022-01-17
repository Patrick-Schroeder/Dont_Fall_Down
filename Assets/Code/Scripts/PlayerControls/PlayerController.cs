using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Public Variables
    public bool isOnGround;
    public bool canPlayerMove;
    public bool hasPowerup = false;
    public List<GameObject> powerupIndicators;

    // Private Variables
    // [SerializeField] to make private variables visible in the inspector but not in other classes
    private float jumpForce = 2000;
    private float gravityModifier = 3.0f;
    private float power = 1500;
    private float powerModifier = 0.5f;
    private float horizontalInput;
    private float verticalInput;
    private float distToGround = 0.5f;
    private bool canDoubleJump = false;
    private bool canJumpAgain = false;
    private Vector3 originOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private GameManager gameManager;
    private Rigidbody playerRb;
    private BoxCollider playerCollider;
    private GameObject focalPoint;
    private GameObject powerupIndicator;

    [SerializeField] GameObject centerOfMass;

    // Start is called before the first frame update
    void Start()
    {
        isOnGround = true;

        if (!LoadingData.isSceneInitialized)
            Physics.gravity *= gravityModifier;

        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<BoxCollider>();
        playerRb.centerOfMass = centerOfMass.transform.position;
        focalPoint = GameObject.Find(TagsAndNames.FocalPoint);
    }

    // 'FixedUpdate' called before 'Update' calls and happens when the game is trying to calculate any kind of physics
    void FixedUpdate()
    {
        CheckIsOnGround();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        MovePlayer();

        if (powerupIndicator != null)
            powerupIndicator.transform.position = transform.position + new Vector3(0, 1.5f, 0);
    }

    private void MovePlayer()
    {
        // Get player input
        horizontalInput = Input.GetAxis(TagsAndNames.Horizontal);
        verticalInput = Input.GetAxis(TagsAndNames.Vertical);

        if ((verticalInput == 0 && horizontalInput == 0)
            || !canPlayerMove
            || !gameManager.isGameActive)
        {
            return;
        }

        // While not on ground the power gets reduced. But the player still can move while Jumping / flying
        float movePower = isOnGround ? power : power * powerModifier;

        Vector3 focalPointEulerRotation = focalPoint.transform.rotation.eulerAngles;

        if (horizontalInput > 0 && verticalInput == 0) // right
        {
            // Rotate the player to the right
            RotateOnYAxis(focalPointEulerRotation.y + 90);
            // Move the player to the right
            playerRb.AddRelativeForce(Vector3.forward * movePower * horizontalInput);
        }
        else if (horizontalInput < 0 && verticalInput == 0) // left
        {
            // Rotate the player to the left
            RotateOnYAxis(focalPointEulerRotation.y + 270);
            // Move the player to the left
            playerRb.AddRelativeForce(Vector3.back * movePower * horizontalInput);
        }
        else if (verticalInput > 0 && horizontalInput == 0) // up
        {
            // Rotate the player forward
            RotateOnYAxis(focalPointEulerRotation.y);
            // Move the player forward
            playerRb.AddRelativeForce(Vector3.forward * movePower * verticalInput);
        }
        else if (verticalInput < 0 && horizontalInput == 0) // down
        {
            // Rotate the player backward
            RotateOnYAxis(focalPointEulerRotation.y + 180);
            // Move the player backward
            playerRb.AddRelativeForce(Vector3.back * movePower * verticalInput);
        }
        else if (horizontalInput > 0 && verticalInput > 0) // up-right
        {
            // Rotate the player up-right
            RotateOnYAxis(focalPointEulerRotation.y + 45);
            // Move the player up-right
            playerRb.AddRelativeForce(Vector3.forward * movePower * (horizontalInput / 2 + verticalInput / 2));
        }
        else if (horizontalInput > 0 && verticalInput < 0) // down-right
        {
            // Rotate the player down-right
            RotateOnYAxis(focalPointEulerRotation.y + 135);
            // Move the player down-right
            playerRb.AddRelativeForce(((Vector3.back * movePower * verticalInput) + (Vector3.forward * movePower * horizontalInput)) / 2);
        }
        else if (horizontalInput < 0 && verticalInput > 0) // up-left
        {
            // Rotate the player up-left
            RotateOnYAxis(focalPointEulerRotation.y + 315);
            // Move the player up-left
            playerRb.AddRelativeForce(((Vector3.forward * movePower * verticalInput) + (Vector3.back * movePower * horizontalInput)) / 2);
        }
        else if (horizontalInput < 0 && verticalInput < 0) // down-left
        {
            // Rotate the player down-left
            RotateOnYAxis(focalPointEulerRotation.y + 225);
            // Move the player down-left
            playerRb.AddRelativeForce(Vector3.back * movePower * (horizontalInput / 2 + verticalInput / 2));
        }
    }

    private void Jump()
    {
        canJumpAgain = isOnGround && canDoubleJump ? true : canJumpAgain;

        if (Input.GetKeyDown(KeyCode.Space) && (isOnGround || (canJumpAgain && canDoubleJump)) && gameManager.isGameActive)
        {
            canJumpAgain = !isOnGround && canJumpAgain ? false : canJumpAgain;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // TODO:
            // FLIEGEN


            // TODO:
            //playerAnim.SetTrigger("Jump_trig");
            //dirtParticle.Stop();
            //playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
    }

    private void RotateOnYAxis(float angle)
    {
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    void CheckIsOnGround()
    {
        // Check if there is an object under the player
        isOnGround = Physics.Raycast(playerCollider.bounds.min + originOffset
            , -Vector3.up,
            distToGround + 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if collision with a powerup
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            canDoubleJump = other.GetComponent<PowerupExtension>().canDoubleJump;
            canJumpAgain = canDoubleJump;

            var indicatorName = PowerupIndicatorDictionary.PowerupIndicators[other.name];
            // store currentPowerup
            powerupIndicator = powerupIndicators.First(p => p.gameObject.name == indicatorName);
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine(indicatorName));
        }
    }

    IEnumerator PowerupCountdownRoutine(string indicatorName)
    {
        // Powerup is activated for 10s
        yield return new WaitForSeconds(10);
        hasPowerup = false;
        canDoubleJump = false;
        canJumpAgain = false;
        powerupIndicator.gameObject.SetActive(false);
    }
}
