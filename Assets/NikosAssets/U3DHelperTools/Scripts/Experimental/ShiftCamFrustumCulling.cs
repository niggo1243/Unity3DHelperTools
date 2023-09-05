using UnityEngine;
using UnityEngine.Rendering;

namespace NikosAssets.Helpers.Experimental
{
    /// <summary>
    /// Experimental helper class to shift the culling matrix of a <see cref="Camera"/> Component.
    /// This can be useful if the tessellated and elevated terrain mesh gets visually culled too early in the view (mostly in the corner) 
    /// </summary>
    public class ShiftCamFrustumCulling : BaseNotesMono
    {
        /// <summary>
        /// The camera to shift the frustum culling matrix
        /// </summary>
        [SerializeField]
        [Tooltip("The camera to shift the frustum culling matrix")]
        protected Camera _cam;

        /// <summary>
        /// Shift the frustum culling matrix by... 
        /// </summary>
        [SerializeField]
        [Tooltip("Shift the frustum culling matrix by... ")]
        protected float _shiftBy = 2;
        
        /// <summary>
        /// Shift the frustum culling matrix along the given local axis
        /// </summary>
        [SerializeField]
        [Tooltip("Shift the frustum culling matrix along the given local axis")]
        protected Vector3 _alongLocalAxis = Vector3.forward;

        protected virtual void Start()
        {
            RenderPipelineManager.beginCameraRendering += RenderPipelineManagerOnBeginCameraRendering;
        }

        protected virtual void OnDestroy()
        {
            RenderPipelineManager.beginCameraRendering -= RenderPipelineManagerOnBeginCameraRendering;
        }

        protected virtual void OnDisable()
        {
            _cam.ResetCullingMatrix();
        }
        
        protected virtual void RenderPipelineManagerOnBeginCameraRendering(ScriptableRenderContext scriptableRenderContext, Camera camera)
        {
            OnPreCull();
        }

        protected virtual void OnPreCull()
        {
            _cam.ResetCullingMatrix();
            _cam.cullingMatrix *= Matrix4x4.Translate(_cam.transform.TransformDirection(_alongLocalAxis) * _shiftBy);
        }
    }
}
