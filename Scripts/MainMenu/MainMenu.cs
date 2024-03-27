using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   

    // Scene 1 = Single Player
    public void loadSinglePlayer() {
        SceneManager.LoadScene(1);   
        PlayerPrefs.SetInt("Coop" , 0);
    }

    // Scene 2 = Co op Mode
    public void loadCoOp() {
        SceneManager.LoadScene(2);
        PlayerPrefs.SetInt("Coop" , 1);
    }
}
