using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

public class MovementController : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    private Vector2 inputDirection;
    private Vector2 velocity;
    private bool canMove = true;

    [SerializeField] private float dashSpeed = 9f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 3f;
    private float dashTimer;


    [SerializeField] private LayerMask exclusionLayers;
    [SerializeField] private LayerMask emptyLayer;

    private Rigidbody2D rigidBody;
    private BoxCollider2D coll;
    private Animator animator;

    [SerializeField] private float timeCooldown = 3f;
    private float timeTimer = 0;
    private Queue<Vector3> previousPos;

    [SerializeField] private GameObject fade;


    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        SetupPreviousPos();
    }

    void Update() {
        HandleInputs();
        dashTimer -= Time.deltaTime;
        timeTimer -= Time.deltaTime;
    }

    private void SetupPreviousPos() {
        previousPos = new Queue<Vector3>(150);
        for (int i = 0; i < 150; i++) {
            previousPos.Enqueue(transform.position);
        }
    }

    private void HandleInputs() {
        inputDirection = Utilities.Input.instance.playerControls.Gameplay.Move.ReadValue<Vector2>();
        if (Utilities.Input.instance.playerControls.Gameplay.Dash.IsPressed() && canMove && inputDirection.magnitude != 0 && dashTimer < 0) {
            StartCoroutine(Dash());
        }
        if (Utilities.Input.instance.playerControls.Gameplay.Rewind.IsPressed() && canMove && timeTimer < 0) {
            RewindTime();
        }
    }

    private void FixedUpdate() {
        InsertNewPosition();
        if (canMove) {
            velocity = inputDirection.normalized * speed;
        }
        rigidBody.velocity = velocity;
    }

    private IEnumerator Dash() {
        canMove = false;
        coll.excludeLayers = exclusionLayers;
        // animator.SetTrigger("dash"); TODO: We don't have a dash animation yet!
        velocity = inputDirection.normalized * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        dashTimer = dashCooldown;
        canMove = true;
        coll.excludeLayers = emptyLayer;
    }

    private void RewindTime() {
        timeTimer = timeCooldown;
        transform.position = previousPos.Dequeue();
    }

    private void InsertNewPosition() {
        _ = previousPos.Dequeue();
        previousPos.Enqueue(transform.position);
    }
}