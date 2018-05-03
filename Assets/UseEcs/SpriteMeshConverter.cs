using System;
using System.Linq;
using UnityEngine;

public static class SpriteMeshConverter
{
    public static Mesh SpriteToMesh(Sprite sprite)
    {
        var mesh = new Mesh();
        mesh.SetVertices(Array.ConvertAll(sprite.vertices, c => (Vector3)c).ToList());
        mesh.SetUVs(0, sprite.uv.ToList());
        mesh.SetTriangles(Array.ConvertAll(sprite.triangles, c => (int)c), 0);

        return mesh;
    }
}
