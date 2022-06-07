using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    private Button startButton;

    private void Awake() {
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        startButton.onClick.AddListener(() => {
            StartGame();    
        });
    }

    public void StartGame() {
        SceneManager.LoadScene("Level1");
    }

}
