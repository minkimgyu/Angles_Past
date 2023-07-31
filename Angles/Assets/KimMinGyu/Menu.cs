using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject TutorialPanel;

    public GameObject[] tutorials;
    int tutorialIndex = 0;

    public void GoToPlayerScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void OnOffSetting(bool nowOn)
    {
        settingPanel.SetActive(nowOn);
    }

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnOffTutorial(bool nowOn)
    {
        TutorialPanel.SetActive(nowOn);
        if(nowOn == true)
        {
            tutorialIndex = 0;
            tutorials[0].SetActive(true);
            tutorials[1].SetActive(false);
            tutorials[2].SetActive(false);
        }
    }

    public void SwitchTutorialPanel(int tmp)
    {
        tutorialIndex += tmp;

        if (tutorialIndex == -1)
        {
            tutorialIndex = tutorials.Length - 1;
        }
        else if(tutorialIndex == tutorials.Length)
        {
            tutorialIndex = 0;
        }

        for (int i = 0; i < tutorials.Length; i++)
        {
            if(i == tutorialIndex)
                tutorials[i].gameObject.SetActive(true);
            else
                tutorials[i].gameObject.SetActive(false);
        }
    }
}
