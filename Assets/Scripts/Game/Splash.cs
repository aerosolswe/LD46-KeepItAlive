using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

    public CanvasGroup cg;

    private IEnumerator Start() {
        float fromAlpha = 1;
        float toAlpha = 0;

        float time = 1;
        float currentTime = 0;

        yield return SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);

        bool lerping = true;

        while (lerping) {
            currentTime += Time.deltaTime;

            if (currentTime > time) {
                currentTime = time;
            }

            float t = currentTime / time;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, t);

            cg.alpha = alpha;

            if (t >= 1) {
                lerping = false;
            }

            yield return null;
        }

        UIManager.instance.ShowUI();
        yield return SceneManager.UnloadSceneAsync("SplashScene");
    }
}
