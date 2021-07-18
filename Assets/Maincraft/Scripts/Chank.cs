using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chank : MonoBehaviour {

	public BlockSO[] blocks;


	[Header("Components")]
	public MeshRenderer mr;
	public MeshCollider mc;
	public MeshFilter mf;

	[Header("Vision Checker")]
	public ChankVisibleChecker visionChecker;

	[Header("Near Chanks")]
	public NearChanks nearChanks;

	//void Awake(){
	//	blocks = new BlockSO[ChankGenerator.CHANK_SIZE * ChankGenerator.CHANK_SIZE * ChankGenerator.CHANK_SIZE];
	//	int rnd;
	//	for (int i = 0; i < blocks.Length; i++) {
	//		rnd = Random.Range(-1, 2);
	//		blocks [i] = rnd == -1 ? null : AllBlocks.Instance.blocks[rnd];
	//	}
	//}

	private void Start() {
		for (int i = 0; i < blocks.Length; i++) {
			if (blocks[i] != null) {
				RegenerateChank();
				break;
			}
		}

	}

	public bool IsEmpty() {
		for (int i = 0; i < blocks.Length; i++) {
			if (blocks[i] != null) {
				return false;
			}
		}
		return true;
	}

	public void RegenerateChank() {
		ChankGenerator.Instance.GenerateChank(this);
	}

	public void OnRaycast(RaycastHit hit, Chank chank , out Block block) {
		Vector3 point = hit.point - transform.position;
		Vector3 normal = hit.normal;

		point.x = (normal.x == 0) ? Mathf.FloorToInt(point.x) : Mathf.FloorToInt(point.x - normal.x * 0.5f);
		point.y = (normal.y == 0) ? Mathf.FloorToInt(point.y) : Mathf.FloorToInt(point.y - normal.y * 0.5f);
		point.z = (normal.z == 0) ? Mathf.FloorToInt(point.z) : Mathf.FloorToInt(point.z - normal.z * 0.5f);

		int indexInChank = ChankGenerator.ConvertFrom_Vector3_To_Int(point);
		if (blocks[indexInChank]) {
			block = BlockGenerator.Instance.InstantiateOneBlock(this, indexInChank, transform.position + point, blocks[indexInChank]);
			chank.blocks[indexInChank] = null;
			RegenerateChank();
		} else {
			block = null;
		}
	}

	public void CheckRegenerateNearChanks(Vector3Int coordinates) {
		CheckRegenerateNearChank(coordinates.x, 0, nearChanks.left);
		CheckRegenerateNearChank(coordinates.x, ChankGenerator.CHANK_SIZE - 1, nearChanks.right);

		CheckRegenerateNearChank(coordinates.y, 0, nearChanks.down);
		CheckRegenerateNearChank(coordinates.y, ChankGenerator.CHANK_SIZE - 1, nearChanks.up);

		CheckRegenerateNearChank(coordinates.z, 0, nearChanks.back);
		CheckRegenerateNearChank(coordinates.z, ChankGenerator.CHANK_SIZE - 1, nearChanks.forward);
	}

	void CheckRegenerateNearChank(int value1, int value2, Chank chank) {
		if (chank) {
			if(value1 == value2) {
				chank.RegenerateChank();
			}
		}
	}


	public void SetMesh(Mesh _mesh){
		mf.sharedMesh = _mesh;
		mc.sharedMesh = _mesh;

		mc.enabled = true;
	}

	public void SetVision(bool _isVisible) {
		mr.enabled = _isVisible;
		mc.enabled = _isVisible;

		visionChecker.isVisible = _isVisible;
	}

}


[System.Serializable]
public class NearChanks {
	public Chank up;
	public Chank down;
	public Chank left;
	public Chank right;
	public Chank forward;
	public Chank back;
}