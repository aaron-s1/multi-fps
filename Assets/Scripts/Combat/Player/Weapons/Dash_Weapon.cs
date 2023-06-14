using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Weapon : MonoBehaviour, IFireable
{

    [SerializeField] float levitateDuration;
    [SerializeField] float levitateDistance;

    [SerializeField] float dashDuration;
    [SerializeField] float dashDistance;

    GameObject player;
    Rigidbody playerRigid;
      
    float elapsedTime = 0f;

    bool acceptPlayerInputs;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        playerRigid = player.GetComponent<Rigidbody>();
    }


    public void Fire(GameObject instance) =>
        StartCoroutine(StartLevitation(player.transform));


    IEnumerator StartLevitation(Transform playerTransform)
    {
        acceptPlayerInputs = player.GetComponent<FirstPersonMovement>().acceptingMovementInput = false;

        elapsedTime = 0f;
        Vector3 startLevitatePosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
        Vector3 endLevitatePosition = new Vector3(startLevitatePosition.x, startLevitatePosition.y + levitateDistance, startLevitatePosition.z);

        Vector3 endDashPosition = player.transform.localPosition + playerTransform.forward * dashDistance;


        while (elapsedTime < levitateDuration)
        {
            player.transform.localPosition = Vector3.Lerp(startLevitatePosition, endLevitatePosition, elapsedTime / levitateDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.localPosition = endLevitatePosition;

        yield return StartCoroutine(StartDash(playerTransform, endLevitatePosition, endDashPosition));        
    }

    IEnumerator StartDash(Transform playerTransform, Vector3 endLevitatePosition, Vector3 endDashPosition)
    {
        elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            player.transform.localPosition = Vector3.Lerp(endLevitatePosition, endDashPosition, elapsedTime / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        player.transform.localPosition = endDashPosition;
        acceptPlayerInputs = player.GetComponent<FirstPersonMovement>().acceptingMovementInput = false;

        yield break;
    }


    // Tilt camera variables.
    // [SerializeField] float tiltCameraFrequency = 0.5f; 
    // [SerializeField] float tiltCameraDuration = 5f;
    // [SerializeField] float tiltCameraRange = 1f;
    // FirstPersonLook playerCamera;
    // float originalCameraSensitivity;
    // float tiltTimeElapsed = 0f;
    // bool isTiltingForward = true;

            // playerCamera = player.GetComponentInChildren<FirstPersonLook>();
            // originalCameraSensitivity = playerCamera.sensitivity;
    
//    void TiltCamera()
//     {
//         playerCamera.sensitivity = 0;

//         if (!isDashing)
//             return;        

//         tiltTimeElapsed += Time.deltaTime;

//         if (tiltTimeElapsed >= tiltCameraFrequency)
//         {
//             tiltTimeElapsed = 0f;
//             isTiltingForward = !isTiltingForward;            
//         }

//         float tiltAmount = Mathf.PingPong(tiltTimeElapsed / tiltCameraFrequency, 1f);
//         if (!isTiltingForward)
//             tiltAmount = 1f - tiltAmount;

//         float tiltZ = Mathf.Lerp(-tiltCameraRange, tiltCameraRange, Mathf.Abs(tiltAmount - 0.5f) * 2f);

//         Vector3 rotation = playerCamera.transform.rotation.eulerAngles;

//         rotation.z = tiltZ;
//         playerCamera.transform.rotation = Quaternion.Euler(rotation);
//     }
}
