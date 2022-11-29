/*
John Bruneau
Nov 2020 (CC BY 3.0)

Rig Tools controls all the handle drag and drop actions for manipulating a Cube (or any generic game object)
The Cube and the mouse events are assigned by the InteractionManager.CS
The actions for which each tool represents can be set in the Inspector using Enum dropdown menus.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigTools : MonoBehaviour
{
    [HideInInspector]
    public GameObject cube;
    public enum Change { Position, Rotation, Scale };
    public enum Axis { X, Y, Z };
    public Change change;
    public Axis axis;
    public Material defaultMat;
    public Material transMat;

    [HideInInspector]
    public Renderer mRenderer;
    private Vector2 mouseDelta;

    Vector3 startPosition;
    private bool pressed;

    private RigTools[] allRigTools;
    Transform camTrans;

    private void Start()
    {
        camTrans = Camera.main.transform;
        mRenderer = GetComponent<Renderer>();
        //defaultMat = mRenderer.material;

        allRigTools = transform.parent.GetComponentsInChildren<RigTools>();
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
        // Local Space

        Vector3 cubePosition = cube.transform.position;
        Vector3 toolPosition = transform.parent.position;

        //Project the transform vectors of movement onto a 2D plane using the camera as the normal
        Vector3 projecX = Vector3.ProjectOnPlane(cube.transform.right, camTrans.forward);
        Vector3 projecY = Vector3.ProjectOnPlane(cube.transform.up, camTrans.forward);
        Vector3 projecZ = Vector3.ProjectOnPlane(cube.transform.forward, camTrans.forward);

        //Determine the angle and positive or negative rotation direction based on an axis
        float aX = Vector3.SignedAngle(projecX, camTrans.right, camTrans.forward) * Mathf.Deg2Rad;
        float aY = Vector3.SignedAngle(projecY, camTrans.up, camTrans.forward) * Mathf.Deg2Rad;
        float aZ1 = Vector3.SignedAngle(projecZ, camTrans.forward, camTrans.up) * Mathf.Deg2Rad;
        float aZ2 = Vector3.SignedAngle(projecZ, camTrans.forward, camTrans.right) * Mathf.Deg2Rad;

        //Turn the angle into a 1,-1 or 0 scaler for each axis using trig functions and rounding to the nearest int.
        Vector3 moveX = transform.parent.right * (Mathf.Round(Mathf.Cos(aX)) * mouseDelta.x - Mathf.Round(Mathf.Sin(aX)) * mouseDelta.y);
        Vector3 moveY = transform.parent.up * (Mathf.Round(Mathf.Cos(aY)) * mouseDelta.y + Mathf.Round(Mathf.Sin(aY)) * mouseDelta.x);
        Vector3 moveZ = transform.parent.forward * (Mathf.Round(Mathf.Sin(aZ2)) * mouseDelta.y - Mathf.Round(Mathf.Sin(aZ1)) * mouseDelta.x);


        switch (axis)
        {
            case Axis.X:
                cubePosition += moveX;
                toolPosition += moveX;
                break;
            case Axis.Y:
                cubePosition += moveY;
                toolPosition += moveY;
                break;
            case Axis.Z:
                cubePosition += moveZ;
                toolPosition += moveZ;
                break;
            default:
                print("no valid axis");
                break;
        }

        cube.transform.position = cubePosition;
        transform.parent.position = toolPosition;

        /*
        // World Space

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
        */
    }

    private void UpdateRotation()
    {
        // Local Space
        int speed = 20;
        switch (axis)
        {
            case Axis.X:
                cube.transform.Rotate(mouseDelta.y * speed, 0, 0, Space.Self);
                transform.parent.Rotate(mouseDelta.y * speed, 0, 0, Space.Self);
                break;
            case Axis.Y:
                cube.transform.Rotate(0, -mouseDelta.x * speed, 0, Space.Self);
                transform.parent.Rotate(0, -mouseDelta.x * speed, 0, Space.Self);
                break;
            case Axis.Z:
                cube.transform.Rotate(0, 0, -mouseDelta.x * speed, Space.Self);
                transform.parent.Rotate(0, 0, -mouseDelta.x * speed, Space.Self);
                break;
            default:
                print("no valid axis");
                break;
        }


        /*
        // World Space

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
        */
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

    //When a tool is used this makes the rest of the rig semitransparent
    void ShowRig(bool show)
    {
        foreach (RigTools tool in allRigTools)
        {
            if (tool.gameObject != this.gameObject)
            {
                if (show)
                {
                    tool.mRenderer.material = tool.defaultMat;
                }
                else
                {
                    tool.mRenderer.material = tool.transMat;
                }
            }
        }
    }
}
