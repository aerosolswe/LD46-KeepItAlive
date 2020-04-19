using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetMovement : MonoBehaviour {

    public enum MovementState {
        Follow, Avoid, Idle, Kamikaze
    }

    public Transform target;

    private Rigidbody rb;

    public Transform rotateBody;
    private GameObject avoidBody;

    public float deacceleration = 10;
    public float speed = 10;
    public float rotationSpeed = 10;
    public float minYPosition = 10;
    public float maxYPosition = 12.5f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 dir = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 heading = Vector3.forward;

    public MovementState state = MovementState.Follow;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate() {
        switch (state) {
            case MovementState.Follow:
                if (target == null) {
                    state = MovementState.Idle;
                    return;
                }

                dir = transform.position - target.position;
                dir = dir.normalized;

                targetPos = target.position;

                heading = transform.forward;
                break;
            case MovementState.Kamikaze:
                if (target == null) {
                    state = MovementState.Idle;
                    return;
                }

                dir = transform.position - target.position;
                dir = dir.normalized;

                targetPos = target.position;

                heading = transform.forward;
                break;
            case MovementState.Avoid:
                dir = transform.position - avoidBody.transform.position;
                dir = dir.normalized;

                targetPos = avoidBody.transform.position;
                heading = -transform.forward;

                break;
            case MovementState.Idle:
                Vector3 idlePos = new Vector3(0, 10, 0);

                dir = transform.position - idlePos;
                dir = dir.normalized;

                targetPos = idlePos;

                heading = transform.forward;
                break;
        }

        Move(dir, targetPos);
        
        rb.velocity = velocity;

        if (rb.velocity != Vector3.zero) {
            transform.forward = rb.velocity.normalized;
        }
    }

    private void LateUpdate() {
        float zRot = (rb.angularVelocity.y * -10);
        Vector3 eRot = rotateBody.localEulerAngles;
        eRot.z = zRot;
        rotateBody.localEulerAngles = eRot;
    }

    void Move(Vector3 dir, Vector3 targetPos) {
        // Test randomness
        Vector3 tmpForward = transform.forward;
        transform.forward = dir;
        transform.Rotate(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
        dir = transform.forward;
        transform.forward = tmpForward;

        float rotateModifier = 1;
        float speedModifier = 1;
        
        if (state == MovementState.Idle) {
            rotateModifier = 0.5f;
        }

        if (state != MovementState.Kamikaze && state != MovementState.Follow) {
            if (transform.position.y < minYPosition) {
                float diff = minYPosition - transform.position.y;
                dir.y = -1;
                rotateModifier = 2;
                speedModifier = 0.5f;
            } else if (transform.position.y > maxYPosition) {
                dir.y = 1;
                rotateModifier = 2;
                speedModifier = 1f;
            }
        } else {
            rotateModifier = 4;
            speedModifier = 1f;
        }

        float dist = Vector3.Distance(targetPos, transform.position);

        float distModifier = dist / 10;
        distModifier = Mathf.Clamp(distModifier, 0.2f, 1);

        var rotateAmount = Vector3.Cross(dir, heading);
        rb.angularVelocity = rotateAmount * (rotationSpeed * distModifier * rotateModifier); // speed * distModifier
        
        float correctedSpeed = speed * distModifier * speedModifier;
        
        velocity = transform.forward * speed;
    }

    public void ContinueFollowing() {
        state = MovementState.Follow;
    }

    public void AvoidObject(GameObject go) {
        avoidBody = go;
        state = MovementState.Avoid;
    }

    IEnumerator Wait(float time, System.Action onComplete) {
        yield return new WaitForSeconds(time);

        onComplete?.Invoke();
    }

}
