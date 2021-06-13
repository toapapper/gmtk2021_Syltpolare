using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    SceneLoaderAndController sceneLoaderAndController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            sceneLoaderAndController.LoadNextScene();
        }
    }
}
