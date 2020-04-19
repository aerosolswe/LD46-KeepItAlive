using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float scaleModifier = 1;

    private void OnEnable() {
        StartCoroutine(Animation());
    }

    IEnumerator Animation() {
        float fromScale = 1 * scaleModifier;
        float toScale = 3 * scaleModifier;

        transform.localScale = new Vector3(fromScale, fromScale, fromScale);

        float time = 0.6f;
        float currentTime = 0;

        bool lerping = true;

        while (lerping) {

            currentTime += Time.deltaTime;

            if (currentTime > time) {
                currentTime = time;
            }

            float t = currentTime / time;
            float scale = Mathf.Lerp(fromScale, toScale, t);
            transform.localScale = new Vector3(scale, scale, scale);

            if (t >= 1) {
                lerping = false;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
