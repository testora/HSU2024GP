using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    PlayerController player;
    Vector3 initVec = Vector3.zero;
    Vector3 goalVec = Vector3.zero;
    Vector3 vector = Vector3.zero;

    [SerializeField] float speed = 100f;
    [SerializeField] float lifeTime = 5f;
    float timeAcc = 0f;

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
        if (GameInstance.Instance.mbBrother != null)
            goalVec = GameInstance.Instance.mbBrother.transform.position - transform.position;

        timeAcc += Time.deltaTime;
        vector = Vector3.Lerp(initVec, goalVec, Utility.ProportionalRatio(timeAcc, 0f, lifeTime));
        transform.position += vector * speed * Time.deltaTime;
    }

    void OnEnable()
    {
        if (IsInvoking("RetrieveBullet"))
            CancelInvoke("RetrieveBullet");
        Invoke("RetrieveBullet", lifeTime);

        timeAcc = 0f;
        initVec = GameInstance.Instance.mbCamera.transform.forward;
        Vector3 diffusion = Utility.Diffusion(initVec, 15f * Mathf.Deg2Rad, 30f * Mathf.Deg2Rad);
        goalVec = initVec * 2f - diffusion;
        initVec = diffusion;
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
