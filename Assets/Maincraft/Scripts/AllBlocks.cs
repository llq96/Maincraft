using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllBlocks : MonoBehaviour
{
    public static AllBlocks Instance;

    [Header ("ALL BLOCKS SO")]
    public BlockSO[] blocks;

    void Awake() {
        Instance = this;
    }



}
