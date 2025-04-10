﻿/// <summary>
/// FileName: Blue_Action_CameraControl.cs
/// Author: Liu Huanqian
/// Created Time: 2015.10.28
/// Version: 1.0
/// Company: Sunnytech
/// Function: 获取主相机；
///
/// Changed By:
/// Modification Time:
/// Discription:
/// </summary>

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{

	private bool _switch = true;

	public Transform _refTrans = null;

//        private BlueFloat _rotateSpeed = new BlueFloat();
//        private BlueFloat _moveSpeed = new BlueFloat();
//        private BlueFloat _zoomSpeed = new BlueFloat();

    public float RotateSpeed = 250f;
    public float MoveSpeed = 8f;
    public float ZoomSpeed = 2f;

    //用于调节正交相机的缩放速率
    private float orthpgraphicRatio = 0.1f;

    public static float rotate_delta_x = 0.0f;
    public static float rotate_delta_y = 0.0f;
    private float move_delta_x = 0.0f;
    private float move_delta_y = 0.0f;
    private float zoom_delta_z = 0.0f;

    private float distance = 5f;

    private Quaternion rotation = Quaternion.identity;
    private Vector3 position = Vector3.zero;
    //private Ray ray;
    //private RaycastHit rayHit;
    //private Vector3 rayVec = new Vector3(0.5f, 0.5f, 0f);

	public Camera _camera = null;
//	public Camera _camera = null;
	private bool _rotateFlag = false;

	void Start()
	{
		
	}

	public void SetBestView(){
		Camera mycam = GameObject.Find ("Main Camera").GetComponent<Camera>();
		mycam.transform.position = new Vector3(25f,46.7f,0.3f);
		mycam.transform.rotation = Quaternion.Euler(90,0,0) ;
		mycam.orthographicSize = 60;
		_rotateFlag = false;
		try{

			if(GameObject.Find("Plane10")){
				GameObject.Find ("Canvas/Text1").GetComponent<Text>().enabled = true;

			}
			if(GameObject.Find("Plane20")){
				GameObject.Find ("Canvas/Text2").GetComponent<Text>().enabled = true;

			}
			GameObject.Find ("Canvas/Text3").GetComponent<Text>().enabled = true;
			GameObject.Find ("Canvas/Text4").GetComponent<Text>().enabled = true;
		}
		catch{
			
		}
	
	}

    void LateUpdate ()
    {
        if (_switch && _refTrans != null)
        {
            //中键旋转		
            if (Input.GetMouseButton(2) && !Input.GetMouseButton(1))
            {
				this.GetComponent<test> ().NonShowElementLine ();

                _refTrans.parent = null;
                if (!_rotateFlag)
                {
                    rotate_delta_x = _camera.transform.eulerAngles.y;
                    rotate_delta_y = _camera.transform.eulerAngles.x;
                }
                _rotateFlag = true;
                distance = ( _camera.transform.position - _refTrans.position ).magnitude;
                rotate_delta_x += Input.GetAxis("Mouse X") * RotateSpeed * 0.02f;
				rotate_delta_y -= Input.GetAxis("Mouse Y") * RotateSpeed * 0.02f;
                rotation = Quaternion.Euler(rotate_delta_y, rotate_delta_x, 0);
                position = rotation * new Vector3(0.0f, 0.0f, -distance) + _refTrans.position;
                _camera.transform.rotation = rotation;
                _camera.transform.position = position;
                _refTrans.parent = _camera.transform;
            }
            else
            {
                _rotateFlag = false;
            }

            //中键+右键平移	
            if (Input.GetMouseButton(2) && Input.GetMouseButton(1))
            {
				this.GetComponent<test> ().NonShowElementLine ();

                //ray = camera.ViewportPointToRay(rayVec);
                //if (Physics.Raycast(ray, out rayHit))
                //{
                //    distance = rayHit.distance;
                //}
                //RefTrans.position = ray.GetPoint(distance);
				move_delta_x = Input.GetAxis("Mouse X") * MoveSpeed;
				move_delta_y = Input.GetAxis("Mouse Y") * MoveSpeed;
                

                //镜头其实就是在平行地面的水平面上四处移动 所以构造一个变换矩阵 与世界坐标唯一的差异就是y轴旋转的角度
                //Quaternion rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                //把镜头要移动的分量乘以这个变化矩阵就是相对世界坐标的移动量啦 再加上镜头当前坐标 就是镜头新位置啦
                //transform.position = rotation * new Vector3(-delta_x, 0, -delta_y) + transform.position;
                //_refTrans.parent = _camera.transform;
                _camera.transform.Translate(new Vector3(-move_delta_x, -move_delta_y, 0) * Time.deltaTime, Space.Self);
            }

            //滚轮放大
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
				this.GetComponent<test> ().NonShowElementLine ();

                //用于计算鼠标当前指向的世界坐标点，暂时放弃
                //ray = camera.ScreenPointToRay(Input.mousePosition);
                //Vector3 distanceV = ray.origin;
                //Debug.Log("1: " + ray.origin.x + "," + ray.origin.y + "," + ray.origin.z);

                zoom_delta_z = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
                //直接用镜头的translate函数在镜头参考系Z方向上运动就可以啦
                if (!_camera.orthographic)
                {
                    _camera.transform.Translate(0, 0, -zoom_delta_z);
                    //再用delta_z修正镜头与视野中心点的距离 其实不记录也是可以的 每次都用sin算比较麻烦
                    distance += zoom_delta_z;
                }

                if (_camera.orthographic)
                {
                    if (_camera.orthographicSize > 0.01f || zoom_delta_z > 0)
                    {
                        _camera.orthographicSize += zoom_delta_z * _camera.orthographicSize * orthpgraphicRatio;
                    }
                    else
                    {
                        _camera.orthographicSize = 0.01f;
                    }
                    if (_camera.orthographicSize < 0.01f)
                    {
                        _camera.orthographicSize = 0.01f;
                    }
                }

                //用于计算缩放后鼠标所指向的世界坐标点
                //ray = camera.ScreenPointToRay(Input.mousePosition);
                //Debug.Log("2: " + ray.origin.x + "," + ray.origin.y + "," + ray.origin.z);

                //世界坐标点的位置变化
                //distanceV = ray.origin - distanceV;
                //Debug.Log("ha: " + distanceV.magnitude);
                ////屏幕中心点指向鼠标点的向量
                //Vector3 moveV = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);
                ////计算摄像机应平移的向量
                //if (zoom_delta_z < 0)
                //	moveV = -moveV.normalized * distanceV.magnitude;
                //else
                //	moveV = moveV.normalized * distanceV.magnitude;
                ////Debug.Log(moveV.x + "," + moveV.y);
                ////摄像机平移
                //camera.transform.Translate(new Vector3(moveV.x * distance / (distance - zoom_delta_z), moveV.y * distance / (distance - zoom_delta_z), 0), Space.Self);

                //ray = camera.ViewportPointToRay(rayVec);
                //if (Physics.Raycast(ray, out rayHit))
                //{
                //    distance = rayHit.distance;
                //}
                //RefTrans.position = ray.GetPoint(distance);
            }
        }
    }
}