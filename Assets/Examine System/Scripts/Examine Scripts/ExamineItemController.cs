using UnityEngine;

namespace ExamineSystem
{
    public class ExamineItemController : MonoBehaviour
    {
        [Header("Camera Options")]
        [SerializeField] private Camera mainCamera = null;
        [SerializeField] private Transform examinePoint = null;

        [Header("Initial Rotation for objects")]
        [SerializeField] private Vector3 initialRotationOffset = new Vector3(0, 0, 0);

        [Header("Zoom Settings")]
        [SerializeField] private float initialZoom = 1f;
        [SerializeField] private Vector2 zoomRange = new Vector2(0.5f, 2f);
        [SerializeField] private float zoomSensitivity = 0.1f;

        [Header("Examine Rotation")]
        [SerializeField] private float horizontalSpeed = 5.0F;
        [SerializeField] private float verticalSpeed = 5.0F;

        [Header("Emissive Highlight")]
        [SerializeField] private bool showHighlight = false;

        [Header("Item UI Type")]
        [SerializeField] private UIType _UIType = UIType.None;

        [Header("Item Name")]
        public string itemName;

        [Header("Item Name Settings")]
        [SerializeField] private int textSize = 40;
        [SerializeField] private Font fontType = null;
        [SerializeField] private FontStyle fontStyle = FontStyle.Normal;
        [SerializeField] private Color fontColor = Color.white;

        [Space(5)] [TextArea] public string itemDescription;

        [Header("Item Descriptor Settings")]
        [SerializeField] private int textSizeDesc = 32;
        [SerializeField] private Font fontTypeDesc = null;
        [SerializeField] private FontStyle fontStyleDesc = FontStyle.Normal;
        [SerializeField] private Color fontColorDesc = Color.white;

        private Material thisMat;
        Vector3 originalPosition;
        Quaternion originalRotation;
        private Vector3 startPos;
        private bool canRotate;
        private float currentZoom = 1;
        private const string emissive = "_EMISSION";
        private const string mouseX = "Mouse X";
        private const string mouseY = "Mouse Y";
        private const string interact = "Interact";
        private const string examineLayer = "ExamineLayer";

        private ExamineRaycast raycastManager;

        public enum UIType { None, BasicLowerUI, RightSideUI }

        void Start()
        {
            initialZoom = Mathf.Clamp(initialZoom, zoomRange.x, zoomRange.y);

            originalPosition = transform.position;
            originalRotation = transform.rotation;
            startPos = gameObject.transform.localEulerAngles;

            thisMat = GetComponent<Renderer>().material; 
            thisMat.DisableKeyword(emissive);

            raycastManager = Camera.main.GetComponent<ExamineRaycast>();          
        }

        public void MainHighlight(bool isHighlighted) 
        {
            if (showHighlight)
            {
                if (isHighlighted)
                {
                    thisMat.EnableKeyword(emissive);
                }
                else
                {
                    thisMat.DisableKeyword(emissive);
                }
            }
        }

        /// <summary>
        /// Handles adjusting the zoom amount of the object
        /// </summary>
        /// <param name="value">The distance from the camera to position the object</param>
        /// <param name="moveSelf">Whether to move the actual object. If set to false the object may not move, but only the represented point.</param>
        private void MoveZoom(float value, bool moveSelf = true)
        {
            examinePoint.transform.localPosition = new Vector3(0, 0, value);

            if(moveSelf)
            {
                transform.position = examinePoint.transform.position;
            }
        }

        public void StopInteractingObject()
        {
            gameObject.layer = LayerMask.NameToLayer(interact);
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            ExamineDisableManager.instance.DisablePlayer(false);
            canRotate = false;

            switch (_UIType)
            {
                case UIType.None:
                    ExamineUIManager.instance.noUICloseButton.SetActive(false);
                    break;
                case UIType.BasicLowerUI:
                    ExamineUIManager.instance.basicItemNameUI.text = null;
                    ExamineUIManager.instance.basicExamineUI.SetActive(false);
                    break;
                case UIType.RightSideUI:
                    ExamineUIManager.instance.rightItemNameUI.text = null;
                    ExamineUIManager.instance.rightExamineUI.SetActive(false);
                    break;
            }
        }

