using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Text scoreGT;
    private void Start()
    {
        //ПОЛУЧИТЬ ССЫЛКУ НА ИГРОВОЙ ОБЬЕКТ SCORECOUNTER
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        //ПОЛУЧИТТЬ КОМПОНЕНТ TEXT ЭТОГО ИГРОВОГО ОБЬЕКТА
        scoreGT = scoreGO.GetComponent<Text>();
        //УСТАНОВИТЬ НАЧАЛЬНОЕ ЧИСЛО ОЧКОВ РАВНЫМ 0
        scoreGT.text = "0";
    }
    private void Update()
    {
        //ПОЛУЧИТЬ ТЕКУЩИЕ КООРДИНАТЫ УКАЗАТЕЛЯ МЫШИ НА ЭКРАНЕ ИЗ INPUT
        /*Переменной mousePos2D присваивается значение Input.mousePosition. Это
        экранные координаты, то есть расстояние в пикселах от левого верхнего
        угла экрана до указателя мыши. Координата Z в Input.mousePositon всегда
        равна 0, потому что экран, по сути, это двумерная плоскость.
        */
        Vector3 mousePos2D = Input.mousePosition;

        //КООРДИНАТА Z КАМЕРЫ ОПРЕДЕЛЯЕМ, КАК ДАЛЕКО В ТРЕХМЕРНОМ ПРОТРАНСТВЕ
        //НАХОДИТСЯ УКАЗАТЕЛЬ МЫШИ
        /*Эта строка присваивает координате Z в mousePos2D значение координаты Z
        главной камеры с обратным знаком. В игре координата Z главной камеры
        равна -10, соответственно mousePos2D. z получит значение 10. Тем самым мы
        сообщаем последующему вызову функции ScreenToWorldPoint(), как далеко
        от камеры должна находиться точка mousePos3D в трехмерном пространстве,
        фактически помещая ее на плоскость Z=0.
        */
        mousePos2D.z = -Camera.main.transform.position.z;

        //ПРЕОБРАЗОВАТЬ ТОЧКУ НА ДВУМЕРНОЙ ПРОСКОСТИ ЭКРАНА В 
        //ТРЕХМЕРНЫЕ КООРДИНАТЫ ИГРЫ
        /*ScreenToWorldPoint () преобразует экранные координаты mousePoint2D в координаты
        в трехмерном игровом пространстве. Если значение mousePos2D. z
        оставить равным 0, точка mousePos3D получит координату Z, равную -10
        (координата Z главной камеры). Присвоив mousePos2D.z значение 10, мы
        поместили точку mousePos3D в трехмерном пространстве на удалении 10
        метров от главной камеры, благодаря чему поле mousePos3D. z получило значение
        0. В игре Apple Picker это не имеет большого значения, но в будущих
        играх координата Z указателя мыши будет играть более важную роль.
        */
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //ПЕРЕМЕСТИТЬ КОРЗИНУ ВДОЛЬ ОСИ Х В КООРДИНАТУ Х УКАЗАТЕЛЯ МЫШИ
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;
    }
    private void OnCollisionEnter(Collision coll)
    {
        //ОТЫСКАТЬ ЯБЛОКО, ПОПАВШЕЕ В ЭТУ КОРЗИНУ
        /*Эта строка присваивает локальной переменной collidedWith ссылку на
        игровой объект, столкнувшийся с корзиной.
        */
        GameObject colldeWith = coll.gameObject;
        if (colldeWith.tag == "Apple")
        {
            Destroy(colldeWith);
        }
        //ПРЕОБРАЗОВАТЬ ТЕКСТ В SCOREGT В ЦЕЛОЕ ЧИСЛО
        int score = int.Parse(scoreGT.text);
        //ДОБАВИТЬ ОЧКИ ЗА ПОЙМОННОЕ ЯБЛОКО
        score += 100;
        //ПРЕОБРАЗОВАТЬ ЧИСЛО ОЧКОВ ОБРАТНО В СТРОКУ И ВЫВЕСТИ ЕЕ НА ЭКРАН
        scoreGT.text = score.ToString();
        //ЗАПОМНИТЬ ВЫСШЕЕ ДОСТИЖЕНИЕ
        if (score > HighScore.score)
        {
            HighScore.score = score;
        }
    }
}
