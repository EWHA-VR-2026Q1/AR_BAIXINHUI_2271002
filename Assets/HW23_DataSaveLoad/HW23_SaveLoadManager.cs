using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HW23_SaveLoadManager : MonoBehaviour
{
    public static HW23_SaveLoadManager Instance { get; private set; }

    [Header("저장 대상 (List 기반 - 인스펙터에서 할당 또는 자동 탐색)")]
    public List<GameObject> targetObjects = new List<GameObject>();

    [Header("플레이어 오브젝트 (Tag=Player 자동 탐색)")]
    public GameObject playerObject;

    private string SavePath => Path.Combine(Application.persistentDataPath, "HW23_worldData.json");

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        // 인스펙터에 미할당 시 자동 탐색
        if (playerObject == null)
            playerObject = GameObject.FindWithTag("Player");

        if (targetObjects.Count == 0)
        {
            foreach (var mover in FindObjectsByType<HW23_ObjectMover>(FindObjectsSortMode.None))
                targetObjects.Add(mover.gameObject);
        }

        LoadData();
    }

    public void SaveData()
    {
        HW23_WorldData worldData = new HW23_WorldData();

        // 플레이어 저장
        if (playerObject != null)
        {
            worldData.objects.Add(new HW23_TransformData
            {
                objectName = playerObject.name,
                isPlayer = true,
                position = playerObject.transform.position,
                rotation = playerObject.transform.rotation
            });
        }

        // 오브젝트 목록 저장
        foreach (GameObject obj in targetObjects)
        {
            if (obj == null) continue;
            worldData.objects.Add(new HW23_TransformData
            {
                objectName = obj.name,
                isPlayer = false,
                position = obj.transform.position,
                rotation = obj.transform.rotation
            });
        }

        string json = JsonUtility.ToJson(worldData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[HW23] 저장 완료: {SavePath}\n{json}");
    }

    public void LoadData()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("[HW23] 저장 파일 없음 - 기본 위치 유지");
            return;
        }

        string json = File.ReadAllText(SavePath);
        HW23_WorldData worldData = JsonUtility.FromJson<HW23_WorldData>(json);
        if (worldData == null || worldData.objects == null) return;

        Dictionary<string, HW23_TransformData> map = new Dictionary<string, HW23_TransformData>();
        foreach (var data in worldData.objects)
            if (!map.ContainsKey(data.objectName))
                map[data.objectName] = data;

        // 플레이어 복원
        if (playerObject != null && map.TryGetValue(playerObject.name, out HW23_TransformData pd))
        {
            playerObject.transform.position = pd.position;
            playerObject.transform.rotation = pd.rotation;
        }

        // 오브젝트 복원
        foreach (GameObject obj in targetObjects)
        {
            if (obj == null) continue;
            if (map.TryGetValue(obj.name, out HW23_TransformData td))
            {
                obj.transform.position = td.position;
                obj.transform.rotation = td.rotation;
            }
        }

        Debug.Log("[HW23] 로드 완료!");
    }

    public void ResetData()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("[HW23] 데이터 초기화 완료");
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveData();
    }
}
