using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseXSensitivity = 60f;
        public Transform playerBody;
        float xRotation = 0f;
        public GameObject crosshair;
        public PauseMenu pauseMenu; // Referência ao script PauseMenu

        // Start is called before the first frame update
        void Start()
        {
            LockCursor();
        }

        // Update is called once per frame
        void Update()
        {
            if (!pauseMenu.isPaused)
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
                    float mouseY = Input.GetAxis("Mouse Y") * mouseXSensitivity * Time.deltaTime;

                    xRotation -= mouseY;
                    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                    transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                    playerBody.Rotate(Vector3.up * mouseX);
                }

                if (Cursor.lockState == CursorLockMode.None && !Cursor.visible)
                {
                    crosshair.SetActive(false);
                    Cursor.visible = true;
                }
                else if (Cursor.lockState == CursorLockMode.Locked && Cursor.visible)
                {
                    crosshair.SetActive(true);
                    Cursor.visible = false;
                }
            }
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
