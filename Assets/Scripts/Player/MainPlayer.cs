using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(CharacterController))]

public class MainPlayer : MonoBehaviour {

    private Animator animator;
    private Animation aanimation;
    private CharacterController controller;
    private SpriteRenderer spriteRenderer;
    private float jumpVel;
    private NetworkView netView;

    public float jumpForce = 0.55f,
                 jumpDecay = 0.3f,
                 speed = 0.09f,
                 gravity = 0.23f,
                 dampTime = 0.15f;
    public bool useAnimations = true;
    
    // Use this for initialization
    void Awake () {

        if (useAnimations)
        {
            animator = GetComponent<Animator>();
            aanimation = GetComponent<Animation>();
        }
        controller = GetComponent<CharacterController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        netView = GetComponent<NetworkView>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!netView.enabled || netView.isMine)
        {
            UpdateInput();
        }

	}

    private void UpdateInput()
    {
        float rawhor = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");

        if (jump && controller.isGrounded)
        {
            jumpVel = jumpForce;
        }


        if (rawhor > 0.05f)
        {
            spriteRenderer.flipX = false;

            if (useAnimations)
                animator.Play("WalkForward");
        }
        else if (rawhor < -0.05f)
        {
            spriteRenderer.flipX = true;
            if (useAnimations)
                animator.Play("WalkForward");
        }
        else
        {
            if (useAnimations)
                animator.Play("StandFront");
        }
        
        controller.Move(new Vector2(rawhor * speed, jumpVel - gravity));

        if (jumpVel > 0)
        {
            jumpVel -= jumpDecay;
        }
    }
}
