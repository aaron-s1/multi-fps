using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDies : MonoBehaviour
{
    
    public void Die()
    {
        Debug.Log("enemy called Die()");
        Destroy(GetComponent<SearchForPlayer>());
        GetComponent<Collider>().enabled = false;
        GetComponent<Animator>().SetTrigger("die");

    }
}
