using UnityEngine;

public class Oscillator : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;

    [SerializeField] private Vector3 movementVector;
    [SerializeField] private float speed;
     private float movementFactor;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;
    }
    private void Update()
    {
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);   
    }
}
