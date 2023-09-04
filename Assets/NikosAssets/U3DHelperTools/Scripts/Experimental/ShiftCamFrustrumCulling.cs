using UnityEngine;
using UnityEngine.Rendering;

namespace NikosAssets.Helpers.Experimental
{
    public class ShiftCamFrustrumCulling : BaseNotesMono
    {
        [SerializeField]
        protected Camera _cam;

        [SerializeField] 
        protected float _shiftBy = 2;
        
        [SerializeField]
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
        
        protected virtual void RenderPipelineManagerOnBeginCameraRendering(ScriptableRenderContext arg1, Camera arg2)
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
