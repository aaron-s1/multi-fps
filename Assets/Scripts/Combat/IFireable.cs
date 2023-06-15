using UnityEngine;

public interface IFireable
{
    void Fire(GameObject weaponPrefab);
    float Cooldown { get; set; }
}
