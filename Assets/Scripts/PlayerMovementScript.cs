using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 25f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2 (10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    // [SerializeField] AudioClip gunshotSFX;
    //I have no idea why the sound keeps giving value cannot be null. A soundfile is being placed in the inspector, and as far as I can tell everything else is written correctly. Googling only gave the result that I needed to put a soundfile in the inspector.

    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    AudioSource playerNoise;
    float gravityScaleAtStart;

    bool isAlive = true;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        // playerNoise = GetComponent<AudioSource>();
        gravityScaleAtStart = playerRigidbody.gravityScale;
    }

    void Update()
    {
        if(!isAlive){return;}
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }
    
    void OnFire(InputValue value){
        if(!isAlive){return;}
        Instantiate(bullet, gun.position, transform.rotation);
        // playerNoise.PlayOneShot(gunshotSFX, 0.7f);
    }

    void OnMove(InputValue value){
        if(!isAlive){return;}
        moveInput = value.Get<Vector2>();
        // Debug.Log(moveInput);
    }

    void OnJump(InputValue value){

        if(!isAlive){return;}
        //Basically making it so if the player isn't touching the ground it will not proceed and allow the player to jump- preventing multiple jumps
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            return;
        }

        if(value.isPressed){
            playerRigidbody.velocity += new Vector2 (0f, jumpSpeed);
        }
    }

    void Run(){

        //This playerVelocity variable is allowing for the movement speed to be changed on the x axis, and keep the y axis gravity as is
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = playerVelocity;

        //This sets animation to running if player velocity is above 0 (or 0 equivalent)
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite(){
        //Sets a bool that keeps the player looking left if moved to the left- Abs = absolute, Epsilon = smallest number possible (better than using 0 cos result might be 0.00123 for example)
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;


        if(playerHasHorizontalSpeed){
        //Changes the x scale to -1 when the player velocity is at - as well (when looking left), and 1 when player velocity is at + , don't need to edit Y scale so leave at 1
        transform.localScale = new Vector2 (Mathf.Sign(playerRigidbody.velocity.x), 1f);
        }
    } 
    
    void ClimbLadder(){

        //Same thing as the onJump, makes it so the function doesn't work if there is no collision
         if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){

            playerRigidbody.gravityScale = gravityScaleAtStart;
            playerAnimator.SetBool("isClimbing", false);

            return;
        }

        //Same thing as run function, just changing the y axis instead of x axis
        Vector2 climbVelocity = new Vector2 (playerRigidbody.velocity.x, moveInput.y * climbSpeed);
        playerRigidbody.velocity = climbVelocity;
        playerRigidbody.gravityScale = 0f;
        
        //setting animation to climbing unless player is standing still
        bool playerHasVerticalSpeed = Mathf.Abs(playerRigidbody.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die(){
        if(playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))){
            isAlive = false;
            playerAnimator.SetTrigger("Dying");
            playerRigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
