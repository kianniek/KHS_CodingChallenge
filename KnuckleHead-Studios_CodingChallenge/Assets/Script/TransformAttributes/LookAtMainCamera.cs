using System;
using UnityEngine;

namespace TransformAttributes
{
    public class LookAtMainCamera : MonoBehaviour
    {
        private Transform mainCamera;
        [SerializeField] private bool invertZ = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            mainCamera ??= Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (invertZ)
            {
                transform.LookAt(2 * transform.position - mainCamera.position);
                return;
            }
            transform.LookAt(mainCamera);
        }
    }
}