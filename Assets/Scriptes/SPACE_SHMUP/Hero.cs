using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;
    [Header("Set in Inspector")]
    //поля управляющие движением коробля
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;
    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;
    //хранит ссылку на поседний столкнувшийся игровой обьект
    private GameObject lastTriggerGO = null;
    //объявление делегата
    public delegate void WeaponFireDelegate();
    //создание поля типа
    public WeaponFireDelegate fireDelegate;
    private void Start()
    {
        S = this;
        // fireDelegate += TempFire;
        //очистить массив weapons и начать игру с 1 бластером
        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }
    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        //изменить transform.position опираясь на информацию по осям
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;
        //повернуть кораблю чтобы придать ощущуение динамизма
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
        //позволит кораблю выстрелить
        // if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TempFire();
        //}
        //Произвести выстред из всех видов оружия вызовм fireDelegate
        //сначала проверить нажатие клавиши
        //затем убедится что значение не null чтобы избежать ошибки
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }
    void TempFire()
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        // rigidB.velocity = Vector3.up * projectileSpeed;
        Projectile proj = projGO.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        // print("Triggered: " + go.name);
        //гарантировать невозможность повторного столкновения с тем же обьектом
        if (go == lastTriggerGO)/*Если lastTriggerGo ссылается на тот же объект, что и go (текущий игровой
объект, столкнувшийся с кораблем игрока), это столкновение игнорируется
как повторное, и функция просто возвращает управление (то есть
завершается). Такое может случиться, если два дочерних игровых объекта
одного и того же родителя Enemy столкнутся с коллайдером _Него в одном
кадре.*/
        {
            return;
        }
        lastTriggerGO = go;/*Ссылка из go копируется в lastTriggerGo для обновления перед следующим
вызовом OnTriggerEnter().*/
        if (go.tag == "Enemy")
        {
            //если защитное поле столкнулось с вражеским кораблем 
            //уменьшить уровень защиты на 1
            //и уничтожить врага
            shieldLevel--;
            Destroy(go);/*go — игровой объект вражеского корабля — уничтожается при столкновении
с защитным полем. Так как go хранит ссылку на фактический игровой объект
Enemy, полученную обращением к transform, root, эта операция удалит
весь вражеский корабль (вместе с его дочерними объектами), а не только тот
дочерний объект, что столкнулся с полем.*/
        }
        else if (go.tag == "PowerUp")
        {
            //если защитное поле сталкнулось с бонусом
            AbsorPowerUp(go);
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);
        }
    }
    public void AbsorPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                break;
            default:
                if (pu.type == weapons[0].type)
                {
                    //если оружие тогоже типа
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                    else
                    {
                        //если другого типа
                        ClearWeapons();
                        weapons[0].SetType(pu.type);
                    }
                }
                break;
        }
        pu.AbsorderBy(this.gameObject);
    }
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);
        }
        set
        {/*Mathf.Min() гарантирует, что _shieldLevel никогда не получит значение
выше 4.
c. Если значение value, переданное в метод записи set, меньше 0, объект _Него
уничтожается.
            */
            _shieldLevel = Mathf.Min(value, 4);
            //если уровень поля упал до нуля или нижу
            if (value < 0)
            {
                Destroy(this.gameObject);
                //сообщить обьекту main.S о необходимости перезапуска игры
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }
    void ClearWeapons()
    {
        foreach (Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
}
