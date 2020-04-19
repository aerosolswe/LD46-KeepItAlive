using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetAvoid : MonoBehaviour {

    private JetMovement jetMovement;

    public float continueDstOffset = 4;
    private float reachDst = 0;
    private Transform otherObject;

    private void Start() {
        jetMovement = GetComponentInParent<JetMovement>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (jetMovement == null)
            return;

        /*otherObject = other.transform;

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null) {
            Vector3 forceDir = transform.position - other.transform.position;

            rb.AddForce(forceDir * 10, ForceMode.Force);
        }*/
    }
    
}
