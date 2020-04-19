using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {

    private static CloudController instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
    }

    public GameObject cloudPrefab;

    public float radius = 5;
    public float y = 5;

    public int clouds = 7;

    private void Start() {
        for (int i = 0; i < clouds; i++) {
            GameObject cloud = CreateCloud();
            cloud.SetActive(true);
        }
    }

    private void Update() {
        Vector3 rot = transform.localEulerAngles;
        rot.y += Time.deltaTime;
        transform.localEulerAngles = rot;
    }

    GameObject CreateCloud() {
        GameObject co = Instantiate(cloudPrefab, this.transform);
        co.transform.localPosition = new Vector3();

        Vector2 offset = Random.insideUnitCircle;
        offset.Normalize();
        offset *= radius;

        co.transform.localPosition = new Vector3(offset.x, y, offset.y);

        Vector3 noYPos = co.transform.localPosition;
        noYPos.y = 0;
        Vector3 dir = transform.position - noYPos;
        dir.Normalize();

        co.transform.right = dir;

        return co;
    }
}
