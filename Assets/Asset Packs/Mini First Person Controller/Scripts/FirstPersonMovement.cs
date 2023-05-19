using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    // Not used. Keep run speed at normal speed, running key at where it won't be triggered.
    [Header("Running")]
    public bool canRun = true;        
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.I;
    public KeyCode dashKey = KeyCode.N;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    #region Dash variables
    [Space(10)]
    [SerializeField] float dashSpeed = 2f;
    [SerializeField] float dashDuration = 2f;
    public bool isDashing;
    public bool dashIsActiveWeapon = true;
    public bool canSwapWeapons = true;
    bool acceptingMovementInput = true;
    #endregion

    FirstPersonLook firstPersonLookCamera;
    float originalLookSensitivity;

    
    [Space(10)]
    public float tiltCameraFrequency = 0.5f; 
    public float tiltCameraDuration = 5f;
    public float tiltCameraRange = 1f;
    float tiltTimeElapsed = 0f;
    bool isTiltingForward = true;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        firstPersonLookCamera = GetComponentInChildren<FirstPersonLook>();
    }

    void Update()
    {
        if (Input.GetKeyDown(dashKey) & !isDashing & dashIsActiveWeapon)
            StartCoroutine(Dash());

        TiltCameraIfDashing();
    }

    void TiltCameraIfDashing()
    {
        if (!isDashing)
            return;

        tiltTimeElapsed += Time.deltaTime;

        if (tiltTimeElapsed >= tiltCameraFrequency)
        {
            tiltTimeElapsed = 0f;
            isTiltingForward = !isTiltingForward;
        }

        float tiltAmount = Mathf.PingPong(tiltTimeElapsed / tiltCameraFrequency, 1f);
        if (!isTiltingForward)
            tiltAmount = 1f - tiltAmount;

        float tiltZ = Mathf.Lerp(-tiltCameraRange, tiltCameraRange, Mathf.Abs(tiltAmount - 0.5f) * 2f);

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = tiltZ;
        transform.rotation = Quaternion.Euler(rotation);

        if (tiltTimeElapsed >= tiltCameraDuration)
            enabled = false;
    }


    IEnumerator Dash()
    {
        print("dashing");
        originalLookSensitivity = firstPersonLookCamera.sensitivity;
        firstPersonLookCamera.sensitivity = 0;

        acceptingMovementInput = canRun = false;
        isDashing = true;
        // add look at + latch onto enemy.

        while (isDashing)
        {
            canSwapWeapons = false;
            rigidbody.velocity = transform.forward * dashSpeed;
            yield return new WaitForSeconds(dashDuration);
            isDashing = false;
        }
        
        isDashing = false;
        acceptingMovementInput = true;
        canSwapWeapons = true;

        firstPersonLookCamera.sensitivity = originalLookSensitivity;
        transform.rotation = Quaternion.identity;

        print("dash ended");
    }


    // IEnumerator DashOld()
    // {
    //     print("dashing");
    //     originalLookSensitivity = firstPersonLookCamera.sensitivity;
    //     firstPersonLookCamera.sensitivity = 0;

    //     acceptingMovementInput = canRun = false;
    //     isDashing = true;
    //     // look at + latch onto enemy.

    //     float dashTimer = 0f;
    //     while (dashTimer < dashDuration)
    //     {
    //         dashTimer += Time.deltaTime;
    //         transform.position += transform.forward * dashSpeed * Time.deltaTime;
    //         yield return null;
    //     }
        
    //     isDashing = false;
    //     acceptingMovementInput = true;

    //     firstPersonLookCamera.sensitivity = originalLookSensitivity;
    //     transform.rotation = Quaternion.identity;

    //     print("dash ended");
    // }


    void FixedUpdate()
    {
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();

        if (!acceptingMovementInput)
            return;

        Vector2 targetVelocity = new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if (isDashing)
            {
                isDashing = false;
                StopCoroutine(Dash());
                Debug.Log("hit wall");
            }
        }
    }
}