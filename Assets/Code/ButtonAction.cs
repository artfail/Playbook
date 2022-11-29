/*
John Bruneau
Nov 2020 (CC BY 3.0)

A very simple script to give the button some user feedback.
A brighter unlit material is swapped in to highlight the click
and then the material is reset.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public Material[] buttonMats; //0 default, 1 highlight
    public Renderer buttonRenderer;
    private WaitForSeconds point2;

    private void Start()
    {
        point2 = new WaitForSeconds(.2f);
        buttonRenderer.material = buttonMats[0];
    }

    public void MouseDown()
    {
        buttonRenderer.material = buttonMats[1];
        StartCoroutine(Unhighlight());
    }

    private IEnumerator Unhighlight()
    {
        yield return point2;
        buttonRenderer.material = buttonMats[0];
    }
}