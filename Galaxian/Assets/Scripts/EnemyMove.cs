using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
    
    public Boundary bound;

    private float targetManeuverX;
    private float targetManeuverY;

    public float smoothing;

    private Rigidbody2D rb;

    void Start() {
        StartCoroutine(Evade());
        rb = GetComponent<Rigidbody2D>();
    }

    IEnumerator Evade() {
        yield return new WaitForSeconds(Random.Range(6, 10));
        while (true) {
            targetManeuverX = Random.Range(1, 3f) * -Mathf.Sign(transform.position.x);
            if (GameController.level >= 2)
                targetManeuverY = Random.Range(1, 3f) * -Mathf.Sign(transform.position.y);

            yield return new WaitForSeconds(Random.Range(1f, 5f));

            targetManeuverX = 0;
            targetManeuverY = 0;

            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }
    void FixedUpdate() {
        float newManeuverx = Mathf.MoveTowards(rb.velocity.x, targetManeuverX, Time.deltaTime * smoothing);
        float newManeuvery = Mathf.MoveTowards(rb.velocity.y, targetManeuverY, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newManeuverx, newManeuvery, 0.0f);
        rb.position = new Vector3(Mathf.Clamp(rb.position.x, bound.Xmin, bound.Xmax), Mathf.Clamp(rb.position.y, bound.Ymin, bound.Ymax), 0.0f);
    }
}
