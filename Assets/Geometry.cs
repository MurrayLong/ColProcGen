using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public static class Geometry
    {
        public static Mesh Plane(int resolution) {
            Vector3[] verts = new Vector3[resolution * resolution];
            int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
            int t = 0;
            for (int i = 0; i < resolution * resolution; i++)
            {
                int x = i % resolution;
                int y = i / resolution;
                int meshMapIndex = y * resolution + x;

                Vector2 pos = new Vector2(x, y) / resolution - Vector2.one * 0.5f;
                verts[meshMapIndex] = new Vector3(pos.x, 0, pos.y);

                // Construct triangles
                if (x != resolution - 1 && y != resolution - 1)
                {
                    t = (y * (resolution - 1) + x) * 3 * 2;

                    triangles[t + 0] = meshMapIndex + resolution;
                    triangles[t + 1] = meshMapIndex + resolution + 1;
                    triangles[t + 2] = meshMapIndex;

                    triangles[t + 3] = meshMapIndex + resolution + 1;
                    triangles[t + 4] = meshMapIndex + 1;
                    triangles[t + 5] = meshMapIndex;
                    t += 6;
                }
            }

            var mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.vertices = verts;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }

    }
}
