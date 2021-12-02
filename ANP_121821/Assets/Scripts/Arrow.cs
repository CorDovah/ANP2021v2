using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    BowEnemy_Behaviour bow;
    SpriteRenderer spr;
    Vector3 rotate;

    void OnEnable()
    {
        Invoke(nameof(Deactivate), 0.5f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Deactivate));
    }

    private void Start()
    {
        bow = FindObjectOfType<BowEnemy_Behaviour>();
        spr = GetComponent<SpriteRenderer>();
        rotate = new Vector3(-0.45f, 0.5f, 0.0f);
    }

    private void Update()
    {
        if (bow.transform.localScale == rotate)
            spr.flipX = true;
        else
            spr.flipX = false;
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
