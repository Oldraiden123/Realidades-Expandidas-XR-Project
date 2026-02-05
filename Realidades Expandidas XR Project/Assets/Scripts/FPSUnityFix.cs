using UnityEngine;

public class FPSTarget : MonoBehaviour
{
    [SerializeField] int FPS = 72;

    void Start()
    {
        Application.targetFrameRate = FPS;
    }
}
