using UnityEngine;
using DG.Tweening;
using TMPro;

public class MenuAnimations : MonoBehaviour
{
    public TMP_Text TitleText;

    private RectTransform _titleTextTransform;

    public void Awake()
    {
        _titleTextTransform = TitleText.GetComponent<RectTransform>();
        //ScaleTitleText();
        DropDownTitleText();
    }

    private void DropDownTitleText()
    {
        Vector3 startPosition = new Vector2(0, -239f);
        _titleTextTransform.position = new Vector2(0, 200);
        _titleTextTransform.DOAnchorPos(startPosition, 2f);
    }

    private void ScaleTitleText()
    {
        TitleText.transform.DOScale(0.8f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnDestroy()
    {
        TitleText.transform.DOKill();
    }
}
