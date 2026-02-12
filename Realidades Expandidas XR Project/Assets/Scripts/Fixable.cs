using UnityEngine;

public class Fixable : MonoBehaviour
{
    IFixable _fixableComponent;

    [SerializeField] private bool _isFixed = false;
    public bool IsFixed => _isFixed;

    [SerializeField] private FixableType _type;
    public FixableType Type => _type;

    public enum FixableType { Sliders, Valve }


    public void Start()
    {
        _fixableComponent = GetComponent<IFixable>();
    }

    public void UnFix()
    {
        if (_fixableComponent == null)
        {
            _fixableComponent = GetComponent<IFixable>();
        }

        if (_fixableComponent != null)
        {
            _fixableComponent.UnFix();
            _isFixed = false;
        }
    }

    private void Update()
    {
        _isFixed = _fixableComponent.IsFixed();
    }
}
