using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorHowerTip : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private TMP_Text tipText;
    [SerializeField]
    [TextArea]
    private string tipString;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tipText.text = tipString;
    }
}
