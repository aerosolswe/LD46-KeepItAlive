using System;
using System.Collections;
using UnityEngine;

public class Lerper : MonoBehaviour {

    public static Lerper instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
    }

    public void LerpValue(float from, float to, float time, Action<float> onUpdate, Action onComplete) {
        StartCoroutine(lerpValue(from, to, time, onUpdate, onComplete));
    }

    IEnumerator lerpValue(float from, float to, float time, Action<float> onUpdate, Action onComplete) {

        float currentTime = 0;

        bool lerping = true;

        while (lerping) {
            currentTime += Time.deltaTime;

            if (currentTime > time) {
                currentTime = time;
            }

            float t = currentTime / time;

            float val = Mathf.Lerp(from, to, t);
            onUpdate?.Invoke(val);

            if (t >= 1) {
                lerping = false;
            }

            yield return null;
        }

        onComplete?.Invoke();
    }
}
