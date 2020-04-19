using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour {

    public Fighter enemy = null;
    public Building targetBuilding = null;

    private JetMovement jetMovement;
    
    public LayerMask enemyMask;
    public LayerMask buildingsMask;

    public bool humanoid = true;

    public float fireRange = 5;

    public FighterWeapon[] weapons;

    public int Health = 100;
    public int buildingDamage = 25;

    private bool removing = false;

    public float checkRate = 0.75f;
    public float rate = 0;

    private void Awake() {
        jetMovement = GetComponent<JetMovement>();
    }

    private void Update() {
        if (removing)
            return;

        if (targetBuilding != null) {
            if (!targetBuilding.active) {
                targetBuilding = null;
                jetMovement.target = null;
                jetMovement.state = JetMovement.MovementState.Idle;
                return;
            }

            float dist = Vector3.Distance(transform.position, targetBuilding.BuildingInfo.target.position);

            if (dist <= 0.4f) {
                targetBuilding.Health -= buildingDamage;

                Explosion e = ExplosionParent.instance.GetExplosion();
                e.transform.position = transform.position;
                e.gameObject.SetActive(true);

                Remove();
            }
            return;
        }

        if (enemy == null) {
            rate += Time.deltaTime;

            if (rate >= checkRate) {
                LookForSuitableEnemy();
                rate = 0;
            }

        } else {
            float dstToEnemy = Vector3.Distance(enemy.transform.position, transform.position);

            if (dstToEnemy <= fireRange) {
                Attack();
            }
        }
    }

    private void Attack() {
        foreach (FighterWeapon fw in weapons) {
            fw.Fire();
        }
    }

    public void LookForSuitableEnemy() {
        var colliders = Physics.OverlapSphere(transform.position, 10, enemyMask);

        LookForEnemies(colliders);

        if (!humanoid && enemy == null && FightersManager.instance.spawnedHumanoids == 0) { // add check if jets are alive
            LookForBuildings();
        }
    }

    public void LookForEnemies(Collider[] colliders) {

        foreach (Collider col in colliders) {
            Fighter fighter = col.GetComponent<Fighter>();

            if (fighter != null) {
                enemy = fighter;
                jetMovement.target = fighter.transform;
                jetMovement.ContinueFollowing();
            }
        }
    }

    public void LookForBuildings() {

        Building building = BuildingsManager.instance.GetClosestBuilding(transform.position);

        if (building != null && building.BuildingInfo != null && building.active) {
            enemy = null;
            targetBuilding = building;
            jetMovement.target = building.BuildingInfo.target;
            jetMovement.state = JetMovement.MovementState.Kamikaze;
        }
        
    }

    public void TakeDamage(HitInfo hitInfo) {
        Health -= hitInfo.damage;

        if (Health < 0) {
            Remove();
        }
    }

    public void Remove() {
        if (removing)
            return;

        removing = true;

        if (humanoid) {
            FightersManager.instance.spawnedHumanoids -= 1;
        } else {
            FightersManager.instance.spawnedAliens -= 1;
        }

        Destroy(this.gameObject);
    }
}

public class HitInfo {
    public int damage;
    public Fighter attacker;
}
