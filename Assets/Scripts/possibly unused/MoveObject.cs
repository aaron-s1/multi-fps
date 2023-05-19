using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {

    [SerializeField] public float moveDistance = 12f; // The distance to move on the Z axis
    [SerializeField] public float throwSpeed = 0.01f; // The time it takes to move the object
    [SerializeField] public float returnSpeed = 0.01f;

    private Vector3 startPosition; // The starting position of the object
    private Vector3 endPosition; // The end position of the object
    private bool isMoving = false; // Whether the object is currently moving

    public bool canKillEnemy;

    [SerializeField] public GameObject player;

    void Start () {
        startPosition = transform.localPosition; // Record the starting position of the object
        endPosition = startPosition + new Vector3(0, 0, moveDistance); // Calculate the end position of the object
    }

    void Update () {
        if (!isMoving) {
        }
    }

    IEnumerator MoveObjectCoroutine() {
        player.GetComponent<FireWeapon>().ammoLevel.text = "0";
        isMoving = true;
        canKillEnemy = true;

        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0, 0, moveDistance);

        float t = 0f;
        while (t < throwSpeed) {
            t += Time.deltaTime;
            float step = moveDistance / throwSpeed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t / throwSpeed);
            yield return null;
        }

        t = 0f;
        while (t < throwSpeed) {
            t += Time.deltaTime;
            float step = moveDistance / throwSpeed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(endPosition, startPosition, t / throwSpeed * returnSpeed); // use the returnSpeed variable to control the speed of the return movement
            yield return null;
        }

        player.GetComponent<FireWeapon>().ammoLevel.text = "1";

        canKillEnemy = false;

        transform.localPosition = startPosition;
        isMoving = false;
    }

    public void StartMoving() {
        if (!isMoving) {
            StartCoroutine(MoveObjectCoroutine()); // Start the coroutine to move the object
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy")
        {
            if (canKillEnemy)
                other.gameObject.GetComponent<EnemyDies>().Die();
        }
    }
}