        public void ExamineObject()
        {
            ExamineUIManager.instance.examineController = gameObject.GetComponent<ExamineItemController>();
            ExamineAudioManager.instance.Play("ExamineInteract");

            currentZoom = initialZoom; MoveZoom(initialZoom);

            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
            transform.Rotate(initialRotationOffset);

            ExamineDisableManager.instance.DisablePlayer(true);

            gameObject.layer = LayerMask.NameToLayer(examineLayer);
            thisMat.DisableKeyword(emissive);
            canRotate = true;

            switch (_UIType)
            {
                case UIType.None:
                    ExamineUIManager.instance.noUICloseButton.SetActive(true);
                    break;
                case UIType.BasicLowerUI:
                    ExamineUIManager.instance.basicItemNameUI.text = itemName;
                    ExamineUIManager.instance.basicItemDescUI.text = itemDescription;
                    TextCustomisation();
                    ExamineUIManager.instance.basicExamineUI.SetActive(true);
                    break;
                case UIType.RightSideUI:
                    ExamineUIManager.instance.rightItemNameUI.text = itemName;
                    ExamineUIManager.instance.rightItemDescUI.text = itemDescription;
                    TextCustomisation();
                    ExamineUIManager.instance.rightExamineUI.SetActive(true);
                    break;
            }
        }

        private void TextCustomisation()
        {
            switch (_UIType)
            {
                case UIType.BasicLowerUI:
                    ExamineUIManager.instance.basicItemNameUI.fontSize = textSize;
                    ExamineUIManager.instance.basicItemNameUI.fontStyle = fontStyle;
                    ExamineUIManager.instance.basicItemNameUI.font = fontType;
                    ExamineUIManager.instance.basicItemNameUI.color = fontColor;
                    ExamineUIManager.instance.basicItemDescUI.fontSize = textSizeDesc;
                    ExamineUIManager.instance.basicItemDescUI.fontStyle = fontStyleDesc;
                    ExamineUIManager.instance.basicItemDescUI.font = fontTypeDesc;
                    ExamineUIManager.instance.basicItemDescUI.color = fontColorDesc;
                    break;
                case UIType.RightSideUI:
                    ExamineUIManager.instance.rightItemNameUI.fontSize = textSize;
                    ExamineUIManager.instance.rightItemNameUI.fontStyle = fontStyle;
                    ExamineUIManager.instance.rightItemNameUI.font = fontType;
                    ExamineUIManager.instance.rightItemNameUI.color = fontColor;
                    ExamineUIManager.instance.rightItemDescUI.fontSize = textSizeDesc;
                    ExamineUIManager.instance.rightItemDescUI.fontStyle = fontStyleDesc;
                    ExamineUIManager.instance.rightItemDescUI.font = fontTypeDesc;
                    ExamineUIManager.instance.rightItemDescUI.color = fontColorDesc;
                    break;
            }         
        }

        void Update()
        {
            if (canRotate)
            {
                float h = horizontalSpeed * Input.GetAxis(mouseX);
                float v = verticalSpeed * Input.GetAxis(mouseY);

                if (Input.GetKey(ExamineInputManager.instance.rotateKey))
                {
                    gameObject.transform.Rotate(v, h, 0);
                }

                else if (Input.GetKeyDown(ExamineInputManager.instance.dropKey))
                {
                    StopInteractingObject();
                    raycastManager.interacting = false;
                }

                //Handle zooming
                bool zoomAdjusted = false;
                float scrollDelta = Input.mouseScrollDelta.y;
                if (scrollDelta > 0)
                {
                    currentZoom += zoomSensitivity;
                    zoomAdjusted = true;
                }
                else if (scrollDelta < 0)
                {
                    currentZoom -= zoomSensitivity;
                    zoomAdjusted = true;
                }

                if(zoomAdjusted)
                {
                    currentZoom = Mathf.Clamp(currentZoom, zoomRange.x, zoomRange.y);
                    MoveZoom(currentZoom);
                }
            }
        }

        private void OnDestroy()
        {
            Destroy(thisMat);
        }
    }
}