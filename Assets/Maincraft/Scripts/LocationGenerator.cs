using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationGenerator : MonoBehaviour{
    public static LocationGenerator Instance;

    public float[,] map; //Карта высот
    public Texture2D tex;

    public Vector3Int locSize = new Vector3Int(256, 256, 256);


    [Header("Chank")]
    public GameObject chankPrefab;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GenerateMap();
    }

    public void GenerateMap(){
        //tex = new Texture2D(512, 512);

        map = new float[locSize.x, locSize.y];

        for (int x = 0; x < locSize.x; x++) {
            for (int y = 0; y < locSize.y; y++) {
                map[x, y] = 64;
            }
        }


        float timeStart = Time.realtimeSinceStartup;

        Vector2 center;
        for (int i = 0; i < 40; i++) {
            center.x = Random.Range(0, locSize.x);
            center.y = Random.Range(0, locSize.y);
            Deform(center, Random.Range (10 , 50), Random.Range(-10 , 10));
        }

        float timeEnd = Time.realtimeSinceStartup;

        Debug.Log($"Generate Map : {timeEnd - timeStart}");

        //DrawOnTexture();

        timeStart = Time.realtimeSinceStartup;
        CreateChanks();
        timeEnd = Time.realtimeSinceStartup;

        Debug.Log($"Set Blocks For Chanks : {timeEnd - timeStart}");
    }

    public void CreateChanks() {
        int chankSize = ChankGenerator.CHANK_SIZE;

        Vector3Int countChanks = new Vector3Int(locSize.x / chankSize, locSize.y / chankSize, locSize.z / chankSize);

        Chank[,,] chanks = new Chank[countChanks.x, countChanks.y, countChanks.z];

        for (int x = 0; x < countChanks.x; x++) {
            for (int y = 0; y < countChanks.y; y++) {
                for (int z = 0; z < countChanks.z; z++) {
                    GameObject go = Instantiate(chankPrefab, transform);
                    go.transform.position = new Vector3(x * chankSize, y * chankSize, z * chankSize);

                    Chank chank = go.GetComponent<Chank>();
                    chank.blocks = new BlockSO[chankSize * chankSize * chankSize];
                    Vector3 pos;
                    for (int i = 0; i < chank.blocks.Length; i++) {
                        pos = go.transform.position + ChankGenerator.ConvertFrom_Int_To_Vector3(i);
                        if (map[(int)pos.x, (int)pos.z] <= pos.y) {
                            chank.blocks[i] = null;
                        } else {
                            chank.blocks[i] = AllBlocks.Instance.blocks[1];
                        }
                    }

                    chanks[x, y, z] = chank;
                }
            }
        }


        int[,] surfaces = new int[countChanks.x, countChanks.z];
        for (int x = 0; x < countChanks.x; x++) {
            for (int z = 0; z < countChanks.z; z++) {
                surfaces[x, z] = -1;
            }
        }


        for (int x = 0; x < countChanks.x; x++) {
            for (int y = 0; y < countChanks.y; y++) {
                for (int z = 0; z < countChanks.z; z++) {
                    chanks[x, y, z].nearChanks.up = (y == countChanks.y - 1) ? null : chanks[x, y + 1, z];
                    chanks[x, y, z].nearChanks.down = (y == 0) ? null : chanks[x, y - 1, z];

                    chanks[x, y, z].nearChanks.right = (x == countChanks.x - 1) ? null : chanks[x + 1, y, z];
                    chanks[x, y, z].nearChanks.left = (x == 0) ? null : chanks[x - 1, y, z];

                    chanks[x, y, z].nearChanks.forward = (z == countChanks.z - 1) ? null : chanks[x, y, z + 1];
                    chanks[x, y, z].nearChanks.back = (z == 0) ? null : chanks[x , y, z - 1];

                    if (y > surfaces[x, z]) {
                        if (!chanks[x, y, z].IsEmpty()) {
                            surfaces[x, z] = y;
                        }
                    }

                }
            }
        }

        for (int x = 0; x < countChanks.x; x++) {
            for (int z = 0; z < countChanks.z; z++) {
                MarkAsSurface(chanks[x, surfaces[x, z], z]);
                if(surfaces[x, z] >= 1) {
                    MarkAsSurface(chanks[x, surfaces[x, z] - 1, z]);
                }
            }
        }

    }

    void MarkAsSurface(Chank _chank) {
        Vector3Int vec;
        //_chank.transform.position += Vector3.up * 100;
        for (int i = 0; i < _chank.blocks.Length; i++) {
            if (_chank.blocks[i]) {
                vec = ChankGenerator.ConvertFrom_Int_To_Vector3(i);
                if (vec.y != ChankGenerator.CHANK_SIZE - 1) {
                    if (_chank.blocks[ChankGenerator.ConvertFrom_Vector3_To_Int(vec + Vector3.up)] == null) {
                        _chank.blocks[i] = AllBlocks.Instance.blocks[0];
                    }
                } else {
                    _chank.blocks[i] = AllBlocks.Instance.blocks[0];
                }
            }
        }
    }


    //void DrawOnTexture() {
    //    tex = new Texture2D(locSize.x, locSize.y);
    //    for (int x = 0; x < locSize.x; x++) {
    //        for (int y = 0; y < locSize.y; y++) {
    //            float grey = map[x, y] / 256f;
    //            tex.SetPixel(x, y, new Color(grey, grey, grey));
    //        }
    //    }

    //    tex.Apply();
    //}


    void Deform(Vector2 center , float radius , float maxForce) {
        Vector2 from;
        from.x = Mathf.Clamp(center.x - radius, 0, locSize.x);
        from.y = Mathf.Clamp(center.y - radius, 0, locSize.y);

        Vector2 to;
        to.x = Mathf.Clamp(center.x + radius, 0, locSize.x);
        to.y = Mathf.Clamp(center.y + radius, 0, locSize.y);

        float sqrRadius = radius * radius;
        float curSqr;

        for (int x = (int)from.x; x < to.x; x++) {
            for (int y = (int)from.y; y < to.y; y++) {

                curSqr = (x - center.x) * (x - center.x) + (y - center.y) * (y - center.y);

                if (curSqr <= sqrRadius) {
                    map[x, y] += maxForce * (1f - curSqr / sqrRadius);
                }

            }
        }

    }


}
