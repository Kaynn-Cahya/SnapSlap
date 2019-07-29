using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class GraphicFader : MonoBehaviour {

    public delegate void OnGraphicFaded();

    private Graphic objGraphic;

    private Coroutine fadeCoroutine;

    private void Awake() {
        objGraphic = GetComponent<Graphic>();
    }

    public void FadeGraphicToColor(Color color, float fadeSpeed = 1f, OnGraphicFaded callback = null) {
        StopCoroutineIfExists();

        fadeCoroutine = StartCoroutine(FadeGraphicToColorCoroutine(color, fadeSpeed, callback));

        #region Local_Function

        void StopCoroutineIfExists() {
            if (fadeCoroutine != null) {
                StopCoroutine(fadeCoroutine);
            }
        }

        #endregion
    }

    private IEnumerator FadeGraphicToColorCoroutine(Color color, float fadeSpeed, OnGraphicFaded callback) {

        Color start = objGraphic.color;

        float progress = 0f;
        while (progress < 1f) {
            objGraphic.color = Color.Lerp(start, color, progress);

            progress += Time.deltaTime * fadeSpeed;
            yield return new WaitForEndOfFrame();
        }

        objGraphic.color = color;

        callback?.Invoke();
        yield return null;

    }

}
