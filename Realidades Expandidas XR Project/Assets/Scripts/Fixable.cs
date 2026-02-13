using UnityEngine;

public class Fixable : MonoBehaviour
{
    IFixable _fixableComponent;

    [SerializeField] private bool _isFixed = true;
    public bool IsFixed
    {
        get => _isFixed;
        private set
        {
            if (_isFixed != value)
            {
                _isFixed = value;

                if (_isFixed)
                {
                    Debug.Log($"Fixing {gameObject.name} of type {Type}");
                    _lightMeshRenderer.material = _lightOffMaterial;
                    _light.enabled = false;
                }
            }
            else _isFixed = value;
        }
    }

    private bool _broken = false;

    [SerializeField] private FixableType _type;
    public FixableType Type => _type;

    public enum FixableType { Sliders, Valve }

    [SerializeField] private MeshRenderer _lightMeshRenderer;
    [SerializeField] private Material _lightOnMaterial;
    [SerializeField] private Material _lightOffMaterial;
    [SerializeField] private Light _light;



    public void Awake()
    {
        _fixableComponent = GetComponent<IFixable>();

        _lightMeshRenderer.material = _lightOffMaterial;
        _light.enabled = false;
    }

    public void UnFix()
    {
        Debug.Log($"Unfixing {gameObject.name} of type {Type}");

        _isFixed = false;
        _broken = true;

        _lightMeshRenderer.material = _lightOnMaterial;
        _light.enabled = true;

        _fixableComponent.UnFix();
    }

    private void Update()
    {
        if (_broken) IsFixed = _fixableComponent.IsFixed();
    }
}
