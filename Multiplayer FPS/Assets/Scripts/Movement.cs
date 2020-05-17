using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Variables
    public float speed;
    public float sprintModifier;
    public float jumpForce;
    public Camera normalCam;

    Rigidbody rb;

    private float baseFOV;
    private float sprintFOVModifier = 1.25f;
    public Transform groundDetector;
    public LayerMask ground;
    #endregion

    #region Monobehavior Callbacks
    void Start()
    {
        baseFOV = normalCam.fieldOfView;
        Camera.main.enabled = false;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Axels
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");

        //Controls
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool jump = Input.GetKeyDown(KeyCode.Space);

        //States
        bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;

        //JUMPING
        if (isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }

    }


    void FixedUpdate()
    {
        //Axels
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");
        
        //Controls
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool jump = Input.GetKeyDown(KeyCode.Space);

        //States
        bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;
         
        



        //Movement
        Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);

        t_direction.Normalize();


        float t_adjustedSpeed = speed;

        if (isSprinting) t_adjustedSpeed *= sprintModifier;


        Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * t_adjustedSpeed * Time.deltaTime;

        t_targetVelocity.y = rb.velocity.y;

        rb.velocity = t_targetVelocity;

        //FOV
        if(isSprinting)
        {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV *sprintFOVModifier, Time.deltaTime * 8f);
        }
        else
        {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
        }

    }
    #endregion
}
