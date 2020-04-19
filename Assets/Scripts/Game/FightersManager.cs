using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightersManager : MonoBehaviour {

    public static FightersManager instance = null;
    
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
    }

    public Fighter humanoidFighter;
    public Fighter alienFighter;

    public int spawnedAliens = 0;
    public int spawnedHumanoids = 0;

    public float alienYSpawnPos = 20;
    public float humanoidYSpawnPos = 3;
    
    public void SpawnAlienFighter() {
        Vector3 randomPos = Random.onUnitSphere * 20;
        randomPos.y = alienYSpawnPos;

        SpawnFighter(alienFighter, randomPos);
        spawnedAliens += 1;
    }

    public void SpawnHumanoidFighter() {
        EconomyManager.instance.Money -= UIManager.instance.JetCost;

        Vector3 randomPos = Random.insideUnitSphere * 10;
        randomPos.y = humanoidYSpawnPos;

        SpawnFighter(humanoidFighter, randomPos);
        spawnedHumanoids += 1;
    }

    public void SpawnFighter(Fighter prefab, Vector3 position) {
        Fighter f = (Fighter)Instantiate(prefab, transform);
        f.transform.position = position;
        f.gameObject.SetActive(true);
    }

}
