using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public bool isSprite;
    public float speed = 1, turnRadius = 1;
    
    private float speedMultiplier = 0.25f;
    // finalSpeed = speedMultiplier [private] * speed [normalised, exposed global]
    private float finalSpeed;
    // Property is used by camera tracker
    public float FinalSpeed
    {
        get
        {
            return finalSpeed;
        }
    }
    private CharacterController c;
    //private Follower2 companion;
    private GameObject companion;

	void Start () {
        c = GetComponent<CharacterController>();
        companion = GameObject.Find("Companion");
	}

	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        finalSpeed = speed * speedMultiplier;
        Vector3 movement = new Vector3(h, v, 0);
        c.Move(finalSpeed * movement);
        if(movement.magnitude > 0.1f)
            Rotate(movement.normalized);

        if(Input.GetButtonDown("Fire1"))
        {
            // GetComponent will return FIRST encountered implementation, even if the script not active.
            companion.GetComponent<IFollower>().ToggleMotion();
        }
    }

    void Rotate(Vector3 direction)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        // The quad flips on y-axis if V3.up and direction are 180 degrees apart, i.e., when pressing straight down. 
        // Brute force fix.
        // UPDATE: Not required for a 2D sprite.
        if ( !isSprite && direction.x == 0 && direction.y < 0)
        {
            transform.Rotate(0, 180, 0);
        }
    }
}
