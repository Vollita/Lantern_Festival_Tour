using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class DrawerInteracterable : XRGrabInteractable
{
    [SerializeField] XRSocketInteractor keySocket;
    [SerializeField] bool isLocked;
    [SerializeField] Transform drawerTransform;
    [SerializeField] private Vector3 limitDistance=new Vector3(0.1f,0.1f,0);
    [SerializeField] GameObject keyPop;
    [SerializeField] float drawerLimitZ = 0.8f;

    private Transform parentTransform;
    private const string defaultLayer = "Default";
    private const string grabLayer = "Grab";
    private bool isGrabbed;
    private Vector3 limitPosotions;
    // Start is called before the first frame update
    void Start()
    {
        if( keySocket != null)
        {
            keySocket.selectEntered.AddListener(OnDrawerUnlocked);
            keySocket.selectExited.AddListener(OnDrawerLocked);
        }
        parentTransform = transform.parent.transform;
        limitPosotions = drawerTransform.localPosition;
    }

    private void OnDrawerUnlocked(SelectEnterEventArgs arg0)
    {
        isLocked = false;
        if (keyPop != null)
        {
            keyPop.gameObject.SetActive(false);
        }
        Debug.Log("***Drawer UNLOCKED");
    }
    private void OnDrawerLocked(SelectExitEventArgs arg0)
    {
        isLocked = true;
        Debug.Log("***Drawer LOCKED");
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (!isLocked)
        {
            // ½âËø×´Ì¬
            transform.SetParent(parentTransform);
            isGrabbed = true;
        }
        else
        {
            // Ëø×¡×´Ì¬
            ChangeLayerMask(defaultLayer);
        }

        }

    protected override void OnSelectExited(SelectExitEventArgs args) 
    {
        base.OnSelectExited(args);
        ChangeLayerMask(grabLayer); ;
        isGrabbed = false;
        transform.localPosition= drawerTransform.localPosition;
    } 



    // Update is called once per frame

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed && drawerTransform != null)
        {
            drawerTransform.localPosition = new Vector3(drawerTransform.localPosition.x, drawerTransform.localPosition.y,
                transform.localPosition.z);

            CheckLimits();
        }
    }

    private void CheckLimits() 
    {
        if (transform.localPosition.x >= limitPosotions.x + limitDistance.x ||
            transform.localPosition.x <= limitPosotions.x - limitDistance.x)
        {
            ChangeLayerMask(defaultLayer);
        }
        else if (
            transform.localPosition.y >= limitPosotions.y + limitDistance.y ||
            transform.localPosition.y <= limitPosotions.y - limitDistance.y)
        {
            ChangeLayerMask(defaultLayer);
        }
        else if (drawerTransform.localPosition.z <= limitPosotions.z - limitDistance.z)
        {
            isGrabbed = false;
            drawerTransform.localPosition = limitPosotions;
            ChangeLayerMask(defaultLayer);
        }
        else if (drawerTransform.localPosition.z >= limitDistance.z+drawerLimitZ)
        {
            isGrabbed = false;
            drawerTransform.localPosition = new Vector3(drawerTransform.localPosition.x,
                drawerTransform.localPosition.y, drawerLimitZ);
            ChangeLayerMask(defaultLayer);
        }

    }

    private void ChangeLayerMask(string mask)
    {
        interactionLayers = InteractionLayerMask.GetMask(mask);
    }
}
