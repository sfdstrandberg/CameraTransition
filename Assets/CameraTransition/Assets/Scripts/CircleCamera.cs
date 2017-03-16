//
//  Camera dissolve effect. Copyright 2017 Sebastian Strandberg
//
//  This script is just for giving some motion to the cameras in the demo scene. Feel free to use it for whatever you want though.

using UnityEngine;

namespace CameraTransition.Demo
{
    public class CircleCamera : MonoBehaviour
    {
        /// <summary>
        /// The point to rotate around
        /// </summary>
        public Vector3 Origin;
        
        /// <summary>
        /// This distance from the point to rotate
        /// </summary>
        public float Radius = 5;
        
        /// <summary>
        /// The point to look towards
        /// </summary>
        public Vector3 FocalPoint;
        
        /// <summary>
        /// How fast to rotate
        /// </summary>
        public float SpinRate = 10;


        private float angle;

        //Draw the circle to make things a little easier to edit.
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < 360; i += 5)
            {
                var a = Origin + Quaternion.AngleAxis(i, Vector3.up) * Vector3.forward * Radius;
                var b = Origin + Quaternion.AngleAxis(i + 5, Vector3.up) * Vector3.forward * Radius;

                Gizmos.DrawLine(a, b);
            }

            Gizmos.DrawLine(Origin + Vector3.forward * Radius, FocalPoint);
        }


        void Update()
        {
            angle += Time.deltaTime * SpinRate;

            //Place the transform based on the current angle.
            transform.position = Origin + Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * Radius;
            transform.LookAt(FocalPoint);
        }
    }
}