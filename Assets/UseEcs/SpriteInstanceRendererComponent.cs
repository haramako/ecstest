using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace toinfiniityandbeyond.Rendering2D
{
    [Serializable]
    public struct SpriteInstanceRenderer : ISharedComponentData
    {
        public Sprite Sprite;

        public SpriteInstanceRenderer(Sprite sprite)
        {
            this.Sprite = sprite;
        }
    }

    public class SpriteInstanceRendererComponent : SharedComponentDataWrapper<SpriteInstanceRenderer> { }
}
