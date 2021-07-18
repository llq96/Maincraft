using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChankVisibleChecker : MonoBehaviour{
	[Header("Chank")]
	public Chank chank;

	[Header("Settings")]
	public float sqrDistanceToPlayer = 100f;

	[Header ("Info")]
	public bool isVisible = true;


	public void OnBecameVisible() {
		if (isVisible) {
			return;
		}

		chank.SetVision(true);

	}

	private void OnBecameInvisible() {
		if (!isVisible) {
			return;
		}

		if (Player.Instance) {
			if ((Player.Instance.transform.position - transform.position).sqrMagnitude <= sqrDistanceToPlayer) {
				return;
			}
		}

		//chank.SetVision(false);
	}

}
