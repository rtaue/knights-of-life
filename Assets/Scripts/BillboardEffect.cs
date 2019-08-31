using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardEffect : MonoBehaviour {

    public Camera m_Camera;

    void Awake()
    {

        if (!m_Camera)
        {

            m_Camera = Camera.main;

        }

    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {

        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);

    }

}
