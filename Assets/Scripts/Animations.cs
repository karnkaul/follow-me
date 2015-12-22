using UnityEngine;
using System.Collections;

public static class Animations {

    public delegate void VoidFunc();

    private const float ttl = 100f;

	public static IEnumerator AlternateTextures(Renderer rend, Texture one, Texture two, float alteratingTime, float totalTime=0, float initWait = 0, VoidFunc callAtEnd=null)
    {
        yield return new WaitForSeconds(initWait);
        float elapsedTime = 0;
        if (totalTime == 0)
            totalTime = ttl;
        while (elapsedTime < totalTime)
        {
            rend.material.mainTexture = two;
            yield return new WaitForSeconds(alteratingTime);
            rend.material.mainTexture = one;
            yield return new WaitForSeconds(alteratingTime);
            elapsedTime += 2 * alteratingTime;
        }
        if (callAtEnd != null)
            callAtEnd();
    }

    public static IEnumerator ScaleToZero(GameObject obj, VoidFunc callAtZero=null, bool rotate=false, float initWait = 0f, float factor = 0.8f)
    {
        yield return new WaitForSeconds(initWait);
        Transform transform = obj.transform;
        while(transform.localScale.sqrMagnitude > 0.1f)
        {
            if(rotate)
                transform.Rotate(0, 0, 20);
            transform.localScale *= factor;
            //yield return new WaitForSeconds(time);
            yield return null;
        }
        if(callAtZero != null)
            callAtZero();
    }
}
