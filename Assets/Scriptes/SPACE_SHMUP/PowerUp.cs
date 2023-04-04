using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    //Vector2.x хранит минимальное значение а у максимальное для метода рандом
    public Vector2 rotMinMaax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f;
    public float fadeTime = 4f;
    [Header("Set Dynamically")]
    public WeaponType type; // Тип бонуса
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;//скорость вращения
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;
    private void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();
        //выбрать случайную скорость
        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;
        //установить угол поворогта равным R:[0,0,0]
        transform.rotation = Quaternion.identity;
        //выбрать случайную скорость вращения для куба
        rotPerSecond = new Vector3(Random.Range(rotMinMaax.x, rotMinMaax.y),
            Random.Range(rotMinMaax.x, rotMinMaax.y),
            Random.Range(rotMinMaax.x, rotMinMaax.y));
        birthTime = Time.time;
    }
    private void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        //эффект растворения куба с течением времени
        //по умолчанию бонус существует 10 секунд
        //расворяется 4
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        //в течении lifeTime секунд значение u будет <=0 затем оно станет 
        //положительным и через fadeTime станет боьше 1
        //если u>=1 уничтожить бонус
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }
        //использовать u для опеределения альфа значения куба и буквы
        if (u > 0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;
            //буква тоже роастворяется но медленнее
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }
        if (!bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
    }
    public void SetType(WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        cubeRend.material.color = def.color;//установить цвет куба
        letter.text = def.letter;//установить отображаемую букву
        type = wt;
    }
    public void AbsorderBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
