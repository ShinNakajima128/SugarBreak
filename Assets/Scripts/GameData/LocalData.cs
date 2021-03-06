using UnityEngine;
using System.IO;

namespace SugarBreak
{
    class LocalData
    {
        /// <summary>
        /// ローカルファイルにデータを保存する
        /// </summary>
        /// <typeparam name="T"> データの型 </typeparam>
        /// <param name="file"> 保存先のファイル名 </param>
        /// <param name="data"> 現在のデータ </param>
        public static void Save<T>(string file, T data)
        {
            StreamWriter writer;
            var json = JsonUtility.ToJson(data);
            using (writer = new StreamWriter(Application.dataPath + "/" + file, false))
            {
                writer.Write(json);
                writer.Flush();
                writer.Close();
            }
        }

        public static T Load<T>(string file)
        {
            string datastr;
            StreamReader reader;
            try
            {
                using (reader = new StreamReader(Application.dataPath + "/" + file))
                {
                    datastr = reader.ReadToEnd();
                    reader.Close();
                }

                var gameData = JsonUtility.FromJson<T>(datastr); // ロードしたデータで上書き

                if (gameData != null)
                {
                    Debug.Log(gameData + "のデータをロードしました");

                    return gameData;
                }
                else
                {
                    return default;
                }
            }
            catch
            {
                Debug.Log("データを取得できませんでした。フォルダを作成します");
                string folderPath = Path.Combine(Application.dataPath, @"SaveData");
                Directory.CreateDirectory(folderPath);
                return default;
            }
        }
    }
}

