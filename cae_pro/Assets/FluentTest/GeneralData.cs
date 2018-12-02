using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralData {

	public string ModelName;
	//考虑根据物理量支持可扩展
	//节点信息
	public int NodeNumber;//节点总数
	public int NodeInnerNumber;//内部节点总数
	public int NodeOutterNumber;//表面节点总数
	//节点坐标
	public List<double[]> NodeCoord;//节点坐标
    public List<double[]> NodeOffPos;//节点坐标(形变量)
	//单元信息
	public int ElemNumber;//单元总数
	public int ElemInnerNumber;//内部单元总数
	public int ElemOutterNumber;//表面单元总数
	public List<int[]> Elem_NodesIndex;//单元的节点组成编号
	//节点解
	public List<string> nSlnTypeNames;//物理量名称
	public List<double>[] nSlnTypeValue;//物理量结果(标量)
	public List<double[]>[] nSlnTypeValues;//物理量结果（矢量）
	public double[] nSlnMaxValue;//物理量结果最大值（在标量情况下）
	public double[] nSlnMinValue;//物理量结果最小值（在标量情况下）
	//单元解
	public List<string> eSlnTypeNames;//物理量名称
	public List<double> eSlnTypeValue;//物理量结果(标量)
	public List<double[]> eSlnTypeValues;//物理量结果（矢量）
	public double[] eSlnMaxValue;//物理量结果最大值（在标量情况下）
	public double[] eSlnMinValue;//物理量结果最小值（在标量情况下）

    //特征值
    //最大值
    //最小值

    

}
