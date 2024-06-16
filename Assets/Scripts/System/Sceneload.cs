using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceneload : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "shop")
        {
            SceneManager.LoadScene("2");
        }

        else if (other.tag == "dungeon")
        {
            SceneManager.LoadScene("3");
        }

        else if (other.tag == "portal")
        {
            SceneManager.LoadScene("Final");
        }

        else if (other.tag == "exit")
        {
            SceneManager.LoadScene("1");
        }
    }
}
