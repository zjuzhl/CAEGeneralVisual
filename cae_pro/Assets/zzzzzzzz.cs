using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zzzzzzzz : MonoBehaviour {


    

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButtonDown(0))
        {
           //从摄像机发出到点击坐标的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo)) 
            {
                Debug.Log(hitInfo.triangleIndex);
                MeshCollider mc = hitInfo.collider as MeshCollider;
                Mesh ms = mc.sharedMesh;

                Vector3[] vertices = ms.vertices;

                int[] triangles = ms.triangles;
                int tIndex = hitInfo.triangleIndex;
                //取回碰到的本地坐标点

                Vector3 p0 = vertices[triangles[tIndex * 3 + 0]];
                Vector3 p1 = vertices[triangles[tIndex * 3 + 1]];
                Vector3 p2 = vertices[triangles[tIndex * 3 + 2]];

                Debug.Log(p0.x + "  " + p0.y + "  " + p0.z);
                Debug.Log(p1.x + "  " + p1.y + "  " + p1.z);
                Debug.Log(p2.x + "  " + p2.y + "  " + p2.z);

            }

        }
	}
}
