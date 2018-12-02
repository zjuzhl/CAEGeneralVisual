using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceDesign : MonoBehaviour {


	public Texture2D t2dBestView;
	public Texture2D t2dPrePosition;
	public Texture2D t2dPLAY;
	public Texture2D t2dSTOP;
	public Texture2D t2dREAGYPLAY;

	private Texture2D t2dMove;
	private bool bIsPlaying;

	CameraControl cc;


	private Mesh _meshA;
	private Mesh _meshB;

	private float speed;

	// Use this for initialization
	void Start () {
		t2dMove = t2dPLAY;
		bIsPlaying = false;
		cc = new CameraControl ();
//		_meshA = GameObject.Find ("Plane1").GetComponent<MeshFilter>().mesh;
//		_meshB = GameObject.Find ("Plane1").GetComponent<MeshFilter>().mesh;
//		for (int i = 0; i < _meshA.triangles.Length/3; i++) {
//			Vector3 temp = _meshA.vertices [_meshA.triangles [3 * i + 0]];
//			Debug.Log ("0: " + temp.x + "  " +temp.y + "  " + temp.z);
//			temp = _meshA.vertices [_meshA.triangles [3 * i + 1]];
//			Debug.Log ("1: " + temp.x + "  " +temp.y + "  " + temp.z);
//			temp = _meshA.vertices [_meshA.triangles [3 * i + 2]];
//			Debug.Log ("2: " + temp.x + "  " +temp.y + "  " + temp.z);
//		}
//		speed = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {

//		for (int i = 0; i < _meshA.vertices.Length; i++) {
//			Vector3 temp = _meshA.vertices [i];
//			if((temp.z - (-5))/10 )
//			temp = 
//		}
	}


	void OnGUI(){

	}
}
