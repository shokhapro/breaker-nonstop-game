using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;

[CustomEditor(typeof(GlobalEvent))]
[CanEditMultipleObjects]
public class GlobalEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GlobalEvent myTarget = (GlobalEvent)target;

        if (HasPrefab(myTarget.gameObject))
        {
            if (GUILayout.Button("Copy Values From Prefab"))
            {
                var prefabObject = PrefabUtility.GetPrefabParent(myTarget.gameObject);
                Assert.IsNotNull(prefabObject);
                var prefabGameObject = prefabObject as GameObject;
                Assert.IsNotNull(prefabGameObject);

                var prefab = prefabGameObject.GetComponent<GlobalEvent>();

                myTarget.SetEvents(prefab.GetEvents());
            }
        }
    }

    private bool HasPrefab(GameObject go)
    {
        return PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance;
    }
}