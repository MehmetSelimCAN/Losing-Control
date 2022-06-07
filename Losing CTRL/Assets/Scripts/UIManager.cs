using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static int remainingCTRLCount;
    private static Transform CTRLs;
    private Transform keyC;
    private Transform keyV;

    private static Sprite normalCTRL;
    private Sprite pressedCTRL;
    private Sprite normalC;
    private Sprite pressedC;
    private Sprite normalV;
    private Sprite pressedV;

    private static Transform outOfCTRL;

    private Button restartButton;

    private void Awake() {
        CTRLs = GameObject.Find("CTRLs").transform;
        keyC = GameObject.Find("keyC").transform;
        keyV = GameObject.Find("keyV").transform;

        remainingCTRLCount = CTRLs.childCount;

        normalCTRL = Resources.Load<Sprite>("Sprites/NormalCTRL");
        normalC = Resources.Load<Sprite>("Sprites/NormalC");
        normalV = Resources.Load<Sprite>("Sprites/NormalV");

        pressedCTRL = Resources.Load<Sprite>("Sprites/PressedCTRL");
        pressedC = Resources.Load<Sprite>("Sprites/PressedC");
        pressedV = Resources.Load<Sprite>("Sprites/PressedV");

        outOfCTRL = GameObject.Find("OutOfCTRL").transform;

        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        restartButton.onClick.AddListener(() => {
            Restart();
        });
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            if (remainingCTRLCount > 0) {
                CTRLs.GetChild(CTRLs.childCount - remainingCTRLCount).GetComponent<Image>().sprite = pressedCTRL;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            if (remainingCTRLCount > 0) {
                CTRLs.GetChild(CTRLs.childCount - remainingCTRLCount).GetComponent<Image>().sprite = normalCTRL;
            }
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            keyC.GetComponent<Image>().sprite = pressedC;
        }

        if (Input.GetKeyUp(KeyCode.C)) {
            keyC.GetComponent<Image>().sprite = normalC;
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            keyV.GetComponent<Image>().sprite = pressedV;
        }

        if (Input.GetKeyUp(KeyCode.V)) {
            keyV.GetComponent<Image>().sprite = normalV;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Restart();
        }
    }

    public static void UseCTRL() {
        if (remainingCTRLCount > 0) {
            CTRLs.GetChild(CTRLs.childCount - remainingCTRLCount).GetComponent<Image>().color = new Color32(255,255,255, 100);
            CTRLs.GetChild(CTRLs.childCount - remainingCTRLCount).GetComponent<Image>().sprite = normalCTRL;
            remainingCTRLCount--;
        }
    }

    public static void OutOfControl() {
        if (!outOfCTRL.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("OutOfCtrlText")) {
            outOfCTRL.GetComponent<Animator>().Play("OutOfCtrlText", -1, 0f);
        }
    }


    private static void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
