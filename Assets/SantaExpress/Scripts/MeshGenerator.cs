using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * https://www.youtube.com/watch?v=64NblGkAabk
 */
namespace SantaExpress.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshGenerator : MonoBehaviour
    {
        public MeshGenerator prev;

        private Mesh _mesh;
        private int[] _triangles;
        private Vector3[] _vertices;

        private int _xSize;
        private int _zSize;
        private float _perlinMin;
        private float _perlinMax;
        private bool _isInitialPiece;

        public void SetPrev(MeshGenerator prev)
        {
            if (prev == default)
            {
                _isInitialPiece = true;
                return;
            }

            this.prev = prev;
        }

        public void SetSize(int xSize, int zSize)
        {
            _xSize = xSize;
            _zSize = zSize;
        }

        public void SetPerlin(float min, float max)
        {
            _perlinMin = min;
            _perlinMax = max;
        }

        private void Start()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
            CreateShape();
            UpdateMesh();
        }

        // Optionally, draw spheres at each vertex
        // private void OnDrawGizmos()
        // {
        //     if (_vertices == default)
        //         return;
        //     foreach (var v in _vertices)
        //         Gizmos.DrawSphere(v, 0.1f);
        // }

        private void CreateShape()
        {
            if (_isInitialPiece)
            {
                _vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];

                for (int i = 0, z = 0; z <= _zSize; z++)
                for (var x = 0; x <= _xSize; x++)
                {
                    var y = Mathf.PerlinNoise(x * .3f, z * .3f) * Random.Range(_perlinMin, _perlinMax);
                    _vertices[i] = new Vector3(x, y, z);
                    i++;
                }
            }
            else
            {
                var prevRightest = prev == default ? default : prev.GetRightests();

                _vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];

                for (int i = 0, z = 0; z <= _zSize; z++)
                for (var x = 0; x <= _xSize; x++)
                {
                    if (i % (_xSize + 1) == 0 && prev != default) // for continuous generation.
                    {
                        _vertices[i] = prevRightest[i / (_xSize + 1)];
                    }
                    else
                    {
                        var y = Mathf.PerlinNoise(x * .3f, z * .3f) * Random.Range(_perlinMin, _perlinMax);
                        _vertices[i] = new Vector3(_isInitialPiece ? x + 1 : prevRightest.First().x + x, y, z);
                    }

                    i++;
                }
            }

            _triangles = new int[_xSize * _zSize * 6];

            var vert = 0;
            var tris = 0;

            for (var z = 0; z < _zSize; z++)
            {
                for (var x = 0; x < _xSize; x++)
                {
                    _triangles[tris + 0] = vert + 0;
                    _triangles[tris + 1] = vert + _xSize + 1;
                    _triangles[tris + 2] = vert + 1;
                    _triangles[tris + 3] = vert + 1;
                    _triangles[tris + 4] = vert + _xSize + 1;
                    _triangles[tris + 5] = vert + _xSize + 2;

                    vert++;
                    tris += 6;
                }

                vert++;
            }
        }

        private void UpdateMesh()
        {
            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.normals = CalculateNormals();
           // _mesh.RecalculateNormals();
            // optionally, add a mesh collider (As suggested by Franku Kek via Youtube comments).
            // To use this, your MeshGenerator GameObject needs to have a mesh collider
            // component added to it.  Then, just re-enable the code below.
            /*
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        //*/
        }

        private Vector3[] CalculateNormals()
        {
            Vector3[] vertexNormals = new Vector3[_vertices.Length];
            int triangleCount = _triangles.Length / 3;
            for (int i = 0; i < triangleCount; i++)
            {
                int normalTriangleIndex = i * 3;
                int vertexIndexA = _triangles[normalTriangleIndex];
                int vertexIndexB = _triangles[normalTriangleIndex + 1];
                int vertexIndexC = _triangles[normalTriangleIndex + 2];
                var triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
                vertexNormals[vertexIndexA] += triangleNormal;
                vertexNormals[vertexIndexB] += triangleNormal;
                vertexNormals[vertexIndexC] += triangleNormal;
            }

            for (int i = 0; i < vertexNormals.Length; i++)
            {
                vertexNormals[i].Normalize();
            }

            return vertexNormals;
        }

        private Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
        {
            var pointA = _vertices[indexA];
            var pointB = _vertices[indexB];
            var pointC = _vertices[indexC];
            var sideAB = pointB - pointA;
            var sideAC = pointC - pointA;
            return Vector3.Cross(sideAB, sideAC).normalized;
        }

    public List<Vector3> GetRightests()
        {
            var result = new List<Vector3>();
            for (var i = _xSize; i < _vertices.Length; i += _xSize + 1)
                result.Add(_vertices[i]);
            return result;
        }
    }
}