using UnityEngine;
using Unity.XR.CoreUtils;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject canvasObject;
    private GameObject playerXR;
    [SerializeField] private XROrigin xrOrigin;
    private bool canInteract = true;
    private float uiCooldownTimer = 0f;
    [SerializeField] private float uiCooldown = 0.5f;
    [SerializeField] private PlayerInitializer playerInitializer;


    private void Awake()
    {
        playerXR = GameObject.Find("XR Origin (XR Rig)");
        xrOrigin = playerXR.GetComponent<XROrigin>();
    }

    private void Update()
    {
        if(uiCooldownTimer > 0f)
        {
            uiCooldownTimer -= Time.deltaTime;
            if(uiCooldownTimer < 0f)
            {
                canInteract = true;
            }
            else
            {
                canInteract = false;
            }
                
        }
    }
    public void ExitGame()
    {
        if (canInteract)
        {
            Debug.Log("Closed Game");
            Application.Quit();
            uiCooldownTimer = uiCooldown;
        }
        
    }
    public void TestButton()
    {
        if (canInteract)
        {
            Debug.Log("Button Pressed");
            uiCooldownTimer = uiCooldown;
        }
        
    }

    public void ToggleUI()
    {
        if (canInteract)
        {
            canvasObject.SetActive(!canvasObject.activeSelf);
            uiCooldownTimer = uiCooldown;
        }
        
    }

    public void SetHeightShort()
    {
        if (canInteract)
        {
            xrOrigin.CameraYOffset = 0f;
            playerInitializer.playerHeight = 0f;
            uiCooldownTimer = uiCooldown;
        }
        
    }
    public void SetHeightMedium()
    {
        if (canInteract)
        {
            xrOrigin.CameraYOffset = 0.15f;
            playerInitializer.playerHeight = 0.15f;
            uiCooldownTimer = uiCooldown;
        }
        
    }
    public void SetHeightTall()
    {
        if (canInteract)
        {
            xrOrigin.CameraYOffset = 0.3f;
            playerInitializer.playerHeight = 0.3f;
            uiCooldownTimer = uiCooldown;
        }
        
    }
}
