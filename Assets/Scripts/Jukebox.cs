using UnityEngine;
using System.Collections;

public class Jukebox : MonoBehaviour {

    [System.Serializable]
    public struct MusicFile
    {
        public AudioClip clip;
        public int repeat;
        public float volume;
    };

    public MusicFile[] clips;
    public AudioSource audioSource;

    private Coroutine play;

    void Start()
    {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
        if (clips.Length == 0)
        {
            Debug.Log("No audio clips attached.");
            gameObject.SetActive(false);
        }
        play = StartCoroutine(Play());
    }

	IEnumerator Play () {
        while (true)
        {
            foreach (MusicFile file in clips)
            {
                for (int i = 0; i <= file.repeat; ++i)
                {
                    audioSource.PlayOneShot(file.clip, file.volume);
                    yield return new WaitForSeconds(file.clip.length);
                }
            }
        }
	}
	
    public void End()
    {
        StopCoroutine(play);
    }

	void Update () {
	
	}
}
