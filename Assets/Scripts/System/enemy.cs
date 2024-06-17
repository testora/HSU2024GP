using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;

    Rigidbody rigid;
    SphereCollider sphereCollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }


     void OnTriggerEnter(Collider other)
    {
        if(other.tag == "bullet")
        {
            curHealth -= 10;
            StartCoroutine(onDamage());
        }

        IEnumerator onDamage()
        {
            yield return new WaitForSeconds(0.1f);

             if(curHealth == 0)
                Destroy(gameObject, 2);
            
        }
    }
}
