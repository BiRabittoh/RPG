using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class FollowAI : MonoBehaviour
{
    private Transform orc;
    private Animator anim;
    public bool gotem = false;
    private float moveSpeed = 1;
    
    void Start()
    {
        orc = transform.GetChild(0);
        anim = orc.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            gotem = true;
            //anim.SetTrigger("Spotted");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            gotem = true;
            anim.SetBool("Moving", true);
            orc.LookAt(other.transform);
            transform.Translate(orc.forward * Time.deltaTime * moveSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            gotem = false;
            anim.SetBool("Moving", false);
            //anim.SetTrigger("Lost");
        }
    }
}
