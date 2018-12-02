using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class TextFileOperation 
{


	public static List<string> Read(string path)
	{
		if (string.IsNullOrEmpty (path))
			return null;
		List<string> Contents = new List<string> ();
		StreamReader sr = new StreamReader(path,Encoding.Default);
		string line;
		while ((line = sr.ReadLine()) != null) 
		{
			Contents.Add (line);
//			Debug.Log (line);
		}
		return Contents;
	}

	public static void Write(string path, string content)
	{
		FileStream fs = new FileStream(path, FileMode.Create);
		StreamWriter sw = new StreamWriter(fs);
		//开始写入
		sw.Write(content);
		//清空缓冲区
		sw.Flush();
		//关闭流
		sw.Close();
		fs.Close();
	}
}
