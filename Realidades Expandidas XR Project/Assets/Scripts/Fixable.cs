using UnityEngine;

public class Fixable : MonoBehaviour
{
    IFixable _fixableComponent;

    [SerializeField] private bool _isFixed = true;
    public bool IsFixed => _isFixed;

    private bool _broken = false;

    [SerializeField] private FixableType _type;
    public FixableType Type => _type;

    public enum FixableType { Sliders, Valve }


    public void Start()
    {
        _fixableComponent = GetComponent<IFixable>();
        _isFixed = true;
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
            _broken = true;
        }
    }

    private void Update()
    {
        if (_broken) _isFixed = _fixableComponent.IsFixed();
    }
}
