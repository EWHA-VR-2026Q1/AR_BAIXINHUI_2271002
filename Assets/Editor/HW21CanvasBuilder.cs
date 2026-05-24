using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class HW21CanvasBuilder
{
    private const string ScenePath = "Assets/HW21_Canvas/Scenes/HW21_Canvas.unity";

    [MenuItem("Tools/HW21/Build Canvas Scene")]
    public static void BuildCanvasScene()
    {
        Debug.Log("[HW21] Rebuilding Canvas scene...");
        ImportSprite("Assets/HW21_Canvas/Images/Image_A_Puppy.png");
        ImportSprite("Assets/HW21_Canvas/Images/Image_B_Cat.jpg");
        ImportSprite("Assets/HW21_Canvas/Images/Image_C_Cat_Nose.jpg");

        var scene = EditorSceneManager.OpenScene(ScenePath);
        foreach (var root in scene.GetRootGameObjects())
            Object.DestroyImmediate(root);

        CreateCamera();
        CreateEventSystem();

        var canvas = CreateCanvas();
        CreatePanel("Background", canvas.transform, Vector2.zero, Vector2.one, new Color(0.88f, 0.91f, 0.94f));
        Debug.Log("[HW21] Background created.");

        var title = CreateText("Title Text", canvas.transform, "Image A", 48, FontStyle.Bold,
            new Vector2(0.08f, 0.86f), new Vector2(0.92f, 0.95f), TextAnchor.MiddleCenter, Color.black);
        var caption = CreateText("Caption Text", canvas.transform, "A: Puppy", 28, FontStyle.Normal,
            new Vector2(0.08f, 0.80f), new Vector2(0.92f, 0.86f), TextAnchor.MiddleCenter, new Color(0.2f, 0.24f, 0.28f));
        Debug.Log("[HW21] Text labels created.");

        var frame = CreatePanel("Image Area", canvas.transform, new Vector2(0.02f, 0.18f), new Vector2(0.98f, 0.98f), Color.white);
        var displayImage = CreateDisplayImage(frame.transform);
        Debug.Log("[HW21] Image area created.");

        var row = CreateButtonRow(canvas.transform);
        row.transform.SetAsLastSibling();
        var switcher = new GameObject("Image Switcher").AddComponent<ButtonTextChanger>();

        var serialized = new SerializedObject(switcher);
        serialized.FindProperty("targetImage").objectReferenceValue = displayImage;
        serialized.FindProperty("titleText").objectReferenceValue = title;
        serialized.FindProperty("captionText").objectReferenceValue = caption;
        serialized.FindProperty("imageA").objectReferenceValue =
            AssetDatabase.LoadAssetAtPath<Sprite>("Assets/HW21_Canvas/Images/Image_A_Puppy.png");
        serialized.FindProperty("imageB").objectReferenceValue =
            AssetDatabase.LoadAssetAtPath<Sprite>("Assets/HW21_Canvas/Images/Image_B_Cat.jpg");
        serialized.FindProperty("imageC").objectReferenceValue =
            AssetDatabase.LoadAssetAtPath<Sprite>("Assets/HW21_Canvas/Images/Image_C_Cat_Nose.jpg");
        serialized.ApplyModifiedPropertiesWithoutUndo();

        CreateButton(row.transform, "A", switcher.ShowA);
        CreateButton(row.transform, "B", switcher.ShowB);
        CreateButton(row.transform, "C", switcher.ShowC);
        Debug.Log("[HW21] Buttons created.");

        switcher.ShowA();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("[HW21] Canvas scene ready for Play Mode: Canvas, 3 Buttons, OnClick events, Image swap.");
    }

    private static void ImportSprite(string path)
    {
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null)
            return;

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.mipmapEnabled = false;
        importer.SaveAndReimport();
    }

    private static void CreateCamera()
    {
        var go = new GameObject("Main Camera");
        go.tag = "MainCamera";
        go.transform.position = new Vector3(0f, 0f, -10f);

        var camera = go.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.88f, 0.91f, 0.94f);
        camera.orthographic = true;
        camera.orthographicSize = 5f;
        go.AddComponent<AudioListener>();
    }

    private static void CreateEventSystem()
    {
        var go = new GameObject("EventSystem");
        go.AddComponent<EventSystem>();

        var inputSystemModuleType = System.Type.GetType(
            "UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
        if (inputSystemModuleType != null)
            go.AddComponent(inputSystemModuleType);
        else
            go.AddComponent<StandaloneInputModule>();
    }

    private static Canvas CreateCanvas()
    {
        var go = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var scaler = go.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.matchWidthOrHeight = 0f;
        return canvas;
    }

    private static GameObject CreatePanel(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        go.GetComponent<Image>().color = color;
        return go;
    }

    private static Image CreateDisplayImage(Transform parent)
    {
        var go = new GameObject("Target Image", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(24f, 24f);
        rect.offsetMax = new Vector2(-24f, -24f);

        var image = go.GetComponent<Image>();
        image.color = Color.white;
        image.preserveAspect = true;
        image.raycastTarget = false;
        return image;
    }

    private static GameObject CreateButtonRow(Transform parent)
    {
        var go = new GameObject("Button Row", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.02f, 0.02f);
        rect.anchorMax = new Vector2(0.98f, 0.16f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        var layout = go.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = 12f;
        layout.padding = new RectOffset(12, 12, 12, 12);
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = true;
        return go;
    }

    private static Text CreateText(string name, Transform parent, string value, int size, FontStyle style,
        Vector2 anchorMin, Vector2 anchorMax, TextAnchor alignment, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Text));
        go.transform.SetParent(parent, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        var text = go.GetComponent<Text>();
        text.text = value;
        text.font = Font.CreateDynamicFontFromOSFont("Arial", size);
        text.fontSize = size;
        text.fontStyle = style;
        text.alignment = alignment;
        text.color = color;
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 12;
        text.resizeTextMaxSize = size;
        return text;
    }

    private static void CreateButton(Transform parent, string label, UnityEngine.Events.UnityAction action)
    {
        var go = new GameObject($"Button {label}", typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
        go.transform.SetParent(parent, false);

        var image = go.GetComponent<Image>();
        image.color = new Color(0.12f, 0.36f, 0.62f);

        var button = go.GetComponent<Button>();
        var colors = button.colors;
        colors.normalColor = new Color(0.12f, 0.36f, 0.62f);
        colors.highlightedColor = new Color(0.18f, 0.48f, 0.78f);
        colors.pressedColor = new Color(0.08f, 0.24f, 0.44f);
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;
        UnityEventTools.AddPersistentListener(button.onClick, action);

        go.GetComponent<LayoutElement>().minHeight = 72f;

        var text = CreateText($"Button {label} Text", go.transform, label, 42, FontStyle.Bold,
            Vector2.zero, Vector2.one, TextAnchor.MiddleCenter, Color.white);
        text.raycastTarget = false;
    }
}
