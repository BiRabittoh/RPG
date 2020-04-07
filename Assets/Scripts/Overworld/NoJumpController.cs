using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoJumpController : MonoBehaviour
{
    public float moveSpeed;
    public CharacterController controller;
    public float gravityScale;
    public Transform cameraTransform;
    private Animator anim;
    private Vector3 moveHorizontal;
    private Vector3 moveVertical;
    

    //private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        moveHorizontal = (cameraTransform.forward * Input.GetAxis("Vertical")) + (cameraTransform.right * Input.GetAxis("Horizontal"));
        moveHorizontal = moveHorizontal.normalized * moveSpeed;
        moveHorizontal.y = 0;
        if (controller.isGrounded)
        {
            moveVertical.y = 0f;
        } else
        {
            moveVertical.y = moveVertical.y + (Physics.gravity.y * gravityScale);
        }
        

        

        controller.Move((moveHorizontal + moveVertical)* Time.deltaTime);

        if (moveHorizontal != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveHorizontal.x, 1, moveHorizontal.z)), 0.15F);
            anim.SetBool("Running", true);
        } else
        {
            anim.SetBool("Running", false);
        }
            
    }
}
