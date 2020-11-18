using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class SelectHexes : MonoBehaviour
{
    Mouse mouse => Mouse.current;
    Camera cam;
    private void Awake() => cam = Camera.main;

    public void LeftClick(CallbackContext context)
    {
        if(!context.performed)
            return;
            
        if(Physics.Raycast(cam.ScreenPointToRay(mouse.position.ReadValue()), out RaycastHit hit))
        {
            if(hit.collider == null)
                return;
            
            Hex hex = hit.collider.GetComponent<Hex>();
            if(hex != null)
                hex.ToggleSelect();
        }
    }
}
