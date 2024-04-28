using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class FallingCube : MonoBehaviour
{
    [SerializeField] private Color _baseColor;
    
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    
    public bool HasTouchedFloor { get; private set; } = false;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        HasTouchedFloor = false;
        _rigidbody.useGravity = true;
        _renderer.material.color = _baseColor;
    }

    private void OnDisable()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private Color GetRandomColor()
    {
        float minColorChannelValue = 0;
        float maxColorChannelValue = 1;

        float redChannel = Random.Range(minColorChannelValue, maxColorChannelValue);
        float greenChannel = Random.Range(minColorChannelValue, maxColorChannelValue);
        float blueChannel = Random.Range(minColorChannelValue, maxColorChannelValue);

        return new Color(redChannel, greenChannel, blueChannel);
    }

    public void CollideWithFloor()
    {
        if(HasTouchedFloor)
            return;

        _renderer.material.color = GetRandomColor();
        HasTouchedFloor = true;
    }
}
