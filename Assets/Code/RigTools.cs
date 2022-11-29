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
        Vector3[] moveVectors = GetMoveVectors();

        switch (axis)
        {
            case Axis.X:
                cubePosition += moveVectors[0];
                toolPosition += moveVectors[0];
                break;
            case Axis.Y:
                cubePosition += moveVectors[1];
                toolPosition += moveVectors[1];
                break;
            case Axis.Z:
                cubePosition += moveVectors[2];
                toolPosition += moveVectors[2];
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
    }

    private void UpdateScale()
    {
        Vector3 cubeScale = cube.transform.localScale;
        Vector3 toolPosition = transform.localPosition;
        float[] scaleDir = GetProjectedDelta();

        switch (axis)
        {
            case Axis.X:
                cubeScale.x -= scaleDir[0];
                toolPosition.x += scaleDir[0];
                break;
            case Axis.Y:
                cubeScale.y += scaleDir[1];
                toolPosition.y += scaleDir[1];
                break;
            case Axis.Z:
                cubeScale.z -= scaleDir[2];
                toolPosition.z += scaleDir[2];
                break;
            default:
                print("no valid axis");
                break;
        }

        cube.transform.localScale = cubeScale;
        transform.localPosition = toolPosition;
    }

    Vector3[] GetMoveVectors()
    {
        float[] scaleDir = GetProjectedDelta();

        //Turn the angle into a 1,-1 or 0 scaler for each axis using trig functions and rounding to the nearest int.
        Vector3 moveX = transform.parent.right * scaleDir[0];
        Vector3 moveY = transform.parent.up * scaleDir[1];
        Vector3 moveZ = transform.parent.forward * scaleDir[2];

        return new Vector3[] { moveX, moveY, moveZ };
    }

    float[] GetProjectedDelta()
    {
        //Project the transform vectors of movement onto a 2D plane using the camera as the normal
        Vector3 projecX = Vector3.ProjectOnPlane(cube.transform.right, camTrans.forward);
        Vector3 projecY = Vector3.ProjectOnPlane(cube.transform.up, camTrans.forward);
        Vector3 projecZ = Vector3.ProjectOnPlane(cube.transform.forward, camTrans.forward);

        //Determine the angle and positive or negative rotation direction based on an axis
        float aX = Vector3.SignedAngle(projecX, camTrans.right, camTrans.forward) * Mathf.Deg2Rad;
        float aY = Vector3.SignedAngle(projecY, camTrans.up, camTrans.forward) * Mathf.Deg2Rad;
        float aZ1 = Vector3.SignedAngle(projecZ, camTrans.forward, camTrans.up) * Mathf.Deg2Rad;
        float aZ2 = Vector3.SignedAngle(projecZ, camTrans.forward, camTrans.right) * Mathf.Deg2Rad;

        float scaleX = Mathf.Round(Mathf.Cos(aX)) * mouseDelta.x - Mathf.Round(Mathf.Sin(aX)) * mouseDelta.y;
        float scaleY = Mathf.Round(Mathf.Cos(aY)) * mouseDelta.y + Mathf.Round(Mathf.Sin(aY)) * mouseDelta.x;
        float scaleZ = Mathf.Round(Mathf.Sin(aZ2)) * mouseDelta.y - Mathf.Round(Mathf.Sin(aZ1)) * mouseDelta.x;

        return new float[] { scaleX, scaleY, scaleZ };
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
