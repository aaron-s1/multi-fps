using UnityEngine;
using System.Collections;

public class mORB_Weapon : MonoBehaviour, IFireable
{
    [SerializeField] Transform player;

    // [SerializeField] Vector3 spawnLocation;

    [SerializeField] float moveDistance = 12f;
    [SerializeField] float throwSpeed = 0.01f;
    [SerializeField] float returnSpeed = 0.01f;

    Quaternion startRotation;
    // public Transform playerPosition;
    
    // Rename later? Populate this to make sure [m]Orb displays alongside UI. 
    [SerializeField] Vector3 UI_offsetPosition;

    // Transform player;
    // Vector3 startPos;
    // Vector3 endPos;

    public bool isMoving;
    bool onCooldown;
    bool canKillEnemy;

    static GameObject prefabInstance;

    void Awake()
    {
        // prefabInstance = this.gameObject;        
        // testing spawn/start location
        // startPosition = new Vector3 (-1.35f, 1.1f, 2.25f);
        // startPos = transform.position;

        // playerPos = GetComponentInParent<Transform>().position;
        // playerPos = GameObject.FindGameObjectWithTag("Player");
        
        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);

        transform.parent = player;

        transform.localPosition = UI_offsetPosition;                
        startRotation = transform.localRotation;
        // originalPosition = 
        // Debug.Log(player);
        // Invoke("FindPlayer", 3f);
        // Invoke("FindPlayer2", 3f);
        // player = GetComponentInParent<FirstPersonMovement>().transform;
        
        // startPosition = GameObject.FindGameObjectWithTag("TheMorb").transform.position;
        // endPos = startPos + new Vector3(0, 0, moveDistance);
    }

    // void FindPlayer()
    // {
    //     player = GameObject.FindGameObjectWithTag("Player").transform;
    //     Debug.Log(player);
    // }

    // void FindPlayer2()
    // {
    //     player = GetComponentInParent<FirstPersonMovement>().gameObject.transform;
    //     Debug.Log(player);
    // }

    public void Fire(GameObject weapon) {
        // if (prefabInstance == null)
            // Instantiate(weapon.equippedPrefab, startPosition, player.transform.rotation);
        
        if (onCooldown || isMoving)
            return;

        onCooldown = true;
        
        // prefabInstance.SetActive(true);
        transform.parent = player;
        UI_offsetPosition = transform.localPosition;
        startRotation = transform.localRotation;

        StartCoroutine(FireTheMORB());
    }



    IEnumerator FireTheMORB() {
        onCooldown = isMoving = canKillEnemy = true;

        transform.parent = null;

        Vector3 startPosition = transform.localPosition;
        Vector3 forwardDirection = player.forward;
        Vector3 endPosition = startPosition + forwardDirection * moveDistance;


        float t = 0f;
        while (t < throwSpeed) {
            Debug.Log("original pos = " + UI_offsetPosition);
            t += Time.deltaTime;
            float step = moveDistance / throwSpeed * Time.deltaTime;

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t / throwSpeed);

            yield return null;
        }

        t = 0f;
        while (t < throwSpeed) {
            transform.parent = player;
                        
            t += Time.deltaTime;
            float step = moveDistance / throwSpeed * Time.deltaTime;

            transform.localPosition = Vector3.Lerp(endPosition, UI_offsetPosition, t / throwSpeed * returnSpeed);

            yield return null;
        }

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
