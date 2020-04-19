using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {

    public static BulletPool instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
    }

    private List<FighterWeaponBullet> humanoidBullets = new List<FighterWeaponBullet>();
    private List<FighterWeaponBullet> alienBullets = new List<FighterWeaponBullet>();

    public FighterWeaponBullet humanoidBulletPrefab;
    public FighterWeaponBullet alienBulletPrefab;

    public int bulletAmount = 50;

    private void Start() {
        for (int i = 0; i < bulletAmount; i++) {
            FighterWeaponBullet hb = CreateBullet(humanoidBulletPrefab);
            humanoidBullets.Add(hb);
        }

        for (int i = 0; i < bulletAmount; i++) {
            FighterWeaponBullet hb = CreateBullet(alienBulletPrefab);
            alienBullets.Add(hb);
        }
    }

    public FighterWeaponBullet GetHumanoidBullet() {
        return GetBullet(humanoidBullets, humanoidBulletPrefab);
    }

    public FighterWeaponBullet GetAlienBullet() {
        return GetBullet(alienBullets, alienBulletPrefab);
    }

    public FighterWeaponBullet GetBullet(List<FighterWeaponBullet> bulletList, FighterWeaponBullet prefab) {
        foreach (FighterWeaponBullet bullet in bulletList) {
            if (!bullet.gameObject.activeInHierarchy) {
                return bullet;
            }
        }

        FighterWeaponBullet b = CreateBullet(prefab);
        bulletList.Add(b);

        return b;
    }

    private FighterWeaponBullet CreateBullet(FighterWeaponBullet prefab) {
        FighterWeaponBullet hb = Instantiate(prefab, transform);
        hb.gameObject.SetActive(false);
        hb.transform.localPosition = Vector3.zero;

        return hb;
    }
}
