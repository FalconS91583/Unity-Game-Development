using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string fullPath;
    private bool encryptData;
    private string codeWord = "QueenShadow";

    public FileDataHandler(string dataDirPath, string dataFileName, bool encryptData)
    {
        fullPath = Path.Combine(dataDirPath, dataFileName);
        this.encryptData = encryptData;
    }

    public void SaveData(GameData gameData)
    {
        try
        {
            // Create Directiotry if doesnt exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Convert to JSON
            string dataToSave = JsonUtility.ToJson(gameData, true);

            // Encrypt the data 
            if(encryptData)
                dataToSave = EncryptDecrypt(dataToSave);

            //Create/Open a new file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                // Wrote JSON to text file
                using (StreamWriter write = new StreamWriter(stream))
                {
                    write.Write(dataToSave);
                }
            }
        }

        catch (Exception e)
        {
            Debug.LogError("Error on traying to save data to file" + fullPath + "\n" + e);
        }
    }

    public GameData LoadData()
    {
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (encryptData)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }

            catch (Exception e)
            {
                Debug.LogError("Error on loading data form file: " + fullPath + "\n" + e);
            }
        }

        return loadData;
    }

    public void DeleteData()
    {
        if(File.Exists(fullPath))
            File.Delete(fullPath);
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }

        return modifiedData;
    }
}

