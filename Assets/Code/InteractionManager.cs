using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{

    public LayerMask clickMask;
    private Camera mainCam;
    RigTools selectedTool;
    bool toolSelected = false;
    Vector3 lastMousePos = Vector3.zero;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void SpawnCube()
    {
        print("spawn cube");
    }

    public void MouseClick(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            MouseDown();
        }
        else
        {
            Deselect();
        }
    }

    private void MouseDown()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, 200, clickMask))
        {
            if (hit.collider.CompareTag("Button"))
            {
                if (hit.collider.gameObject.TryGetComponent<ButtonAction>(out ButtonAction buttonAction))
                {
                    buttonAction.MouseDown();
                    SpawnCube();
                }
            }

            else if (hit.collider.CompareTag("RigTool"))
            {
                if (hit.collider.gameObject.TryGetComponent<RigTools>(out RigTools rigTool))
                {
                    toolSelected = true;
                    selectedTool = rigTool;
                    lastMousePos = TransformHelper();
                    selectedTool.MouseDown();
                }
            }
            else
            {
                Deselect();
            }
        }
    }

    private void Deselect()
    {
        if (toolSelected)
        {
            selectedTool.MouseUp();
            selectedTool = null;
            toolSelected = false;
        }
    }

    public void MouseMove(InputAction.CallbackContext context)
    {
        if (toolSelected)
        {
            Vector3 mousePos = TransformHelper();
            selectedTool.MouseMove(mousePos - lastMousePos);
            lastMousePos = mousePos;
        }
    }

    public Vector3 TransformHelper()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = mainCam.WorldToScreenPoint(selectedTool.transform.position).z;
        return mainCam.ScreenToWorldPoint(mousePos);
    }
}
