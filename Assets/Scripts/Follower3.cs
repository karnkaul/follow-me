using UnityEngine;
using System.Collections;

public class Follower3 : MonoBehaviour, IFollower
{
    public bool moveOnStart = true;

    [Range(0.1f, 0.9f)]
    public float stopDamper = 0.25f;

    public float stopDistance = 25, turnSpeed = 1, moveSpeed = 1;
    public string status;
    public Transform target;

    public AudioSource audioSource;
    public AudioClip anchor;
    [Range(0.1f, 1)]
    public float volume = 0.5f;

    // Make private after debugging. Only public and serializable to show in inspector.
    public enum State { Moving, Retarding, Stopped, Ready };
    public State state;
    [System.Serializable]
    public struct Properties
    {
        public Vector3 planarDistance;
        public bool anchored;
    }
    public Properties properties;
    // end

    private bool _move = true, _motionDelayed = false, _retarding = false;
    private float angle;
    private Rigidbody rb;
    private Animator animator;

    void Start ()
    {
        if (!target)
            target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        properties.anchored = !moveOnStart;
        _move = moveOnStart; // Transfer

        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
	}

    void Update ()
    {
        properties.planarDistance = new Vector3(target.position.x, target.position.y, 0) - transform.position;

        // Reset()
        if (state == State.Ready)
        {
            //properties.anchored = false;
            if (properties.planarDistance.sqrMagnitude < stopDistance)
                return;
            state = State.Moving;
        }

        // Retard()
        if (state == State.Retarding)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, stopDamper);
            // Stop()
            if (rb.velocity.sqrMagnitude < 1f)
            {
                rb.velocity = Vector3.zero;
                state = State.Stopped;
                //properties.anchored = true;
                StartCoroutine(DelayMotion());
            }
        }

        // Don't move if anchored
        if (properties.anchored)
            return;

        // Move()
        if (!properties.anchored && state == State.Moving)
        {
            if (properties.planarDistance.sqrMagnitude < stopDistance)
            {
                state = State.Retarding;
                _move = false;
                if (!_motionDelayed)
                {
                    StartCoroutine(DelayMotion());
                    _motionDelayed = true;
                }
            }
            rb.velocity = transform.up * moveSpeed * 8;
        }
    }

    void LateUpdate()
    {
        // LookAtTarget()
        Vector3 vectorToTarget = target.transform.position - transform.position;
        Vector3 cross = Vector3.Cross(transform.up, vectorToTarget.normalized);
        angle = 0;
        if (cross.sqrMagnitude > 0.01f)
        {
            if (cross.z > 0)
                // Rotate clockwise
                angle = 1;
            else
                // Rotate counter-clockwise
                angle = -1;
        }
        transform.Rotate(0, 0, angle * (turnSpeed * 200) * Time.deltaTime);
    }

    IEnumerator DelayMotion()
    {
        float random = (float)Random.Range(0, 16) / 10;
        yield return new WaitForSeconds(random);
        //properties.anchored = false;
        state = State.Ready;
    }

    // Interface method. Controllers call IFollower.ToggleMotion()
    public void ToggleMotion()
    {
        if (state != State.Stopped)
            state = State.Retarding;
        else
            state = State.Ready;
        properties.anchored = !properties.anchored;
        animator.SetBool("isAnchored", properties.anchored);
        audioSource.PlayOneShot(anchor, volume);
    }
}
