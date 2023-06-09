using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Weapon : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Rigidbody playerRigid;
    bool acceptPlayerInputs;

    [Space(10)]
    [SerializeField] float dashSpeed = 2f;
    [SerializeField] float dashDuration = 2f;
    

    // bool alreadyDashing;
    FirstPersonLook firstPersonLookCamera;
    float originalLookSensitivity;

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
        acceptPlayerInputs = player.GetComponent<FirstPersonMovement>().acceptingMovementInput;
        firstPersonLookCamera = player.GetComponentInChildren<FirstPersonLook>();        
    }


    public void Fire(GameObject weapon) =>
        StartCoroutine(StartDash());


    IEnumerator StartDash()
    {
        Debug.Log("dash began firing");
        acceptPlayerInputs = false;

        originalLookSensitivity = firstPersonLookCamera.sensitivity;
        firstPersonLookCamera.sensitivity = 0;

        // add look at + latch onto enemy.

        while (isDashing)
        {
            playerRigid.velocity = transform.forward * dashSpeed;
            yield return new WaitForSeconds(dashDuration);
        }
        
        isDashing = false;
        acceptPlayerInputs = true;

        firstPersonLookCamera.sensitivity = originalLookSensitivity;
        transform.rotation = Quaternion.identity;
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
}
