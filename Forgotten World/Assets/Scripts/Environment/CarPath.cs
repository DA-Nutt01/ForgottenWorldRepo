using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPath : MonoBehaviour
{
    [SerializeField, Tooltip("A List of transforms to create a path for the car")]
    private List<Transform> _waypoints;

    [SerializeField, Tooltip("Moderates the speed of the car.")]
    private float _driveSpeed = 5f;

    [SerializeField, Tooltip("A reference to this car's charactercontroller component")]
    private CharacterController _controller;

    private int _waypointIndex = 0;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        FollowWaypoint();
    }

    private void FollowWaypoint()
    {

        Vector3 targetPosition = _waypoints[_waypointIndex].position;
        Vector3 direction = targetPosition - transform.position;

        Vector3 move = direction.normalized;

        _controller.Move(move);
    }
    
}
