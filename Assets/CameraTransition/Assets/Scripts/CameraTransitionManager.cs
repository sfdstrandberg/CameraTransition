//
//  Camera dissolve effect. Copyright 2017 Sebastian Strandberg
//
//  This class is the one that coordinates effects and render texture targetting of cameras.

using System;
using UnityEngine;

namespace CameraTransition
{

    public class CameraTransitionManager : MonoBehaviour
    {
        /// <summary>
        /// This should point to Hidden/CameraFadeShader
        /// </summary>
        [HideInInspector]
        public Shader CameraFadeShader;

        //Are we currently fading?
        private bool fading;

        //Camera to fade from
        private Camera camA;
        //Camera to fade to
        private Camera camB;
        //Current progress on fading
        private float fadeFactor;
        //How quickly to fade
        private float fadeRate;

        //References to the rendertexture and effect for manipulation and cleanup.
        private RenderTexture rt;
        private CameraFadeEffect effect;


        /// <summary>
        /// Executes a dissolve fade between the given cameras.
        /// </summary>
        /// <param name="fadeFrom">The camera to fade from.</param>
        /// <param name="fadeTo">The camera to fade to.</param>
        /// <param name="time">The amount of time to fade over.</param>
        public void DissolveBetween(Camera fadeFrom, Camera fadeTo, float time)
        {
            //If we were already fading, clean up the last one
            if (fading) endFade();


            //Check that all the values are okay.
            if (fadeFrom == null) throw new ArgumentNullException("fadeFrom");
            if (fadeTo == null) throw new ArgumentNullException("fadeTo");
            if (Single.IsNaN(time)) throw new ArgumentOutOfRangeException("time");



            startFade(fadeFrom, fadeTo, time);
        }

        void Update()
        {
            if (fading)
            {
                fadeFactor += Time.deltaTime * fadeRate;
                //If fadefactor is over 1 we've finished fading and can clean up.
                if (fadeFactor > 1)
                {
                    endFade();
                }
                else
                {
                    ///Quadratic in/out easing is used because it makes the transition a lot smoother.
                    effect.FadeFactor = quadInOut(fadeFactor);
                }
            }
        }

        /// <summary>
        /// Set up the render texture and effect on the two cameras.
        /// </summary>
        void startFade(Camera fadeFrom, Camera fadeTo, float time)
        {
            fading = true;


            camA = fadeFrom;
            camB = fadeTo;

            fadeFactor = 0;
            //Make sure time is over zero.
            time = Mathf.Max(time, Mathf.Epsilon);
            fadeRate = 1 / time;

            //Set up the render texture for the camera we're fading from. 

            //Use a render texture format to match the camera's HDR state;
            var rtformat = camA.hdr ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
            //We want to match the current AA level if forward rendering is used, otherwise 1;
            var aalevel = camA.actualRenderingPath == RenderingPath.Forward ? QualitySettings.antiAliasing : 1;
            rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 24, rtformat, RenderTextureReadWrite.Default, aalevel);

            //Point camera A to render to the RT.
            camA.enabled = true;
            camA.targetTexture = rt;
            camB.enabled = true;
            
            //Add the image effect to camera B and set up the shader and rendertexture vars on it.
            effect = camB.gameObject.AddComponent<CameraFadeEffect>();
            effect.CameraFadeShader = CameraFadeShader;
            effect.FadeFrom = rt;
            effect.FadeFactor = 0;
        }

        /// <summary>
        /// Clean up the camera's used to fade from and to.
        /// </summary>
        void endFade()
        {
            fading = false;

            camA.enabled = false;
            camA.targetTexture = null;
            camB.enabled = true;
            Destroy(effect);

            RenderTexture.ReleaseTemporary(rt);
        }

        /// <summary>
        /// Just a simple quadratic in/out ease.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static float quadInOut(float t)
        {
            return t <= .5 ? t * t * 2 : 1 - (--t) * t * 2;
        }
    }
}