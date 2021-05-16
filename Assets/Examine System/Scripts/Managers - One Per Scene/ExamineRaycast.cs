using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;

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

        [SerializeField]
        private GameObject lastClicked;

        private const string pickupTag = "Pickup";
        private const string cabinDoor = "Cabin Door";
        private const string cabinWindow = "Cabin Window";
        private const string brokenFence = "Broken Fence";
        private const string fallenTree = "Fallen Tree";

        [SerializeField]
        private GameObject _fixedFence;

        private MyDoorController _doorObject;
        private WindowController _windowObject;

        private UIManager _uiManager;
        private SFXManager _sfx;

        private FirstPersonController _first;


        private void Start()
        {
            _uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();

            _first = GameObject.Find("FPSController").GetComponent<FirstPersonController>();

            _sfx = GameObject.Find("SFX Manager").GetComponent<SFXManager>();

            if(_uiManager == null)
            {
                Debug.LogError("UI Manager is null");
            }
        }

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
                else if (hit.collider.CompareTag(brokenFence))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        lastClicked = hit.transform.gameObject;
                        _uiManager.fixFence.SetActive(true);
                        _first.MouseUnlock();
                    }             
                }
                else if (hit.collider.CompareTag(fallenTree))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Hello");
                        lastClicked = hit.transform.gameObject;
                        _uiManager.clearTree.SetActive(true);
                        _first.MouseUnlock();
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

        public void FixFence()
        {
            _sfx.PlaySFX(0);
            StartCoroutine(WaitAndFix());
        }

        public void ClearTree()
        {
            _sfx.PlaySFX(1);
            StartCoroutine(WaitAndDestroy());
        }

        private IEnumerator WaitAndFix()
        {
            yield return new WaitForSeconds(5f);
            Instantiate(_fixedFence, lastClicked.gameObject.transform.position, lastClicked.gameObject.transform.rotation);
            Destroy(lastClicked.gameObject);
        }

        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(5f);
            Destroy(lastClicked.gameObject);
        }

    }
}
