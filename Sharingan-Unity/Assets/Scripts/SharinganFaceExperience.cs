using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Adds a minimal, self-contained Sharingan face effect to the AR template.
/// It deliberately uses AR Foundation's camera-facing selector instead of
/// WebCamTexture: AR face tracking requires the AR camera stream.
/// </summary>
[DefaultExecutionOrder(-100)]
public sealed class SharinganFaceExperience : MonoBehaviour
{
    const string OverlayName = "Sharingan eye overlay";

    ARCameraManager m_CameraManager;
    ARFaceManager m_FaceManager;
    ARSession m_Session;
    CameraFacingDirection m_SelectedDirection = CameraFacingDirection.User;
    Texture2D m_EyeTexture;
    GUIStyle m_TitleStyle;
    GUIStyle m_MessageStyle;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Install()
    {
        if (FindFirstObjectByType<SharinganFaceExperience>() != null)
            return;

        var host = new GameObject("Sharingan Face Experience");
        DontDestroyOnLoad(host);
        host.AddComponent<SharinganFaceExperience>();
    }

    void Awake()
    {
        // The Mobile AR template's root UI contains its tutorial, object-spawning,
        // and debug menus. This face-only experience uses its own camera controls.
        var templateUi = GameObject.Find("UI");
        if (templateUi != null)
            templateUi.SetActive(false);

        m_CameraManager = FindFirstObjectByType<ARCameraManager>();
        m_FaceManager = FindFirstObjectByType<ARFaceManager>();
        m_Session = FindFirstObjectByType<ARSession>();
        // Drop additional eye variants in Assets/Resources/SharinganEyes. The supplied
        // PNG replaces the fallback procedural drawing without affecting tracking.
        m_EyeTexture = Resources.Load<Texture2D>("SharinganEyes/Eye-Lens-Sharingan-PNG-Image")
            ?? CreateEyeTexture();
    }

    IEnumerator Start()
    {
        // Give the AR subsystem a frame to start, then ask for the selfie camera.
        yield return null;
        SelectCamera(CameraFacingDirection.User);
    }

    void Update()
    {
        if (m_SelectedDirection != CameraFacingDirection.User || m_FaceManager == null)
            return;

        foreach (var face in m_FaceManager.trackables)
        {
            if (face != null && face.transform.Find(OverlayName) == null)
                AddEyes(face);
        }
    }

    void SelectCamera(CameraFacingDirection direction)
    {
        m_SelectedDirection = direction;

        if (m_CameraManager != null)
            m_CameraManager.requestedFacingDirection = direction;

        // ARKit and ARCore only provide ARFace landmarks with the user-facing camera.
        // Disable the manager for the world camera so the app never suggests it is tracking.
        if (m_FaceManager != null)
            m_FaceManager.enabled = direction == CameraFacingDirection.User;

        if (m_Session != null)
            m_Session.Reset();
    }

    void AddEyes(ARFace face)
    {
        var root = new GameObject(OverlayName).transform;
        root.SetParent(face.transform, false);

        // ARKit-capable devices provide individual eye poses. ARCore devices that do
        // not expose them fall back to the proven face-relative placement.
        var hasIndividualEyePoses = face.leftEye != null && face.rightEye != null
            && Vector3.Distance(face.leftEye.localPosition, face.rightEye.localPosition) > 0.01f;

        if (hasIndividualEyePoses)
        {
            CreateEye(face.leftEye, "Left Sharingan", Vector3.zero);
            CreateEye(face.rightEye, "Right Sharingan", Vector3.zero);
            Debug.Log("Sharingan: using individual AR eye poses.");
        }
        else
        {
            CreateEye(root, "Left Sharingan", new Vector3(-0.032f, 0.016f, -0.038f));
            CreateEye(root, "Right Sharingan", new Vector3(0.032f, 0.016f, -0.038f));
            Debug.Log("Sharingan: eye poses unavailable; using face-relative placement.");
        }
    }

    void CreateEye(Transform parent, string eyeName, Vector3 localPosition)
    {
        var eye = new GameObject(eyeName);
        eye.transform.SetParent(parent, false);
        eye.transform.localPosition = localPosition;
        // About 26% smaller than the original overlay, so the real eye remains visible.
        eye.transform.localScale = Vector3.one * 0.028f;

        var renderer = eye.AddComponent<SpriteRenderer>();
        renderer.sprite = Sprite.Create(m_EyeTexture, new Rect(0, 0, m_EyeTexture.width, m_EyeTexture.height),
            new Vector2(0.5f, 0.5f), m_EyeTexture.width);
        renderer.sortingOrder = 100;
    }

    static Texture2D CreateEyeTexture()
    {
        const int size = 256;
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear };
        var center = (size - 1) * 0.5f;
        var pixels = new Color32[size * size];

        for (var y = 0; y < size; y++)
        for (var x = 0; x < size; x++)
        {
            var dx = (x - center) / center;
            var dy = (y - center) / center;
            var radius = Mathf.Sqrt(dx * dx + dy * dy);
            var color = new Color32(0, 0, 0, 0);

            // White sclera, red iris, a black pupil, and the three Sharingan tomoe.
            if (radius < 0.98f) color = new Color32(245, 242, 238, 255);
            if (radius < 0.70f) color = new Color32(220, 18, 24, 255);
            if (radius > 0.66f && radius < 0.72f) color = new Color32(55, 0, 0, 255);
            if (radius < 0.18f) color = new Color32(10, 0, 0, 255);

            // Three evenly spaced tomoe around the pupil.
            for (var tomoe = 0; tomoe < 3; tomoe++)
            {
                var angle = tomoe * Mathf.PI * 2f / 3f - Mathf.PI * 0.5f;
                var tx = Mathf.Cos(angle) * 0.43f;
                var ty = Mathf.Sin(angle) * 0.43f;
                var tomoeDistance = Mathf.Sqrt((dx - tx) * (dx - tx) + (dy - ty) * (dy - ty));
                if (tomoeDistance < 0.105f)
                    color = new Color32(12, 0, 0, 255);
            }
            pixels[y * size + x] = color;
        }

        texture.SetPixels32(pixels);
        texture.Apply(false, true);
        return texture;
    }

    void OnGUI()
    {
        EnsureStyles();
        var scale = Mathf.Clamp(Screen.width / 1080f, 0.75f, 1.5f);
        var width = 460 * scale;
        var height = 52 * scale;
        var x = (Screen.width - width) * 0.5f;
        var y = 30 * scale;

        GUI.Label(new Rect(x, y, width, height), "SHARINGAN · Cámara", m_TitleStyle);
        y += 56 * scale;

        if (GUI.Button(new Rect(x, y, width * .49f, height), "Frontal · seguimiento facial"))
            SelectCamera(CameraFacingDirection.User);
        if (GUI.Button(new Rect(x + width * .51f, y, width * .49f, height), "Trasera · AR normal"))
            SelectCamera(CameraFacingDirection.World);

        y += 60 * scale;
        var message = m_SelectedDirection == CameraFacingDirection.User
            ? "Busca una cara: el efecto reemplaza los ojos con Sharingan."
            : "La cámara trasera está activa. ARKit/ARCore no ofrecen seguimiento facial en ella.";
        GUI.Label(new Rect(x, y, width, 70 * scale), message, m_MessageStyle);
    }

    void EnsureStyles()
    {
        if (m_TitleStyle != null) return;
        m_TitleStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 26, fontStyle = FontStyle.Bold };
        m_TitleStyle.normal.textColor = Color.white;
        m_MessageStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter, fontSize = 18, wordWrap = true };
        m_MessageStyle.normal.textColor = Color.white;
    }
}
