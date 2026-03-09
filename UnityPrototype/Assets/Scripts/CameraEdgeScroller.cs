using UnityEngine;

namespace FinalProject.HexStrategy
{
    /// <summary>
    /// Moves the camera using edge scrolling + arrow keys and can enforce a strict top-down view.
    /// </summary>
    public class CameraEdgeScroller : MonoBehaviour
    {
        [Header("Pan")]
        [SerializeField] private float panSpeed = 12f;
        [SerializeField] private float edgeThickness = 20f;
        [SerializeField] private Vector2 xBounds = new Vector2(-30f, 30f);
        [SerializeField] private Vector2 zBounds = new Vector2(-30f, 30f);

        [Header("Top-Down View")]
        [SerializeField] private bool enforceTopDown = true;
        [SerializeField] private float fixedHeight = 22f;

        /// <summary>
        /// Applies top-down configuration once scene objects are initialized.
        /// </summary>
        private void Start()
        {
            ApplyTopDownView();
        }

        /// <summary>
        /// Re-applies top-down constraints after movement each frame.
        /// </summary>
        private void LateUpdate()
        {
            ApplyTopDownView();
        }

        /// <summary>
        /// Processes edge/keyboard input and pans camera on the map plane.
        /// </summary>
        private void Update()
        {
            var direction = Vector3.zero;
            var mouse = Input.mousePosition;

            if (mouse.x <= edgeThickness)
            {
                direction.x -= 1;
            }
            else if (mouse.x >= Screen.width - edgeThickness)
            {
                direction.x += 1;
            }

            if (mouse.y <= edgeThickness)
            {
                direction.z -= 1;
            }
            else if (mouse.y >= Screen.height - edgeThickness)
            {
                direction.z += 1;
            }

            direction.x += Input.GetAxisRaw("Horizontal");
            direction.z += Input.GetAxisRaw("Vertical");

            if (direction.sqrMagnitude <= 0.001f)
            {
                return;
            }

            direction.Normalize();
            transform.position += direction * (panSpeed * Time.deltaTime);

            var clamped = transform.position;
            clamped.x = Mathf.Clamp(clamped.x, xBounds.x, xBounds.y);
            clamped.z = Mathf.Clamp(clamped.z, zBounds.x, zBounds.y);
            transform.position = clamped;
        }

        /// <summary>
        /// Locks camera rotation/height for a true top-down perspective.
        /// </summary>
        private void ApplyTopDownView()
        {
            if (!enforceTopDown)
            {
                return;
            }

            var pos = transform.position;
            pos.y = fixedHeight;
            transform.position = pos;
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
