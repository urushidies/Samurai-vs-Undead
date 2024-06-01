using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CharManager : MonoBehaviour
{
    public DBCharacters characterDB;

    public SpriteRenderer artworkSprite;
    private int selectedOption = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            load();
        }
        UpdateCharacter(selectedOption);
    }
    public void NextOption()
    {
        selectedOption++;
        if(selectedOption >= characterDB.CharacterCount)
        {
            selectedOption = 0;
        }
        UpdateCharacter(selectedOption);
        Savename();
    }
    public void backOption()
    {
        selectedOption --;
        if (selectedOption <0)
        {
            selectedOption = characterDB.CharacterCount - 1;
        }
        UpdateCharacter(selectedOption);
        Savename();
    }

    private void UpdateCharacter(int selectedOption)
    {
        CharSelect character = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite;
    }
    private void load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
    private void Savename()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
    }
}