using UnityEngine;
using System.Collections;
namespace UnityEngine.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class Empty4RayCast : MaskableGraphic
    {
        protected Empty4RayCast()
        {
            useLegacyMeshGeneration = false;
        }
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}