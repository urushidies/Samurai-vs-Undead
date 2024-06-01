using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
[CreateAssetMenu]
public class DBCharacters : ScriptableObject
{
    public CharSelect[] character;

    public int CharacterCount
    {
        get
        {
            return character.Length;
        }
    }
    public CharSelect GetCharacter(int index)
    {
        return character[index];
    }
}
