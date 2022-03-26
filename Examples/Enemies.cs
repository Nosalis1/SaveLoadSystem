using UnityEngine;

using Forma;

public class Enemies : MonoBehaviour
{
    [SaveVar("Enemy")] public int Count;
    [SaveVar("Enemy")] public Transform[] Holder;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 120, 120, 120));
        GUILayout.BeginVertical("box");

        if (GUILayout.Button("SaveEnemy")) DataManager.Instance.Save(null, "Enemy");
        if (GUILayout.Button("LoadEnemy")) DataManager.Instance.Load("Enemy");

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
