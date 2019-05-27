using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;

public class GameManager : MonoBehaviour
{
    private Client client;

    public GameObject player;
    [HideInInspector]
    public GameObject playerOne;
    [HideInInspector]
    public GameObject playerTwo;

    private Transform spawnPoint1;
    private Transform spawnPoint2;

    Quaternion chara1Rot;
    Quaternion chara2Rot;

    GameObject canvas;
    Text pointText1;
    Text pointText2;
    Text winText;

    public int pOnePoints = 0;
    public int pTwoPoints = 0;

    public string connection;
    private IDbCommand dbCommand;
    private IDataReader reader;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        client = GameObject.FindGameObjectWithTag("Client").GetComponent<Client>();
        pointText1 = canvas.transform.GetChild(0).GetComponent<Text>();
        pointText2 = canvas.transform.GetChild(1).GetComponent<Text>();
        winText = canvas.transform.GetChild(2).GetComponent<Text>();

        winText.gameObject.SetActive(false);

        SpawnPlayers();

        connection = "URI=file:" + Application.dataPath + "/Database/database.s3db";
        UpdateScores(0, 50);
    }

    public void SpawnPlayers()
    {
        spawnPoint1 = GameObject.FindGameObjectWithTag("SpawnPoint1").transform;
        spawnPoint2 = GameObject.FindGameObjectWithTag("SpawnPoint2").transform;

        playerOne = Instantiate(player, spawnPoint1.position, Quaternion.identity);
        playerTwo = Instantiate(player, spawnPoint2.position, Quaternion.Euler
            (new Vector3(transform.rotation.x, transform.rotation.y - 180, transform.rotation.z)));

        if (client.id == 0)
        {
            playerTwo.transform.Find("Main Camera").gameObject.SetActive(false);
            playerTwo.GetComponent<PlayerClient>().enabled = false;
        }
        else if (client.id == 1)
        {
            playerOne.transform.Find("Main Camera").gameObject.SetActive(false);
            playerOne.GetComponent<PlayerClient>().enabled = false;
        }
    }

    public void UpdateUI(int id)
    {
        int score = 0;
        int score2 = 0;
        using (IDbConnection dbConnection = new SqliteConnection(connection))
        {
            dbConnection.Open();
            dbCommand = dbConnection.CreateCommand();
            string sqlQuery = string.Format("SELECT * FROM HighScore WHERE PlayerID = \"{0}\"", id);
            dbCommand.CommandText = sqlQuery;
            reader = dbCommand.ExecuteReader();

            while (reader.Read())
            {
                id = reader.GetInt32(0);
                score = reader.GetInt32(1);
                score2 = reader.GetInt32(1);
                Debug.Log(score);
                Debug.Log(score2);
            }

            FinishDatabaseAction();
        }

        pointText1.text = "PLAYER 1 POINTS: " + score;
        pointText2.text = "PLAYER 2 POINTS: " + score2;

        if (pOnePoints == 10)
            client.DeclareWinner("WINNER", true);
        if (pTwoPoints == 10)
            client.DeclareWinner("WINNER", false);
    }

    public void EndGame(int winner)
    {
        winText.gameObject.SetActive(true);

        if (winner == 0)
            winText.text = "PLAYER 1 WON.\nTo play again, please restart the application.";
        else
            winText.text = "PLAYER 2 WON.\nTo play again, please restart the application.";
    }

    public void UpdateScores(int id, int score)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connection))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string query = string.Format("UPDATE HighScore SET Score = \"{1}\" WHERE PlayerID= \"{0}\"", score, id);
                //string query = string.Format("INSERT INTO HighScore(PlayerID, Score) VALUES(\"{0}\", \"{1}\")", id, score);
                dbCmd.CommandText = query;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    private void DeleteScore()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connection))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string query = string.Format("DELETE FROM HighScore");
                dbCmd.CommandText = query;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    private void FinishDatabaseAction()
    {
        reader.Close();
        reader = null;

        dbCommand.Dispose();
        dbCommand = null;

        print("Finished");
    }


    private void OnDestroy()
    {
        DeleteScore();
    }
}
