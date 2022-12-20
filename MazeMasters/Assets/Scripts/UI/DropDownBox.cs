using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownBox : MonoBehaviour
{
    public Image image;
    public ContentSizeFitter sizeFitter;
    public VerticalLayoutGroup content;
    public RectTransform maskTransform;

    private bool droppedDown = true;
    private float transitionTime = 0.1f;
    private Coroutine toggling;

    public void ToggleDropDown()
    {
        if (toggling != null)
        {
            StopCoroutine(toggling);
            toggling = null;
        }
        toggling = StartCoroutine("ToggleDrop");
    }

    IEnumerator ToggleDrop()
    {

        float targetAngle = droppedDown ? 180 : 0;
        float speedRotate = (targetAngle - image.rectTransform.rotation.eulerAngles.z) / transitionTime;

        float contentHeight = 0;

        RectTransform[] transforms = image.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform rec in transforms)
        {
            contentHeight += rec.sizeDelta.y;
        }
        float targetOffset = droppedDown ? -contentHeight : 0;
        float speedDrop = contentHeight / transitionTime;
        float targetSize = droppedDown ? 0 : contentHeight;
        float offsetTracker = content.padding.top;

        sizeFitter.enabled = false;
        droppedDown = !droppedDown;

        for (; image.rectTransform.rotation.eulerAngles.z != targetAngle;)
        {
            //calculate changes
            float diffRotate = targetAngle - image.rectTransform.rotation.eulerAngles.z;
            float changeAmountRotate = speedRotate * Time.deltaTime;

            float diffOffset = targetOffset - content.padding.top;
            float changeOffset = speedDrop * Time.deltaTime;
            


            if (Mathf.Abs(diffRotate) <= Mathf.Abs(changeAmountRotate)) image.rectTransform.rotation = Quaternion.Euler(0, 0, targetAngle);

            if (Mathf.Abs(diffOffset) <= Mathf.Abs(changeOffset))
            {
                offsetTracker = targetOffset;
                content.padding.top = Mathf.RoundToInt(offsetTracker);
            }

            //check if finished
            if (image.rectTransform.rotation.eulerAngles.z == targetAngle && content.padding.top == targetOffset && true) continue;
            

            //add changes
            image.rectTransform.rotation = Quaternion.Euler(0, 0, diffRotate + image.rectTransform.rotation.eulerAngles.z);
            content.padding.top = Mathf.RoundToInt(offsetTracker);
            yield return null;
        }

        sizeFitter.enabled = droppedDown;
        toggling = null;
        
    }
}
