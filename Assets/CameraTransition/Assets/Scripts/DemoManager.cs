//
//  Camera dissolve effect. Copyright 2017 Sebastian Strandberg
//
//  This is just a script that cycles through cameras in an array with a given interval between fades.

using UnityEngine;

namespace CameraTransition.Demo
{
    public class DemoManager : MonoBehaviour
    {
        public CameraTransitionManager CameraManager;
        public Camera[] Cameras;

        public float FadeTime = 2;
        public float TimeBetweenFades = 3;

        private float timer;
        private int cameraIndex;

        void Start()
        {
            //Don't start immediately fading
            timer = TimeBetweenFades;
        }

        void Update()
        {
            //Can't cycle with less than two cameras
            if (Cameras.Length < 2) return;

            timer -= Time.deltaTime;

            if(timer < 0)
            {
                timer = FadeTime + TimeBetweenFades;

                var currentCamera = Cameras[cameraIndex];
                cameraIndex = ++cameraIndex % Cameras.Length;
                var nextCamera = Cameras[cameraIndex];

                //Tell the camera manager to execute the fade effect.
                CameraManager.DissolveBetween(currentCamera, nextCamera, FadeTime);
                
            }
        }
    }
}