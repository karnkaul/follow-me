using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {

    public Texture originalTexture, highlightedTexture, completedTexture;
    public float highlightSpeed = 0.25f, highlightTime = 5;

    private int playerEntered = 0, followerEntered = 0;
    private Renderer rend;
    private Coroutine highlightAnimation, completedAnimation;

	void Start () {
        //texture = GetComponent<Renderer>().material.mainTexture;
        rend = GetComponent<Renderer>();
	}
	
	void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            if(playerEntered == 0)
                playerEntered = 1;
        }

        if(other.gameObject.name == "Companion")
        {
            if(followerEntered == 0 && playerEntered > 0)
                followerEntered = 1;
        }
    }

    void LateUpdate()
    {
        if (playerEntered == 1)
        {
            Animations.VoidFunc reset = ResetHighlight;
            highlightAnimation = StartCoroutine(Animations.AlternateTextures(rend, originalTexture, highlightedTexture, highlightSpeed, totalTime: highlightTime, callAtEnd: reset));
            playerEntered = 2;
        }
        if (followerEntered == 1 && playerEntered > 0)
        {
            StopCoroutine(highlightAnimation);
            rend.material.mainTexture = completedTexture;
            followerEntered = 2;
            Animations.VoidFunc destroy = Destroy;
            completedAnimation = StartCoroutine(Animations.ScaleToZero(gameObject, destroy, true, initWait: 0.5f, factor: 0.9f));
        }
    }

    
    void ResetHighlight()
    {
        playerEntered = 0;
    }

    void Destroy()
    {
        //Animate();
        Destroy(gameObject);
    }

}
