using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager slManager;
    private PlayerController playerRef;
    private AIAgent aiRef;
    public int SavedScore = 0;
    const string fileName = "/defaultSaveSlot.dat";

    public void Awake()
    {
        if (slManager == null)
        {
            slManager = this;
            playerRef = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>();
            aiRef = GameObject.FindGameObjectWithTag("Enemy").GetComponentInChildren<AIAgent>();
        }
    }

    [Serializable]
    class GameSaveData
    {
        public float[] PlayerPosition;
        public float[] EnemyPosition;

        public int Score;
    };

    public void LoadSaveSlot()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.Open, FileAccess.Read);

            GameSaveData data = (GameSaveData)bf.Deserialize(fs);
            fs.Close();

            slManager.SavedScore = data.Score;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().setplayerscore(SavedScore);

            if (data.PlayerPosition != null)
            {
                slManager.playerRef.gameObject.transform.position = new Vector3(data.PlayerPosition[0], data.PlayerPosition[1], data.PlayerPosition[2]);
            }

            if (data.EnemyPosition != null)
            {
                slManager.aiRef.gameObject.transform.position = new Vector3(data.EnemyPosition[0], data.EnemyPosition[1], data.EnemyPosition[2]);            
            }
        }
    }

    public void SaveDefaultSlot()
    {
        SavedScore = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().getplayerscore();

        BinaryFormatter bf = new BinaryFormatter(); 
        FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.OpenOrCreate); 

        GameSaveData data = new GameSaveData();
        data.Score = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().getplayerscore();

        data.PlayerPosition = new float[]{ playerRef.transform.position.x, playerRef.transform.position.y, playerRef.transform.position.z };
        data.EnemyPosition = new float[]{ aiRef.transform.position.x, aiRef.transform.position.y, aiRef.transform.position.z };

        bf.Serialize(fs, data); 
        fs.Close();
    }
}