using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Condition
{
    public ConditionID ID;
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }
    public Action<Toyo> OnAfterTurn { get; set; }
    public Action<Toyo> OnStart { get; set; }
    public Func<Toyo, bool> OnBeforeMove { get; set; }
    public List<StatBoost> StatBoosts { get; set; }
}
