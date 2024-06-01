using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject BlackSamurai;
    public GameObject ArcherSamurai;
    public GameObject MainGuySamurai;

    private int characterFlag;

    private void Start()
    {
        characterFlag = PlayerPrefs.GetInt("selectedOption", 0);
        SelectCharacter();
    }

    public void SelectCharacter()
    {
        // Включаем/отключаем персонажей в зависимости от значения characterFlag
        switch (characterFlag)
        {
            case 0:
                BlackSamurai.SetActive(true);
                ArcherSamurai.SetActive(false);
                MainGuySamurai.SetActive(false);
                break;
            case 1:
                BlackSamurai.SetActive(false);
                ArcherSamurai.SetActive(true);
                MainGuySamurai.SetActive(false);
                break;
            case 2:
                BlackSamurai.SetActive(false);
                ArcherSamurai.SetActive(false);
                MainGuySamurai.SetActive(true);
                break;
            default:
                Debug.LogError("Invalid characterFlag value: " + characterFlag);
                break;
        }
    }
}