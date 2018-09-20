using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // Goes to calendar scene
    public void MainMenuNewGameButton()
    {
		StaticValueHolder.CurrentMoney = 0;
		SceneManager.LoadScene("CalendarScene");
	}

	public void MainMenuContinueButton()
	{
		// Add the saved value for money
		StaticValueHolder.CurrentMoney = 0;
		SceneManager.LoadScene("CalendarScene");
	}
}
