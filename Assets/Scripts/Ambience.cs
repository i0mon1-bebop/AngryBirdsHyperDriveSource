using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{

   
    [SerializeField] private AudioSource ambienceSource;

    [SerializeField] private AudioClip music1;
    [SerializeField] private AudioClip music2;

    // Start is called before the first frame update
    void Start()
    {
        ambienceSource.PlayOneShot(music1);
        ambienceSource.PlayOneShot(music2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
