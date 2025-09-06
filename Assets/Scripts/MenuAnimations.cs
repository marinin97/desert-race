using UnityEngine;
using DG.Tweening;
using TMPro;

public class MenuAnimations : MonoBehaviour
{
    public RectTransform TitleText;

    public void Awake()
    {
        ScaleTitleText();
        DropDownTitleText();
    }

    private void DropDownTitleText()
    {
        Vector3 startPosition = TitleText.anchoredPosition; 
        TitleText.anchoredPosition = new Vector2(0, 200);
        TitleText.DOAnchorPos(startPosition, 2f).SetEase(Ease.OutBounce);
    }

    private void ScaleTitleText()
    {
        TitleText.DOScale(0.8f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnDestroy()
    {
        TitleText.DOKill();
    }
}
