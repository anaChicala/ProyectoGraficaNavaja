using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip MusicClip;

    [SerializeField] private AudioSource MusicSource;
    // Start is called before the first frame update

    private void Start()
    {
        MusicSource.clip = MusicClip;
        
    }

    public void PlaySound()
    {
        MusicSource.Play();
        
    }
   

    
}
