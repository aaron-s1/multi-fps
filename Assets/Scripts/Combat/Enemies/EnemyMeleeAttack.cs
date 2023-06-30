using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack
{
    bool canAttack = true;

    void Awake()
    {
        
    }


    public void PrepareAttack() =>
        throw new System.NotImplementedException();


    public IEnumerator BeginAttack(Transform playerPos) =>
        throw new System.NotImplementedException();


    public bool CanAttack() => canAttack;
}
