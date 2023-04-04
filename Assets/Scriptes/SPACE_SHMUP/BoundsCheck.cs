using UnityEngine;
/// <summary>
/// предотврощает выход игрового обьекта за границы экраеа
/// Важно! работает только с ортографической каметой в 0 0 0
/// </summary>

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = true;/*keepOnScreen помогает определить режим работы сценария BoundsCheck — когда
он не позволяет игровому объекту выйти за границы экрана (true) и когда
позволяет, но уведомляет вас, что объект вышел за пределы экрана (false).*/
    [Header("Set Dynamically")]
    public bool isOnScreen = true;/*isOnScreen получает значение false, если игровой объект вышел за границы
экрана. Точнее, когда он миновал границу экрана и удалился от нее дальше,
чем на величину радиуса radius. Именно поэтому radius для Enemy_0 получил
значение -2.5, чтобы объект полностью скрылся с экрана, прежде чем
признак isOnScreen получит значение false.*/
    public float camWidth;
    public float camHeight;
    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;/*Здесь объявляются четыре переменные, по одной для каждой границы
экрана, которую пересек игровой объект, покинув экран. По умолчанию
всем им присваивается значение false. Строка [Hideininspector], предшествующая
этому объявлению, препятствует появлению этих четырех полей
в инспекторе даже при том, что они остаются общедоступными и могут
читаться (и изменяться) другими классами. Атрибут [Hideininspector]
применяется ко всем четырем переменным off__(то есть offRight, offLeft
и т. д.), потому что все они объявляются в одной строке под ним. Если бы
переменные off__объявлялись в четырех отдельных строках, нам пришлось
бы добавить атрибут [Hideininspector] перед каждой из них, чтобы
добиться того же эффекта.*/
    private void Awake()
    {/*Camera.main открывает доступ к первой камере с тегом MainCamera в сцене.
Далее, если камера ортографическая,. orthographicSize вернет число из поля
Size в инспекторе (в данном случае 40). То есть переменной camHeight будет
присвоено расстояние от начала мировых координат (позиция [ 0, 0, 0 ]) до
верхнего или нижнего края экрана.*/
        camHeight = Camera.main.orthographicSize;
        /*Camera. main. aspect — это отношение ширины к высоте поля зрения камеры,
как определяет отношение сторон панели Game (Игра) — в настоящий момент
Portrait (3:4). Умножив camHeight на .aspect, можно получить расстояние от
центра до левой или правой границы экрана.*/
        camWidth = camHeight * Camera.main.aspect;
    }
    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        if (pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            isOnScreen = false;/*Если условие в любой из этих четырех инструкций if выполняется, то
игровой объект оказался за пределами области, где он должен находиться.
Переменной isOnScreen присваивается значение false, а свойство pos корректируется
так, чтобы при необходимости легко можно было вернуть игровой
объект обратно «на экран».*/
            offRight = true;
        }
        if (pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            isOnScreen = false;
            offLeft = true;
        }
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
            isOnScreen = false;
            offUp = true;
        }
        if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
            isOnScreen = false;
            offDown = true;
        }
        isOnScreen = !(offRight || offLeft || offUp || offDown);
        if (keepOnScreen && !isOnScreen)/*Если keepOnScreen имеет значение true, то сценарий должен заставить игровой
объект оставаться в пределах экрана. Если keepOnScreen имеет значение
true и isOnScreen имеет значение false, то игровой объект покинул границы
экрана и его нужно вернуть назад. В этом случае в transform.position записывается
обновленное значение pos, соответствующее позиции на экране,
а переменной isOnScreen присваивается true, потому что игровой объект
только что вернулся на экран.
Если keepOnScreen имеет значение false, тогда значение pos не записывается
в transform.position — игровому объекту позволяется покинуть экран,
и переменная isOnScreen продолжает хранить false. Также возможна ситуация,
когда игровой объект оставался на экране все время, и в этом случае
isOnScreen сохранит значение true,*/
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }
    }
    //рисует границы
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
