//
//  Camera dissolve effect. Copyright 2017 Sebastian Strandberg
//
//  This is the image effect for dissolving between cameras.

using UnityEngine;

namespace CameraTransition
{
    [ExecuteInEditMode]
    public class CameraFadeEffect : MonoBehaviour
    {
        /// <summary>
        /// The crossfade shader. This is automatically set by CameraFadeManager when calling FadeBetween() 
        /// </summary>
        public Shader CameraFadeShader;
        private Material material;

        /// <summary>
        /// The render texture to fade from. This is automatically set by CameraFadeManager when calling FadeBetween()
        /// </summary>
        public RenderTexture FadeFrom;

        /// <summary>
        /// Controls the blend of the final image. This is automatically set by CameraFadeManager when calling FadeBetween()
        /// </summary>
        [Range(0, 1)]
        public float FadeFactor;

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            //Recreate material if it doesn't exist
            if (material == null)
            {
                if (CameraFadeShader == null) return;
                material = new Material(CameraFadeShader);
                material.hideFlags = HideFlags.HideAndDontSave;
            }

            material.SetFloat("_FadeFactor", FadeFactor);

            //Blit additively onto the destination using different passes for (1 - fade) and (fade) multipliers.
            Graphics.Blit(source, destination, material, 0);
            Graphics.Blit(FadeFrom, destination, material, 1);
        }
    }
}