using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UnityEngine.UI;

public class WorldTimeAPI : MonoBehaviour
{
    public Text requestResult;
    public WorldTimeData timeData;
    
    void Start()
    {
        StartCoroutine(GetDateTimeOnline());
    }

    public IEnumerator GetDateTimeOnline() 
    {
        string url = "http://worldtimeapi.org/api/timezone/America/Sao_Paulo";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError) 
        {
            requestResult.text = "Erro de conexão: "+ www.error;
        }
        else 
        {
            string jsonDownloaded = www.downloadHandler.text;
            timeData = JsonUtility.FromJson<WorldTimeData>(jsonDownloaded);
            string utc = timeData.utc_datetime.Split('.')[0].Split("T")[1];
            requestResult.text = utc;
        }

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(GetDateTimeOnline());
    }

}
