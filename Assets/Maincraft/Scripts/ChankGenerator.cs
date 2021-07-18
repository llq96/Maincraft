using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChankGenerator : MonoBehaviour {
	public static ChankGenerator Instance;

	private Chank chank;

	public const int CHANK_SIZE = 8;
	public const int CHANK_SIZE_Sqr = 64;
	Mesh mesh;

	public int[] _triangles;
	public int tempInt;

	public int countBlocksInChank;
	public int curBlockNum;
	public int curPlaneNum;

	float tempFloat;

	List<Vector3> _verticesList = new List<Vector3> ();
	List<int> _trianglesList = new List<int>();
	List<Vector2> _uvs = new List<Vector2>();

	private void Awake() {
		Instance = this;
	}

	void ClearMesh(){
		mesh = new Mesh ();

		_verticesList.Clear ();
		_trianglesList.Clear ();
		_uvs.Clear ();
	}

	public void GenerateChank(Chank _chank){
		chank = _chank;
		ClearMesh ();

		curBlockNum = 0;
		curPlaneNum = 0;

		for (int i = 0; i < chank.blocks.Length; i++) {
			if (chank.blocks[i] == null) {
				continue;
			}
				
			AddBlockInMesh(i);
		}

		mesh.SetVertices (_verticesList);
		mesh.SetTriangles (_trianglesList , 0);
		mesh.SetUVs (0 ,_uvs);

//		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
//		mesh.RecalculateTangents ();
		chank.SetMesh (mesh);
	}

	public static int GetIndex_index_Plus_Vector3(int index , Vector3 vec){
//		return 0;
		return ConvertFrom_Vector3_To_Int(ConvertFrom_Int_To_Vector3 (index) + vec);
	}

	void AddBlockInMesh(int index){
		//UP
		Vector3Int start = ConvertFrom_Int_To_Vector3(index);


		if (start.y != CHANK_SIZE - 1) {
			if (chank.blocks[GetIndex_index_Plus_Vector3(index, Vector3.up)] == null) {
				AddPoligon(start + Vector3.up, Vector3.forward, Vector3.right, chank.blocks[index].uv_up);
			}
		} else {
			if (chank.nearChanks.up) {
				if (chank.nearChanks.up.blocks[ConvertFrom_Vector3_To_Int(start.x, 0, start.z)] == null) {
					AddPoligon(start + Vector3.up, Vector3.forward, Vector3.right, chank.blocks[index].uv_up);
				}
			}
		}

		//BACK
		if (start.z != 0) {
			if (chank.blocks[GetIndex_index_Plus_Vector3(index, Vector3.back)] == null) {
				AddPoligon(start, Vector3.up, Vector3.right, chank.blocks[index].uv_back);
			}
		} else {
			if (chank.nearChanks.back) {
				if (chank.nearChanks.back.blocks[ConvertFrom_Vector3_To_Int(start.x, start.y, CHANK_SIZE - 1)] == null) {
					AddPoligon(start, Vector3.up, Vector3.right, chank.blocks[index].uv_back);
				}
			}
		}

		//RIGHT
		if (start.x != CHANK_SIZE - 1) {
			if (chank.blocks[GetIndex_index_Plus_Vector3(index, Vector3.right)] == null) {
				AddPoligon(start + Vector3.right, Vector3.up, Vector3.forward, chank.blocks[index].uv_right);
			}
		} else {
			if (chank.nearChanks.right) {
				if (chank.nearChanks.right.blocks[ConvertFrom_Vector3_To_Int(0, start.y, start.z)] == null) {
					AddPoligon(start + Vector3.right, Vector3.up, Vector3.forward, chank.blocks[index].uv_right);
				}
			}
		}

		//FORWARD
		if (start.z != CHANK_SIZE - 1) {
			if (chank.blocks[GetIndex_index_Plus_Vector3(index, Vector3.forward)] == null) {
				AddPoligon(start + Vector3.forward + Vector3.right, Vector3.up, Vector3.left, chank.blocks[index].uv_forward);
			}
		} else {
			if (chank.nearChanks.forward) {
				if (chank.nearChanks.forward.blocks[ConvertFrom_Vector3_To_Int(start.x, start.y, 0)] == null) {
					AddPoligon(start + Vector3.forward + Vector3.right, Vector3.up, Vector3.left, chank.blocks[index].uv_forward);
				}
			}
		}

		//LEFT
		if (start.x != 0) {
			if (chank.blocks [GetIndex_index_Plus_Vector3 (index, Vector3.left)] == null) {
				AddPoligon (start + Vector3.forward, Vector3.up, Vector3.back, chank.blocks[index].uv_left);
			}
		} else {
			if (chank.nearChanks.left) {
				if (chank.nearChanks.left.blocks[ConvertFrom_Vector3_To_Int(CHANK_SIZE - 1, start.y, start.z)] == null) {
					AddPoligon(start + Vector3.forward, Vector3.up, Vector3.back, chank.blocks[index].uv_left);
				}
			}
		}

		//DOWN
		if (start.y != 0) {
			if (chank.blocks[GetIndex_index_Plus_Vector3(index, Vector3.down)] == null) {
				AddPoligon(start + Vector3.right, Vector3.forward, Vector3.left, chank.blocks[index].uv_down);
			}
		} else {
			if (chank.nearChanks.down) {
				if (chank.nearChanks.down.blocks[ConvertFrom_Vector3_To_Int(start.x, CHANK_SIZE - 1, start.z)] == null) {
					AddPoligon(start + Vector3.right, Vector3.forward, Vector3.left, chank.blocks[index].uv_down);
				}
			}
		}

		curBlockNum++;
	}


	void AddPoligon(Vector3 start , Vector3 vec1 , Vector3 vec2 , BlockSO_UVs uvs){
		_verticesList.Add (start);
		_verticesList.Add (start + vec1);
		_verticesList.Add (start + vec2);
		_verticesList.Add (start + vec1 + vec2);

		_uvs.Add (uvs.v0);
		_uvs.Add (uvs.v1);
		_uvs.Add (uvs.v2);
		_uvs.Add (uvs.v3);

		tempInt = curPlaneNum * 4;

		_trianglesList.Add (tempInt + 0);
		_trianglesList.Add (tempInt + 1);
		_trianglesList.Add (tempInt + 2);
		_trianglesList.Add (tempInt + 2);
		_trianglesList.Add (tempInt + 1);
		_trianglesList.Add (tempInt + 3);

		curPlaneNum++;
	}



	public static int ConvertFrom_Vector3_To_Int(Vector3 vec){
		return (int)(vec.x + vec.z * CHANK_SIZE + vec.y * CHANK_SIZE_Sqr);
	}

	public static int ConvertFrom_Vector3_To_Int(float x ,float y , float z) {
		return (int)(x + z * CHANK_SIZE + y * CHANK_SIZE_Sqr);
	}


	public static Vector3Int ConvertFrom_Int_To_Vector3(int index){
		return new Vector3Int(index % CHANK_SIZE , index / CHANK_SIZE_Sqr , (index % CHANK_SIZE_Sqr)/CHANK_SIZE);
	}

}
