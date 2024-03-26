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

    private Mode mode;
    private Vector3 followDelta;
    private Vector3 desiredPosition;


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
    }

    private void CalcMode()
    {
        float elevation = Vector3.Angle(target.up, Vector3.up);

        if (elevation > floorAngleThreshold) mode = Mode.WallFollow;
        else mode = Mode.FreeFollow;
    }

    private void CalcFollowDeltaFree()
    {
        Vector3 desiredDelta = Vector3.forward * followDistance;
        Quaternion angle = Quaternion.Euler(-angleStandard, target.rotation.eulerAngles.y, 0);

        followDelta = angle * desiredDelta;
    }

    private void CalcFollowDeltaWall()
    {
        Vector3 desiredDelta = Vector3.forward * followDistance;
        float angleYaw = Vector3.Angle(Vector3.forward, Vector3.ProjectOnPlane(target.up, Vector3.up));

        Quaternion elevation = Quaternion.Euler(-angleWall, angleYaw, 0);

        followDelta = elevation * desiredDelta;
    }

    private void GotoDesiredPos()
    {
        Ray ray = new Ray(target.position, desiredPosition - target.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
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
