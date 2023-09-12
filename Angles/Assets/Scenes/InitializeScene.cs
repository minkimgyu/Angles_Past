using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeScene : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("Menu");
    }
}
