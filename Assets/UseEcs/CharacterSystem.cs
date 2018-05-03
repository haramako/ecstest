using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

public class CharacterSystem : JobComponentSystem
{
    [ComputeJobOptimization]
    struct PositionJob : IJobProcessComponentData<Position, CharacterComponent>
    {
        [ReadOnly]
        public NativeArray<float> HeightMap;

        public float dt;
        public float Time;
        //public Terrain Ground;

        float SampleHeight(float x, float z)
        {
            x = (x + 50) / 2f;
            z = (z + 50) / 2f;
            if (x < 0 || x >= 49 || z < 0 || z >= 49)
            {
                return 0;
            }
            else
            {
                var dx = math.mod(x, 1);
                var dz = math.mod(z, 1);
                var ix = (int)math.floor(x);
                var iz = (int)math.floor(z);
                var v00 = HeightMap[ix + iz * 50];
                var v01 = HeightMap[ix + 1 + iz * 50];
                var v10 = HeightMap[ix + (iz + 1) * 50];
                var v11 = HeightMap[ix + 1 + (iz + 1) * 50];
                return math.lerp(math.lerp(v00, v01, dx), math.lerp(v10, v11, dx), dz);
            }
        }

        public void Execute(ref Position position, ref CharacterComponent c)
        {
            var pos = position.Value;
            var h = SampleHeight(pos.x, pos.z);
            pos.y = h + 0.5f;

            var v = new float3(math.cos(c.dir_ / 180f * Mathf.PI), 0, math.sin(c.dir_ / 180f * Mathf.PI));
            var v2 = v * dt;
            pos.x += v2.x;
            pos.y += v2.y;
            pos.z += v2.z;

            var r = Mathf.PerlinNoise(c.rand_, Time * 0.02f);
            c.dir_ += r * dt * 80f;

            position.Value = pos;
        }
    }

    NativeArray<float> hm = new NativeArray<float>(50 * 50, Allocator.Persistent);

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var g = UseEcsManager.Instance.Ground;
        for (int x = 0; x < 50; x++)
        {
            for (int z = 0; z < 50; z++)
            {
                hm[x + z * 50] = g.SampleHeight(new Vector3(x * 2 - 50, 0, z * 2 - 50));
            }
        }
        var job = new PositionJob() { HeightMap = hm, Time = Time.time, dt = Time.deltaTime };
        return job.Schedule(this, 1024, inputDeps);
    }

    protected override void OnDestroyManager()
    {
        if (hm.IsCreated)
        {
            hm.Dispose();
        }
        base.OnDestroyManager();
    }
}
