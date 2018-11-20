﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelKontrol : MonoBehaviour {

    float speed = 40;
	
	void Update ()
    {
        panelHareket();
    }

    void panelHareket()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0); //sağa veya sola döndür
    }

    void OnCollisionEnter(Collision col) //palete carpan sekillerin hangi acidan carptigini saptama
    {
        if (col.gameObject.tag.Equals("squareTag"))
        {
            Vector3 hit = col.contacts[0].normal;
            //Debug.Log(hit);
            float angle = Vector3.Angle(hit, Vector3.up);

            if (Mathf.Approximately(angle, 0))
            {
                //Down
                //Debug.Log("Down");
            }
            if (Mathf.Approximately(angle, 180))
            {
                //Up
                //Debug.Log("Up");
            }
            if (Mathf.Approximately(angle, 90))
            {
                // Sides
                Vector3 cross = Vector3.Cross(Vector3.forward, hit);
                if (cross.y > 0)
                { // left side of the player
                    //Debug.Log("Left");
                }
                else
                { // right side of the player
                    //Debug.Log("Right");
                }
            }
        }
    }
}
