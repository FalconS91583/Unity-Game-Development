using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
//klasa od dzia³ania wê¿a
public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;

    private List<Transform> _segments = new List<Transform>();
    public Transform segmentPrefab;
    public int initialSize = 2;
    public SnakeGameManager gameManager;
    private bool isGameOver = false;

    private void Start()
    {
        ResetState();  

    }
    //funkcja od poruszania siê 
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            if(_direction.y != -1)
            {
                _direction.x = 0;
                _direction.y = +1;
            }
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            if (_direction.y != +1)
            {
                _direction.x = 0;
                _direction.y = -1;
            }
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            if (_direction.x != +1)
            {
                _direction.x = -1;
                _direction.y = 0;
            }
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            if (_direction.x != -1)
            {
                _direction.x = +1;
                _direction.y = 0;
            }
        }
    }
    //funkcja od zderzenia ze sob¹ 
    private void FixedUpdate()
    {
        if (isGameOver)
        {
            return;
        }
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }
    //funkcja od dodatkowych segmentów snake
    private void Grow()
    {
        gameManager = FindObjectOfType<SnakeGameManager>();

        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);

        gameManager.IncreaseScore();
    }
    //reset gry
    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);

        for(int i = 1; i < this.initialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector3.zero;
        gameManager.ResetScore();
       gameManager.EndGame();
        isGameOver = true;

    }
    //funkcja od zarz¹dzania zdarzeniami po wejœciu w œcianê lub zjedzeniu jedzenia 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
        }
        else if (other.CompareTag("Obstacle"))
        {
            ResetState();
        }
    }


}
