﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    Camera raycastCam;
    public static InputManager Instance{
        get { return _instance;}
    }
    private static InputManager _instance;
    private bool holdRoom = false;
    private float holdRoomTime = 0f;

    public Room currentRoom;

    private float speed = 10f;
    private float x;
    private float z;
   
    void Awake(){
        if(_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0) {
            switch (Input.GetTouch(0).phase) {
                case TouchPhase.Began:
                    Ray ray = raycastCam.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit)) {
                        Room temp = hit.collider.GetComponent<Room>();
                        if (temp) {
                            currentRoom = temp;
                            currentRoom.SetMoveRoom(true);
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    holdRoom = false;
                    holdRoomTime = 0f;
                    currentRoom.MoveRoom(Input.GetTouch(0).deltaPosition);
                    break;
                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    if(currentRoom != null && holdRoom == false) {
                        currentRoom.SetMoveRoom(false);
                        currentRoom = null;
                    }
                    break;

                case TouchPhase.Stationary:
                    holdRoomTime += Time.deltaTime;
                    if (holdRoomTime > 0.65f) {
                        if ( !holdRoom)
                        {
                            UI_RotateButton.Instance.gameObject.SetActive(true);
                            UI_RotateButton.Instance.SetRotateAction( ()=> {
                                if (currentRoom != null)
                                    currentRoom.transform.eulerAngles =
                                        new Vector3(0f, 90f, 0f) + currentRoom.transform.eulerAngles;
                            }, Input.GetTouch(0).position);
                        }
                        holdRoom = true;
                    }
                    break;
            }
        }

        x = Input.GetAxis("Mouse X");
        z = Input.GetAxis("Mouse Y");

        if(Input.GetMouseButtonDown(0)) {
            Ray ray = raycastCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)) {
                Room temp = hit.collider.GetComponent<Room>();
                if(temp) {
                    currentRoom = temp;
                    currentRoom.SetMoveRoom(true);
                }
            }
        }
        if(currentRoom != null) {
            currentRoom.transform.localPosition = new Vector3(currentRoom.transform.localPosition.x + x/2, currentRoom.transform.localPosition.y, currentRoom.transform.localPosition.z + z/2);

        }

        if(Input.GetMouseButtonUp(0)) {
            currentRoom = null;
        }
        //var mousex : float = Input.GetAxis("Mouse X");
        //var mousey : float = Input.GetAxis("Mouse Y");

        //if(mousex > mousexThreshold) {
        //    // Code for mouse moving right
        //} else if(mousex < -mousexThreshold) {
        //    // Code for mouse moving left
        //} else {
        //    // Code for mouse standing still
        //}

        //if(mousey > mouseyThreshold) {
        //    // Code for mouse moving forward
        //} else if(mousey < -mouseyThreshold) {
        //    // Code for mouse moving backward
        //} else {
        //    // Code for mouse standing still
        //}
        //}
    }
}
