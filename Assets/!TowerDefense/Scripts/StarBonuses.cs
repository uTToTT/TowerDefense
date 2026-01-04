using UnityEngine;

public class StarBonuses : MonoBehaviour
{
    private void ActivateBonuses()
    {
        int i = 0;

        while (true)
        {
            if (!PlayerPrefs.HasKey("StarBuff" + i))
            {
                break;
            }

            int trueInt = PlayerPrefs.GetInt("StarBuff" + i);

            if (trueInt == 1)
            {
                if (i == 0)
                {

                }
                if (i == 1)
                {

                }
                if (i == 2)
                {

                }
                if (i == 3)
                {

                }
            }

            i++;
        }
    }
}
