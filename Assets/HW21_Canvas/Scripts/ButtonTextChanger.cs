using UnityEngine;
using TMPro;

public class ButtonTextChanger : MonoBehaviour
{
    public TextMeshProUGUI targetText;

    private int _clickCount = 0;
    private static readonly string[] Messages =
    {
        "버튼을 클릭하세요!",
        "1번 클릭됨 ✓",
        "2번 클릭됨 ✓",
        "리셋! 다시 클릭해보세요."
    };

    public void OnButtonClick()
    {
        _clickCount = (_clickCount + 1) % Messages.Length;
        if (targetText != null)
            targetText.text = Messages[_clickCount];
    }
}
