using UnityEngine;
using System.Collections;

public class mORB_Weapon : MonoBehaviour, IFireable
{
    [SerializeField] Transform player;

    [SerializeField] float moveDistance = 12f;

    [Tooltip("Lowering value increases speed.")]
    [SerializeField] float throwSpeed = 0.1f;
    [Tooltip("Lowering value increases speed.")]
    [SerializeField] float returnSpeed = 0.1f;

    // Populate this to make sure [m]Orb displays at intended UI position.
    [SerializeField] Vector3 UI_offsetPosition;

    Vector3 startPositionLocal;
    Quaternion startRotationLocal;

    bool isMoving;
    bool onCooldown;
    bool canKillEnemy;

    static GameObject prefabInstance;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);

        transform.parent = player;

        transform.localPosition = startPositionLocal = UI_offsetPosition;

        startRotationLocal = transform.localRotation;
    }


    public void Fire(GameObject weapon) {
        if (onCooldown || isMoving)
            return;

        onCooldown = true;
        
        transform.parent = player;

        StartCoroutine(FireTheMORB());
    }



    IEnumerator FireTheMORB() {
        onCooldown = isMoving = canKillEnemy = true;
        
        Vector3 forwardDirection = player.forward;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + forwardDirection * moveDistance;

        transform.parent = null;
        
        float t = 0f;
        while (t < throwSpeed)
        {
            t += Time.deltaTime;

            transform.position = Vector3.Lerp(startPosition, endPosition, t / throwSpeed);

            yield return null;
        }


        t = 0f;
        while (t < throwSpeed)
        {
            transform.parent = player;
            t += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(transform.localPosition, startPositionLocal, t / returnSpeed);
            transform.localEulerAngles = new Vector3(0,0,0);

            yield return null;
        }

        transform.localPosition = startPositionLocal;

        onCooldown = isMoving = canKillEnemy = false;
    }


    void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy")
        {
            if (canKillEnemy)
                other.gameObject.GetComponent<EnemyDies>().Die();
        }
    }


    public bool OnCooldown() => onCooldown;
}
