using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingInfo {
    public GameObject model;
    public int health;
    public Transform target;
}

public class Building : MonoBehaviour {

    public BuildingInfo[] buildingInfos = new BuildingInfo[0];

    public GameObject typesParent;

    private BuildingInfo bi;
    private int currentHealth;

    public bool increaseGroundSize = false;

    [HideInInspector]
    public bool active = false;

    private int currentLevel = -1;

    private void Awake() {
        SetRandomRotation();
    }

    public void UpdateBuildingInfo(int level) {
        if (currentLevel == level) {
            return;
        }

        bool lerpPosition = true;

        if (this.bi != null) {
            this.bi.model.SetActive(false);
            lerpPosition = false;
        }

        this.bi = buildingInfos[level];

        if (lerpPosition) {
            Lerper.instance.LerpValue(-5, 0, 1, (float val) => {
                Vector3 pos = this.bi.model.transform.localPosition;
                pos.y = val;
                this.bi.model.transform.localPosition = pos;
            }, () => {
            });
        }

        bi.model.SetActive(true);
        Health = bi.health;
        active = true;
        currentLevel = level;

        float div = (float)(level + 1) / (float)BuildingInfoAmount;
        Vector3 scale = new Vector3(1, 1 * ((Random.Range(0.01f, 0.2f) * div) + 1), 1);
        typesParent.transform.localScale = scale;
    }

    void SetRandomRotation() {
        int randomIndex = Random.Range(0, 4);
        float angle = randomIndex * 90;

        Vector3 localRot = transform.localEulerAngles;
        localRot.y = angle;
        transform.localEulerAngles = localRot;
    }

    public int Health {
        get {
            return currentHealth;
        }
        set {
            currentHealth = value;

            if (currentHealth <= 0) {
                currentHealth = 100;

                active = false;
                currentLevel = -1;

                if (this.bi != null) {
                    Lerper.instance.LerpValue(0, -5, 1, (float val) => {
                        Vector3 pos = this.bi.model.transform.localPosition;
                        pos.y = val;
                        this.bi.model.transform.localPosition = pos;
                    }, () => {
                        this.bi.model.SetActive(false);
                        Vector3 pos = this.bi.model.transform.localPosition;
                        pos.y = 0;
                        this.bi.model.transform.localPosition = pos;
                        this.bi = null;
                    });
                }

                BuildingsManager.instance.activeBuildings -= 1;
            }
        }
    }

    public BuildingInfo BuildingInfo {
        get {
            return bi;
        }
    }

    public int BuildingInfoAmount {
        get {
            return buildingInfos.Length;
        }
    }

}
