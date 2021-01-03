using UnityEngine;
using System.Collections;

//Responsible for the walking/running of the player.
public class WalkScript : MonoBehaviour
{
    //A container responsible for storing commonly used / shared variables.
    public float maxMoveSpeedSquared = 15f;
    public float moveForce = 2500f;                             //The walk force multiplier.
    public float stoppingFrictionMultiplier = 5;                //Friction multiplier.
    public float movingFrictionMultiplier = 0.5f;               //Friction multiplier.
    public float airFrictionMultiplier = 2;                  //Friction multiplier.
    public float gravity = -9.81f;
    public AudioSource audioSource;
    [Range(0, 1)]
    public float footstepVolume = 0.1f;
    public float delayBetweenFootsteps = 0.2f;
    public AudioClip[] footstepSounds;


    Vector2 horizontalVel;                                      //The Horizontal velocity of the player relative to their forward vector.

    //FindVelRealtiveToLook
    float lookAngle;                                            //The y rotation of the player
    float moveAngle;                                            //The y rotation of the player's velocity.
    float u;                                                    //The angle difference between the look and move angle.
    float magnitude;                                            //The magnitude of the player's velocity.
    float yMag;                                                 //The z/forward component of the velocty (Y component from top down).
    float xMag;                                                 //The x/right componend of the velocity (X component from top down).
    float multiplier = 1f;
    [HideInInspector]
    public bool canMove = true;
    Vector3 moveDir;
    Vector3 dirSlope;

    private void Awake()
    {
        PlayerVariables.walkScript = this;
        PlayerVariables.maxVelocity = maxMoveSpeedSquared;
        PlayerVariables.playerRB = GetComponent<Rigidbody>();
        StartCoroutine(PlayFootsteps());
    }


    //Using FixedUpdate for consistent movement.
    //If the player isn't on the ground, checks if the player is moving into a wall and if so, stops movement
    //This is to prevent players from latching onto unwanted walls.
    void FixedUpdate()
    {
        //print(PlayerVariables.playerRB.velocity.magnitude);
        if (canMove)
        {
            Move();
        }

        ApplyGravity();
    }

    void Update()
    {
        PlayerVariables.x = Input.GetAxisRaw("Horizontal");
        PlayerVariables.y = Input.GetAxisRaw("Vertical");

        UpdateWalkingAnim();
    }

    //Finds the velocity relative to the player's forward direction (duh).
    private Vector2 FindVelRelativeToLook()
    {
        lookAngle = transform.eulerAngles.y;
        moveAngle = Mathf.Atan2(PlayerVariables.playerRB.velocity.x, PlayerVariables.playerRB.velocity.z) * Mathf.Rad2Deg;

        u = Mathf.DeltaAngle(lookAngle, moveAngle);
        magnitude = PlayerVariables.playerRB.velocity.magnitude;
        yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        xMag = magnitude * Mathf.Sin(u * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    //Applies countermovement/friction to the player only when the player isn't pressing W/S or A/D.
    private void CounterMovement(float x, float y, Vector2 Mag)
    {
        float multiplier;
        //print("( " + Mathf.Abs(PlayerVariables.playerRB.velocity.x) + "," + Mathf.Abs(PlayerVariables.playerRB.velocity.z) + " )");
        if (x == 0 && y == 0 && PlayerVariables.isGrounded)
            multiplier = stoppingFrictionMultiplier;
        else if (!PlayerVariables.isGrounded)
            multiplier = airFrictionMultiplier;
        else
            multiplier = movingFrictionMultiplier;

        PlayerVariables.playerRB.AddForce(PlayerVariables.playerRB.mass * transform.right * -Mag.x * multiplier);
        PlayerVariables.playerRB.AddForce(PlayerVariables.playerRB.mass * transform.forward * -Mag.y * multiplier);
    }

    //Applies movement and countermovement.
    void Move()
    {
        horizontalVel = FindVelRelativeToLook();

        // Only appies countermovement if the player is on the ground, is dashing (on the ground or not), or is not crouching on the ground
        //if (PlayerVariables.isGrounded[0] || PlayerVariables.isDashing || (!PlayerVariables.isCrouching && PlayerVariables.isGrounded[0]))
        if (PlayerVariables.isDashing || (!PlayerVariables.isCrouching /*&& PlayerVariables.isGrounded*/))
            CounterMovement(PlayerVariables.x, PlayerVariables.y, horizontalVel);

        moveDir = transform.rotation * new Vector3(PlayerVariables.x, 0, PlayerVariables.y);
        moveDir.Normalize();

        if (moveDir.magnitude != 0 && PlayerVariables.playerRB.velocity.magnitude < maxMoveSpeedSquared)
            PlayerVariables.playerRB.AddForce(moveDir * moveForce * PlayerVariables.playerRB.mass * multiplier);
    }

    void ApplyGravity()
    {
        //PlayerVariables.playerRB.AddForce(new Vector3(0, gravity, 0));
        PlayerVariables.playerRB.velocity += gravity * Vector3.up;
    }

    void UpdateWalkingAnim()
    {
        if (PlayerVariables.playerRB.velocity.magnitude > 2f && !PlayerVariables.KatanaAnimator.GetBool("Moving"))
            PlayerVariables.KatanaAnimator.SetBool("Moving", true);
        else
            PlayerVariables.KatanaAnimator.SetBool("Moving", false);
    }

    IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (!audioSource.isPlaying && PlayerVariables.x != 0 || PlayerVariables.y != 0 && PlayerVariables.playerRB.velocity.magnitude > 1 && PlayerVariables.isGrounded)
            {
                audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)], footstepVolume);
            }
            yield return new WaitForSeconds(delayBetweenFootsteps);
        }
    }

}
