using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Text statusText;

    private void Awake()
    {
        EnsureHud();
    }

    public void UpdateStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private void EnsureHud()
    {
        if (FindObjectOfType<Canvas>() != null)
        {
            Canvas existingCanvas = FindObjectOfType<Canvas>();
            if (existingCanvas != null)
            {
                statusText = existingCanvas.GetComponentInChildren<Text>();
                if (statusText != null)
                {
                    return;
                }
            }
        }

        GameObject canvasObject = new GameObject("HUD Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        GameObject textObject = new GameObject("Status Text");
        textObject.transform.SetParent(canvasObject.transform, false);

        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);
        rectTransform.pivot = new Vector2(0f, 1f);
        rectTransform.anchoredPosition = new Vector2(20f, -20f);
        rectTransform.sizeDelta = new Vector2(500f, 60f);

        statusText = textObject.AddComponent<Text>();
        statusText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        statusText.text = "Recording movement...";
        statusText.fontSize = 24;
        statusText.color = Color.white;
        statusText.alignment = TextAnchor.UpperLeft;
    }
}
