using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float runSpeed = 9;
    [SerializeField] KeyCode runningKey = KeyCode.I;
    [SerializeField] KeyCode dashKey = KeyCode.N;

    // Not used. Keep run speed at normal speed, running key at where it won't be triggered.
    [Header("Running")]
    bool canRun = true;        
    public bool IsRunning { get; private set; }

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
    [SerializeField] float tiltCameraFrequency = 0.5f; 
    [SerializeField] float tiltCameraDuration = 5f;
    [SerializeField] float tiltCameraRange = 1f;
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
    }


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