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
    private Transform[] legRayOrigins = null;

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

    [SerializeField]
    private float velocityForwardFactor = 0.1f;

    [SerializeField]
    private float velocityForwardCap = 0.02f;

    // store original target positions at start
    [SerializeField, HideInInspector]
    private Vector3[] targetRestPositions;

    [SerializeField, HideInInspector]
    private Vector3[] targetWorldPositions;

    private bool[] groundedFoot;
    private bool[] attachedFoot;
    private Vector3 velocity;
    private Vector3 positionLastFrame;

    private void OnValidate()
    {
        targetRestPositions = new Vector3[legIkRigs.Length];
        targetWorldPositions = new Vector3[legIkRigs.Length];

        for (int i = 0; i < legIkRigs.Length; i++)
        {
            var targ = legIkRigs[i].data.target;
            targetWorldPositions[i] = targ.position;

            targetRestPositions[i] = Quaternion.Inverse(transform.rotation) * (targ.position - transform.position);
        }
    }
    private void Start()
    {
        // base all non-serialized arrays on length of a serialized one

        positionLastFrame = transform.position;
        groundedFoot = Enumerable.Repeat(true, legIkRigs.Length).ToArray();
        attachedFoot = Enumerable.Repeat(false, legIkRigs.Length).ToArray();

        targetRestPositions = new Vector3[legIkRigs.Length];
        targetWorldPositions = new Vector3[legIkRigs.Length];

        for (int i = 0; i < legIkRigs.Length; i++)
        {
            var targ = legIkRigs[i].data.target;
            targetWorldPositions[i] = targ.position;

            targetRestPositions[i] = Quaternion.Inverse(transform.rotation) * (targ.position - transform.position);
        }
    }

    private void Update()
    {
        velocity = (transform.position - positionLastFrame) / Time.deltaTime;

        positionLastFrame = transform.position;

        for (int i = 0; i < legIkRigs.Length; i++)
        {
            var targ = legIkRigs[i].data.target;

            Vector3? want = FindFooting(i);

            if (want.HasValue)
            {
                // the purpose of attached array is that when a leg finds footing after being unable, it will step from the
                // last place the target was laft (could be anywhere), making a bad looking jump from random place

                // by keeping track of what ones are  attached and not, we check if we found footing whether we hadnt found it
                // last time, in which case immediately step, otherwise just follow normal step protocol
                if (!attachedFoot[i])
                {
                    targetWorldPositions[i] = want.Value;
                    attachedFoot[i] = true;
                }
                else if (CanStep(i) && (want.Value - targetWorldPositions[i]).sqrMagnitude > strideDist * strideDist)
                {
                    StartCoroutine(Step(i));
                }

                // smoothly add weight since we have somewhere to put the leg
                legIkRigs[i].weight = Mathf.Clamp01(legIkRigs[i].weight + Time.deltaTime / stepDuration);
            }
            else
            {
                // smoothly remove weight resort to normal animation clip if we have nowhere to put the leg
                legIkRigs[i].weight = Mathf.Clamp01(legIkRigs[i].weight - Time.deltaTime / stepDuration);

                if (attachedFoot[i])
                    attachedFoot[i] = false;

                // we wanna assume its grounded when it is in rest pose so other legs arent blocked from stepping
            }


            // make targets track their positions appropriately (as they are children so naturally want to follow spider)
            if (groundedFoot[i])
            {
                targ.position = targetWorldPositions[i];
            }
        }
    }

    private Vector3? FindFooting(int leg)
    {
        Vector3 restPosWorldSpace = transform.position + transform.rotation * targetRestPositions[leg];

        Vector3 velocityOffset = Vector3.ClampMagnitude(velocity * velocityForwardFactor, velocityForwardCap);
        Ray ray = new Ray(legRayOrigins[leg].position, (restPosWorldSpace - (legRayOrigins[leg].position - velocityOffset)).normalized);

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

    private IEnumerator Step(int leg)
    {
        groundedFoot[leg] = false;
        float t = 0;

        Transform targ = legIkRigs[leg].data.target;

        Vector3 from = targ.position;
        Vector3? to = FindFooting(leg);


        while (t < 1)
        {
            to = FindFooting(leg);
            if (!to.HasValue)
            {
                groundedFoot[leg] = true;
                yield break;
            }

            targ.position = Vector3.Lerp(from, to.Value, t);
            targ.Translate(transform.up * stepYCurve.Evaluate(t));

            t += Time.deltaTime / stepDuration;
            yield return null;
        }

        targ.position = targetWorldPositions[leg] = to.Value;

        groundedFoot[leg] = true;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < legRayOrigins.Length; i++)
        {
            Vector3 restPosWorldSpace = transform.position + transform.rotation * targetRestPositions[i];

            Vector3 velocityOffset = Vector3.ClampMagnitude(velocity * velocityForwardFactor, velocityForwardCap);
            Ray ray = new Ray(legRayOrigins[i].position, (restPosWorldSpace - (legRayOrigins[i].position - velocityOffset)).normalized);

            Color color = i % 2 == 0 ? Color.green : Color.red;
            color *= 1 - (float)i / legRayOrigins.Length;
            color.a = 1;

            Gizmos.color = color;

            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * legLength);
        }
    }
}
