using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBounds : MonoBehaviour {
    
    private void OnTriggerExit(Collider other) {
        FighterWeaponBullet bullet = other.GetComponentInParent<FighterWeaponBullet>();

        bullet?.ResetBullet();

        JetMovement jetMovement = other.GetComponent<JetMovement>();
        if (jetMovement != null) {
            jetMovement.state = JetMovement.MovementState.Idle;
        }
    }
}
