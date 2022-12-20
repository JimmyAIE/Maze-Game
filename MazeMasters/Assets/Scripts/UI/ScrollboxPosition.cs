using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollboxPosition : MonoBehaviour
{
    public static void AdjustContentPosition(RectTransform viewPort, RectTransform content, Scrollbar scrollbar, bool topToBottom)
    {
        if (content.sizeDelta.y < viewPort.sizeDelta.y) scrollbar.value = 0; //stops scrolling when there is not enough content to fill the page
        float valueUpOrDown = topToBottom ? 1: -1; // sets if the value should make the content go up or down as it increases

        float yAdjust = (content.sizeDelta.y / 2 + ((content.anchorMin.y - 1) * viewPort.sizeDelta.y)) * valueUpOrDown * -1;

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, (content.sizeDelta.y - viewPort.sizeDelta.y) * scrollbar.value * valueUpOrDown + yAdjust);
    }

    public static void ScrollContentPosition(float scrollValue, float sens, RectTransform viewPort, RectTransform content, Scrollbar scrollbar, bool topToBottom)
    {
        float upOrDown = scrollValue > 0 ? 1 : -1;
        float valueUpOrDown = topToBottom ? -1 : 1; // sets if the value should make the content go up or down as it increases

        scrollbar.value += (sens * upOrDown / (content.sizeDelta.y - viewPort.sizeDelta.y)) * valueUpOrDown;
        if (scrollbar.value > 1) scrollbar.value = 1;
        if (scrollbar.value < 0) scrollbar.value = 0;
        AdjustContentPosition(viewPort, content, scrollbar, topToBottom);
    }
}
