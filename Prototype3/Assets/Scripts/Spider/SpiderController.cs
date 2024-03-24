/*
 * Credit:
 *   Scanner patterns innspired by procedural spider showcased here:
 *   https://www.youtube.com/watch?v=AhywDyu0EGw&t=47s
 */

using UnityEngine;


public class SpiderController : MonoBehaviour
{
    [Header("Root Position")]

    [SerializeField]
    private float movementSpeed = 10f;

    [SerializeField]
    private float movementScannerHeight;

    [SerializeField]
    private float movementScannerAngle;

    [SerializeField]
    private float movementScannerRange;

    [Header("Root Rotation")]

    [SerializeField, Tooltip("Speed at which the root rotation aligns with the scanned surface normal.")]
    private float rotateAdaptationSpeed = 10f;

    [SerializeField]
    private float inputRotationSpeed = 20f;

    [SerializeField]
    private float inputRotationInertia = 20f;

    [SerializeField, Range(8, 64)]
    private int surfaceScannerRayCount = 30;

    [SerializeField]
    private float surfaceScannerHeight = 0.2f;

    [SerializeField]
    private float surfaceScannerRange = 0.2f;

    [SerializeField]
    private float surfaceScannerAngle = 10f;

    [SerializeField]
    private float surfaceScannerOffsetA = 0.2f;

    [SerializeField]
    private float surfaceScannerOffsetB = 0.15f;

    [SerializeField]
    private float weightA = 1f;

    [SerializeField]
    private float weightB = 10f;

    [Header("Misc Scan")]

    [SerializeField]
    private LayerMask groundLayer = new LayerMask();


    [SerializeField, HideInInspector]
    private Ray[] rayCache;

    private Vector3 moveWaypoint;
    private float yawInertia;

    public Vector3 MoveInput { get; set; }

    public Transform checkpoint; // Assign the checkpoint transform in the Inspector
    private int lives = 3;


    private void OnValidate()
    {
        rayCache = new Ray[surfaceScannerRayCount];

        for (int i = 0; i < surfaceScannerRayCount; i++)
        {
            float arg = (i * Mathf.PI * 2) / surfaceScannerRayCount;

            float mod = i % 2 == 0 ? surfaceScannerOffsetB : surfaceScannerOffsetA;

            Vector3 offset = new Vector3(
                Mathf.Cos(arg) * mod,
                surfaceScannerHeight,
                Mathf.Sin(arg) * mod
                );

            Vector3 tangent = new Vector3(offset.z, 0, -offset.x);

            float rayAngle = i % 2 == 0 ? this.surfaceScannerAngle : -this.surfaceScannerAngle;

            Vector3 direction = Quaternion.AngleAxis(rayAngle, tangent) * -transform.up;

            rayCache[i] = new Ray(offset, direction * surfaceScannerRange);
        }
    }
    private void Start()
    {
        rayCache = new Ray[surfaceScannerRayCount];

        for (int i = 0; i < surfaceScannerRayCount; i++)
        {
            float arg = (i * Mathf.PI * 2) / surfaceScannerRayCount;

            float mod = i % 2 == 0 ? surfaceScannerOffsetB : surfaceScannerOffsetA;

            Vector3 offset = new Vector3(
                Mathf.Cos(arg) * mod,
                surfaceScannerHeight,
                Mathf.Sin(arg) * mod
                );

            Vector3 tangent = new Vector3(offset.z, 0, -offset.x);

            float rayAngle = i % 2 == 0 ? this.surfaceScannerAngle : -this.surfaceScannerAngle;

            Vector3 direction = Quaternion.AngleAxis(rayAngle, tangent) * -transform.up;

            rayCache[i] = new Ray(offset, direction * surfaceScannerRange);
        }
    }
    private void Update()
    {
        var scanData = ScanSurroundings();


        // rotation

        Vector3 up = scanData.averageSurfaceNormal.HasValue ? scanData.averageSurfaceNormal.Value : transform.up;

        Vector3 forward = Vector3.Cross(up, -transform.right);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forward, up), Time.deltaTime * rotateAdaptationSpeed );

        if (Mathf.Abs(MoveInput.x) > 0.01f)
            yawInertia = Mathf.Clamp(yawInertia + MoveInput.x * Time.deltaTime * inputRotationInertia, -1f, 1f);
        else
            yawInertia = Mathf.Sign(yawInertia) * Mathf.Clamp01(Mathf.Abs(yawInertia) - Time.deltaTime * inputRotationInertia);

        transform.Rotate(0, yawInertia * inputRotationSpeed * Time.deltaTime, 0, Space.Self);

        // movement

        if (Mathf.Abs(MoveInput.y) > 0.01f)
            moveWaypoint = scanData.surfaceWaypoint.HasValue ? scanData.surfaceWaypoint.Value : moveWaypoint;

        transform.position = Vector3.Lerp(transform.position, moveWaypoint, Time.deltaTime * movementSpeed);
    }

    private (Vector3? averageSurfaceNormal, Vector3? surfaceWaypoint) ScanSurroundings()
    {
        // get the waypoint for position to lerp to

        Vector3? waypoint = null;

        {
            Vector3 origin = transform.position + transform.up * movementScannerHeight;

            Vector3 direction = -transform.up;

            direction = Quaternion.AngleAxis(MoveInput.y * movementScannerAngle, transform.right) * direction;

            Ray ray = new Ray(origin, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, movementScannerRange, groundLayer))
            {
                waypoint = hit.point;
            }
        }


        // get the average normal for the rotation to slerp to

        Vector3? averagedNormal = null;

        {
            // scan a ring of rays aimed inward
            for (int i = 0; i < rayCache.Length; i++)
            {
                Ray groundScanRay = new Ray(transform.position + transform.rotation * rayCache[i].origin, transform.rotation * rayCache[i].direction);

                if (Physics.Raycast(groundScanRay, out RaycastHit hit, surfaceScannerRange, groundLayer))
                {
                    if (!averagedNormal.HasValue) averagedNormal = Vector3.zero;

                    averagedNormal += hit.normal * (i % 2 == 0 ? weightA : weightB);
                }

            }
            if (averagedNormal.HasValue)
                averagedNormal.Value.Normalize();
        }

        return (averagedNormal, waypoint);
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + transform.up * movementScannerHeight;

        Vector3 direction = -transform.up;

        direction = Quaternion.AngleAxis(MoveInput.y * movementScannerAngle, transform.right) * direction;

        Ray ray = new Ray(origin, direction);

        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(ray);

        for (int i = 0; i < rayCache.Length; i++)
        {
            ray = new Ray(transform.position + transform.rotation * rayCache[i].origin, transform.rotation * rayCache[i].direction);

            Gizmos.color = i % 2 == 0 ? Color.magenta : Color.blue;

            Gizmos.DrawRay(ray);

        }
    }

    public void HandleParticleCollision()
    {
        LoseLife();
        if (lives > 0)
        {
            // Respawn player at the checkpoint
            transform.position = checkpoint.position;
        }
        else
        {
            // Handle game over logic
            Debug.Log("Game Over");
        }
    }

    void LoseLife()
    {
        lives--;
        Debug.Log("Life lost. Remaining lives: " + lives);
    }
}
