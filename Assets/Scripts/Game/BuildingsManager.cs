using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsManager : MonoBehaviour {

    public static BuildingsManager instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
    }

    public Transform groundScaler;
    
    private List<Building> buildings = new List<Building> ();

    public int activeBuildings = 0;

    private int currentIndex = 0;
    private int scaleIndex = 0;

    private void Start() {
        buildings = GetComponentsInChildren<Building>().ToList();
    }

    public void Init() {
        currentIndex = 0;
        buildings[currentIndex].UpdateBuildingInfo(0);
        activeBuildings = 1;
    }

    public void AddBuilding() {
        if (currentIndex == buildings.Count - 1) {
            return;
        }
        
        currentIndex++;

        EconomyManager.instance.Money -= UIManager.instance.BuildingCost;
        EconomyManager.instance.Income = (currentIndex + 1) * EconomyManager.instance.baseIncome;
        activeBuildings += 1;

        buildings[currentIndex].UpdateBuildingInfo(0);
        if (buildings[currentIndex].increaseGroundSize) {
            scaleIndex++;

            Vector3 scale = groundScaler.localScale;

            float newScaleValue = scaleIndex + 1;
            Lerper.instance.LerpValue(scale.x, newScaleValue, 1, (float val) => {

                scale.x = val;
                scale.y = 1;
                scale.z = val;
                groundScaler.localScale = scale;
            }, () => {
                scale.x = newScaleValue;
                scale.y = 1;
                scale.z = newScaleValue;
                groundScaler.localScale = scale;
            });

            CameraController.instance.Distance += 4;
            CameraController.instance.centerPos += new Vector3(0, 1, 0);
        }

        UpdateBuildings();
    }

    public void UpdateBuildings() {
        for (int i = 0; i < buildings.Count; i++) {
            if (!buildings[i].active) {
                continue;
            }

            int level = Mathf.Clamp(currentIndex - i, 0, buildings[0].BuildingInfoAmount - 1);
            buildings[i].UpdateBuildingInfo(level);
        }
    }

    public Building GetClosestBuilding(Vector3 pos) {
        List<Building> copyList = new List<Building>();

        for (int i = 0; i < buildings.Count; i++) {
            if (buildings[i].active && buildings[i].BuildingInfo != null) {
                copyList.Add(buildings[i]);
            }
        }

        copyList = copyList.OrderBy(x => Vector3.Distance(pos, x.BuildingInfo.target.position)).ToList();

        if (copyList.Count == 0) {
            return null;
        }

        return copyList[0];
    }
    
}
