using UnityEngine;
using UnityEditor;

using System;

namespace Forma
{
    public class DataManager : Singleton<DataManager>
    {
        public string DataPath;

        private void Reset()
        {
            DataPath = Application.persistentDataPath;
        }

        public void Save(object[] Objects, string Name)
        {
            if (Objects == null) Objects = new object[0];
            DataSystem.SaveAll(Objects, Name);
        }
        public object[] Load(string Name)
        {
            object[] values = DataSystem.LoadWithAttributes(Name);
            return values;
        }
  
    }

    [CustomEditor(typeof(DataManager))]
    public class DataManagerEditor:Editor
    {
        DataManager current;
        private void OnEnable()
        {
            current = (DataManager)target;
        }

        public override void OnInspectorGUI()
        {
            current.DataPath = EditorGUILayout.TextField("Data Path : ", current.DataPath);
            if(GUILayout.Button("Select Folder"))
            {
                current.DataPath = EditorUtility.OpenFolderPanel("Data Path", "", "");
            }
        }
    }
}
