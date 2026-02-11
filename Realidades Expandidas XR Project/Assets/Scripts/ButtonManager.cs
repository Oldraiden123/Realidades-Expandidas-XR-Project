using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ButtonManager : MonoBehaviour
{
    bool canInteract = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Serializable]
    /// <summary>
    /// Function definition for a button click event.
    /// </summary>
    public class ButtonClickedEvent : UnityEvent { }

    // Event delegates triggered on click.
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();


    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.layer == LayerMask.NameToLayer("Hands") && canInteract)
        {
            //todo: decide whether Trigger or Grip
            if(other.gameObject.GetComponentInParent<Animator>().GetFloat("Trigger") > 0.1f)
            {
                m_OnClick.Invoke();
                canInteract = false;
                StartCoroutine(PressCooldown());
            }
            
        }
    }


    IEnumerator PressCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canInteract = true;
    }
}
