using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SpiderAnimator : MonoBehaviour
{
    [SerializeField]
    private Rig legIkRig = null;

    [SerializeField]
    private TwoBoneIKConstraint[] legIkRigs = null;

    [SerializeField]
    private Transform[] leyRayOrigins = null;

    [SerializeField]
    private Transform hips = null;

    [SerializeField]
    private AnimationCurve stepYCurve;

    [SerializeField]
    private float legLength = 0.3f;

    [SerializeField]
    private float stepDuration = 0.01f;

    [SerializeField]
    private float strideDist = 0.01f;

    [SerializeField]
    private LayerMask groundLayer = new LayerMask();


    // store original target positions at start
    [SerializeField, HideInInspector]
    private Vector3[] initialTargetOffsets;

    private Vector3[] targetWorldPositions;

    private bool[] groundedFoot;

    private float legTimer;
    public float Speed { get; set; }


    private void Start()
    {
        // base all non-serialized arrays on length of a serialized one

        groundedFoot = Enumerable.Repeat(true, legIkRigs.Length).ToArray();
        initialTargetOffsets = new Vector3[legIkRigs.Length];
        targetWorldPositions = new Vector3[legIkRigs.Length];

        for (int i = 0; i < legIkRigs.Length; i++)
        {
            var targ = legIkRigs[i].data.target;
            targetWorldPositions[i] = targ.position;

            initialTargetOffsets[i] = Quaternion.Inverse(transform.rotation) * (targ.position - transform.position);
        }
    }

    private void Update()
    {
        for (int i = 0; i < legIkRigs.Length; i++)
        {
            var targ = legIkRigs[i].data.target;

            Vector3? want = FindFooting(i);

            if (want.HasValue)
            {
                // smoothly add weight since we have somewhere to put the leg
                legIkRigs[i].weight = Mathf.Clamp01(legIkRigs[i].weight + Time.deltaTime / stepDuration);
                
                if (CanStep(i) && (want.Value - targetWorldPositions[i]).sqrMagnitude > strideDist * strideDist)
                {
                    StartCoroutine(Step(i, want.Value));
                }
            }
            else
            {
                // smoothly remove weight resort to normal animation clip if we have nowhere to put the leg
                legIkRigs[i].weight = Mathf.Clamp01(legIkRigs[i].weight - Time.deltaTime / stepDuration);
            }

            if (groundedFoot[i])
            {
                targ.position = targetWorldPositions[i];
            }
        }
    }

    private Vector3? FindFooting(int leg)
    {
        Vector3 a = transform.position + transform.rotation * initialTargetOffsets[leg];

        Ray ray = new Ray(leyRayOrigins[leg].position, (a - leyRayOrigins[leg].position).normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, legLength, groundLayer))
        {
            return hit.point;
        }

        return null;
    }

    private bool CanStep(int leg)
    {
        int oppositeIndex = leg + leg % 2 == 0 ? 1 : -1;
        bool oppositeGrounded = groundedFoot[leg + oppositeIndex];

        bool adjacentAGrounded = leg + 2 < groundedFoot.Length && groundedFoot[leg + 2];
        bool adjacentBGrounded = leg - 2 >= 0 && groundedFoot[leg - 2];

        return oppositeGrounded && (adjacentAGrounded || adjacentBGrounded);

    }
    private IEnumerator Step(int leg, Vector3 to)
    {
        groundedFoot[leg] = false;
        float t = 0;

        Transform targ = legIkRigs[leg].data.target;

        Vector3 from = targ.position;

        while (t < 1)
        {
            targ.position = Vector3.Lerp(from, to, t);
            targ.Translate(transform.up * stepYCurve.Evaluate(t));

            t += Time.deltaTime / stepDuration;
            yield return null;
        }
        
        targ.position = targetWorldPositions[leg] = to;

        groundedFoot[leg] = true;
    }

    private Vector3 CalcHipRotor()
    {
        return Vector3.zero;
    }

    private Vector3 CalcHipElevation()
    {
        return Vector3.zero;
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < leyRayOrigins.Length; i++)
        {
            Vector3 a = transform.position + transform.rotation * initialTargetOffsets[i];

            Ray ray = new Ray(leyRayOrigins[i].position, (a - leyRayOrigins[i].position).normalized);

            Gizmos.color = i % 2 == 0 ? Color.green : Color.red;
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * legLength);
        }
    }
}
