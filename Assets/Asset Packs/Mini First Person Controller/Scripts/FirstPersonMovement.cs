using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;

    // Not used. Keep run speed at normal speed, running key at where it won't be triggered.
    [Header("Running")]
    bool canRun = true;        
    public bool IsRunning { get; private set; }

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    public bool acceptingMovementInput = true;


    void Awake() =>
        rigidbody = GetComponent<Rigidbody>();


    void FixedUpdate()
    {
        // Get targetMovingSpeed.
        float targetMovingSpeed = moveSpeed;
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

        }
    }
}