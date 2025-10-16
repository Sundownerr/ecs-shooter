using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Game.Editor
{
    public class LevelObjectsPlacerWindow : EditorWindow
    {
        private readonly List<GameObject> objects = new();
        // Properties
        private float height;
        private int raycastMask = -1; // Default to everything
        private Vector2 scrollPosition;
        private bool showGizmos = true;

        // Initialize and register for scene events
        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;

            // Load saved preferences
            height = EditorPrefs.GetFloat("LevelObjectsPlacerWindow_Height", 0f);
            raycastMask = EditorPrefs.GetInt("LevelObjectsPlacerWindow_RaycastMask", -1);
            showGizmos = EditorPrefs.GetBool("LevelObjectsPlacerWindow_ShowGizmos", true);
        }

        // Cleanup
        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;

            // Save preferences
            EditorPrefs.SetFloat("LevelObjectsPlacerWindow_Height", height);
            EditorPrefs.SetInt("LevelObjectsPlacerWindow_RaycastMask", raycastMask);
            EditorPrefs.SetBool("LevelObjectsPlacerWindow_ShowGizmos", showGizmos);
        }

        // UI rendering
        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Level Objects Placer", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);

            // Height and mask fields
            EditorGUI.BeginChangeCheck();
            height = EditorGUILayout.FloatField("Height", height);
            raycastMask = EditorGUILayout.MaskField("Raycast Mask", raycastMask, InternalEditorUtility.layers);
            showGizmos = EditorGUILayout.Toggle("Show Gizmos", showGizmos);
            if (EditorGUI.EndChangeCheck())
                SceneView.RepaintAll();

            EditorGUILayout.Space(10);

            // Add selected objects button
            if (GUILayout.Button("Add Selected Objects", GUILayout.Height(30)))
                AddSelectedObjects();

            EditorGUILayout.Space(5);

            // Place button
            if (GUILayout.Button("Place Objects", GUILayout.Height(40)))
                Place();

            EditorGUILayout.Space(10);

            // Objects list
            EditorGUILayout.LabelField("Objects List", EditorStyles.boldLabel);

            if (objects.Count == 0) {
                EditorGUILayout.HelpBox(
                    "No objects added. Select objects in the scene and click 'Add Selected Objects'.",
                    MessageType.Info);
            }
            else {
                if (GUILayout.Button("Clear All Objects")) {
                    objects.Clear();
                    SceneView.RepaintAll();
                }

                EditorGUILayout.Space(5);

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                for(var i = 0; i < objects.Count; i++) {
                    EditorGUILayout.BeginHorizontal();

                    // Display object field
                    objects[i] = (GameObject) EditorGUILayout.ObjectField(objects[i], typeof(GameObject), true);

                    // Remove button
                    if (GUILayout.Button("Remove", GUILayout.Width(70))) {
                        objects.RemoveAt(i);
                        SceneView.RepaintAll();
                        i--; // Adjust index after removal
                        continue;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }
        }

        // Window creation
        [MenuItem("Tools/Level Objects Placer")]
        public static void ShowWindow() =>
            GetWindow<LevelObjectsPlacerWindow>("Level Objects Placer");

        // Scene view integration for gizmos
        private void OnSceneGUI(SceneView sceneView)
        {
            if (!showGizmos || objects.Count == 0)
                return;

            // Filter out null objects
            objects.RemoveAll(obj => obj == null);

            if (objects.Count == 0)
                return;

            // Draw lines and cubes for each object
            foreach (var obj in objects) {
                if (obj == null)
                    continue;

                var position = obj.transform.position;
                position.y = height;

                Handles.color = Color.green;
                Handles.DrawLine(obj.transform.position, position);

                // Draw cube at the height position
                Handles.DrawWireCube(position, Vector3.one * 0.4f);
            }

            // Draw plane at specified height
            var bounds = new Bounds(objects[0].transform.position, Vector3.zero);

            foreach (var obj in objects) {
                if (obj == null)
                    continue;

                bounds.Encapsulate(obj.transform.position);
            }

            Handles.color = Color.green;
            var center = new Vector3(bounds.center.x, height, bounds.center.z);
            var size = new Vector3(bounds.size.x, 0, bounds.size.z);

            // Draw wire cube for the plane
            Handles.DrawWireCube(center, size);
        }

        // Place functionality
        private void Place()
        {
            // Filter out null objects
            objects.RemoveAll(obj => obj == null);

            if (objects.Count == 0)
                return;

            Undo.RecordObjects(objects.ToArray(), "Place Level Objects");

            foreach (var obj in objects) {
                if (obj == null)
                    continue;

                var position = obj.transform.position;
                position.y = height;
                obj.transform.position = position;

                // Temporarily disable the object
                var wasActive = obj.activeSelf;
                obj.SetActive(false);

                var ray = new Ray(position, Vector3.down);

                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, raycastMask)) {
                    obj.transform.position = hit.point;
                    obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }

                // Restore original active state
                obj.SetActive(wasActive);
            }
        }

        // Add selected objects
        private void AddSelectedObjects()
        {
            GameObject[] selection = Selection.gameObjects;

            if (selection.Length == 0)
                return;

            var added = false;

            foreach (var obj in selection) {
                if (!objects.Contains(obj)) {
                    objects.Add(obj);
                    added = true;
                }
            }

            if (added)
                SceneView.RepaintAll();
            else
                EditorUtility.DisplayDialog("Level Objects Placer", "All selected objects are already in the list.",
                    "OK");
        }
    }
}