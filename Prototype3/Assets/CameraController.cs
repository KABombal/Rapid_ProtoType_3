using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    private enum Mode
    {
        FreeFollow,
        WallFollow
    }

    [SerializeField]
    private Transform target = null;

    [SerializeField]
    private float followDistance = 0.1f;

    [SerializeField]
    private float followSpeed = 1f;

    [SerializeField]
    private float followLagClamp = 0.1f;

    [SerializeField]
    private float angleStandard = 60f;

    [SerializeField]
    private float angleWall = 10f;

    [SerializeField]
    private float floorAngleThreshold = 50f;

    [SerializeField]
    private bool useRaycast = true;

    [SerializeField]
    private float rayCastStart = 0.05f;


    private Mode mode;
    private Vector3 followDelta;
    private Vector3 desiredPosition;

    private Vector3 targetLastPosition;

    private void Update()
    {
        CalcMode();

        switch (mode)
        {
            case Mode.FreeFollow:
                CalcFollowDeltaFree();
                break;
            case Mode.WallFollow:
                CalcFollowDeltaWall();
                break;
        }



        Vector3 newDesiredPos = target.position + followDelta;

        desiredPosition = Vector3.Lerp(desiredPosition, newDesiredPos, Time.deltaTime * followSpeed);
        desiredPosition = newDesiredPos + Vector3.ClampMagnitude(desiredPosition - newDesiredPos, followLagClamp);

        // go to desired pos using a raycast from target to desired pos
        GotoDesiredPos();

        transform.LookAt(target.position);

        if ((target.position - targetLastPosition).sqrMagnitude > 0.0001f)
            targetLastPosition = target.position;
    }

    private void CalcMode()
    {
        float elevation = Vector3.Angle(target.up, Vector3.up);

        if (elevation > floorAngleThreshold) mode = Mode.WallFollow;
        else mode = Mode.FreeFollow;
    }

    private void CalcFollowDeltaFree()
    {
        Vector3 vel = target.position - targetLastPosition;

        Vector3 desiredDelta = Vector3.forward * followDistance;
        Quaternion look = Quaternion.LookRotation(Vector3.ProjectOnPlane(-vel, Vector3.up), Vector3.up);
        Quaternion angle = Quaternion.Euler(-angleStandard, 0, 0);

        followDelta = look * angle * desiredDelta;
    }

    private void CalcFollowDeltaWall()
    {
        Vector3 desiredDelta = Vector3.forward * followDistance;

        Quaternion look = Quaternion.LookRotation(Vector3.ProjectOnPlane(target.up, Vector3.up), Vector3.up);
        Quaternion elevation = Quaternion.Euler(-angleWall, 0, 0);

        followDelta = look * elevation * desiredDelta;
    }

    private void GotoDesiredPos()
    {
        Vector3 start = target.position;
        Vector3 dir = desiredPosition - target.position;

        start += dir.normalized * rayCastStart; // to ensure no weird stuff since it originates from floor

        float dist = Vector3.Distance(start, desiredPosition);

        if (useRaycast && Physics.Raycast(start, dir, out RaycastHit hit, dist))
        {
            transform.position = hit.point;
        }
        else transform.position = desiredPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(target.position, target.position + followDelta);

        Gizmos.DrawWireSphere(desiredPosition, 0.01f);
    }
}
