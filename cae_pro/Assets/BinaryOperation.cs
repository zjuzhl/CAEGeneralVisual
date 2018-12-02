using UnityEngine;
using System.Collections;
using System.IO;


/// <summary>
/// Binary二进制文件的读写.
/// </summary>
public class BinaryOperation 
{
	#region Binary Write Module
	/// <summary>
	/// 创建二进制文件，返回BinaryWriter
	/// </summary>
	/// <returns>二进制写入流BinaryWriter.</returns>
	/// <param name="path">文件路径.</param>
	public static BinaryWriter BinaryWrite(string path)
	{
		if (string.IsNullOrEmpty (path)) {
			Debug.LogError ("path is nuul or empty.");
			return null;
		}
		BinaryWriter bw;
		try
		{
			bw = new BinaryWriter(new FileStream(path, FileMode.Create));
			return bw;
		}catch(IOException ioe){
			Debug.LogError (ioe.Message + " , Cannot create file.");
			return null;
		}
	}

	public static bool BinaryWriteInt(string path, int content)
	{
		BinaryWriter bw = BinaryWrite (path);
		if (bw == null) {
			return false;
		}
		try{
			bw.Write (content);
			return true;
		}catch(IOException ioe){
			Debug.LogError (ioe.Message + " , write error.");
			return false;
		}
	}

	public static bool BinaryWriteStr(string path, string content)
	{
		BinaryWriter bw = BinaryWrite (path);
		if (bw == null) {
			return false;
		}
		try{
			bw.Write (content);
			return true;
		}catch(IOException ioe){
			Debug.LogError (ioe.Message + " , write error.");
			return false;
		}
	}

	public static bool BinaryWriteBool(string path, bool content)
	{
		BinaryWriter bw = BinaryWrite (path);
		if (bw == null) {
			return false;
		}
		try{
			bw.Write (content);
			return true;
		}catch(IOException ioe){
			Debug.LogError (ioe.Message + " , write error.");
			return false;
		}
		bw.Close();
	}
	#endregion

	#region Binary Read Module
	/// <summary>
	/// Binaries打开二进制文件，准备读取，返回读取流
	/// </summary>
	/// <returns>T读取流BinaryReader</returns>
	/// <param name="path">文件路径.</param>
	public static BinaryReader BinaryRead(string path)
	{
		if (string.IsNullOrEmpty (path)) {
			Debug.LogError ("path is nuul or empty.");
			return null;
		}
		BinaryReader br;
		try
		{
			br = new BinaryReader(new FileStream(path, FileMode.Open));
			return br;
		}catch(IOException ioe){
			Debug.LogError (ioe.Message + " , Cannot create file.");
			return null;
		}
	}


	public static int BinaryReadInt(string path)
	{
		BinaryReader br = BinaryRead (path);
		int rst = br.ReadInt32();
		br.Close ();
		return rst;
	}
		
	public static string BinaryReadStr(string path)
	{
		BinaryReader br = BinaryRead (path);
		if (br == null) {
			return null;
		}
		try{
			string rst = br.ReadString();
			br.Close ();
			return rst;
		}catch(IOException ioe){
			Debug.LogError (ioe.Message + " , read error.");
		}
		br.Close ();
		return null;
	}

	public static bool BinaryReadBool(string path)
	{
		BinaryReader br = BinaryRead (path);
		bool rst = br.ReadBoolean();
		br.Close ();
		return rst;
	}
	#endregion

	public static void BinaryTest()
	{
		BinaryWriter bw;
		BinaryReader br;
		int i = 25;
//		int dd = 50;
		double d = 3.14157;
		bool b = true;
		string s = "I am happy \n \r\n你好世界";
		// 创建文件
		try
		{
			bw = new BinaryWriter(new FileStream("mydata.txt",
				FileMode.Create));
		}
		catch (IOException e)
		{
			Debug.Log(e.Message + "\n Cannot create file.");
			return;
		}
		// 写入文件
		try
		{
			bw.Write(i);
			bw.Write(d);
			bw.Write(b);
			bw.Write(s);
//			bw.Write(dd);
		}
		catch (IOException e)
		{
			Debug.Log(e.Message + "\n Cannot write to file.");
			return;
		}

		bw.Close();
		// 读取文件
		try
		{
			br = new BinaryReader(new FileStream("mydata.txt",
				FileMode.Open));
		}
		catch (IOException e)
		{
			Debug.Log(e.Message + "\n Cannot open file.");
			return;
		}
		try
		{
			i = br.ReadInt32();
			Debug.Log("Integer data: " + i);
			d = br.ReadDouble();
			Debug.Log("Double data: " +  d);
			b = br.ReadBoolean();
			Debug.Log("Boolean data: " +  b);
			s = br.ReadString();
			Debug.Log("String data: " +  s);
//			i = br.ReadInt32();
//			Debug.Log("Integer data: " + i);

			MemoryStream mr = new MemoryStream();


		}
		catch (IOException e)
		{
			Debug.Log(e.Message + "\n Cannot read from file.");
			return;
		}
		br.Close();
	}
}
