using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idel,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;
    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitButton;
    public Vector3 castlePos;//место положение замка
    public GameObject[] castles;

    [Header("Set Dynamically")]
    public int level;//текущий уровень
    public int levelMax;//количество уровней
    public int shotsTaken;
    public GameObject castle;//текущий замок
    public GameMode mode = GameMode.idel;
    public string showing = "Show Slingshot";//режим followCam
    private void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    private void StartLevel()
    {
        //уничтожить прежний замок если он существует
        if (castle != null)
        {
            Destroy(castle);
        }
        //уничтожить прежние снаряды, если они существуют
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }
        //создать новый замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;
        //переустановить камеру в начальную позицию
        SwitchView("Show Both");
        ProjectileLine.S.Clear();
        //сбросить цель
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }
    private void UpdateGUI()
    {
        //показать данные в элементах ПИ
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }
    private void Update()
    {
        UpdateGUI();
        //проверитьзавершение уровня
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            //изменить режим чтобы прекратить проверку завершения уровня
            mode = GameMode.levelEnd;
            //уменьшить масштаб
            SwitchView("Shot Both");
            //начать новый уровень через 2 секундц
            Invoke("NextLevel", 2f);
        }
    }
    private void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }
    public void SwitchView(string eView = "")
    /*Общедоступный метод SwitchView() будет вызываться этим экземпляром
MissionDemolition и экземпляром Button пользовательского интерфейса
(который мы вскоре реализуем). Код string eView = определяет значение
по умолчанию "" для параметра eView, то есть, вызывая этот метод, можно
не передавать ему строку. Иными словами, SwitchView() можно вызвать
как SwitchView("Show Both”) или как SwitchView(). Если вызвать метод без
параметра, первая инструкция if в методе запишет в eView текущий текст
на кнопке Button.*/
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }
    //статик метод позволяющий из юбого кода увеличить shotTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
