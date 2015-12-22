using UnityEngine;
using System.Collections;

public class Follower1 : MonoBehaviour {

    public bool fuzzify = false;
    public float initWait = 1, trackWait = 0.5f, trackFuzz = 1, moveFuzz = 1;
	public GameObject player;

    private bool searching = false, moving = false;
    private Vector3 target;

	void Start () {
        if (!player)
            player = GameObject.Find("Player");
        StartCoroutine(Search());
	}
	
	void Update ()
    {
        if (!player)
        {
            Debug.Log("No player");
            return;
        }
        if(searching)
        {
            StartCoroutine(Track());
        }
        if(moving)
        {
            Debug.Log("moving from" + transform.position + " to " + target);
            Vector3 detour = target - transform.position;
            transform.position = transform.position + detour * Time.deltaTime;
            if ((transform.position - target).sqrMagnitude < 0.1f)
            {
                moving = false;
                searching = true;
            }
        }
        
    }

    IEnumerator Search()
    {
        yield return new WaitForSeconds(initWait - moveFuzz * Random.Range(0f, 1f));
        searching = true;
    }

    IEnumerator Track()
    {
        while (true)
        {
            Debug.Log("tracking " + player.transform.position);
            searching = false;
            target = player.transform.position;
            if (fuzzify)
                target = Fuzzify(target);
            yield return new WaitForSeconds(trackWait - moveFuzz * Random.Range(0f, 1f));
            moving = true;
        }
    }

    Vector3 Fuzzify(Vector3 target)
    {
        target = new Vector3(target.x + trackFuzz * Random.Range(-1f, 1f), 
            target.y + trackFuzz * Random.Range(-1f, 1f),
            target.z + trackFuzz * Random.Range(-1f, 1f));
        return target;
    }
}
