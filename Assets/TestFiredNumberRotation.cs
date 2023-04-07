using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFiredNumberRotation : MonoBehaviour
{
    [SerializeField] float z_RotationSpeed = 5f;
    
    void FixedUpdate() {
        
        // transform.Rotate(new Vector3(0, 0, z_RotationSpeed));
        // transform.Rotate(new Vector3(0, -180f, 0));
    }
}
