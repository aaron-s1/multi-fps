using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [HideInInspector] public bool acceptingMovementInput = true;

    Rigidbody rigidbody;

    // [Header("Running")]
    // bool canRun = true;        
    // public bool IsRunning { get; private set; }

   
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    

    void Awake() =>
        rigidbody = GetComponent<Rigidbody>();


    void FixedUpdate()
    {        
        if (!acceptingMovementInput)
            return;

        float targetMovingSpeed = moveSpeed;
        if (speedOverrides.Count > 0)
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();


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