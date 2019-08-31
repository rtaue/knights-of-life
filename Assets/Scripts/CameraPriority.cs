using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPriority : MonoBehaviour {

    public CinemachineVirtualCamera cnCamera;

	// Use this for initialization
	void Start () {
		
        if (!cnCamera)
        {

            cnCamera = GetComponentInChildren<CinemachineVirtualCamera>();

        }

	}

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {

            cnCamera.LookAt = other.gameObject.transform;
            cnCamera.Priority = 20;

        }

    }

    public void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            cnCamera.Priority = 1;

        }

    }

}
