using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BlocksSO_ListHelper : MonoBehaviour
{

    [Header("Blocks")]
    public BlockSO[] blocks;

    [Header("Texture Settings")]
    //public Texture2D tex;

    public int fullSizeX = 2048;
    public int fullSizeY = 2048;

    public int sizeBlockX = 96;
    public int sizeBlockY = 16;

    public Vector2 sizeSide = new Vector2(16, 16);

    public void MakeGood() {
        int blocksInRow = fullSizeX / sizeBlockX;
        int blocksInCollumn = fullSizeY / sizeBlockY;

        int curX = 0; //from 0 to blocksInRow - 1
        int curY = 0; //from 0 to blocksInCollumn - 1



        //Debug.Log(blocksInRow * blocksInCollumn);

        for (int i = 0; i < blocks.Length; i++) {
            blocks[i].index = i;
        }


        for (int i = 0; i < blocks.Length; i++) {
            int startX = curX * sizeBlockX;
            int startY = curY * sizeBlockY;

            BlockSO_UVs[] array = new BlockSO_UVs[6];
            for (int j = 0; j < array.Length; j++) {
                array[j] = new BlockSO_UVs();

                //Левый нижний угол
                array[j].v0.x = startX + j * sizeSide.x;
                array[j].v0.y = startY;
                //Левый верхний угол

                array[j].v1.x = startX + j * sizeSide.x;
                array[j].v1.y = startY + sizeSide.y;

                //Правый нижний угол
                array[j].v2.x = startX + j * sizeSide.x + sizeSide.x;
                array[j].v2.y = startY;

                //Правый верхний угол
                array[j].v3.x = startX + j * sizeSide.x + sizeSide.x;
                array[j].v3.y = startY + sizeSide.y;


                array[j].v0.x /= (float)fullSizeX;
                array[j].v0.y /= (float)fullSizeY;
                array[j].v1.x /= (float)fullSizeX;
                array[j].v1.y /= (float)fullSizeY;
                array[j].v2.x /= (float)fullSizeX;
                array[j].v2.y /= (float)fullSizeY;
                array[j].v3.x /= (float)fullSizeX;
                array[j].v3.y /= (float)fullSizeY;
            }

            blocks[i].SetAllUVs(array);

            curX++;
            if(curX >= blocksInRow) {
                curX = 0;
                curY++;
                if(curY >= blocksInCollumn) {
                    Debug.LogError("Texture Have No Empty Space!");
                    break;
                }
            }
        }

    }


}






#if UNITY_EDITOR
[CustomEditor(typeof(BlocksSO_ListHelper)), CanEditMultipleObjects]
public class BlocksSO_ListHelper_Editor : Editor {

    private BlocksSO_ListHelper list;

    private void OnEnable() {
        list = (BlocksSO_ListHelper)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Make Good" , GUILayout.ExpandWidth(false) )){
            list.MakeGood();
        }
    }
}
#endif