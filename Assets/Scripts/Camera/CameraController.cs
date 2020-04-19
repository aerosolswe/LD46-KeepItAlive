using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
    }

    public Vector3 centerPos = new Vector3();

    public float yMinClamp = 0;
    public float yMaxClamp = 90;

    private float distance = 20;

    private Vector3 mousePos = new Vector3();

    private float xAngle = 0;
    private float yAngle = 0;

    private void Start() {
        Application.targetFrameRate = 144; // move this to a more fitting script

        xAngle = transform.eulerAngles.y;
        yAngle = transform.eulerAngles.x;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            mousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) {
            Vector3 newMousePos = Input.mousePosition;
            var delta = newMousePos - mousePos;

            xAngle += delta.x * distance * 0.02f;
            yAngle -= delta.y * distance * 0.02f;
            mousePos = newMousePos;
        }

        yAngle = ClampAngle(yAngle, yMinClamp, yMaxClamp);

        Quaternion rotation = Quaternion.Euler(yAngle, xAngle, 0);
        
        Vector3 position = rotation * new Vector3(0, 0, -distance) + centerPos;

        transform.rotation = rotation;
        transform.position = position;
    }

    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public float Distance {
        get {
            return distance;
        }
        set {
            Lerper.instance.LerpValue(distance, value, 1, (float val) => {
                distance = val;
            }, () => {
                distance = value;
            });
        }
    }

}
