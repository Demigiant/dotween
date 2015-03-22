using System;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EaseCurves : BrainBase
{
	public RawImage image;
    public AnimationCurve easeCurve;
    public float tweenDuration = 1;
    public int txDistance = 2;

    int txH, txW;
    int txBorder, easeH;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        Setup();
    }

	void Setup()
	{
        RectTransform rt = image.GetComponent<RectTransform>();
	    txW = (int)rt.sizeDelta.x;
	    txH = (int)rt.sizeDelta.y;
	    easeH = (int)(txH * 0.35f);
        txBorder = (int)((txH - easeH) * 0.5f);
        Color32[] colors = new Color32[txW * txH];
        for (int c = 0; c < colors.Length; ++c) colors[c] = new Color(0.1f, 0.1f, 0.1f, 1);
	    int lineP = txBorder * txW;
        for (int c = lineP; c < lineP + txW; ++c) {
            colors[c] = new Color(0.25f, 0.25f, 0.25f, 1);
            colors[c + txW * easeH] = new Color(0.25f, 0.25f, 0.25f, 1);
        }

        // Create a tween for each easeType
        int totTypes = Enum.GetNames(typeof(Ease)).Length;
        int distX = txW;
        int distY = txH;
        int totCols = Screen.width / txW - 1;
        float startX = image.transform.position.x;
        float startY = image.transform.position.y;
        Vector2 gridCount = Vector2.zero;
        for (int i = 0; i < totTypes + 1; ++i) {
            // Instantiate and position new Images
            Transform t = ((GameObject)Instantiate(image.gameObject)).transform;
            t.SetParent(image.transform.parent);
            t.position = new Vector3(startX + distX * gridCount.x + txDistance * gridCount.x, startY - distY * gridCount.y - txDistance * gridCount.y, 0);
            gridCount.x++;
            if (gridCount.x > totCols) {
                gridCount.y++;
                gridCount.x = 0;
            }
            // Set textures
            Texture2D tx = new Texture2D(txW, txH, TextureFormat.ARGB32, false);
            tx.filterMode = FilterMode.Point;
            tx.SetPixels32(colors);
            tx.Apply();
            RawImage img = t.GetComponent<RawImage>();
            img.texture = tx;
            // Set tween and text
            Ease easeType = (Ease)i;
            float val = txBorder;
            Tween tween = DOTween.To(() => val, x => val = x, txBorder + easeH, tweenDuration).SetDelay(1);
            tween.OnUpdate(() => SetTextureEase(easeType, tx, tween.Elapsed(), (int)val));
            if (i == totTypes) {
                img.GetComponentInChildren<Text>().text = "custom";
                tween.SetEase((time, duration, overshootOrAmplitude, period)=> {
                    return (float)Math.Sin(time /= duration) / (float)Math.Cos(time /= duration);
                });
            } else {
                img.GetComponentInChildren<Text>().text = easeType.ToString();
                if (easeType == Ease.INTERNAL_Custom) tween.SetEase(easeCurve);
                else tween.SetEase(easeType);
            }
        }
        // Disable original image
        image.gameObject.SetActive(false);
	}

    void SetTextureEase(Ease easeType, Texture2D tx, float elapsed, int y)
    {
        int x = (int)((txW - 1) * (elapsed / tweenDuration));
        if (y > txH - 1 || y < 0) return; // elastic/back eases

        tx.SetPixel(x, y, Color.white);
        tx.Apply();
    }
}