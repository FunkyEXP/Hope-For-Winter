using UnityEngine;
using UnityEngine.UI;

namespace ExamineSystem
{
    public class ExamineRaycast : MonoBehaviour
    {
        [Header("Raycast Features")]
        [SerializeField] private float rayLength = 5;
        [SerializeField] private LayerMask layerMaskInteract;
        [SerializeField] private string layerToExclude = null;
        private ExamineItemController raycastedObj;

        [Header("Crosshair")]
        [SerializeField] private Image uiCrosshair = null;     
        [HideInInspector] public bool interacting = false;
        private bool isCrosshairActive;
        private bool doOnce;

        [SerializeField] private KeyCode openDoorKey = KeyCode.Mouse0;

        private const string pickupTag = "Pickup";
        private const string cabinDoor = "Cabin Door";
        private const string cabinWindow = "Cabin Window";

        private MyDoorController _doorObject;
        private WindowController _windowObject;

        void Update()
        {
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            int Mask = 1 << LayerMask.NameToLayer(layerToExclude) | layerMaskInteract.value;

            if (Physics.Raycast(transform.position, fwd, out hit, rayLength, Mask))
            {
                if (hit.collider.CompareTag(pickupTag))
                {
                    if (!interacting)
                    {
                        raycastedObj = hit.collider.gameObject.GetComponent<ExamineItemController>();
                        raycastedObj.MainHighlight(true);
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    interacting = true;

                    if (Input.GetKeyDown(ExamineInputManager.instance.interactKey))
                    {
                        raycastedObj.ExamineObject();
                    }
                }
            }

            else
            {
                if (isCrosshairActive)
                {
                    //raycastedObj.MainHighlight(false);
                    CrosshairChange(false);
                    interacting = false;
                }
            }

            if (Physics.Raycast(transform.position, fwd, out hit, rayLength, Mask))
            {
                if (hit.collider.CompareTag(cabinDoor))
                {
                    if (!interacting)
                    {
                        _doorObject = hit.collider.gameObject.GetComponent<MyDoorController>();
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    interacting = true;

                    if (Input.GetKeyDown(openDoorKey))
                    {
                        _doorObject.PlayAnimation();
                    }
                }
            }
            else
            {
                if (isCrosshairActive)
                {
                    CrosshairChange(false);
                    interacting = false;
                }
            }

            if (Physics.Raycast(transform.position, fwd, out hit, rayLength, Mask))
            {
                if (hit.collider.CompareTag(cabinWindow))
                {
                    if (!interacting)
                    {
                        _windowObject = hit.collider.gameObject.GetComponent<WindowController>();
                        CrosshairChange(true);
                    }

                    isCrosshairActive = true;
                    interacting = true;

                    if (Input.GetKeyDown(openDoorKey))
                    {
                        _windowObject.PlayWindowAnimation();
                    }
                }
            }
            else
            {
                if (isCrosshairActive)
                {
                    CrosshairChange(false);
                    interacting = false;
                }
            }

        }

        void CrosshairChange(bool on)
        {
            if (on && !interacting)
            {
                uiCrosshair.color = Color.red;
            }
            else
            {
                uiCrosshair.color = Color.white;
                isCrosshairActive = false;
            }
        }
    }
}
