using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [Header ("RayCast Settings")]
    public Camera cam;
    public LayerMask layerRaycast;

    [Header ("Info :")]
    public Block curBlock;
    public PlayerRayCastState raycastState;

    private void Awake() {
        Instance = this;
    }

    public void Press() {
        raycastState = PlayerRayCastState.Start;
        Raycast();
        raycastState = PlayerRayCastState.PressNow;
    }

    public void UnPress() {
        raycastState = PlayerRayCastState.End;
        EndRaycast();
        raycastState = PlayerRayCastState.None;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Press();
            return;
        }
       // if (Input.GetMouseButton(0)) {
       //     raycastState = PlayerRayCastState.PressNow;
       // }
        if (Input.GetMouseButtonUp(0)) {
            UnPress();
            return;
        }


        //if((raycastState == PlayerRayCastState.Start)||(raycastState == PlayerRayCastState.PressNow)) {
        //    Raycast();
        //}

        if (raycastState == PlayerRayCastState.PressNow) {
            Raycast();
        }

        //if ((raycastState == PlayerRayCastState.None) || (raycastState == PlayerRayCastState.End)) {
        //    EndRaycast();
        //}
    }

    void Raycast() {
        //Debug.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward, Color.green, 0.1f);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5f, layerRaycast)) {
            OnRaycast(hit);
        }
    }

    void EndRaycast() {
        SetBlock(null);
    }

    public void OnRaycast(RaycastHit hit) {
        Chank chank = hit.collider.gameObject.GetComponent<Chank>();
        if (chank) {
            Block _blockResult = null;
            chank.OnRaycast(hit , chank , out _blockResult);
            if (_blockResult) {
                SetBlock(_blockResult);
            }
            return;
        }

        Block _block = hit.collider.gameObject.GetComponent<Block>();
        if (_block) {
            SetBlock(_block);
            return;
        }
    }



    void SetBlock(Block _block) {
        if(_block != curBlock) {
            if (curBlock) {
                curBlock.StopMining();
            }

            curBlock = _block;
            if (curBlock) {
                curBlock.StartMininig();
            }
        }
    }

    


    public enum PlayerRayCastState {
        None,
        Start,
        PressNow,
        End
    }
}


