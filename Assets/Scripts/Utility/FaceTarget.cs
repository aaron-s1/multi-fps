using System.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    
    void FixedUpdate()
    {
        transform.LookAt(target);
    }
}
