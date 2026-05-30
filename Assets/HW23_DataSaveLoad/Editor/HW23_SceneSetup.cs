using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class HW23_SceneSetup
{
    [MenuItem("HW23/Setup HW23_DataSaveLoad Scene")]
    public static void SetupScene()
    {
        // ── Canvas 설정 ──────────────────────────────────────────────
        var canvasGO = GameObject.Find("Canvas");
        if (canvasGO == null)
        {
            canvasGO = new GameObject("Canvas");
            canvasGO.AddComponent<Canvas>();
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // ── EventSystem ──────────────────────────────────────────────
        if (GameObject.FindObjectOfType<EventSystem>() == null)
        {
            var esGO = new GameObject("EventSystem");
            esGO.AddComponent<EventSystem>();
            esGO.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
        }
        else
        {
            // StandaloneInputModule → InputSystemUIInputModule 교체
            var esGO = GameObject.FindObjectOfType<EventSystem>().gameObject;
            var sim = esGO.GetComponent<StandaloneInputModule>();
            if (sim != null)
            {
                GameObject.DestroyImmediate(sim);
                esGO.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            }
        }

        // ── 패널 배경 ────────────────────────────────────────────────
        var panel = MakePanel(canvasGO.transform);

        // ── 버튼 생성 ────────────────────────────────────────────────
        var saveBtn  = MakeButton(panel.transform, "SaveButton",  "Save",  new Vector2(-160, -30));
        var loadBtn  = MakeButton(panel.transform, "LoadButton",  "Load",  new Vector2(0,   -30));
        var resetBtn = MakeButton(panel.transform, "ResetButton", "Reset", new Vector2(160, -30));

        // ── 상태 텍스트 ──────────────────────────────────────────────
        var statusGO = new GameObject("StatusText");
        statusGO.transform.SetParent(panel.transform, false);
        var statusText = statusGO.AddComponent<Text>();
        statusText.text = "";
        statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        statusText.fontSize = 20;
        statusText.color = Color.yellow;
        statusText.alignment = TextAnchor.MiddleCenter;
        var statusRT = statusGO.GetComponent<RectTransform>();
        statusRT.anchoredPosition = new Vector2(0, 20);
        statusRT.sizeDelta = new Vector2(400, 40);

        // ── UIController 연결 ─────────────────────────────────────────
        var uiGO = GameObject.Find("Canvas") ?? canvasGO;
        var uiCtrl = uiGO.GetComponent<HW23_UIController>() ?? uiGO.AddComponent<HW23_UIController>();
        uiCtrl.saveButton  = saveBtn.GetComponent<Button>();
        uiCtrl.loadButton  = loadBtn.GetComponent<Button>();
        uiCtrl.resetButton = resetBtn.GetComponent<Button>();
        uiCtrl.statusText  = statusText;

        // ── GameManager 확인 ─────────────────────────────────────────
        var gm = GameObject.Find("GameManager");
        if (gm == null) { gm = new GameObject("GameManager"); }
        if (gm.GetComponent<HW23_SaveLoadManager>() == null)
            gm.AddComponent<HW23_SaveLoadManager>();

        EditorUtility.SetDirty(canvasGO);
        EditorUtility.SetDirty(gm);
        EditorSceneManager.SaveOpenScenes();
        Debug.Log("[HW23] Scene setup complete!");
    }

    private static GameObject MakePanel(Transform parent)
    {
        var go = new GameObject("ButtonPanel");
        go.transform.SetParent(parent, false);
        var img = go.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0.5f);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.pivot     = new Vector2(0.5f, 0f);
        rt.anchoredPosition = new Vector2(0, 10);
        rt.sizeDelta = new Vector2(520, 90);
        return go;
    }

    private static GameObject MakeButton(Transform parent, string goName, string label, Vector2 pos)
    {
        var go = new GameObject(goName);
        go.transform.SetParent(parent, false);
        var img = go.AddComponent<Image>();
        img.color = Color.white;
        var btn = go.AddComponent<Button>();
        var rt  = go.GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(140, 50);

        var textGO = new GameObject("Text");
        textGO.transform.SetParent(go.transform, false);
        var txt = textGO.AddComponent<Text>();
        txt.text = label;
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.fontSize = 18;
        txt.color = Color.black;
        txt.alignment = TextAnchor.MiddleCenter;
        var trt = textGO.GetComponent<RectTransform>();
        trt.anchorMin = Vector2.zero;
        trt.anchorMax = Vector2.one;
        trt.sizeDelta = Vector2.zero;

        return go;
    }
}
