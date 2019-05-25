using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WavingMesh : MonoBehaviour
{
	public int _width;
	public int _height;

	public float _amplitude;
	public float _speed;
	public float _waveLength;
	public Material _cubeMaterial;

	int[] newTris;
	MeshFilter _meshFilter
	{
		get
		{
			return this.GetComponent<MeshFilter>();
		}
	}
	MeshRenderer _meshRenderer
	{
		get
		{
			return this.GetComponent<MeshRenderer>();
		}
	}
	// Start is called before the first frame update
	void Start()
	{
		InitialCube();
	}

	// Update is called once per frame
	void Update()
	{
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		for (int i = 0; i < _width * _height; i++)
		{
			float k = ((2 * Mathf.PI) / _waveLength);
			vertices[i] = new Vector3(vertices[i].x, _amplitude * Mathf.Sin(k * (i - _speed * Time.time)), vertices[i].z);
		}
		mesh.vertices = vertices;
	}
	void InitialCube()
	{
		Vector3[] vertices = new Vector3[_width * _height * 2];
		Vector3[] normals = new Vector3[vertices.Length];
		Vector2[] newUV = new Vector2[vertices.Length];
		newTris = new int[(_width - 1) * (_height - 1) * 6 * 4];
		int count = 0;
		for (int i = 0; i < _height; i++)
		{
			for (int j = 0; j < _width; j++)
			{
				vertices[count] = new Vector3(2 * j, 0, 2 * i);
				normals[count] = Vector3.up;
				newUV[count] = new Vector2((float)j / ((float)_width - 1), (float)i / ((float)_height - 1));
				count++;
			}
		}
		for (int i = 0; i < _height; i++)
		{
			for (int j = 0; j < _width; j++)
			{
				vertices[count] = new Vector3(2 * j, -3, 2 * i);
				normals[count] = -Vector3.up;
				newUV[count] = new Vector2((float)j / ((float)_width - 1), (float)i / ((float)_height - 1));
				count++;
			}
		}
		DrawTris();
		_meshFilter.mesh.vertices = vertices;
		_meshFilter.mesh.triangles = newTris;
		_meshFilter.mesh.uv = newUV;
		for (int i = 0; i < _meshFilter.mesh.uv.Length; i++)
		{
		}
		_meshRenderer.material = _cubeMaterial;
	}
	void DrawTris()
	{

		int index = 0;
		int _upperTrisCount = (_width - 1) * (_height - 1) * 6;
		for (int i = 0; i < _upperTrisCount; i += 6)
		{
			if ((index + 1) % _width != 0)
			{
				//Upper Tris
				newTris[i] = index;
				newTris[i + 1] = index + _width;
				newTris[i + 2] = index + _width + 1;
				//Lower Tris
				newTris[i + 3] = index;
				newTris[i + 4] = index + _width + 1;
				newTris[i + 5] = index + 1;
			}
			else
			{
				i -= 6;
			}
			index++;
		}
		index = _width * _height;
		for (int i = _upperTrisCount; i < _upperTrisCount * 2; i += 6)
		{
			if ((index + 1) % _width != 0)
			{
				//Upper Tris
				newTris[i] = index;
				newTris[i + 1] = index + _width + 1;
				newTris[i + 2] = index + _width;
				//Lower Tris
				newTris[i + 3] = index;
				newTris[i + 4] = index + 1;
				newTris[i + 5] = index + _width + 1;
			}
			else
			{
				i -= 6;
			}
			index++;
		}
		int _lowerStartIndex = _width * _height;
		index = _width * _height;
		for (int i = _upperTrisCount * 2; i < _upperTrisCount * 2 + _width * 6; i += 6)
		{
			if ((index + 1) % _width != 0)
			{
				//Upper Tris
				newTris[i] = index;
				newTris[i + 1] = index - _lowerStartIndex;
				newTris[i + 2] = index - (_lowerStartIndex - 1);
				//Lower Tris
				newTris[i + 3] = index;
				newTris[i + 4] = index - (_lowerStartIndex - 1);
				newTris[i + 5] = index + 1;
			}
			index++;
		}
		index = _width * _height + _width * (_height - 1);
		for (int i = _upperTrisCount * 2 + _width * 6; i < _upperTrisCount * 2 + _width * 6 * 2; i += 6)
		{
			if ((index + 1) % _width != 0)
			{
				//Upper Tris
				newTris[i] = index;
				newTris[i + 1] = index - (_lowerStartIndex - 1);
				newTris[i + 2] = index - _lowerStartIndex;
				//Lower Tris
				newTris[i + 3] = index;
				newTris[i + 4] = index + 1;
				newTris[i + 5] = index - (_lowerStartIndex - 1);
			}
			index++;
		}
		index = _width * _height + _width - 1;
		for (int i = _upperTrisCount * 2 + _width * 6 * 2; i < _upperTrisCount * 2 + _width * 6 * 2 + (_height - 1) * 6; i += 6)
		{
			//Upper Tris
			newTris[i] = index;
			newTris[i + 1] = index - _lowerStartIndex;
			newTris[i + 2] = index - _lowerStartIndex + _width;
			//Lower Tris
			newTris[i + 3] = index;
			newTris[i + 4] = index - _lowerStartIndex + _width;
			newTris[i + 5] = index + _width;
			index += _width;
		}
		
		index = _width * _height + _width;
		for(int i=_upperTrisCount * 2 + _width * 6 * 2 + (_height - 1) * 6;i < _upperTrisCount * 2 + _width * 6 * 2 + (_height - 1) * 6*2;i+=6)
		{
			//Upper Tris
			newTris[i] = index;
			newTris[i + 1] = index - _lowerStartIndex;
			newTris[i + 2] = index - _lowerStartIndex - _width;
			//Lower Tris
			newTris[i + 3] = index;
			newTris[i + 4] = index - _lowerStartIndex - _width;
			newTris[i + 5] = index - _width;
			index += _width;
		}
	}
}
