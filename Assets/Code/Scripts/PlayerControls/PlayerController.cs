using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public Variables
    public string PlayerName { get; set; } // private set
    public bool CanPlayerMove { get; set; } // private set

    // Private Variables
    // [SerializeField] to make private variables visible in the inspector but not in other classes
    [SerializeField] private bool isOnGround;
    [SerializeField] private bool hasPowerup = false;
    [SerializeField] private List<GameObject> powerupIndicators;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip jumpSound;
    private float jumpForce = 500;
    private float flyUpwardForce = 200;
    private float gravityModifier = 1.0f;
    private float power = 3500;
    private float airControlModifier = 0.75f;
    private float horizontalInput;
    private float verticalInput;
    private float distToGround = 0.5f;
    private float walkSoundWaitingTime = 0.3f;
    private float walkSoundTimer;
    private bool canDoubleJump = false;
    private bool isFlying = false;
    private bool canJumpAgain = false;
    private Vector3 originOffset = new Vector3(0, 0.5f, 0);
    private Vector3 powerupIndicatorOffset = new Vector3(0, 0.1f, 0);
    [SerializeField] private GameManager gameManager;
    private Rigidbody playerRb;
    private Animator playerAnim;
    private BoxCollider playerCollider;
    private AudioSource playerAudio;
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
        playerAudio = GetComponent<AudioSource>();
        focalPoint = GameObject.Find(TagsAndNames.FocalPoint);
        playerAnim = GetComponent<Animator>();
    }

    // 'FixedUpdate' called before 'Update' calls and happens when the game is trying to calculate any kind of physics
    void FixedUpdate()
    {
        CheckIsOnGround();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFlying)
        {
            Jump();
        }

        MovePlayer();

        if (powerupIndicator != null)
            powerupIndicator.transform.position = transform.position + powerupIndicatorOffset;
    }

    private void MovePlayer()
    {
        // Get player input
        horizontalInput = Input.GetAxis(TagsAndNames.Horizontal);
        verticalInput = Input.GetAxis(TagsAndNames.Vertical);

        if ((verticalInput == 0 && horizontalInput == 0)
            || !CanPlayerMove
            || !gameManager.isGameActive
            )
        {
            playerAnim.SetFloat("Speed_f", 0.1f);
            return;
        }

        walkSoundTimer += Time.deltaTime;
        if (isOnGround)
        {
            if (verticalInput != 0 || horizontalInput != 0)
            {
                playerAnim.SetFloat("Speed_f", 0.6f);
            }

            if (walkSoundTimer > walkSoundWaitingTime)
            {
                playerAudio.PlayOneShot(walkSound, 0.9f);
                walkSoundTimer = 0;
            }
        }

        // While not on ground the power gets reduced. But the player still can move while Jumping / flying
        float movePower = isOnGround ? power : power * airControlModifier;

        Vector3 focalPointEulerRotation = focalPoint.transform.rotation.eulerAngles;

        //// TODO:
        //// Steuerung überarbeiten:
        ////
        //// Bsp.:
        //// 1) Links = 1, Up = 0.1(1 / 0.1 = 10)
        //// => Vektor nach links + 4.5° (10 % von 90°/ 2) nach oben
        //// 2) Links = 0.3, Up = 0.1(0.3 / 0.1 = 3)
        //// => 45° *(1 / 3) nach oben. 30°
        //// 3) Links = 0.5, Up = 0.5(0.5 / 0.5 = 1)
        //// => 45° *1 nach oben. 45°
        ////
        //// 4) Links = 0.1, Up = 1(0.1 / 1 = 0.1)
        //// => Vektor nach oben + 4.5° (10 % von 90°/ 2) nach links
        //// 5) Links = 0.1, Up = 0.3(0.1 / 0.3 = 0.333)
        ////
        //// => Quotient < 1, dann * 100 und von oben nach links
        //// hVInput = left + up < 1 ? left + up : 1;

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
            playerRb.AddRelativeForce(Vector3.forward * movePower * (horizontalInput / 1.75f + verticalInput / 1.75f));
        }
        else if (horizontalInput > 0 && verticalInput < 0) // down-right
        {
            // Rotate the player down-right
            RotateOnYAxis(focalPointEulerRotation.y + 135);
            // Move the player down-right
            playerRb.AddRelativeForce(((Vector3.back * movePower * verticalInput) + (Vector3.forward * movePower * horizontalInput)) / 1.75f);
        }
        else if (horizontalInput < 0 && verticalInput > 0) // up-left
        {
            // Rotate the player up-left
            RotateOnYAxis(focalPointEulerRotation.y + 315);
            // Move the player up-left
            playerRb.AddRelativeForce(((Vector3.forward * movePower * verticalInput) + (Vector3.back * movePower * horizontalInput)) / 1.75f);
        }
        else if (horizontalInput < 0 && verticalInput < 0) // down-left
        {
            // Rotate the player down-left
            RotateOnYAxis(focalPointEulerRotation.y + 225);
            // Move the player down-left
            playerRb.AddRelativeForce(Vector3.back * movePower * (horizontalInput / 1.75f + verticalInput / 1.75f));
        }
    }

    private void Jump()
    {
        canJumpAgain = isOnGround || canJumpAgain;

        if (Input.GetKeyDown(KeyCode.Space) && (isOnGround || (canJumpAgain && canDoubleJump)) && gameManager.isGameActive)
        {
            canJumpAgain = isOnGround || !canJumpAgain;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            playerAudio.PlayOneShot(jumpSound, 2.0f);

            playerAnim.SetTrigger("Jump_trig");
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
        if (other.CompareTag(TagsAndNames.Powerup) && !hasPowerup)
        {
            hasPowerup = true;
            var ext = other.GetComponent<PowerupExtension>();
            canDoubleJump = ext.canDoubleJump;
            canJumpAgain = canDoubleJump;

            var indicatorName = PowerupIndicatorDictionary.PowerupIndicators[other.name];
            // store currentPowerup
            powerupIndicator = powerupIndicators.First(p => p.gameObject.name == indicatorName);
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);

            if (ext.canFly)
            {
                StartCoroutine(FlyRoutine());
            }

            StartCoroutine(PowerupCountdownRoutine(indicatorName));
        }

        // Check if collision with Finish_Platform
        if (other.CompareTag(TagsAndNames.Finish))
        {
            CanPlayerMove = false;
            playerRb.MovePosition(other.transform.position);
            RotateOnYAxis(focalPoint.transform.rotation.eulerAngles.y + 180);
            playerAnim.SetTrigger("Jump_trig");
            gameManager.GameFinished();
        }
    }

    IEnumerator PowerupCountdownRoutine(string indicatorName)
    {
        // Powerup is activated for 10s
        yield return new WaitForSeconds(10);
        hasPowerup = false;
        canDoubleJump = false;
        canJumpAgain = false;
        isFlying = false;
        powerupIndicator.gameObject.SetActive(false);
        StopCoroutine(FlyRoutine());
    }

    IEnumerator FlyRoutine()
    {
        isFlying = true;
        playerRb.AddForce(Vector3.up * flyUpwardForce, ForceMode.Impulse);
        playerRb.useGravity = false;

        // Powerup is activated for 10s
        yield return new WaitForSeconds(10);
        playerRb.useGravity = true;
    }
}
