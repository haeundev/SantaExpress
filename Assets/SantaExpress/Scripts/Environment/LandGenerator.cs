using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * https://www.youtube.com/watch?v=64NblGkAabk
 */
namespace SantaExpress.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    public class LandGenerator : MonoBehaviour
    {
        private LandGenerator _prev;
        private Mesh _mesh;
        private int[] _triangles;
        private Vector3[] _vertices;

        private int _xSize;
        private int _zSize;
        private float _perlinMin;
        private float _perlinMax;
        private bool _isInitialPiece;

        public void SetPrev(LandGenerator prev)
        {
            if (prev == default)
            {
                _isInitialPiece = true;
                return;
            }

            _prev = prev;
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

        public void Create()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
            CreateShape();
            UpdateMesh();
        }
        
        // private void OnDrawGizmos()
        // {
        //     if (_vertices == default)
        //         return;
        //     foreach (var v in _vertices)
        //         Gizmos.DrawSphere(v + transform.position, 0.1f);
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
                var prevRightest = _prev == default ? default : _prev.GetRightestVertices();
            
                //transform.localPosition = new Vector3(prevRightest.Last()..x, transform.localPosition.y, transform.localPosition.z);
            
                _vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];
            
                for (int i = 0, z = 0; z <= _zSize; z++)
                for (var x = 0; x <= _xSize; x++)
                {
                    if (i % (_xSize + 1) == 0 && _prev != default) // for continuous generation.
                    {
                        var rightest = prevRightest[i / (_xSize + 1)];
                        _vertices[i] = new Vector3(x, rightest.y, rightest.z);
                    }
                    else
                    {
                        var y = Mathf.PerlinNoise(x * .3f, z * .3f) * Random.Range(_perlinMin, _perlinMax);
                        _vertices[i] = new Vector3(x, y, z);
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
            
            if (_isInitialPiece)
                _mesh.RecalculateNormals();
            else
                RecalculateNormalsSeamless();
        }

        private void RecalculateNormalsSeamless()
        {
            var leftests = GetLeftestIndices();
            var prevRightests = _prev.GetRightestIndices();
            
            var midNormals = new List<Vector3>();
            var prevNormals = _prev._mesh.normals;
            _mesh.RecalculateNormals();
            var thisNormals = _mesh.normals;
            for (var i = 0; i < leftests.Count; i++) 
                midNormals.Add((prevNormals[prevRightests[i]] + thisNormals[leftests[i]]) / 2);
            
            var newNormalsForThis = new List<Vector3>();
            for (var i = 0; i < _mesh.normals.Length; i++)
                newNormalsForThis.Add(leftests.Contains(i) ? midNormals[i/(_xSize+1)] : _mesh.normals[i]);
            _mesh.SetNormals(newNormalsForThis);

            var newNormalsForPrev = new List<Vector3>();
            for (var i = 0; i < _prev._mesh.normals.Length; i++)
                newNormalsForPrev.Add(prevRightests.Contains(i) ? midNormals[i/(_xSize+1)] : _prev._mesh.normals[i]);
            _prev._mesh.SetNormals(newNormalsForPrev);
        }

        private List<Vector3> GetRightestVertices()
        {
            var result = new List<Vector3>();
            for (var i = _xSize; i < _vertices.Length; i += _xSize + 1)
                result.Add(_vertices[i]);
            return result;
        }
        
        private List<int> GetRightestIndices()
        {
            var result = new List<int>();
            for (var i = _xSize; i < _vertices.Length; i += _xSize + 1)
                result.Add(i);
            return result;
        }

        private List<int> GetLeftestIndices()
        {
            var result = new List<int>();
            for (var i = 0; i < _vertices.Length; i += _xSize + 1)
                result.Add(i);
            return result;
        }
        
        public Vector3 GetLastVerticePosition()
        {
            if (_vertices == default)
                return Vector3.zero;
            var temp = _vertices[_xSize] + transform.position;
            return new Vector3(temp.x, 0, temp.z);
        }
    }
}