using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerCC : MonoBehaviour {

    public delegate void OnInteracting();
    public OnInteracting onInteracting;

    public CharacterController charCtrl;
    public Animator anim;
    private int animMoving = Animator.StringToHash("moving");
    private int animSpeed = Animator.StringToHash("speed");
    private int animWalkDirection = Animator.StringToHash("walkdirection");
    private int animIdle01 = Animator.StringToHash("idle01");
    private int animIdle02 = Animator.StringToHash("idle02");
    private int animGetItem = Animator.StringToHash("getItem");
    //private int animLeaveItem = Animator.StringToHash("leaveItem");

    public enum PlayerState { Normal, Exhausted, Stunned}
    public PlayerState pState;

    private float inputX;
    private float inputZ;
    private bool action;
    private bool dash;


    private float speed;
    public float rotateSpeed = 3.0f;

    public float walkParameter = 6.0f;
    private float walk;

    public float runningParameter = 12f;
    private float running;

    public float staminaParameter = 100f;
    private float stamina;

    public float exhaustedParameter = 3f;
    private float exhausted;
    public float exhaustion = 2f;
    public float timeExhausted = 3f;
    private float exhaustedTimer;
    public float recovery = 2f;

    public Slider dashSlider;
    public GameObject dashSliderGO;

    public float timeStunned = 3f;
    private float stunnedTimer;

    private float idleTimer;

    // Use this for initialization
    void Start () {

        if (!charCtrl)
        {

            charCtrl = GetComponent<CharacterController>();

        }

        if (!anim)
        {

            anim = GetComponent<Animator>();

        }

        walk = walkParameter;
        running = runningParameter;
        exhausted = exhaustedParameter;
        stamina = staminaParameter;

        dashSlider.maxValue = staminaParameter;
        
	}
	
	// Update is called once per frame
	void Update () {

        AdjustParameters();

        Inputs();

        AnimatorParameters();

    }

    private void FixedUpdate()
    {
        
        switch (pState)
        {

            case (PlayerState.Normal):
                Movement();
                Dash();
                Interact();
                break;

            case (PlayerState.Exhausted):
                Exhausted();
                Movement();
                Interact();
                break;

            case (PlayerState.Stunned):
                Stunned();
                break;

        }

    }

    void Inputs()
    {

        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        action = Input.GetButtonDown("Interact");
        dash = Input.GetButton("Dash");

    }

    void AnimatorParameters()
    {

        anim.SetFloat(animMoving, Mathf.Abs(inputZ));
        anim.SetFloat(animWalkDirection, inputZ);
        anim.SetFloat(animSpeed, speed / 6f);

        if (inputX == 0f && inputZ == 0)
        {

            idleTimer += Time.deltaTime;
            if (idleTimer > 5f)
            {

                idleTimer = 0f;

                if (UnityEngine.Random.Range(0, 10) < 5)
                {

                    if (UnityEngine.Random.Range(0, 10) < 5)
                        anim.SetTrigger(animIdle01);
                    else
                        anim.SetTrigger(animIdle02);

                }

            }

        }
        

    }

    void AdjustParameters()
    {

        if (walk != walkParameter)
            walk = walkParameter;

        if (running != runningParameter)
            running = runningParameter;

        if (exhausted != exhaustedParameter)
            exhausted = exhaustedParameter;

    }

    void Movement()
    {
        
        // Rotate around y - axis
        transform.Rotate(0, inputX * rotateSpeed, 0);
        
        // Move forward / backward
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * inputZ;
        charCtrl.SimpleMove(forward * curSpeed);

    }

    void Dash()
    {

        if (dash && stamina > 0f && inputZ > 0f)
        {

            speed += 6f*Time.deltaTime;
            if (speed > running)
                speed = running;

            stamina -= exhaustion * Time.deltaTime;

        }
        else if (stamina <= 0f)
        {

            pState = PlayerState.Exhausted;

        }
        else
        {
          
            if(speed > walk)
            {

                speed -= 6f * Time.deltaTime;
                if (speed < walk)
                    speed = walk;

            }
            else if (speed < walk)
            {

                speed += 6f * Time.deltaTime;
                if (speed > walk)
                    speed = walk;

            }

            stamina += recovery * Time.deltaTime;
            if (stamina > staminaParameter)
            {

                stamina = staminaParameter;

            }

        }

        if (stamina < 0f)
        {

            stamina = 0f;

        }

        if (stamina < dashSlider.maxValue)
        {

            dashSliderGO.SetActive(true);

        }
        else
        {

            dashSliderGO.SetActive(false);

        }

        dashSlider.value = stamina;

    }

    void Exhausted()
    {

        speed -= 6f*Time.deltaTime;
        if (speed < exhausted)
            speed = exhausted;

        exhaustedTimer += Time.deltaTime;
        if (exhaustedTimer > timeExhausted)
        {

            exhaustedTimer = 0f;

            pState = PlayerState.Normal;

        }

        stamina += recovery * Time.deltaTime;
        if (stamina > staminaParameter)
        {

            stamina = staminaParameter;

        }

        if (stamina < dashSlider.maxValue)
        {

            dashSliderGO.SetActive(true);

        }
        else
        {

            dashSliderGO.SetActive(false);

        }

        dashSlider.value = stamina;

    }

    void Interact()
    {

        if (action)
        {

            if (onInteracting != null)
            {

                onInteracting.Invoke();
                onInteracting = null;

                anim.SetTrigger(animGetItem);

            }
            else
            {

                Debug.Log("Nothing to interact with!");

            }

        }

    }

    void Stunned()
    {

        stunnedTimer += Time.deltaTime;
        if (stunnedTimer > timeStunned)
        {

            stunnedTimer = 0;

            pState = PlayerState.Normal;

        }

    }

}
