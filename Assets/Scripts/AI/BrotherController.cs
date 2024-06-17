using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrotherController : MonoBehaviour
{
    public float hp = 100f;
    public Slider hpSlider;

    Animator animator;
    public GameObject laser;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = 0.6f;
        transform.position = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            hp -= 1f;
            hpSlider.value = hp / 100f;
            animator.SetTrigger("Flinch");
        }

        if (hp <= 0f)
        {
            hpSlider.gameObject.SetActive(false);
            Destroy(GetComponent<BehaviorTreeRunner>());
            animator.SetTrigger("Death");
            GameInstance.Instance.Mute();
            laser.SetActive(true);
            GameObject.Find("score").GetComponent<Score>().ScorePoint(100);
        }
    }
}
