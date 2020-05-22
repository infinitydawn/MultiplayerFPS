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

    //Head BOB
    public Transform weaponParent;
    private Vector3 weaponParentOrigin;
    private float movementCounter;
    private float idleCounter;
    private Vector3 targetWeaponBobPosition;

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
        weaponParentOrigin = weaponParent.localPosition;
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


        //HEAD BOB

        if (t_hmove == 0 && t_vmove == 0) 
        { 
            headBob(idleCounter, 0.025f, 0.025f);
            idleCounter += Time.deltaTime;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);
        }
        else if(!isSprinting)
        { 
            headBob(movementCounter, 0.05f, 0.05f);
            movementCounter += Time.deltaTime * 5;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 6f);
        }
        else
        {
            headBob(movementCounter, 0.2f, 0.07f);
            movementCounter += Time.deltaTime * 8;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 20f);
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

    #region Private Methods
    void headBob(float p_z, float p_x_intensity, float p_y_intensity)
    {
        targetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intensity , 0);
    }


    #endregion
}
