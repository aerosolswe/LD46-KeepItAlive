using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
    }

    public GameObject startView;
    public GameObject gameView;

    public Text bigText;
    public Text moneyText;
    public Text incomeText;

    public Button restartButton;

    public Button buildingButton;
    public Text buildingButtonText;

    public Button jetButton;
    public Text jetButtonText;

    public int BuildingCost = 50;
    public int JetCost = 25;

    public void Init() {
        EconomyManager.instance.SetBaseEconomy();

        buildingButtonText.text = "Buy building " + EconomyManager.instance.FormatDollar(BuildingCost);
        jetButtonText.text = "Buy jet " + EconomyManager.instance.FormatDollar(JetCost);

        buildingButton.interactable = false;
    }

    public void ShowUI() {
        startView.SetActive(true);
    }

    public void RestartGame() {
        restartButton.gameObject.SetActive(false);
        gameView.SetActive(false);
        startView.SetActive(false);

        SceneManager.LoadScene("SplashScene");
    }
}
