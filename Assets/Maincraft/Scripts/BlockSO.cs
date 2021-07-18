using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(menuName = "Maincraft/BlockSO")]
public class BlockSO : ScriptableObject{
    [Header("Basic Info")]
    public int index;
    public string blockName;

    [Header("Properties")]
    public float strength = 10; // 10 - базовая прочность(разобъёт за 1 секунду)

    [Header ("UVs")]
    //Направления из центра кубика
    public BlockSO_UVs uv_up;
    public BlockSO_UVs uv_forward;
    public BlockSO_UVs uv_right;
    public BlockSO_UVs uv_back;
    public BlockSO_UVs uv_left;
    public BlockSO_UVs uv_down;

    public void SetAllUVs(BlockSO_UVs[] allUVs) {
        if(allUVs.Length != 6) {
            Debug.LogError("Wrong Size Array");
            return;
        }

        uv_up = new BlockSO_UVs(allUVs[0]);
        uv_back = new BlockSO_UVs(allUVs[1]);
        uv_right = new BlockSO_UVs(allUVs[2]);
        uv_forward = new BlockSO_UVs(allUVs[3]);
        uv_left = new BlockSO_UVs(allUVs[4]);
        uv_down = new BlockSO_UVs(allUVs[5]);   
    }
}

[System.Serializable]
public class BlockSO_UVs {
    public Vector2 v0, v1, v2, v3;

    public BlockSO_UVs() {

    }
    public BlockSO_UVs(BlockSO_UVs from) {
        v0 = from.v0;
        v1 = from.v1;
        v2 = from.v2;
        v3 = from.v3;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(BlockSO)), CanEditMultipleObjects]
public class BlockSO_Editor : Editor {

    private BlockSO block;

    private void OnEnable() {
        block = (BlockSO)target;
        
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }
}
#endif