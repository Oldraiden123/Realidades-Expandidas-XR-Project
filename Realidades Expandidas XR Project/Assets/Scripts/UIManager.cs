using UnityEngine;
using Unity.XR.CoreUtils;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private XROrigin xrOrigin;

    public void ExitGame()
    {
        Debug.Log("Closed Game");
        Application.Quit();
    }
    public void TestButton()
    {
        Debug.Log("Button Pressed");
    }

    public void ToggleUI()
    {
        canvasObject.SetActive(!canvasObject.activeSelf);
    }

    public void SetHeightShort()
    {
        xrOrigin.CameraYOffset = 1.55f;
    }
    public void SetHeightMedium()
    {
        xrOrigin.CameraYOffset = 1.7f;
    }
    public void SetHeightTall()
    {
        xrOrigin.CameraYOffset = 1.85f;
    }
}
