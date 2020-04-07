using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour
{
    public bool alreadyTalking = false;
    [Header("Dialogue Box")]
    public Animator animator;
    public Text nameText;
    public Text dialogueText;

    [Header("Player Input")]
    public Orbit mainCamera;
    public NoJumpController player;
    
    private bool endDialogue = false;
    private AudioClip voiceClip;
    private AudioSource asource;

    private string sentence;
    private Queue<string> sentences;


    
    void Start()
    {
        asource = GetComponent<AudioSource>();
        sentences = new Queue<string>();
    }
    private void Update()
    {
        if (alreadyTalking)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                DisplayNextSentence();
            }
            if (Input.GetKeyDown("e"))
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if(dialogue.sentences.Length == 0)
        {
            return;
        }
        alreadyTalking = true;
        setInputEnabled(false);
        endDialogue = false;
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        voiceClip = dialogue.sound;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            endDialogue = true;
        }
        if (UI.displaying_text)
        {
            UI.stopText();
            dialogueText.text = sentence;
        } else
        {
            if (endDialogue)
            {
                EndDialogue();
                return;
            }
            sentence = sentences.Dequeue();
            UI.changeDialogue(dialogueText, sentence, asource, voiceClip);
        }
    }

    public void EndDialogue()
    {
        setInputEnabled(true);
        UI.stopText();
        animator.SetBool("IsOpen", false);
        StartCoroutine(CoolDown(0.3f));
    }

    private void setInputEnabled(bool state)
    {
        if (mainCamera)
        {
            mainCamera.enabled = state;
        } else
        {
            Debug.Log("Camera Component not set in inspector");
        }
        if (player)
        {
            player.enabled = state;
            player.gameObject.GetComponent<Animator>().SetBool("Running", false);
        }
        else
        {
            Debug.Log("Player Component not set in inspector");
        }
    }

    private IEnumerator CoolDown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        alreadyTalking = false;
    }
}
