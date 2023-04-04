using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 10f;
    public float fireRate = 0.3f;//секунд между выстрелами
    public float health = 10;
    public int score = 100;//очки за уничтожение коробля
    public float showDamageDurantion = 0.1f;//длительность эффекта попадания
    public float powerUpDropChance = 1f;//вероятность сброса бонусов
    [Header("Set Dynamically Enemy")]
    public Color[] originalColors;
    public Material[] materials;
    public bool showingDanage = false;
    public float damageDoneTime;//время прекращения отображения эффекта
    public bool notifiedDestruction = false;
    protected BoundsCheck bndCheck;
    //это свойство, метод действует как поле
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }
    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        /*Вызывается новый метод Utils.GetAllMaterials() и заполняется массив
materials. Затем выполняется обход всех материалов и сохраняются их исходные
цвета. Несмотря на то что в настоящий момент все игровые объекты
Enemy окрашены в белый цвет, этот метод позволяет окрашивать их в любые
цвета по вашему выбору, подсвечивать их красным цветом при попадании
и затем возвращать исходный цвет.
Обратите внимание, что вызов Utils.GetAllMaterials() производится
в методе Awake(), а результат кэшируется в materials. То есть для каждого
экземпляра Enemy эта операция будет выполнена только один раз.
Utils. GetAllMaterials () использует довольно медленную функцию
GetComponentsInChildren<>(),*/
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }
    private void Update()
    {
        Move();
        if (showingDanage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
        if (bndCheck != null && bndCheck.offDown)/*Сначала проверяется наличие действительной ссылки в bndCheck. Если
подключить сценарий Enemy к игровому объекту, не имеющему компонента
сценария BoundsCheck, это первое условие не выполнится. Проверка выхода
игрового объекта за границы экрана (как определено в сценарии BoundsCheck)
выполняется только при условии bndCheck ! = null.*/
        {
            //уничтожить  когда он за нижней границей
            Destroy(gameObject);
        }
    }
    public virtual void Move()
    {/*Метод Move() получает текущее местоположение данного объекта Enemy_0,
перемещает его вниз, вдоль оси Y, и присваивает новые координаты свойству
pos (устанавливает местоположение игрового объекта).*/
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                //если вражеский корабль за границами экрана то не наносиь  ему повреждения
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                };
                //поразить вражеский корабль
                ShowDamage();
                //поучить разрушающую силу из WEAP_DICT в классе Main
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {//сообщить объекту об уничтожении
                    if (!notifiedDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedDestruction = true;
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
            default:
                print("Enemy hit by non-PrijectileHero: " + otherGO.name);
                break;
        }
    }
    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDanage = true;
        damageDoneTime = Time.time + showDamageDurantion;
    }
    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDanage = false;
    }
}
