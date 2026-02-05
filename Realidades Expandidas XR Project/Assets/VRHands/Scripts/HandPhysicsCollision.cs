using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HandPhysics : MonoBehaviour
{
    [SerializeField] bool isLeftHand;
    [SerializeField] float teleportingDistance = .5f;
    Transform handController;
    Rigidbody rb;
    int rotationFix;
    private Collider[] handColliders;

    //Mark if Left hand

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        handColliders = GetComponentsInChildren<Collider>();
        transform.SetParent(null);
        if (isLeftHand)
        {
            rotationFix = -90;
            handController = GameObject.Find("Left Controller").transform;
        }
        else
        {
            rotationFix = 90;
            handController = GameObject.Find("Right Controller").transform;
        }
    }

    public void EnableHandColliders()
    {
        foreach (Collider collider in handColliders)
        {
            collider.enabled = true;
        }
    }

    public void EnableHandCollidersDelay()
    {
        Invoke("EnableHandColliders", teleportingDistance);
    }

    public void DisableHandColliders()
    {
        foreach (Collider collider in handColliders)
        {
            collider.enabled = false;
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, handController.position);

        //teleport hand back to player if too far
        if(distance > teleportingDistance)
        {
            transform.position = handController.position;
        }
    }

    void FixedUpdate()
    { 
        //pos
        rb.linearVelocity= (handController.position - transform.position) / Time.fixedDeltaTime;

        //rot
        Quaternion postRotation = transform.rotation * Quaternion.Euler(0, 0, rotationFix);
        Quaternion rotationDifference = handController.rotation * Quaternion.Inverse(postRotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);
        Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;
        rb.angularVelocity = (rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);

    }
}
