using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour
{

    public float turnSpeed = 150.0f;
    public Transform player;

    public Vector3 offset;

    void Start()
    {
        //offset = new Vector3(player.position.x, player.position.y + 2.0f, player.position.z + 7.0f);
        //offset = new Vector3(10, 10, 10);
    }

    void LateUpdate()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, Vector3.up) * offset;
        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }
}