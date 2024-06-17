using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;

    Rigidbody rigid;
    SphereCollider sphereCollider;
    Material mat;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        mat = GetComponent<SkinnedMeshRenderer>().material;
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
            mat.color = Color.red;
            yield return new WaitForSeconds(0.1f);

            if (curHealth > 0)
            {
                mat.color = Color.white;
            }
            else
            {
                mat.color = Color.gray;
                Destroy(gameObject, 2);
            }
        }
    }
}
