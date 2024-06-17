using System;
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

    static int GetIndexOfMinValue(List<float> list)
    {
        int minIndex = 0;
        float minValue = list[0];

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] < minValue)
            {
                minValue = list[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameInstance.Instance.mbBrother != null)
        {
            goalVec = GameInstance.Instance.mbBrother.transform.position - transform.position;
            goalVec.y += 0.5f;
        }
        else if (GameInstance.Instance.mbMonsters.Count != 0)
        {
            List<float> list = new List<float>();
            for (int i = 0; i < GameInstance.Instance.mbMonsters.Count; ++i)
            {
                float dist = (GameInstance.Instance.mbMonsters[i].transform.position - transform.position).magnitude;
                list.Add(dist);
            }

            int idx = GetIndexOfMinValue(list);
            if (list[idx] <= 20f)
                goalVec = GameInstance.Instance.mbMonsters[idx].transform.position - transform.position;

            goalVec.y += 0.5f;
        }

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
        GetComponent<TrailRenderer>().Clear();
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
