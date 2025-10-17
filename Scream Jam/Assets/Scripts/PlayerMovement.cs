using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    //public float jumpForce;
    //public float jumpCooldown;
    //public float airMultiplier;
    //bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    //[Header("Keybinds")]
    //public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    //public float playerHeight;
    //public LayerMask whatIsGround;
    //public bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    //sound
    private AudioSource audioSource;
    public AudioClip hitClip;

    Rigidbody rb;
    public float fallForce = 10f;
    public float recoveryTime = 1f;
    public float standSpeed = 1f;
    public bool isFallen = false;
    private Quaternion originalRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, 1, 0);
        rb.freezeRotation = true;

        audioSource = GetComponent<AudioSource>();

        //readyToJump = true;
    }

    private void Update()
    {
        // ground check
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        if (isFallen)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }

        MyInput();
        SpeedControl();

        // handle drag
//if (grounded)
            rb.linearDamping = groundDrag;
        //else
        //    rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if (isFallen)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        //if (Input.GetKey(jumpKey) && readyToJump && grounded)
        //{
        //    readyToJump = false;
        //
        //    Jump();
        //
        //    Invoke(nameof(ResetJump), jumpCooldown);
        //}
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        //if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        //else if (!grounded)
        //    rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    public void FallOver(Transform npcPos)
    {
        if (!isFallen)
        {
            StartCoroutine(FallOverCoroutine(npcPos));
        }
    }

    IEnumerator FallOverCoroutine(Transform npcPos)
    {
        isFallen = true;
        PlayHitSound();
        originalRotation = transform.rotation;
        //float originalAngleDrag = rb.angularDamping;
        //rb.angularDamping = 4f;
        float originalHeight = transform.position.y;
        rb.constraints = RigidbodyConstraints.None; // Allow full rotation
        // Apply a force to simulate falling
        Vector3 forceDir = (transform.position - new Vector3(npcPos.position.x, 2f, npcPos.position.z)).normalized;
        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
            );
        rb.AddTorque(randomTorque, ForceMode.Impulse);
        rb.AddForce(forceDir * fallForce, ForceMode.Impulse);

        yield return new WaitForSeconds(recoveryTime);

        // Reset position and rotation over time
        rb.isKinematic = true;
        Quaternion startRotation = transform.rotation;
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, originalRotation, elapsedTime);
            transform.position = Vector3.Lerp(startPos, new Vector3(startPos.x, originalHeight, startPos.z), elapsedTime);
            elapsedTime += Time.deltaTime * recoveryTime;
            yield return null;
        }

        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        transform.rotation = originalRotation;

        rb.freezeRotation = true;
        rb.isKinematic = false;
        //rb.angularDamping = originalAngleDrag;
        isFallen = false;

        FindFirstObjectByType<PlayerCam>().StartCoroutine("RecoveryCoroutine");
    }

    private void PlayHitSound()
{
    if (hitClip != null)
    {
        audioSource.PlayOneShot(hitClip);
        Debug.Log("Falling sound played!");
    }
}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AI"))
        {
            FallOver(collision.transform);
        }
    }

    //private void Jump()
    //{
    // reset y velocity
    //    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    //    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    //}
    //private void ResetJump()
    //{
    //    readyToJump = true;
    //}
}
