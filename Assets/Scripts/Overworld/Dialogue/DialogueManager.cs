using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour
{
    public NPC alreadyTalking = null;
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

    public delegate void MyDelegate();
    static public MyDelegate actionAfterDialogue;


    
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

    public void StartDialogue(NPC npc)
    {
        if(npc.dialogue.sentences.Length == 0)
        {
            return;
        }
        actionAfterDialogue += new MyDelegate(npc.actionAfterDialogue);
        alreadyTalking = npc;
        setInputEnabled(false);
        endDialogue = false;
        animator.SetBool("IsOpen", true);
        nameText.text = npc.dialogue.name;
        voiceClip = npc.dialogue.sound;

        sentences.Clear();

        foreach(string sentence in npc.dialogue.sentences)
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
        if(actionAfterDialogue != null)
            actionAfterDialogue();
        actionAfterDialogue = null;
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
        alreadyTalking = null;
    }
}
