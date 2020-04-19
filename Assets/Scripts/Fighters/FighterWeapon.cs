using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterWeapon : MonoBehaviour {

    public Fighter fighter;

    public int Damage = 50;
    public float fireRate = 0.5f;

    public GameObject bulletObj;

    private bool canFire = true;

    private float rate = 0;

    private void Update() {
        if (!canFire) {
            rate += Time.deltaTime;

            if (rate >= fireRate) {
                rate = 0;
                canFire = true;
            }
        }
    }

    public void Fire() {
        if (!canFire) {
            return;
        }

        canFire = false;

        FighterWeaponBullet bullet = GetBullet();
        bullet.gameObject.SetActive(true);

        HitInfo hi = new HitInfo {
            attacker = fighter,
            damage = Damage
        };

        bullet.Init(hi, transform.position, transform.forward);
    }
    
    private FighterWeaponBullet GetBullet() {
        if (fighter.humanoid) {
            return BulletPool.instance.GetHumanoidBullet();
        } else {
            return BulletPool.instance.GetAlienBullet();
        }
    }
}
