using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransTools : MonoBehaviour
{
    public GameObject Cube;
    public enum Change { Position, Rotation, Scale };
    public enum Axis { X, Y, Z };
    public Change change;
    public Axis axis;


    public void MouseDown()
    {
        switch (change)
        {
            case Change.Position:

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
        switch (axis)
        {
            case Axis.X:
                // code block
                break;
            case Axis.Y:
                // code block
                break;
            case Axis.Z:
                // code block
                break;
            default:
                print("no valid axis");
                break;
        }
    }
}
