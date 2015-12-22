using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    public float turnRadius = 1, moveSpeed = 1;
    public GameObject target;

    private float turnSpeed;
    private Rigidbody rb;

	void Start ()
    {
        if (!target)
            target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        turnSpeed = 10 / turnRadius;
    }
	
	void Update ()
    {
        Vector3 vectorToTarget = target.transform.position - transform.position;
        Vector3 cross = Vector3.Cross(transform.up, vectorToTarget.normalized);
        float angle = 0;
        if(cross.sqrMagnitude > float.Epsilon) { 
            if (cross.z > 0)
                // Rotate clockwise
                angle = 1;
            else
                // Rotate counter-clockwise
                angle = -1;
        }
        transform.Rotate(0, 0, angle * (turnSpeed * 10) * Time.deltaTime);
    }

    void FixedUpdate()
    {
        rb.velocity = transform.up * moveSpeed;
    }
}
