using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float x, z, speed, gravity, jumpForce, fallVelocity, rotationSpeed, rotationCameraSpeed, weapon, life;
    public CharacterController player;
    public Animator animator;
    private Vector3 camForward, camRight, movePlayer, playerInput;
    public Camera mainCam;
    public GameController gameController;
    public GameObject bullet;

    void Start()
    {
        speed = 14f;
        rotationSpeed = 10f;
        rotationCameraSpeed = 5f;
        gravity = 9.8f;
        jumpForce = 10f;
        weapon = 1;
        life = 100;

        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isGrounded)
        {
            animator.SetBool("ground", true);
        }

        else
        {
            animator.SetBool("ground", false);
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        animator.SetFloat("x", x);
        animator.SetFloat("z", z);

        playerInput = new Vector3(x, 0, z);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        Vector3 direction = new Vector3(x, 0, z).normalized;

        if(gameController.canPlay)
        {
            camDirection();

            movePlayer = playerInput.x * camRight + playerInput.z * camForward;
            movePlayer = movePlayer * speed;

            if (direction.magnitude >= 0.2f)
            {
                Quaternion toRotation = Quaternion.LookRotation(playerInput.x * camRight + playerInput.z * camForward, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            SetGravity();
            PlayerSkills();
            player.Move(movePlayer * Time.deltaTime);

            if (Input.GetMouseButtonDown(0) && gameController.coins > 0)
            {
                var newBullet = Instantiate(bullet, transform.Find("Point").position, transform.rotation);
                newBullet.transform.parent = transform.Find("Bullets");
                newBullet.GetComponent<Rigidbody>().velocity = transform.forward * 30f;

                gameController.coins -= 1;

            }

            if(transform.position.y < 0)
            {
                gameController.GameOver();
            }
        }

        else
        {
            speed = 0f;
        }
    }

    void camDirection()
    {
        camForward = mainCam.transform.forward;
        camRight = mainCam.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    public void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }

        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
    }

    public void PlayerSkills()
    {
        if (player.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
            Debug.Log("Space");
        }
    }
}
