/*
John Bruneau
Nov 2020 (CC BY 3.0)

The Interaction Manager, manages the button click and all the mouse actions.
The mouse events are sent as Unity Events from the New Input System Action map.
Mouse clicks are translated from 2D screen space to 3D world space via ScreenPointToRay.
The mouse delta that this version of the New Input System returns is a bit buggy and unreliable
so the interaction manager handles that as well.
*/

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

    public GameObject cubePrefab;
    public GameObject rigPrefab;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void SpawnCube()
    {
        GameObject newCube = Instantiate(cubePrefab);
        GameObject newRig = Instantiate(rigPrefab, newCube.transform.position, newCube.transform.rotation);

        RigTools[] rigTools = newRig.GetComponentsInChildren<RigTools>();
        foreach (RigTools tool in rigTools)
        {
            tool.cube = newCube;
        }
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