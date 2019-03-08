using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class SaveLoad: MonoBehaviour {

	public static SaveLoad instance;
	
	private void Awake(){

		if (instance)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void LoadData()
	{
		string path = Application.persistentDataPath + "/saveFileJson.dat";

		if (File.Exists(path)) // Check if the file for load exists
		{
			StreamReader sr = new StreamReader(path);

			SavedLevels data = new SavedLevels();
			data = JsonUtility.FromJson<SavedLevels>(sr.ReadLine());

			sr.Close();



			print("File loaded from " + path);
		}

		// If the file does not exist
		else
		{
			print("File does not exist at " + Application.persistentDataPath);
		}
	}

	public void SaveData()
	{
		SaveLoad data = new SaveLoad();
	

		string path = Application.persistentDataPath + "/saveFileJson.dat";
		// FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		StreamWriter sw = new StreamWriter(path);       

		sw.WriteLine(JsonUtility.ToJson(data));

		sw.Close();

		print("File saved at " + path);
	}
}
[Serializable]
public class SavedLevels{
	public int unlockedLevels;
}