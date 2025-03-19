using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PrintInstructions();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float xValue = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        float yValue = 0f;
        float zValue = Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;


        transform.Translate(xValue, yValue, zValue);
    }

    private void PrintInstructions()
    {
        Debug.Log("Press WSAD for move");
        Debug.Log("Avoid the obstacles");
    }

}
