using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SlowTextManager : MonoBehaviour
{
    private float letterPause = 0.05f;
    public AudioClip sound;
    private AudioSource audioSource;
    public Text text;

    private string message;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        text = GetComponent<Text>();
        message = text.text;
        text.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            text.text += letter;
            if (sound)
                audioSource.PlayOneShot(sound);
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
    }
}
