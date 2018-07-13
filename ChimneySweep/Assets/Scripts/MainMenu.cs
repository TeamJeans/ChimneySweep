using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // Goes to calendar scene
    public void MainMenuPlayButton()
    {
		SceneManager.LoadScene("CalendarScene");
	}
}
