using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject canvasObject;

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
}
