using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{

    public LayerMask clickMask;
    private Camera mainCam;

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
                    buttonAction.Clicked();
                    SpawnCube();
                }
            }
        }
    }

    public void MouseMove(InputAction.CallbackContext context)
    {
        print(context.ReadValue<Vector2>());
    }


}
