using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Weapon : MonoBehaviour, IFireable
{
    [HideInInspector] public float cooldown;

    [SerializeField] float levitateDuration;
    [SerializeField] float levitateDistance;
    [SerializeField] float levitateUpwardsPullbackDistance;

    [SerializeField] float dashDuration;
    [SerializeField] float dashDistance;


    GameObject player;
    Rigidbody playerRigid;
      
    float elapsedTime = 0f;

    bool acceptPlayerInputs;


    void Awake()
    {
        // get player's actual controller
        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        playerRigid = player.GetComponent<Rigidbody>();

        
    }

    
    public float Cooldown
    {
        get {
            return levitateDuration + dashDuration + (Time.deltaTime * 3);  // (adding an extra offset)
        }
        set {
            throw new NotImplementedException();
        }
    }


    public void Fire(GameObject instance) 
    {
        StartCoroutine(StartLevitation(player.transform));                
    }


    IEnumerator StartLevitation(Transform playerTransform)
    {
        acceptPlayerInputs = player.GetComponent<FirstPersonMovement>().acceptingMovementInput = false;

        elapsedTime = 0f;
        Vector3 startPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z);
        Vector3 endPosition = player.transform.localPosition
                              + playerTransform.up * levitateDistance
                              + -playerTransform.forward * levitateUpwardsPullbackDistance;

        // Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + levitateDistance, startPosition.z);


        Vector3 endDashPosition = player.transform.localPosition + playerTransform.forward * dashDistance;


        while (elapsedTime < levitateDuration)
        {
            player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / levitateDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.localPosition = endPosition;

        yield return StartCoroutine(StartDash(playerTransform, endPosition, endDashPosition));        
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
        acceptPlayerInputs = player.GetComponent<FirstPersonMovement>().acceptingMovementInput = true;

        // tell x to reset cooldown

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
