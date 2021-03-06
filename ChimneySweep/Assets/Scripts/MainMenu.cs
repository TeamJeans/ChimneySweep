using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // Goes to calendar scene
    public void MainMenuNewGameButton()
    {
		StaticValueHolder.DailyMoney = 0;
		StaticValueHolder.TotalMoney = 0;
		StaticValueHolder.CurrentDay = -1;
		SceneManager.LoadScene("CalendarScene");
	}

	public void MainMenuContinueButton()
	{
		// Add the saved value for money
		StaticValueHolder.DailyMoney = 0;
		StaticValueHolder.TotalMoney = 0;
		StaticValueHolder.CurrentDay = -1;
		SceneManager.LoadScene("CalendarScene");
	}
}
