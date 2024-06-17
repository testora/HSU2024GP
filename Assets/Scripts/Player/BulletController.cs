using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    PlayerController player;
    public Vector3 vector = Vector3.zero;

    [SerializeField] float speed = 100f;
    [SerializeField] float lifeTime = 5f;

    int damage = 1;
    public int Damage { get { return damage; } }

    void Awake()
    {
        //playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += vector * speed * Time.deltaTime;
    }

    void OnEnable()
    {
        if (IsInvoking("RetrieveBullet"))
            CancelInvoke("RetrieveBullet");
        Invoke("RetrieveBullet", lifeTime);
    }

    #region Public Methods

    public void InitBullet(PlayerController controller)
    {
        player = controller;
    }

    public void RetrieveBullet()
    {
        if (IsInvoking("RetrieveBullet"))
            CancelInvoke("RetrieveBullet");
        player.RetrieveBullet(this);
    }

    #endregion
}
