using UnityEngine;
using System.Collections;

public class UVInfo : MonoBehaviour {

	[SerializeField]
	MeshFilter meshFilterTarget;

	[SerializeField]
	Mesh meshTarget;


	[SerializeField]
	Vector2[] uvDataTarget;

	// Use this for initialization
	void Start () {
		meshFilterTarget = GetComponent<MeshFilter>();
		meshTarget = meshFilterTarget.mesh;
		uvDataTarget = meshTarget.uv;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
