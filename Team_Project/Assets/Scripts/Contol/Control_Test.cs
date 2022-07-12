using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_Test : MonoBehaviour
{
    float speed = 10f;
    public GameObject player;

    void Update () 
    {
        Move();
        Rotate();
    }
    void Move()
    {
        if(Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if(Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        if(Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        if(Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void Rotate()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
	    float rayLength;

        if(GroupPlane.Raycast(cameraRay, out rayLength))

        {		
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            player.transform.LookAt(new Vector3(pointTolook.x, player.transform.position.y, pointTolook.z));
        }
    }

}