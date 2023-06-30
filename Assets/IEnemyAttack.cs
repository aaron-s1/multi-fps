using System.Collections;
using UnityEngine;


public interface IEnemyAttack
{
    void PrepareAttack();
    IEnumerator BeginAttack(Transform playerPos);
    bool CanAttack();
    // float Cooldown { get; set; }
}
