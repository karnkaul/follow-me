using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour
{
    public float radius = 2, activationDistance = 5;
    public GameObject player, target;
    public SpriteRenderer spr;

    private Vector3 vectorToTarget;

    void Start()
    {
        if (!target)
            target = GameObject.Find("Companion");
        if (!player)
            player = GameObject.Find("Player");
        spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        vectorToTarget = target.transform.position - player.transform.position;
        vectorToTarget.z = 0;
        Debug.Log(vectorToTarget.sqrMagnitude);
        if (vectorToTarget.sqrMagnitude > activationDistance)
        {
            Color opaque = new Color(1f, 1f, 1f, 1f);
            spr.color = Color.Lerp(spr.color, opaque, Time.deltaTime);
            Debug.DrawLine(player.transform.position, target.transform.position);
            transform.position = player.transform.position;
            transform.position += radius * vectorToTarget.normalized;
            transform.rotation = Quaternion.LookRotation(vectorToTarget, Vector3.forward);
            transform.Rotate(90, 0, 0);
            
            gameObject.GetComponent<Renderer>().enabled = true;
        }

        else
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            spr.color = new Color(1f, 1f, 1f, 0);
        }
    }


}
