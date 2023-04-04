using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;//отступ для позиционирования
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster,
        WeaponType.spread, WeaponType.shield
    };
    private BoundsCheck bndCheck;
    public void ShipDestroyed(Enemy e)/*Массив powerUpFrequency типов бонусов WeaponType определяет частоту создания
бонусов каждого типа. По умолчанию он содержит два бластера, одну
веерную пушку и один банк энергии для защитного поля, поэтому бонусы
с бластерами будут появляться в два раза чаще, чем другие.
c. Метод ShipDestroyed() будет вызываться экземпляром вражеского корабля
в момент его разрушения. Иногда он будет создавать бонус на месте разрушенного
корабля.*/
    {
        //сгенерировать бонус с заданной вероятностью
        if (Random.value <= e.powerUpDropChance)/*Вражеские корабли всех типов будут иметь поле powerUpDropChance, хранящее
число между 0 и 1. Random.value — это свойство, генерирующее случайное
число типа float между 0 (включительно) и 1 (включительно). (Свойство
Random, value может вернуть любое из граничных значений, и 0, и 1.) Если
полученное случайное число меньше или равно powerUpDropChance, создается
новый экземпляр PowerUp. Поле powerUpDropChance будет объявлено в классе
Enemy, поэтому вражеские корабли разных типов могут иметь более высокую
или низкую вероятность оставить после себя бонус (например, корабли типа
Enemy_0 могли бы оставлять бонусы очень редко, а корабли Enemy_4 — всегда).
Сейчас эта строка подчеркнута красной волнистой линией в редакторе
MonoDevelop, потому что поле powerUpDropChance еще не добавлено в класс
Enemy.*/
        {
            //выбрать тип бонуса
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);/*После выбора типа бонуса вызывается метод SetTyре () созданного экземпляра
PowerUp, а тот, в свою очередь, устанавливает цвет, поле _type и отображает
соответствующую букву в TextMesh.*/
            //поместить в место где хранятся разрушенные каробли
            pu.transform.position = e.transform.position;
        }
    }
    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        //слова с ключами типа WeaponType
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;/*Этот цикл выполняет итерации по элементам массива weaponDefinitions
и создает для них соответствующие записи в словаре WEAP_DICT.*/
        }
    }
    public void SpawnEnemy()
    {
        //выбрать случайный шаблон
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);
        //разместить вражески корабль над экраном в случайной позиции х
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)/*Но если выбранный шаблон имеет компонент BoundsCheck, в роли отступа
используется радиус этого компонента. Иногда радиус может быть отрицательным,
чтобы игровой объект полностью скрылся с экрана, прежде чем
его поле isOnScreen получит значение false, как в случае с Enemy_0, поэтому
берется абсолютное значение радиуса.*/
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        //установить начальные координаты созданного вражеского корабля
        Vector3 pos = Vector3.zero;/*Этот фрагмент кода определяет начальное местоположение нового вражеского
корабля. Он использует компонент BoundsCheck объекта _MainCamera, чтобы
получить camWidth и camHeight, случайно выбирает координату X с условием,
чтобы вражеский корабль оказался в пределах левой и правой границ экрана,
и затем определяет координату Y так, чтобы вражеский корабль оказался
точно над верхней границей экрана.*/
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        SceneManager.LoadScene("SPACE SHMUP");
    }
    ///<summary>
    ///стачическая функция возвращающая WeapoDefinition из статического защищенного поля
    ///WEP_DICT класса Main.
    ///</summary>
    ///<returns>
    ///экземпляр WeaponDefinition или, если нет такого определения
    ///для указанного WeaponType возвращает новый экземпляр WeaponDefinition
    ///с типом none.
    ///</returns>
    ///<param name="wt">
    ///тип оружия WeaponType для которог  требуется получить
    ///WeaponDefinition
    ///</param>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        //проверить наличие указанного ключа в словаре
        //попытка извлечь значение по отсутствующему ключу вызовет ошибку
        //поэтому сл инструкция играет важную роль
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        //сл. инструкция возвращает новый экземпляр WeaponDefinition
        //с типом оружия WeaponType.none что означает неудачную попытку
        //найти требуемое определение WeaponDefinition
        return (new WeaponDefinition());
    }
}
