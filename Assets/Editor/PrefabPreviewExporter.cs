using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CharacterPreviewExporter : EditorWindow
{
    static int resolution = 512; // tăng lên nếu muốn nét hơn nữa, vd 1024
    static float padding = 1.15f; // chừa lề quanh nhân vật

    // Các tên GameObject cần LOẠI TRỪ khỏi ảnh preview (UI, VFX...)
    static string[] excludeNames = { "HP_Bar", "MagicChargeBlue", "StunnedCirclingStars", "ArmoredTag", "HiddenTag" };

    [MenuItem("Tools/Export Character Preview (High Quality)")]
    static void ExportPreview()
    {
        string outputFolder = "Assets/PrefabPreviews";
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        foreach (string guid in Selection.assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefabAsset == null) continue;

            ExportSinglePrefab(prefabAsset, outputFolder);
        }

        AssetDatabase.Refresh();
        Debug.Log("Xuất preview chất lượng cao hoàn tất!");
    }

    static void ExportSinglePrefab(GameObject prefabAsset, string outputFolder)
    {
        // 1. Instantiate tạm prefab vào scene (ẩn, không lưu)
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);

        // 2. Ẩn các object không mong muốn (HP bar, VFX...)
        HideExcludedObjects(instance.transform);

        // 3. Lấy toàn bộ SpriteRenderer còn "active" để tính bounds
        SpriteRenderer[] renderers = instance.GetComponentsInChildren<SpriteRenderer>(false);
        List<SpriteRenderer> validRenderers = new List<SpriteRenderer>();
        Bounds bounds = new Bounds();
        bool boundsInit = false;

        foreach (var r in renderers)
        {
            if (!r.gameObject.activeInHierarchy) continue;
            validRenderers.Add(r);
            if (!boundsInit) { bounds = r.bounds; boundsInit = true; }
            else bounds.Encapsulate(r.bounds);
        }

        if (!boundsInit)
        {
            Debug.LogWarning($"Không tìm thấy sprite hợp lệ: {prefabAsset.name}");
            Object.DestroyImmediate(instance);
            return;
        }

        // 4. Set toàn bộ texture liên quan sang Point Filter (tạm thời, không lưu vĩnh viễn)
        // (giả định texture gốc đã Point filter sẵn cho pixel art; nếu chưa, cần set trong Import Settings)

        // 5. Tạo camera tạm để render
        GameObject camObj = new GameObject("PreviewCamera_TEMP");
        Camera cam = camObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0); // nền trong suốt
        cam.allowMSAA = false; // QUAN TRỌNG: tắt AA để không mờ pixel art
        cam.cullingMask = ~0;
        cam.nearClipPlane = -100f;
        cam.farClipPlane = 100f;

        float size = Mathf.Max(bounds.extents.x, bounds.extents.y) * padding;
        cam.orthographicSize = size;
        cam.transform.position = new Vector3(bounds.center.x, bounds.center.y, -10f);

        // 6. Tạo RenderTexture độ phân giải cao, không mipmap, point filter
        RenderTexture rt = new RenderTexture(resolution, resolution, 24, RenderTextureFormat.ARGB32);
        rt.filterMode = FilterMode.Point;
        rt.antiAliasing = 1;
        cam.targetTexture = rt;

        // 7. Render
        cam.Render();

        RenderTexture.active = rt;
        Texture2D output = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
        output.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        output.Apply();

        // 8. Lưu file PNG
        byte[] png = output.EncodeToPNG();
        string fileName = prefabAsset.name + ".png";
        File.WriteAllBytes(Path.Combine(outputFolder, fileName), png);

        // 9. Dọn dẹp
        RenderTexture.active = null;
        cam.targetTexture = null;
        Object.DestroyImmediate(rt);
        Object.DestroyImmediate(output);
        Object.DestroyImmediate(camObj);
        Object.DestroyImmediate(instance);
    }

    static void HideExcludedObjects(Transform root)
    {
        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            foreach (string ex in excludeNames)
            {
                if (child.name.Contains(ex))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}