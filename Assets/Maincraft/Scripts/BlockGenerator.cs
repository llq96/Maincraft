using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public static BlockGenerator Instance;

    [Header("Instantiate")]
    public GameObject prefab;

    public Mesh mesh;

    public int[] _triangles;
    public int tempInt;

    public int curBlockNum;
    public int curPlaneNum;

    List<Vector3> _verticesList = new List<Vector3>();
    List<int> _trianglesList = new List<int>();
    List<Vector2> _uvs = new List<Vector2>();

    void Awake() {
        Instance = this;

        mesh = new Mesh();

        Vector3 start = new Vector3(0, 0, 0);
        //UP
        AddPoligon(start + Vector3.up, Vector3.forward, Vector3.right);

        //BACK
        AddPoligon(start, Vector3.up, Vector3.right);

        //RIGHT
        AddPoligon(start  + Vector3.right, Vector3.up, Vector3.forward);

        //FORWARD
        AddPoligon(start + Vector3.forward + Vector3.right, Vector3.up, Vector3.left);

        //LEFT
        AddPoligon(start + Vector3.forward, Vector3.up, Vector3.back);

        //DOWN
        AddPoligon(start + Vector3.right, Vector3.forward, Vector3.left);


        mesh.SetVertices(_verticesList);
        mesh.SetTriangles(_trianglesList, 0);
        mesh.SetUVs(0, _uvs);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    void AddPoligon(Vector3 start, Vector3 vec1, Vector3 vec2) {
        _verticesList.Add(start);
        _verticesList.Add(start + vec1);
        _verticesList.Add(start + vec2);
        _verticesList.Add(start + vec1 + vec2);

        tempInt = curPlaneNum * 4;

        _trianglesList.Add(tempInt + 0);
        _trianglesList.Add(tempInt + 1);
        _trianglesList.Add(tempInt + 2);
        _trianglesList.Add(tempInt + 2);
        _trianglesList.Add(tempInt + 1);
        _trianglesList.Add(tempInt + 3);

        curPlaneNum++;
    }


    void AddUVS(BlockSO_UVs uvs) {
        _uvs.Add(uvs.v0);
        _uvs.Add(uvs.v1);
        _uvs.Add(uvs.v2);
        _uvs.Add(uvs.v3);
    }

   public Block InstantiateOneBlock(Chank _chank , int _indexInChank, Vector3 pos , BlockSO _blockSO) {
        _uvs.Clear();
        AddUVS(_blockSO.uv_up);
        AddUVS(_blockSO.uv_back);
        AddUVS(_blockSO.uv_right);
        AddUVS(_blockSO.uv_forward);
        AddUVS(_blockSO.uv_left);
        AddUVS(_blockSO.uv_down);
        mesh.SetUVs(0,_uvs);

        GameObject go = Instantiate(prefab, transform);
        go.transform.position = pos;

        Block newBlock= go.GetComponent<Block>();

        newBlock.Init(_chank , _indexInChank, mesh , _blockSO);
        return newBlock;
    }

}
