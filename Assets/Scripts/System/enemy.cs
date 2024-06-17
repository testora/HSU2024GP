using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;

    Rigidbody rigid;
    SphereCollider sphereCollider;

    public GameObject a;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (curHealth != maxHealth)
            Debug.Log(curHealth);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Debug.Log(curHealth);
            curHealth -= 10;

            if (curHealth <= 0)
            {
                gameObject.SetActive(false);
                GameObject.Find("score").GetComponent<Score>().ScorePoint(10);
            }
        }
    }
}
