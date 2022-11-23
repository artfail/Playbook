using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RigTools : MonoBehaviour
{
    public GameObject cube;
    public enum Change { Position, Rotation, Scale };
    public enum Axis { X, Y, Z };
    public Change change;
    public Axis axis;
    private Vector2 mouseDelta;

    Vector3 startPosition;
    private bool pressed;

    private Transform[] rigTrans;

    private void Start()
    {
        rigTrans = transform.parent.GetComponentsInChildren<Transform>();
        rigTrans = rigTrans.Skip(1).ToArray();
    }

    public void MouseDown()
    {
        pressed = true;
        startPosition = transform.localPosition;
        ShowRig(false);
    }

    public void MouseUp()
    {
        pressed = false;
        transform.localPosition = startPosition;
        ShowRig(true);
    }


    public void MouseMove(Vector2 mdelta)
    {
        if (!pressed) return;

        mouseDelta = mdelta;

        switch (change)
        {
            case Change.Position:
                UpdatePosition();
                break;
            case Change.Rotation:
                UpdateRotation();
                break;

            case Change.Scale:
                UpdateScale();
                break;
            default:
                print("no valid transform");
                break;
        }
    }

    private void UpdatePosition()
    {
        Vector3 cubePosition = cube.transform.position;
        Vector3 toolPosition = transform.parent.position;
        switch (axis)
        {
            case Axis.X:
                cubePosition.x += mouseDelta.x;
                toolPosition.x += mouseDelta.x;
                break;
            case Axis.Y:
                cubePosition.y += mouseDelta.y;
                toolPosition.y += mouseDelta.y;
                break;
            case Axis.Z:
                cubePosition.z += mouseDelta.x;
                toolPosition.z += mouseDelta.x;
                break;
            default:
                print("no valid axis");
                break;
        }

        cube.transform.position = cubePosition;
        transform.parent.position = toolPosition;
    }

    private void UpdateRotation()
    {
        Quaternion cubeRotation = cube.transform.rotation;
        Quaternion toolRotation = transform.parent.rotation;
        switch (axis)
        {
            case Axis.X:
                cubeRotation.x += mouseDelta.y;
                toolRotation.x += mouseDelta.y;
                break;
            case Axis.Y:
                cubeRotation.y -= mouseDelta.x;
                toolRotation.y -= mouseDelta.x;
                break;
            case Axis.Z:
                cubeRotation.z -= mouseDelta.x;
                toolRotation.z -= mouseDelta.x;
                break;
            default:
                print("no valid axis");
                break;
        }

        cube.transform.rotation = cubeRotation;
        transform.parent.rotation = toolRotation;
    }

    private void UpdateScale()
    {
        Vector3 cubeScale = cube.transform.localScale;
        Vector3 toolPosition = transform.localPosition;
        switch (axis)
        {
            case Axis.X:
                cubeScale.x -= mouseDelta.x;
                toolPosition.x += mouseDelta.x;
                break;
            case Axis.Y:
                cubeScale.y += mouseDelta.y;
                toolPosition.y += mouseDelta.y;
                break;
            case Axis.Z:
                cubeScale.z -= mouseDelta.x;
                toolPosition.z += mouseDelta.x;
                break;
            default:
                print("no valid axis");
                break;
        }

        cube.transform.localScale = cubeScale;
        transform.localPosition = toolPosition;
    }

    void ShowRig(bool show)
    {
        foreach (Transform tool in rigTrans)
        {
            print(tool.name);
            if (tool.gameObject != this.gameObject)
            {
                tool.gameObject.SetActive(show);
            }
        }
    }
}
