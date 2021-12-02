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
    [HideInInspector] public Vector3 playerPos;

    [Header("States")]
    public bool CanMove;
    public bool Attackable;
    public bool IsMoving;
    public bool isDead;
    private bool CanDash;
    private bool CanAttack;
    private bool CanGrapple;   
    private bool IsJumping;   

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
    public AudioClip[] clips;

    [Header("Attack & Sword Colliders")]
    public GameObject sword1;
    public GameObject sword2;
    public GameObject sword3;
    public GameObject sword4;
    public int combo;
    public bool attacking;

    [Header("Grapple")]
    public GameObject camBoundry;
    public GameObject leftPlatform;
    public GameObject rightPlatform;
    public GameObject floor;
    GameObject grappleTarget;
    Vector3 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();

        Attackable = true;
        CanDash = true;
        CanGrapple = true;
        CanAttack = true;

        sword1.gameObject.SetActive(false);
    }

    void Update()
    {
        playerPos = this.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        #region Movement
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
            //aud.Stop();
        }
        #endregion

        #region Attack
        if (Input.GetKeyDown(KeyCode.Mouse0) && grounded && CanAttack)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Dash_Attack(targetPosition);
        }
        #endregion

        #region Grapple
        if (Input.GetKeyDown(KeyCode.Mouse1) && CanGrapple)
        {
            Debug.Log("Tocando " + grappleTarget);
            CanGrapple = false;
            grappleTarget = hit.collider.gameObject;
            if (hit.collider.gameObject == camBoundry || hit.collider.gameObject == floor)
            {
                transform.position = transform.position;
            }
            else if((hit.collider.gameObject == leftPlatform) || (hit.collider.gameObject == rightPlatform))
            {
                transform.DOMove(new Vector2(grappleTarget.transform.position.x, grappleTarget.transform.position.y + 0.6f), 1f);
            }
            else
            {
                transform.DOMove(grappleTarget.transform.position, 1f);
            }
            StartCoroutine("Grapple");
        }
        #endregion

        #region Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !spr.flipX && CanDash)
        {
            CanDash = false;
            anim.SetBool("Dash", true);
            StartCoroutine(Dash_Right());
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && spr.flipX && CanDash)
        {
            CanDash = false;
            anim.SetBool("Dash", true);
            StartCoroutine(Dash_Left());
        }
        #endregion

        anim.SetFloat("Falling", rb.velocity.y);
        if(grounded)
            anim.SetBool("Grounded", true);
        else
            anim.SetBool("Grounded", false);
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(grdChecker.position, grdCheckerRad, WhatIsGrd);
        Jump();
    }

    void Jump()
    {
        if (grounded == true)
        {
            anim.SetBool("Grounded", true);
            anim.SetBool("Jump", false);

            if (Input.GetKey(KeyCode.Space))
            {
                IsJumping = true;
                aud.PlayOneShot(clips[3]);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetBool("Jump", true);
                anim.SetBool("Grounded", false);
                AE_Jump();
            }
            else
                IsJumping = false;
        }
    }

    void runRight()
    {
        rb.velocity = new Vector2(RunSpeed, rb.velocity.y);
        anim.SetBool("Running", true);

        aud.clip = clips[2];
        if (IsMoving && !aud.isPlaying) aud.Play();
        if (!IsMoving || !grounded) aud.Stop();
    }

    void runLeft()
    {
        rb.velocity = new Vector2(-RunSpeed, rb.velocity.y);
        anim.SetBool("Running", true);

        aud.clip = clips[2];
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
        if (spr.flipX)
        {
            SpawnDustEffect(m_JumpDust, 0f);
        }
        else
        {
            SpawnDustEffect(m_JumpDust, 0.2f);
        }

    }

    public void Dash_Attack(Vector2 _targetPosition)
    {
        Vector3 dir = targetPosition - transform.position;
        dir.Normalize();
        rb.DOMove(transform.position + dir * 5, 0.2f);
        attacking = true;

        anim.SetTrigger("" + combo);
        aud.PlayOneShot(clips[combo]);

        if (_targetPosition.x < transform.position.x)
        {
            spr.flipX = true;
        }
        else
        {
            spr.flipX = false;
        }
    }

    /*public void Dash_Attack(Vector2 _targetPosition)
    {
        CanAttack = false;
        Vector3 dir = targetPosition - transform.position;
        dir.Normalize();
        rb.DOMove(transform.position + dir * 5, 0.2f);
        attacking = true;

        anim.SetTrigger("" + combo);
        aud.PlayOneShot(clips[combo]);

        if (_targetPosition.x < transform.position.x)
        {
            spr.flipX = true;
        }
        else
        {
            spr.flipX = false;
        }
    }*/

    public void ActivateSwordCollider1()
    {
        if (spr.flipX)
            sword3.gameObject.SetActive(true);
        else
            sword1.gameObject.SetActive(true);
    }

    public void DeactiveSwordCollider1()
    {
        sword1.gameObject.SetActive(false);
        sword3.gameObject.SetActive(false);
    }

    public void ActivateSwordCollider2()
    {
        if (spr.flipX)
            sword4.gameObject.SetActive(true);
        else
            sword2.gameObject.SetActive(true);
    }

    public void DeactiveSwordCollider2()
    {
        sword2.gameObject.SetActive(false);
        sword4.gameObject.SetActive(false);
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

    IEnumerator Grapple()
    {
        yield return new WaitForSeconds(0.1f);
        CanGrapple = true;
    }
}