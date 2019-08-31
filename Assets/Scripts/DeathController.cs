using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeathController : MonoBehaviour {

    #region Singleton

    public static DeathController instance;

    private void Awake()
    {
        if (instance != null)
        {

            Debug.LogWarning("More than one instance of DeathController found!");
            return;

        }

        instance = this;
    }

    #endregion

    Inventory inventory;

    public NavMeshAgent nAgent;
    private Rigidbody rdb;
    private Cloth cloth;

    public delegate void OnInteracting();
    public OnInteracting onInteracting;

    public enum DeathState { Idle, Spawn, Search, Steal, Act, Interact, Flee}
    public DeathState dState;

    public GameObject spawns;
    public Transform[] spawnPoint;
    private int spawnIndex;

    public List<ItemPickup> item;
    private ItemPickup itemTarget = null;
    public GameObject itemsParent;
    public ItemPickup[] itemOnWorld;
    private int itemIndex;

    public GameObject trapsParent;
    public Trap[] trap;
    private int trapIndex;

    private float idleTimer = 0;
    public Transform idlePosition;

    public Item currentItem = null;
    public Transform deathHand;

    public Transform[] players;
    private Transform playerTarget;

    private float stuckTimer;

	// Use this for initialization
	void Start () {

        inventory = Inventory.instance;

        if (!nAgent)
            nAgent = GetComponent<NavMeshAgent>();

        if (!rdb)
            rdb = GetComponent<Rigidbody>();

        if (!cloth)
            cloth = GetComponentInChildren<Cloth>();

        spawnPoint = spawns.GetComponentsInChildren<Transform>();
        spawnIndex = UnityEngine.Random.Range(1, spawnPoint.Length);

        itemOnWorld = itemsParent.GetComponentsInChildren<ItemPickup>();
        itemIndex = UnityEngine.Random.Range(0, item.Count);

        trap = trapsParent.GetComponentsInChildren<Trap>();

        if (MainData.isPlayerMale)
        {

            playerTarget = players[0];

        }
        else
        {

            playerTarget = players[1];

        }

	}

    private void Update()
    {

        UpdateItemOnWorld();

        ChooseItemTarget();

    }

    private void FixedUpdate()
    {

        switch (dState)
        {

            case (DeathState.Idle):
                Idle();
                break;

            case (DeathState.Spawn):
                Spawn();
                break;

            case (DeathState.Search):
                Search();
                break;

            case (DeathState.Steal):
                Steal();
                break;

            case (DeathState.Act):
                Act();
                break;

            case (DeathState.Interact):
                StartCoroutine(Interact());
                break;

            case (DeathState.Flee):
                Flee();
                break;

        }

    }

    private void Idle()
    {
        DisableComponents();

        if(this.transform.position != idlePosition.position)
            this.transform.position = idlePosition.position;

        idleTimer += Time.deltaTime;
        if (idleTimer >= 10f)
        {

            if (currentItem != null || itemTarget != null)
            {

                idleTimer = 0;

                spawnIndex = UnityEngine.Random.Range(1, spawnPoint.Length);
                transform.position = spawnPoint[spawnIndex].position;

                dState = DeathState.Spawn;
                    
            }

        }

    }

    private void Spawn()
    {
        EnableComponents();

        stuckTimer = 0f;

        spawnIndex = UnityEngine.Random.Range(1, spawnPoint.Length);

        if (currentItem != null)
        {

            for(int i = 0; i < trap.Length; i++)
            {

                if (currentItem.name == trap[i].targetItem.name)
                {
                    trapIndex = i;
                    nAgent.SetDestination(trap[trapIndex].interactionTransform.position);
                    dState = DeathState.Act;

                }

            }
            
        }
        else
        {

            nAgent.SetDestination(itemTarget.transform.position);
            dState = DeathState.Search;

        }

           
              
    }

    private void Search()
    {

        ChangeState();

        stuckTimer += Time.deltaTime;
        if (stuckTimer > 30f)
        {

            stuckTimer = 0f;
            dState = DeathState.Flee;

        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(itemTarget.transform.position, out hit, 3f, NavMesh.AllAreas))
        {

            nAgent.SetDestination(hit.position);

        }
        

        Vector3 dir = hit.position - this.transform.position;

        if (dir.magnitude < .5f)
            dState = DeathState.Flee;

    }

    private void Steal()
    {

        ChangeState();

        stuckTimer += Time.deltaTime;
        if (stuckTimer > 30f)
        {

            stuckTimer = 0f;
            dState = DeathState.Flee;

        }

        nAgent.SetDestination(playerTarget.position);

    }

    private void Act()
    {

        nAgent.SetDestination(trap[trapIndex].interactionTransform.position);

        Vector3 dir = trap[trapIndex].interactionTransform.position - this.transform.position;

        if (dir.magnitude < 1f)
            dState = DeathState.Interact;

    }

    private IEnumerator Interact()
    {

        transform.LookAt(trap[trapIndex].transform);

        if (onInteracting != null)
        {

            onInteracting.Invoke();
            onInteracting = null;

        }

        yield return new WaitForSeconds(3);
        dState = DeathState.Flee;

    }

    private void Flee()
    {

        nAgent.SetDestination(spawnPoint[spawnIndex].position);

        Vector3 dir = spawnPoint[spawnIndex].position - this.transform.position;

        if (dir.magnitude < 2)
            dState = DeathState.Idle;

    }

    void UpdateItemOnWorld()
    {

        itemOnWorld = itemsParent.GetComponentsInChildren<ItemPickup>();

    }

    void ChooseItemTarget()
    {

        if (currentItem == null && itemTarget == null)
        {

            itemIndex = UnityEngine.Random.Range(0, item.Count);

            for (int i = 0; i < itemOnWorld.Length; i++)
            {

                if (item[itemIndex].name == itemOnWorld[i].name)
                {

                    itemTarget = item[itemIndex];
                    return;

                }

            }

            for (int i = 0; i < inventory.items.Count; i++)
            {

                if (item[itemIndex].name == inventory.items[i].name)
                {

                    itemTarget = item[itemIndex];
                    return;

                }

            }

        }
        else if (currentItem == null)
        {

            for (int i = 0; i < itemOnWorld.Length; i++)
            {

                if (itemTarget.name == itemOnWorld[i].name)
                {

                    return;

                }

            }

            for (int i = 0; i < inventory.items.Count; i++)
            {

                if (itemTarget.name == inventory.items[i].name)
                {

                    return;

                }

            }

            itemTarget = null;
            return;

        }
        else if (currentItem != null)
        {

            itemTarget = null;
            return;

        }

    }

    void ChangeState()
    {

        if (currentItem == null && itemTarget != null)
        {

            for (int i = 0; i < itemOnWorld.Length; i++)
            {

                if (itemTarget.name == itemOnWorld[i].name)
                {

                    dState = DeathState.Search;

                }

            }

            for (int i = 0; i < inventory.items.Count; i++)
            {

                if (itemTarget.name == inventory.items[i].name)
                {

                    dState = DeathState.Steal;

                }

            }

        }

    }

    void DisableComponents()
    {

        if (nAgent.enabled)
            nAgent.enabled = false;
        if (cloth.enabled)
            cloth.enabled = false;

    }

    void EnableComponents()
    {

        if (!nAgent.enabled)
            nAgent.enabled = true;
        if (!cloth.enabled)
            cloth.enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player") && dState == DeathState.Steal)
        {

            for (int i = 0; i < inventory.items.Count; i++)
            {

                if (itemTarget.name == inventory.items[i].name)
                {

                    Debug.Log("Steal item and stun!");

                    currentItem = inventory.items[i];
                    inventory.RemoveWhenUsed(inventory.items[i]);

                    currentItem.instance.transform.SetParent(deathHand);
                    currentItem.instance.transform.localPosition = Vector3.zero;
                    currentItem.instance.SetActive(true);

                    for (int n = 0; n < inventory.items.Count; n++)
                    {

                        inventory.RemoveFromInventory(inventory.items[n]);

                    }

                    PlayerControllerCC player = other.gameObject.GetComponent<PlayerControllerCC>();
                    if (player != null)
                    {

                        player.pState = PlayerControllerCC.PlayerState.Stunned;

                    }

                }

            }

            dState = DeathState.Flee;

        }

    }
}
