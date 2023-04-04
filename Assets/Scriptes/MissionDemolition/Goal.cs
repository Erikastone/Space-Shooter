using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;
    private void OnTriggerEnter(Collider other)
    {
        //йнцдю б накюярх деиярбхъ рпхцепю оноюдюер врн-рн
        //опнбепхрэ ъбкъеряъ кх щрн ямюпъднл
        if (other.gameObject.tag == "Projectile")
        {
            //еякх щрн ямюпъд опхябнхрэ TRUE
            Goal.goalMet = true;
            //рюйфе хглемхрэ юкэтю йюмюк жберю врнаш сбекхрэ меопнгпювмнярэ
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
