using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 inputDirection;
    private Vector2 velocity;
    private bool canMove = true;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private float dashTimer;


    [SerializeField] private LayerMask exclusionLayers;
    [SerializeField] private LayerMask emptyLayer;

    private Rigidbody2D rigidBody;
    private BoxCollider2D coll;
    private Animator animator;

    [SerializeField] private float timeCooldown;
    private float timeTimer = 0;
    private Vector3[] previousPos;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        SetupPreviousPos();
    }

    void Update()
    {
        HandleInputs();
        dashTimer -= Time.deltaTime;
        timeTimer -= Time.deltaTime;
    }

    private void SetupPreviousPos()
    {
        previousPos = new Vector3[600];
        for (int i = 0; i < previousPos.Length; i++)
        {
            previousPos[i] = transform.position;
        }
    }

    private void HandleInputs()
    {
        inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown("space") && canMove && inputDirection.magnitude != 0 && dashTimer < 0)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.E) && canMove && timeTimer < 0)
        {
            RewindTime();
        }
    }

    private void FixedUpdate()
    {
        InsertNewPosition();
        if (canMove)
        {
            velocity = inputDirection.normalized * speed;
        }
        rigidBody.velocity = velocity;
    }

    private IEnumerator Dash()
    {
        canMove = false;
        coll.excludeLayers = exclusionLayers;
        animator.SetTrigger("dash");
        velocity = inputDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        dashTimer = dashCooldown;
        canMove = true;
        coll.excludeLayers = emptyLayer;
    }

    private void RewindTime()
    {
        timeTimer = timeCooldown;
        transform.position = previousPos[previousPos.Length - 1];
    }

    private void InsertNewPosition()
    {
        for (int i = 0; i < previousPos.Length-1; i++)
        {
            previousPos[i + 1] = previousPos[i];
        }
        previousPos[0] = transform.position;
        Debug.Log(previousPos[previousPos.Length-1]);
    }
}
