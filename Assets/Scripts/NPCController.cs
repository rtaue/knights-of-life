using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour {

    #region Singleton

    public static NPCController instance;

    private void Awake()
    {
        if (instance != null)
        {

            Debug.LogWarning("More than one instance of NPCController found!");
            return;

        }

        instance = this;
    }

    #endregion

    InGameManager inGameManager;

    public NavMeshAgent nAgent;

    public delegate void OnInteracting();
    public OnInteracting onInteracting;

    public enum NPCState { Patrol, Flee, Dead, Interact, Act}
    public NPCState npcState;

    public GameObject safePointsParent;
    public Transform[] safePoints;
    private int safePointIndex;
    private float patrolTimer;

    public GameObject trapsParent;
    public Trap[] traps;
    private int trapIndex;

    private float fleeTimer;
    public Transform fleePoint;

    // Use this for initialization
    void Start () {

        inGameManager = InGameManager.instance;

        if (nAgent != null)
            nAgent = GetComponent<NavMeshAgent>();

        safePoints = safePointsParent.GetComponentsInChildren<Transform>();
        safePointIndex = UnityEngine.Random.Range(1, safePoints.Length);

        traps = trapsParent.GetComponentsInChildren<Trap>();
        trapIndex = UnityEngine.Random.Range(0, traps.Length);

        nAgent.SetDestination(safePoints[safePointIndex].position);

	}

    private void FixedUpdate()
    {
        
        switch (npcState)
        {

            case (NPCState.Patrol):
                Patrol();
                break;

            case (NPCState.Act):
                Act();
                break;

            case (NPCState.Interact):
                StartCoroutine(Interact());
                break;


            case (NPCState.Flee):
                Flee();
                break;

            case (NPCState.Dead):
                Dead();
                break;

        }

    }

    private IEnumerator Interact()
    {

        Debug.Log("Victim: Interacting with something");

        transform.LookAt(traps[trapIndex].transform);

        if (onInteracting != null)
        {

            onInteracting.Invoke();
            onInteracting = null;

        }

        yield return new WaitForSeconds(3);
        safePointIndex = UnityEngine.Random.Range(1, safePoints.Length);

        npcState = NPCState.Patrol;

    }

    private void Patrol()
    {

        nAgent.SetDestination(safePoints[safePointIndex].position);

        Vector3 dir = safePoints[safePointIndex].position - this.transform.position;
        if (dir.magnitude < 1f)
        {

            npcState = NPCState.Interact;

        }

        patrolTimer += Time.deltaTime;
        if (patrolTimer >= 10f)
        {

            patrolTimer = 0f;

            trapIndex = UnityEngine.Random.Range(0, traps.Length);
            nAgent.SetDestination(traps[trapIndex].interactionTransform.position);

            npcState = NPCState.Act;

        }

    }

    private void Act()
    {

        nAgent.SetDestination(traps[trapIndex].interactionTransform.position);

        Vector3 dir = traps[trapIndex].interactionTransform.position - this.transform.position;
        if (dir.magnitude < 1f)
        {

            npcState = NPCState.Interact;

        }

    }

    private void Flee()
    {

        nAgent.SetDestination(fleePoint.position);

        Vector3 dir = fleePoint.position - this.transform.position;
        if (dir.magnitude < 2)
        {

            fleeTimer += Time.deltaTime;
            if (fleeTimer >= 5f)
            {

                fleeTimer = 0;

                safePointIndex = UnityEngine.Random.Range(1, safePoints.Length);

                npcState = NPCState.Patrol;

            }

        }

    }

    public void Dead()
    {

        Debug.Log("NPC is dead");
        //Do something;
        inGameManager.Lose();
        Destroy(gameObject);

    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Death"))
        {

            npcState = NPCState.Flee;

            Debug.Log("Fleeing from Death");

        }

    }

}
