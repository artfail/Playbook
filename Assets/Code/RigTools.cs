using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigTools : MonoBehaviour
{
    public GameObject cube;
    public enum Change { Position, Rotation, Scale };
    public enum Axis { X, Y, Z };
    public Change change;
    public Axis axis;
    private Vector2 mouseDelta;
    private bool pressed;

    private void Start()
    {

    }

    public void MouseDown()
    {
        pressed = true;
    }

    public void MouseUp()
    {
        pressed = false;
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

                break;

            case Change.Scale:

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
}
