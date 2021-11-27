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

    [Header("Attack")]
    public int combo;
    public bool attacking;
    public GameObject sword1, sword2;
    Vector3 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();

        Attackable = true;
        CanDash = true;

        sword1.gameObject.SetActive(false);
    }

    void Update()
    {
        //Movement
        if (Input.GetKey(KeyCode.D) && CanMove == true)
        {
            spr.flipX = false;
            IsMoving = true;
            runRight();
        }
        else if (Input.GetKey(KeyCode.A) && CanMove == true)
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
            anim.SetBool("Dash", true);
            StartCoroutine(Dash_Right());
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && grounded && spr.flipX == true && CanDash == true)
        {
            CanDash = false;
            anim.SetBool("Dash", true);
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

    public void Dash_Attack(Vector2 _targetPosition)
    {
        Vector3 dir = targetPosition - transform.position;
        dir.Normalize();
        rb.DOMove(transform.position + dir * 5, 0.2f);
        attacking = true;
        anim.SetTrigger("" + combo);
        if(_targetPosition.x < transform.position.x)
        {
            spr.flipX = true;
        }
        else
        {
            spr.flipX = false;
        }
    }

    public void ActivateSwordCollider1()
    {
        sword1.gameObject.SetActive(true);
    }

    public void DeactiveSwordCollider1()
    {
        sword1.gameObject.SetActive(false);
    }

    public void ActivateSwordCollider2()
    {
        sword2.gameObject.SetActive(true);
    }

    public void DeactiveSwordCollider2()
    {
        sword2.gameObject.SetActive(false);
    }

    public void StartCombo()
    {
        attacking = false;
        if(combo < 2)
        {
            combo++;
        }
    }

    public void FinishCombo()
    {
        attacking = false;
        combo = 0;
    }

    IEnumerator Dash_Right()
    {
        //transform.Translate(DashSpeed, 0f, 0f);
        rb.DOMove(new Vector2(transform.position.x + 2, transform.position.y), 0.4f);
        aud.Stop();
        Attackable = false;
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Dash", false);
        yield return new WaitForSeconds(0.8f);
        Attackable = true;
        yield return new WaitForSeconds(0.5f);
        CanDash = true;
    }

    IEnumerator Dash_Left()
    {
        //transform.Translate(-DashSpeed, 0f, 0f);
        rb.DOMove(new Vector2(transform.position.x - 2, transform.position.y), 0.4f);
        aud.Stop();
        Attackable = false;
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Dash", false);
        yield return new WaitForSeconds(0.8f);
        Attackable = true;
        yield return new WaitForSeconds(0.5f);
        CanDash = true;
    }
}
