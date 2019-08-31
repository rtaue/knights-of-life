using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody rdb;

    private float inputX;
    private float inputZ;
    public float speed = 1f;
    public float turnSpeed = 1f;

	// Use this for initialization
	void Start () {
		
        if (!rdb)
        {

            rdb = GetComponent<Rigidbody>();

        }

	}
	
	// Update is called once per frame
	void Update () {

        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

	}

    private void FixedUpdate()
    {

        Turn();
        Move();
        
    }

    private void Move()
    {

        rdb.AddForce(transform.forward *inputZ * speed, ForceMode.Force);

    }

    private void Turn()
    {

        transform.Rotate(new Vector3(0, inputX * turnSpeed * Time.deltaTime, 0));

    }

}
