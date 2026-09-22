using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private AudioSource source;
    public List<AudioClip> music = new List<AudioClip>();

    void Start()
    {
        int randomIndex = Random.Range(0, music.Count);
       source.PlayOneShot(music[randomIndex]);

    }

  
}
