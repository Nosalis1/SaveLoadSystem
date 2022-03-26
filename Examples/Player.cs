using UnityEngine;

using Forma;

public class Player : MonoBehaviour
{
    [SaveVar("NetworkSettings")] public NetworkSettings Network = new NetworkSettings();

    public bool CanLook = true;
    public bool CanMove = true;

    void SavePlayerSettings()
    {
        object[] values =
        {
            CanLook,
            CanMove
        };
        DataManager.Instance.Save(values, "PlayerSettings");
        DataManager.Instance.Save(null, "NetworkSettings");
    }
    void LoadPlayerSettings()
    {
        object[] values = DataManager.Instance.Load("PlayerSettings");
        CanLook = (bool)values[0];
        CanMove = (bool)values[1];
        DataManager.Instance.Load("NetworkSettings");
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical("box");

        if (GUILayout.Button("Save")) SavePlayerSettings();
        if (GUILayout.Button("Load")) LoadPlayerSettings();

        GUILayout.EndVertical();
    }
}

[System.Serializable]
public class NetworkSettings
{
    public int ID = 0;
    public string Name = "New Player";
    public string IP = "127.0.0.1";
    public int Port = 2584;
    public bool SpawnOnJoined = true;
}
