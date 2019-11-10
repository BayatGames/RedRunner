using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyGraphic : Graphic
{
    public override void SetMaterialDirty() { return; }
    public override void SetVerticesDirty() { return; }
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        return;
    }
}
