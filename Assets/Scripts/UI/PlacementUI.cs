using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementUI : MonoBehaviour
{
    public RectTransform upperLeft;
    public RectTransform upperRight;
    public RectTransform lowerLeft;
    public RectTransform lowerRight;

    public float offset = 50f;

    private Rect safeArea;

    private void Start()
    {
        safeArea = Screen.safeArea;
        float xSpot, ySpot;

        if (upperLeft)
        {
            upperLeft.pivot = new Vector2(0, 1);
            xSpot = safeArea.x + offset;
            ySpot = Screen.height - (safeArea.y + safeArea.height) - offset;
            upperLeft.anchoredPosition = new Vector2(xSpot, ySpot);
        }

        if (upperRight)
        {
            upperRight.pivot = new Vector2(1, 1);
            xSpot = (safeArea.x + safeArea.width) - Screen.width - offset;
            ySpot = Screen.height - (safeArea.y + safeArea.height) - offset;
            upperRight.anchoredPosition = new Vector2(xSpot, ySpot);
        }

        if (lowerLeft)
        {
            lowerLeft.pivot = new Vector2(0, 0);
            xSpot = safeArea.x + offset;
            ySpot = Screen.height - (safeArea.y + safeArea.height) + offset;
            lowerLeft.anchoredPosition = new Vector2(xSpot, ySpot);
        }

        if (lowerRight)
        {
            lowerRight.pivot = new Vector2(1, 0);
            xSpot = (safeArea.x + safeArea.width) - Screen.width - offset;
            ySpot = Screen.height - (safeArea.y + safeArea.height) + offset;
            lowerRight.anchoredPosition = new Vector2(xSpot, ySpot);
        }
    }
}
