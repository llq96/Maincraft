using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header ("Components")]
    public MeshRenderer mr;
    public MeshCollider mc;
    public MeshFilter mf;

    [Header("Info :")]
    public BlockSO blockSO;

    [Header("Info : Chank")]
    public Chank chank;
    public int indexInChank = -1;

    [Header ("Info : Mining")]
    public bool isMining;
    public float miningProgress;// 0f - 1f
    public float miningSpeed = 1f;


    public void StartMininig() {
        miningProgress = 0f;
        isMining = true;
        UpdateMaterial();
    }

    public void StopMining() {
        isMining = false;
        miningProgress = 0f;
        UpdateMaterial();
    }

    void SetMesh(Mesh _mesh) {
        Mesh _new = new Mesh();
        _new.vertices = _mesh.vertices;
        _new.triangles = _mesh.triangles;
        _new.uv = _mesh.uv;

        _new.RecalculateNormals();

        mf.sharedMesh = _new;
        mc.sharedMesh = _new;

        mc.enabled = true;
    }

    public void Init(Chank _chank, int _indexInChank, Mesh _mesh , BlockSO _blockSO) {
        chank = _chank;
        indexInChank = _indexInChank;
        blockSO = _blockSO;

        SetMesh(_mesh);
    }

    void Update() {
        if (isMining) {
            UpdateMining();
        }
    }

    void UpdateMining() {
        miningProgress += Time.deltaTime * (1f / (blockSO.strength * 0.1f));
        if(miningProgress >= 1f) {
            BlockMined();
            return;
        }

        UpdateMaterial();
    }

    void UpdateMaterial() {
        mr.material.color = Color.Lerp(Color.white, Color.red, miningProgress);
    }

    void BlockMined() {
        Destroy(gameObject);

        if (chank) {
            chank.CheckRegenerateNearChanks(ChankGenerator.ConvertFrom_Int_To_Vector3(indexInChank));
        }
    }

}
