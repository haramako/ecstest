using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Jobs;

public struct CharacterComponent : IComponentData {
    public float dir_;
    public float rand_;
}
