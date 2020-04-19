using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSpawner : MonoBehaviour {

    public static AlienSpawner instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
    }

    public AnimationCurve spawnAmountModifier;
    public AnimationCurve toughnessModifier;

    public int wave = 1;

    public bool waveActive = false;
    public int prepareTime = 10;

    public int baseSpawnAmount = 1;

    public void BeginWaves() {
        UIManager.instance.Init();
        BuildingsManager.instance.Init();

        StartCoroutine(Prepare());
    }

    IEnumerator Prepare() {
        int timeLeft = prepareTime;

        while (timeLeft > 0) {
            UIManager.instance.bigText.gameObject.SetActive(true);
            UIManager.instance.bigText.text = $"Wave {wave} starting in {timeLeft} seconds!";
            yield return new WaitForSeconds(1);
            timeLeft -= 1;
        }

        UIManager.instance.bigText.text = $"Wave {wave} starting!";

        yield return new WaitForSeconds(2);

        UIManager.instance.bigText.gameObject.SetActive(false);
        StartCoroutine(Wave());
    }

    IEnumerator Wave() {
        UIManager.instance.buildingButton.interactable = false;
        UIManager.instance.jetButton.interactable = false;

        int amountOfEnemies = baseSpawnAmount * (int)spawnAmountModifier.Evaluate(wave);
        int spawnedEnemies = 0;

        while (spawnedEnemies < amountOfEnemies) {
            FightersManager.instance.SpawnAlienFighter();
            spawnedEnemies += 1;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(5);

        while (FightersManager.instance.spawnedAliens > 0) {
            if (BuildingsManager.instance.activeBuildings <= 0) {
                StartCoroutine(Lost());
                yield break;
            }

            yield return new WaitForSeconds(1);
        }

        if (BuildingsManager.instance.activeBuildings <= 0) {
            StartCoroutine(Lost());
            yield break;
        }

        int income = EconomyManager.instance.Income;
        EconomyManager.instance.Money += income;
        string incomeFormatted = EconomyManager.instance.FormatDollar(income);
        UIManager.instance.bigText.text = $"Wave {wave} completed. Received {incomeFormatted}";
        UIManager.instance.bigText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3);
        UIManager.instance.bigText.gameObject.SetActive(false);

        wave++;

        StartCoroutine(Prepare());
    }

    IEnumerator Lost() {
        GetComponent<AudioSource>().Play();

        UIManager.instance.bigText.text = $"You lost! \n You made it to wave {wave}!";
        UIManager.instance.bigText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3);

        UIManager.instance.restartButton.gameObject.SetActive(true);
    }
}
