using UnityEngine;

public class FlashlightToFogShader : MonoBehaviour
{
    public Material fogMaterial;   // Material using fog shader
    public Light flashlight;       // Spotlight (VR flashlight)

    void LateUpdate()
    {
        if (!fogMaterial || !flashlight) return;

        Transform t = flashlight.transform;

        // 1. Position (world space)
        fogMaterial.SetVector("_FlashlightPos", t.position);

        // 2. Direction the flashlight is pointing
        fogMaterial.SetVector("_FlashlightDir", t.forward.normalized);

        // 3. Range
        fogMaterial.SetFloat("_FlashlightRange", flashlight.range);

        // 4. Convert spot angle → cosine for shader cone math
        float halfAngleRad = flashlight.spotAngle * 0.5f * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(halfAngleRad);
        fogMaterial.SetFloat("_FlashlightAngle", cosAngle);
    }
}
