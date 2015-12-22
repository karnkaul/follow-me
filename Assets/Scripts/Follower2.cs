using UnityEngine;
using System.Collections;

/*
* This script contains two tracking behaviours: to follow (come to rest at a specified distance from the target), or to chase (crash with target).
* The GameObject turns towards the target at a fixed radius and moves continuously forward (local +Y).
*/

public class Follower2 : MonoBehaviour, IFollower {

    public enum Tracking { Chase, Follow };

    // Default tracking
    public Tracking tracking = Tracking.Chase;
    
    // Lerp t value for damping (not used if tracking == Chase)
    [Range(0.1f, 0.9f)]
    public float stopDamper = 0.25f;

    [HideInInspector]
    public bool publicDoNotMove = false;
    
    // Normalised exposed values
    public float turnRadius = 1, moveSpeed = 1, stopDistance = 1;
    public GameObject target;

    // Used to dampen stopping motion
    private bool _slowToStop = false, _doNotMove = false, _delayedStop = false;
    private float turnSpeed, stopDistanceMultipler = 200, angle;
    private Rigidbody rb;

	void Start ()
    {
        if (!target)
            target = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        turnSpeed = 5 / turnRadius;
    }
	
	void Update ()
    {
        Vector3 vectorToTarget = target.transform.position - transform.position;
        Vector3 cross = Vector3.Cross(transform.up, vectorToTarget.normalized);
        angle = 0;
        if(cross.sqrMagnitude > 0.01f) { 
            if (cross.z > 0)
                // Rotate clockwise
                angle = 1;
            else
                // Rotate counter-clockwise
                angle = -1;
        }
    }

    void FixedUpdate()
    {
        if (tracking == Tracking.Follow)
        {
            if ((target.transform.position - transform.position).sqrMagnitude < stopDistance * stopDistanceMultipler)
            {
                _slowToStop = true;
            }
            else if(!publicDoNotMove)
            {
                _slowToStop = false;
            }
        }
        if(_slowToStop)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, stopDamper);
            if(rb.velocity.sqrMagnitude < 1f)
            {
                rb.velocity = Vector3.zero;
                _slowToStop = false;
                
                if(!_delayedStop)
                {
                    _doNotMove = true;
                    StartCoroutine(DelayStop());
                    _delayedStop = false;
                }
            }
            return;
        }
        if (publicDoNotMove)
        {
            _slowToStop = true;
            return;
        }
        if (!_doNotMove)
            rb.velocity = transform.up * moveSpeed * 30;

        
    }

    void LateUpdate()
    {
        transform.Rotate(0, 0, angle * (turnSpeed * 25) * Time.deltaTime);
    }

    IEnumerator DelayStop()
    {
        float random = (float)Random.Range(0, 16) / 10;
        yield return new WaitForSeconds(random);
        _doNotMove = false;
    }

    public void ToggleMotion()
    {
        publicDoNotMove = !publicDoNotMove;
    }
}
