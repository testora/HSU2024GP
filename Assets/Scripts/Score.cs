using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    int score = 0;

    public TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            Apply();
    }

    public void Apply()
    {
        textMeshPro.text = score.ToString();
    }

    public void ScorePoint(int _score)
    {
        this.score += _score;
        Apply();
    }
}
