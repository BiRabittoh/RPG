using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] buttonSounds = null;
    private AudioSource audioSource;

    public void playButtonSound(int index){
        if(index <= buttonSounds.Length)
            audioSource.PlayOneShot(buttonSounds[index]);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
