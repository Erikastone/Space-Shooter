using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    static public int score = 1000;
    private void Awake()
    {
        //ЕСЛИ ЗНАЧЕНИЕ HIGHTSCORE УЖЕ СУЩЕСТВУЕТ В PLAYERPREFS, ПРОЧИТАТЬ ЕГО
        /*PlayerPrefs — это словарь значений, на которые можно ссылаться по
        ключам (то есть уникальным строкам). В этом случае используется ключ
        "HighScore". Эта строка проверяет наличие целочисленного значения с ключом
        "HighScore" в хранилище PlayerPrefs и, если оно имеется, читает его.
        */
        if (PlayerPrefs.HasKey("HighScore"))
        {
            score = PlayerPrefs.GetInt("HighScore");
        }
        //СОХРАНИТ ВЫСШЕЕ ДОСТЬИЖЕНИЕ HIGHSCORE В ХРАНИЛИЩЕ
        PlayerPrefs.SetInt("HighScore", score);
    }
    private void Update()
    {
        Text gt = this.GetComponent<Text>();
        gt.text = "High Score: " + score;
        //ОБНОВИТЬ HEIGHSCORE В PLAYERPREFS ЕСЛИ НЕОБХОДИМО
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
}
