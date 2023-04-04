using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    static public int score = 1000;
    private void Awake()
    {
        //���� �������� HIGHTSCORE ��� ���������� � PLAYERPREFS, ��������� ���
        /*PlayerPrefs � ��� ������� ��������, �� ������� ����� ��������� ��
        ������ (�� ���� ���������� �������). � ���� ������ ������������ ����
        "HighScore". ��� ������ ��������� ������� �������������� �������� � ������
        "HighScore" � ��������� PlayerPrefs �, ���� ��� �������, ������ ���.
        */
        if (PlayerPrefs.HasKey("HighScore"))
        {
            score = PlayerPrefs.GetInt("HighScore");
        }
        //�������� ������ ����������� HIGHSCORE � ���������
        PlayerPrefs.SetInt("HighScore", score);
    }
    private void Update()
    {
        Text gt = this.GetComponent<Text>();
        gt.text = "High Score: " + score;
        //�������� HEIGHSCORE � PLAYERPREFS ���� ����������
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
}
