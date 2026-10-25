using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class WaypointMover : MonoBehaviour
{
    public Transform fatherWaypoint;
    public float moveSpeed = 2;
    public float waitTime = 2f;
    public bool loopWaypoints = true;

    private Transform[] waypoints;
    private int currentWaypoinIndex;
    private bool isWaiting;

    private Animator animator;
    private float lastInputX;
    private float lastInputY;

    void Start()
    {
        animator = GetComponent<Animator>();
        waypoints = new Transform[fatherWaypoint.childCount];

        for (int i = 0; i < fatherWaypoint.childCount; i++)
        {
            waypoints[i] = fatherWaypoint.GetChild(i);
        }
    }

    void Update()
    {
        if (PauseController.IsGamePaused || isWaiting)
        {
            animator.SetBool("isWalking", false);

            // animator.SetFloat("LastInputX", lastInputX);
            // animator.SetFloat("LastInputY", lastInputY);

            return;
        }

        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypoinIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        // if(direction.magnitude > 0f)
        // {
        //     lastInputX = direction.x;
        //     lastInputY = direction.y;
        // }

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        animator.SetFloat("InputX", direction.x);
        animator.SetFloat("InputY", direction.y);
        animator.SetBool("isWalking", direction.magnitude > 0f);

        if(Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(waitTime);

        // animator.SetFloat("LastInputX", lastInputX);
        // animator.SetFloat("LastInputY", lastInputY);

        currentWaypoinIndex = loopWaypoints ? (currentWaypoinIndex + 1) % waypoints.Length : Mathf.Min(currentWaypoinIndex + 1, waypoints.Length - 1);

        isWaiting = false;
    }
}