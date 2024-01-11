using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 initialPosition;
    private float floorHeight = 0.2f;

    void Start()
    {
        initialPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.y < floorHeight){
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.transform.position = initialPosition;
        }
    }
}
