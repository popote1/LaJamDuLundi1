using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public List<AudioClip> Audio1;
    public List<AudioClip> Audio2;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound1()
    {
        AudioSource.PlayClipAtPoint(Audio1[Random.Range(0, Audio1.Count)],transform.position);

    }
    public void PlaySound2()
    {
        AudioSource.PlayClipAtPoint(Audio2[Random.Range(0, Audio2.Count)],transform.position,0.5f);
    }
}
