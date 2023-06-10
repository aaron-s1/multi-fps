using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Weapon : MonoBehaviour, IFireable
{
    [SerializeField] GameObject player;
    [SerializeField] Rigidbody playerRigid;
    bool acceptPlayerInputs;

    [Space(10)]
    [SerializeField] float dashSpeed = 2f;
    [SerializeField] float dashDuration = 2f;
    

    FirstPersonLook playerCamera;
    float originalCameraSensitivity;

    bool canFire;
    bool isDashing;

    [Space(10)]
    [SerializeField] float tiltCameraFrequency = 0.5f; 
    [SerializeField] float tiltCameraDuration = 5f;
    [SerializeField] float tiltCameraRange = 1f;
    float tiltTimeElapsed = 0f;
    bool isTiltingForward = true;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        playerRigid = player.GetComponent<Rigidbody>();

        playerCamera = player.GetComponentInChildren<FirstPersonLook>();
        originalCameraSensitivity = playerCamera.sensitivity;
    }


    public void Fire(GameObject instance)
    {
        StartCoroutine(StartLevitation(player.transform));
        // StartCoroutine(StartDash(player.transform));
    }


    IEnumerator StartLevitation(Transform playerTransform)
    {
        acceptPlayerInputs = player.GetComponent<FirstPersonMovement>().acceptingMovementInput = false;
        Invoke("EndLevitation", levitationTime);
        isLevitating = true;
        // playerRigid.useGravity = false;

        // This makes the  while loop not work.
        // playerRigid.isKinematic = true;

        while (isLevitating)
        {
            playerRigid.velocity = playerTransform.up * 1f;
            yield return null;
        }

        if (!isLevitating & !isDashing)
            StartCoroutine(StartDash(playerTransform));

        yield return null;
    }

    void EndLevitation()
    {
        playerRigid.useGravity = true;
        playerRigid.isKinematic = false;
        isLevitating = false;
    }

    
    bool isLevitating;
    public float levitationTime;


    IEnumerator StartDash(Transform playerTransform)
    {
        Invoke("EndDash", dashDuration);
        isDashing = true;
               
        // acceptPlayerInputs = player.GetComponent<FirstPersonMovement>().acceptingMovementInput = false;
        
        // add look at + latch onto enemy.
        
        while (isDashing)
        {            
            TiltCamera();
            playerRigid.velocity = playerTransform.forward * dashSpeed;
            yield return null;
        }

        player.GetComponent<FirstPersonMovement>().acceptingMovementInput = true;

        transform.rotation = Quaternion.identity;
    }

    void EndDash() 
    {
        playerCamera.sensitivity = originalCameraSensitivity;
        isDashing = false;
    }

    
   void TiltCamera()
    {
        playerCamera.sensitivity = 0;

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

        Vector3 rotation = playerCamera.transform.rotation.eulerAngles;

        rotation.z = tiltZ;
        playerCamera.transform.rotation = Quaternion.Euler(rotation);
    }
}
