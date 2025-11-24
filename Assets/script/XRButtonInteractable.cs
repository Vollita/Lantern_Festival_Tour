using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
public class XRButtonInteractable : XRSimpleInteractable
{
    // Start is called before the first frame update
    //[SerializeField] Color[] buttonColors=new Color[4];
    [SerializeField] Image buttonImage;
    private bool isPressed;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color pressedColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] GameObject key;
    void Start()
    {
     
        ResetColor();
    }
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        isPressed = false;
        buttonImage.color=highlightColor;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if (!isPressed) 
        {
            ResetColor();
        }
        ResetColor();
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    { 
        base.OnSelectEntered(args); 
        isPressed = true;
        key.gameObject.SetActive(true);
        buttonImage.color = pressedColor;
            }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        buttonImage.color = selectedColor; 
    }
    // Update is called once per frame
    public void ResetColor()
    {
        buttonImage.color = normalColor;
    }
}
