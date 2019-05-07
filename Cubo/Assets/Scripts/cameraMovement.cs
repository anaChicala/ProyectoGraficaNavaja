using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    private Vector3 localRotation;
    [SerializeField] private Camera _camera;
    private bool cameraDisabled = false, rotateDisable = false;
    [SerializeField] cubeManager CubeMan;
    [SerializeField] private float mouseSensibility = 15f, mouseSensibilityX = 15f, mouseSensibilityY = 15f;
    List<GameObject> pieces = new List<GameObject>(),
            planes = new List<GameObject>();
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!rotateDisable)
            {
                RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {

                    cameraDisabled = true;
                    if (pieces.Count < 2 &&
                        !pieces.Exists(x => x == hit.collider.transform.parent.gameObject) &&
                        hit.transform.parent.gameObject != CubeMan.gameObject)
                    {
                        pieces.Add(hit.collider.transform.parent.gameObject);
                        planes.Add(hit.collider.gameObject);
                    }else if (pieces.Count == 2)
                    {
                        CubeMan.DetectRotate(pieces, planes);
                        rotateDisable = true;
                    }

                }
            }
            if (!cameraDisabled)
            {
                rotateDisable = true;
                localRotation.x += Input.GetAxis("Mouse X") * mouseSensibilityX;
                localRotation.y += Input.GetAxis("Mouse Y") * (-mouseSensibilityY);
                localRotation.y = Mathf.Clamp(localRotation.y, -90, 90);
            }
                
            
        }else if (Input.GetMouseButtonUp(0))
        {
            pieces.Clear();
            planes.Clear();
            cameraDisabled = rotateDisable = false;
        }

        Quaternion quaternion = Quaternion.Euler( localRotation.y,localRotation.x, 1);
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, quaternion, Time.deltaTime * mouseSensibility);
    }
}
