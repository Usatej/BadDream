using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour {

    public string levelName;


    public void LoadLevel()
    {
        SceneManager.LoadScene(levelName);
    }
}
