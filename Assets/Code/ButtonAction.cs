using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public Material[] buttonMats; //0 default, 1 highlight
    public Renderer buttonRenderer;

    private void Start()
    {
        buttonRenderer.material = buttonMats[0];
    }

    public void MouseDown()
    {
        buttonRenderer.material = buttonMats[1];
        StartCoroutine(Unhighlight());
    }

    private IEnumerator Unhighlight()
    {
        yield return new WaitForSeconds(.2f);
        buttonRenderer.material = buttonMats[0];
    }
}