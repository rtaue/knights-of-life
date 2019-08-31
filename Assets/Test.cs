using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour {

    Vector3 hitPos;
    public float radius = 2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position,out hit, 2.0f, NavMesh.AllAreas))
        {

            Debug.Log("Point found in NavMesh");

            hitPos = hit.position;

        }

	}

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitPos, radius);

    }
}
