using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Toyo", menuName = "Toyo/Create new party")]
public class PartyReference : ScriptableObject
{
    public List<Toyo> partyMembers; // List of Pokemon objects representing the player's party
    public ToyoParty toyoParty; // List of Pokemon objects representing the player's party

}
