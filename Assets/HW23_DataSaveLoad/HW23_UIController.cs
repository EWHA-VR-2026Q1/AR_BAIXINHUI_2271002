using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HW23_UIController : MonoBehaviour
{
    [Header("UI 버튼")]
    public Button saveButton;
    public Button loadButton;
    public Button resetButton;

    [Header("상태 텍스트")]
    public Text statusText;

    private void Start()
    {
        if (saveButton != null)
            saveButton.onClick.AddListener(OnSave);
        if (loadButton != null)
            loadButton.onClick.AddListener(OnLoad);
        if (resetButton != null)
            resetButton.onClick.AddListener(OnReset);
    }

    private void OnSave()
    {
        HW23_SaveLoadManager.Instance?.SaveData();
        ShowStatus("저장 완료!");
    }

    private void OnLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ShowStatus("불러오기 완료!");
    }

    private void OnReset()
    {
        HW23_SaveLoadManager.Instance?.ResetData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ShowStatus("초기화 완료!");
    }

    private void ShowStatus(string msg)
    {
        if (statusText != null)
        {
            statusText.text = msg;
            CancelInvoke(nameof(ClearStatus));
            Invoke(nameof(ClearStatus), 2f);
        }
        Debug.Log($"[HW23 UI] {msg}");
    }

    private void ClearStatus()
    {
        if (statusText != null)
            statusText.text = "";
    }
}
