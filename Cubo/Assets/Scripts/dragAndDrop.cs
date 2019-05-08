using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragAndDrop : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private bool isMouseDragging;
    Vector3 offsetValue;
    Vector3 positionOfScreen;
    private GameObject getTarget;
    
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            getTarget = ReturnClickedObject(out hitInfo);
            if (getTarget != null)
            {
                isMouseDragging = true;
                //Converting world position to screen position.
                positionOfScreen = _camera.WorldToScreenPoint(getTarget.transform.position);
            }
        }

        //Mouse Button Up
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDragging = false;
        }

        //Is mouse Moving
        if (isMouseDragging)
        {
            //tracking mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z);

            //converting screen position to world position .
            Vector3 currentPosition = _camera.ScreenToWorldPoint(currentScreenSpace) ;

            //update target current postion.
            getTarget.transform.position = currentPosition;
        }


    }

   

    //Return Clicked Object
    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            target = hit.collider.gameObject;
            //Debug.Log(target);
        }
        return target;
    }
}
