using UnityEngine;
/// <summary>
/// это перечисление всех возможных типов оружия
/// также вкючает тип "shield", что бы дать возможноть совершенствовать защиту
/// аббревиатурой [HP] ниже отмечены эементы которые не реализованы
/// </summary>
public enum WeaponType
{
    none,//по умолчанию нет оружия
    blaster,//бластер
    spread,//Веерная пушка, стреляющая несколькими снарядами
    phaser,//[HP]волновой фазер
    missile,//[HP]самоноводящиеся ракеты
    laser,//[HP]наносит повреждение при долговременном воздействии
    shield//увеичивает shielLevel
}
/// <summary>
/// класс WeaponDefinition позвояет настраивать свойства
/// конкретного вида оружия в инспекторы. Для класса Main
/// будет хранить массив элементов типа WeaponDefinition
/// </summary>
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;//буква на кубике изображающем бонус
    public Color color = Color.white;//цвет ствола оружияи кубика бонуса
    public GameObject projectilePrefab;//шаблон снаряда
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;//Разрушительная мощность
    public float continuosDamage = 0;//степень разрушения в секунду (для laser)
    public float delayBetweenShots = 0;
    public float velocity = 20;//скорость полета снаряда
}
public class Weapon : MonoBehaviour
{
    public static Transform PROJECTILE_ANCHOR;
    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer collarRend;
    private void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();
        //вызвать SetType() чтобы заменить тип оружия по умолчанию
        SetType(_type);/*В момент создания игровой объект Weapon вызовет метод SetType() и передаст
ему значение из поля WeaponType _type. Этот вызов скроет оружие
Weapon (если _type имеет значение WeaponType. попе) или окрасит его ствол
в соответствующий цвет (если _type имеет значение WeaponType. blaster или
WeaponType. spread).*/
        //динамически создать точку привязки дя всех снарядов
        if (PROJECTILE_ANCHOR == null)/*PRO3ECTILE_ANCHOR — это статический компонент Transform, который должен
играть роль родителя в иерархии для всех снарядов, создаваемых сценариями
Weapon. Если PROJECTILE_ANCHOR имеет значение null (то есть когда этот
компонент еще не создан), сценарий создает новый игровой объект с именем
_ProjectileAnchor и присваивает ссылку на его компонент Transform полю
PROJECTILE_ANCHOR.*/
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        //найти fireDelegate в корневом игровом обьекте
        GameObject rootGO = transform.root.gameObject;/*Оружие всегда подключается к другому игровому объекту (такому, как _Него).
Эта строка находит корневой игровой объект, к которому подключено это
оружие.*/
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }
    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }
    public void SetType(WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.none)/*Если type имеет значение WeaponType. none, этот игровой объект деактивируется.
Когда игровой объект неактивен, не вызывается ни один из его методов,
унаследованных от класса MonoBehaviour (например, Update (), LateUpdate (),
FixedUpdate(), OnCollisionEnter () и т. д.), он не участвует в моделировании
физики и не отображается в сцене. Однако сохраняется возможность вызывать
функции и изменять значения переменных из сценариев, подключенных
к неактивному игровому объекту, то есть если откуда-то из другого
места вызвать SetType() или присвоить свойству type значение WeaponType.
blaster или WeaponType. spread, метод SetType() активирует игровой объект,
к которому подключен.*/
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0;//сразу после установки _type можно выстрелить
    }
    public void Fire()
    {
        if (!gameObject.activeInHierarchy)/*gameObject.activelnHierarchy имеет значение false, если неактивен данный
экземпляр Weapon или неактивен или уничтожен игровой объект _Него
(корневой родитель для данного оружия). В любом случае, если gameObject.
activelnHierarchy имеет значение false, эта функция тут же возвращает
управление и выстрел не производится.*/
        {
            return;
        }
        //если между выстрелами прошло недостаточно много времени, выйти
        if (Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;/*Задается начальная скорость полета в направлении вверх, но если transform.
up.у < 0 (что возможно для вражеских кораблей, оружие которых обращено
вниз), компонент у скорости vel также обращается вниз.*/
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }
        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;
            case WeaponType.spread:/*Если _type имеет значение WeaponType. spread, создаются три разных снаряда.
Для двух из них производится поворот направления на 10 градусов относительно
оси Vector3. back (то есть оси Z, которая простирается от плоскости
экрана к вам). Затем их скорости Rigidbody.velocity устанавливаются
равными произведению угла поворота на vel*/
                p = MakeProjectile();//снаряд летящий прямо
                p.rigid.velocity = vel;
                p = MakeProjectile();//в право
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();//в лево
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
        }
    }
    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);/*Родителем игрового объекта Projectile назначается PROJECTILE ANCHOR. В результате
этого в панели Hierarchy (Иерархия) снаряд оказывается внутри
_ProjectileAnchor, что обеспечивает относительный порядок в ней и отсутствие
нагромождений из большого количества клонов Projectile. Аргумент true
указывает игровому объекту go, что дочерний объект должен сохранить свои
мировые координаты независимо от родителя.
р. Полю lastShotTime присваивается текущее время, что предотвращает возможность
повторного выстрела раньше, чем через def .delayBetweenShots
секунд.*/
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);
    }
}
