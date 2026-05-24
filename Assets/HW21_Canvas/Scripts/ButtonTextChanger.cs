using UnityEngine;
using UnityEngine.UI;

public class ButtonTextChanger : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Text titleText;
    [SerializeField] private Text captionText;
    [SerializeField] private Sprite imageA;
    [SerializeField] private Sprite imageB;
    [SerializeField] private Sprite imageC;

    private void Start()
    {
        ShowA();
    }

    public void ShowA()
    {
        ShowImage(imageA, "Image A", "A: Puppy");
    }

    public void ShowB()
    {
        ShowImage(imageB, "Image B", "B: Cat close-up");
    }

    public void ShowC()
    {
        ShowImage(imageC, "Image C", "C: Cat nose close-up");
    }

    private void ShowImage(Sprite sprite, string title, string caption)
    {
        if (targetImage != null)
        {
            targetImage.sprite = sprite;
            targetImage.preserveAspect = true;
        }

        if (titleText != null)
            titleText.text = title;

        if (captionText != null)
            captionText.text = caption;
    }
}
