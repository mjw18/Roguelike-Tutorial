using UnityEngine;
using UnityEditor;

public class CreateBoardConfig {

    [MenuItem("Assets/Create/BoardConfig")]
    static void DoCreateBoardConfig()
    {
        var config = ScriptableObject.CreateInstance<BoardConfig>();
        AssetDatabase.CreateAsset(config, "Assets/EntryBoard.asset");
        AssetDatabase.SaveAssets();
    }
}
