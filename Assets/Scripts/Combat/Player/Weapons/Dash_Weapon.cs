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
    // [SerializeField] float dashDuration = 2f;
    

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
        StartCoroutine(StartLevitation2(player.transform));
        // StartCoroutine(StartDash(player.transform));

    }

    void FixedUpdate()
    {
        
    }

    // public Transform startMarker; // Starting position
    // public Transform endMarker; // Ending position
    float elapsedTime = 0f; // Elapsed time for the ler

    [Space(40)]
    Vector3 startLevitatePosition;
    Vector3 endLevitatePosition;

    public float levitateDuration;
    public float levitateDistance;

    Vector3 endDashPosition;

    public float dashDuration;
    public float dashDistance;



    IEnumerator StartLevitation2(Transform playerTransform)
    {
        acceptPlayerInputs = player.GetComponent<FirstPersonMovement>().acceptingMovementInput = false;

        elapsedTime = 0f;
        startLevitatePosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
        endLevitatePosition = new Vector3(startLevitatePosition.x, startLevitatePosition.y + levitateDistance, startLevitatePosition.z);

        // endDashPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z + dashDistance);
        endDashPosition = player.transform.localPosition + playerTransform.forward * dashDistance;


        while (elapsedTime < levitateDuration)
        {
            player.transform.localPosition = Vector3.Lerp(startLevitatePosition, endLevitatePosition, elapsedTime / levitateDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.localPosition = endLevitatePosition;

        yield return StartCoroutine(StartDash2(playerTransform));        
    }

    IEnumerator StartDash2(Transform playerTransform)
    {
        elapsedTime = 0f;

        // endLevitatePosition = GetLerpTarget(playerTransform, levitateDistance);

        while (elapsedTime < dashDuration)
        {
            player.transform.localPosition = Vector3.Lerp(endLevitatePosition, endDashPosition, elapsedTime / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        player.transform.localPosition = endDashPosition;
        acceptPlayerInputs = player.GetComponent<FirstPersonMovement>().acceptingMovementInput = false;
        Debug.Log("dash has ended");
        Debug.Log(player.transform.localPosition);

        yield break;
    }



    // Vector3 GetLerpTarget(Transform playerTransform, Vector3 distanceOffset) 
    // {
    //     return new Vector3 (
    //     player.transform.localPositionx + distanceOffset.x,
    //     player.transform.localPositiony + distanceOffset.y,
    //     player.transform.localPositionz + distanceOffset.z
    //     );
    // }


    IEnumerator StartLevitation(Transform playerTransform)
    {
        acceptPlayerInputs = player.GetComponent<FirstPersonMovement>().acceptingMovementInput = false;
        // Invoke("EndLevitation", levitationTime);
        isLevitating = true;
        // playerRigid.useGravity = false;

        // This makes the  while loop not work.
        // playerRigid.isKinematic = true;

        while (isLevitating)
        {
            playerRigid.velocity = playerTransform.up * 3f;
            yield return null;
        }

        if (!isLevitating & !isDashing)
            StartCoroutine(StartDash(playerTransform));

        yield return null;
    }

    void EndLevitation()
    {
        // playerRigid.useGravity = true;
        playerRigid.isKinematic = false;
        isLevitating = false;
    }

    
    bool isLevitating;
    // public float levitationTime;


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
            playerRigid.velocity = -playerTransform.up * dashSpeed * 0.2f;
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
