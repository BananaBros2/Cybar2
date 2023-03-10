using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    bool airbourne = true;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();

    }

    private void FixedUpdate() // Update is called once per frame
    {
        if(view.IsMine)
        {
            if (airbourne)
            {
                transform.position += Physics.gravity * Time.deltaTime;
            }

            float xTranslation = Input.GetAxis("Horizontal") * Time.deltaTime;
            float zTranslation = Input.GetAxis("Vertical") * Time.deltaTime;

            if (xTranslation != 0 || zTranslation != 0)
            {
                Vector3 input = (new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))).normalized;
                view.transform.GetChild(0).transform.rotation = Quaternion.RotateTowards(transform.GetChild(0).transform.rotation, Quaternion.Euler(Vector3.up * Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg), 1000 * Time.deltaTime);
            }
            var normalizedVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            view.transform.Translate(normalizedVector.x * Time.deltaTime * 10, 0, normalizedVector.y * Time.deltaTime * 10);


            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        }

    }

    private void OnCollisionEnter() 
    {
        //print("collided with " + collidedObject.transform.tag);
        airbourne = false;
    }
    private void OnCollisionExit(Collision exitedObject) 
    {
        //print("exited " + exitedObject.transform.tag);
        airbourne = true;
        
    }

}