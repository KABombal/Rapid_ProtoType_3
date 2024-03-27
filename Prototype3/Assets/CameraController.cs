using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

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
    private float angleStandardWall = 40f;

    [SerializeField]
    private float angleWall = 10f;

    [SerializeField]
    private float floorAngleThreshold = 50f;

    [SerializeField]
    private bool useRaycast = true;

    [SerializeField]
    private float rayCastStart = 0.05f;

    [SerializeField]
    private LayerMask wallLayer = new LayerMask();


    private Mode mode;
    private Vector3 followDelta;
    private Vector3 desiredPosition;

    private Vector3 targetLastPosition;
    private Vector3 targetVel;


    private DepthOfField DoF;


    private void Start()
    {
        GetComponent<Volume>().profile.TryGet(out DoF);

        targetLastPosition = target.position;
        targetVel = Vector3.zero;


        Vector3 desiredDelta = Vector3.forward * followDistance;
        Quaternion look = Quaternion.LookRotation(Vector3.ProjectOnPlane(target.forward, Vector3.up), Vector3.up);
        Quaternion elevation = Quaternion.Euler(-angleStandard, 0, 0);

        followDelta = look * elevation * desiredDelta;
    }

    private void Update()
    {
        CalcMode();



        if (DoF) UpdateDoF();

        targetVel = target.position - targetLastPosition;
        bool isMoving = targetVel.sqrMagnitude > Mathf.Epsilon;
        targetLastPosition = target.position;

        switch (mode)
        {
            case Mode.FreeFollow:
                if (isMoving)
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

    private void UpdateDoF()
    {
        float dist = Vector3.Distance(transform.position, target.position);
        DoF.focusDistance.Override(dist);
    }

    private void CalcMode()
    {
        float elevation = Vector3.Angle(target.up, Vector3.up);

        if (elevation > floorAngleThreshold) mode = Mode.WallFollow;
        else mode = Mode.FreeFollow;
    }

    private void CalcFollowDeltaFree()
    {
        Vector3 flatTargetVel = Vector3.ProjectOnPlane(-targetVel, Vector3.up);

        if (flatTargetVel.sqrMagnitude <= Mathf.Epsilon) return;

        Vector3 desiredDelta = Vector3.forward * followDistance;
        
        Quaternion look = Quaternion.LookRotation(flatTargetVel, Vector3.up);
        Quaternion elevation = Quaternion.Euler(-angleStandard, 0, 0);

        followDelta = look * elevation * desiredDelta;
    }

    private void CalcFollowDeltaWall()
    {
        int wallLayer = target.GetComponent<SpiderController>().CurrentGroundLayer;
        bool isOnActualWall = ((1 << wallLayer) & this.wallLayer.value) != 0;

        float angle = isOnActualWall ? angleWall : angleStandardWall;

        Vector3 desiredDelta = Vector3.forward * followDistance;

        Quaternion look = Quaternion.LookRotation(Vector3.ProjectOnPlane(target.up, Vector3.up), Vector3.up);
        Quaternion elevation = Quaternion.Euler(-angle, 0, 0);

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
