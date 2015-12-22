using UnityEngine;
using System.Collections;

public class CameraTracker : MonoBehaviour {

    public float actionPlaneRadius;

    private GameObject player;
    private Vector3 playerProjection;

    void Start()
    {
        player = GameObject.Find("Player");
    }

	void LateUpdate ()
    {
        playerProjection = player.transform.position;
        playerProjection.z = transform.position.z;
        Vector3 distanceToPlayer = playerProjection - transform.position;

       TrackPlayer(distanceToPlayer);
	}

    void TrackPlayer(Vector3 distanceToPlayer)
    {
        float trackSpeed = player.GetComponent<PlayerController>().FinalSpeed;
        if (distanceToPlayer.sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, playerProjection, 15 * trackSpeed * Time.deltaTime);
            return;
        }
        transform.position = playerProjection;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, actionPlaneRadius);
    }
}
