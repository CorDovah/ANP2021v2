using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player_Behaviour : MonoBehaviour
{
    [Header("Variables")]
    public float RunSpeed;
    public float DashSpeed;
    public float jumpForce = 10f;

    [Header("States")]
    public bool CanMove;
    public bool Attackable;
    public bool IsMoving;
    private bool CanDash;

    //Variables para detectar el suelo y salto
    [Header("GroundCheck")]
    public LayerMask WhatIsGrd;
    public Transform grdChecker;
    public float grdCheckerRad;
    public bool grounded;

    [Header("JumpEffects")]
    public GameObject m_JumpDust;

    Rigidbody2D rb;
    SpriteRenderer spr;
    Animator anim;
    AudioSource aud;

    Vector3 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();

        Attackable = true;
        CanDash = true;
    }

    void Update()
    {
        //Movement
        if (Input.GetKey("d") && CanMove == true)
        {
            spr.flipX = false;
            IsMoving = true;
            runRight();
        }
        else if (Input.GetKey("a") && CanMove == true)
        {
            spr.flipX = true;
            IsMoving = true;
            runLeft();
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("Running", false);
            aud.Stop();
        }
        /////////////////////////////////////////////////////

        //Attack, Grapple & Dash
        if (Input.GetKeyDown(KeyCode.Mouse0) && grounded)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Dash_Attack(targetPosition);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && grounded && spr.flipX == false && CanDash == true)
        {
            CanDash = false;
            StartCoroutine(Dash_Right());
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && grounded && spr.flipX == true && CanDash == true)
        {
            CanDash = false;
            StartCoroutine(Dash_Left());
        }
        /////////////////////////////////////////////////////
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(grdChecker.position, grdCheckerRad, WhatIsGrd);
        Jump();
    }

    void Jump()
    {
        anim.SetBool("Jump", true);

        if (grounded == true)
        {
            anim.SetBool("Jump", false);

            if (Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                AE_Jump();
            }
        }
    }

    void runRight()
    {
        rb.velocity = new Vector2(RunSpeed, rb.velocity.y);
        anim.SetBool("Running", true);

        if (IsMoving && !aud.isPlaying) aud.Play();
        if (!IsMoving || !grounded) aud.Stop();
    }

    void runLeft()
    {
        rb.velocity = new Vector2(-RunSpeed, rb.velocity.y);
        anim.SetBool("Running", true);

        if (IsMoving && !aud.isPlaying) aud.Play();
        if (!IsMoving || !grounded) aud.Stop();
    }

    void SpawnDustEffect(GameObject dust, float dustXOffset = 0)
    {
        if (dust != null)
        {
            Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset, 0.0f, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
            newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(1, 1);
        }
    }

    void AE_Jump()
    {
        SpawnDustEffect(m_JumpDust, 0.2f);
    }

    void Dash_Attack(Vector3 _targetPosition)
    {
        Vector3 dir = _targetPosition - transform.position;
        dir.Normalize();
        rb.DOMove(transform.position + dir * 5, 0.2f);
    }

    IEnumerator Dash_Right()
    {
        transform.Translate(DashSpeed, 0f, 0f);
        aud.Stop();
        Attackable = false;
        yield return new WaitForSeconds(1f);
        Attackable = true;
        yield return new WaitForSeconds(0.5f);
        CanDash = true;
    }

    IEnumerator Dash_Left()
    {
        transform.Translate(-DashSpeed, 0f, 0f);
        aud.Stop();
        Attackable = false;
        yield return new WaitForSeconds(1f);
        Attackable = true;
        yield return new WaitForSeconds(0.5f);
        CanDash = true;
    }
}
