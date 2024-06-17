using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrotherController : MonoBehaviour
{
    public float hp = 100f;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BulletController>() != null)
        {
            hp -= 1f;

            animator.SetTrigger("Flinch");
        }
    }
}
