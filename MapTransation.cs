using System.Collections;
using Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapTransation : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapEdges;
    [SerializeField] private Direction direction;
    [SerializeField] private float movePlayer;
    private enum Direction {Up, Down, Left, Right};
    private CinemachineConfiner cinemachineConfiner;
    private void Awake()
    {
        cinemachineConfiner = FindAnyObjectByType<CinemachineConfiner>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            cinemachineConfiner.m_BoundingShape2D = mapEdges;
            UpdatePlayerDirection(collision.gameObject);
        }
    }

    private void UpdatePlayerDirection (GameObject player)
    {
        Vector2 newPosition = player.transform.position;

        switch(direction)
        {
            case Direction.Up:
                newPosition.y += movePlayer;
                break;
            case Direction.Down:
                newPosition.y -= movePlayer;
                break;
            case Direction.Right:
                newPosition.x += movePlayer;
                break;
            case Direction.Left:
                newPosition.x -= movePlayer;
                break;
        }

        player.transform.position = newPosition;
    }  
}
