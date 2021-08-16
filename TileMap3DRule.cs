using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap3DRule
{
    public Dictionary<Vector3Int, int> rule;
    
    public  TileMap3DRule()
    {
        rule = new Dictionary<Vector3Int, int>();
    }

    public TileMap3DRule(RuleType type)
    {
        rule = new Dictionary<Vector3Int, int>();
        if (type == RuleType.TopSideUp)
        {
            rule.Add(new Vector3Int(-1, 0, 0), 1);
            rule.Add(new Vector3Int(1, 0, 0), 1);
            rule.Add(new Vector3Int(0, 0, 1), -1);
            rule.Add(new Vector3Int(0, 0, -1), 1);
            
        }
        if(type == RuleType.TopCornerUpLeft)
        {
            rule.Add(new Vector3Int(1, 0, 0), 1);
            rule.Add(new Vector3Int(0, 0, -1), 1);
            rule.Add(new Vector3Int(-1, 0, 1), -1);
            rule.Add(new Vector3Int(0, 0, 1), -1);
            rule.Add(new Vector3Int(-1, 0, 0), -1);
        }
        if(type == RuleType.MiddleCornerUpLeft)
        {
            rule.Add(new Vector3Int(1, 0, 0), 1);
            rule.Add(new Vector3Int(0, 0, -1), 1);
            rule.Add(new Vector3Int(-1, 0, 1), -1);
            rule.Add(new Vector3Int(0, 0, 1), -1);
            rule.Add(new Vector3Int(-1, 0, 0), -1);
            rule.Add(new Vector3Int(0, 1, 0), 1);
            rule.Add(new Vector3Int(1, 1, 0), 1);
            rule.Add(new Vector3Int(0, 1, -1), 1);
        }
        if(type == RuleType.MiddleSideUp)
        {
            rule.Add(new Vector3Int(-1, 0, 0), 1);
            rule.Add(new Vector3Int(1, 0, 0), 1);
            rule.Add(new Vector3Int(0, 0, 1), -1);
            rule.Add(new Vector3Int(0, 0, -1), 1);
            rule.Add(new Vector3Int(-1, 1, 0), 1);
            rule.Add(new Vector3Int(1, 1, 0), 1);
            rule.Add(new Vector3Int(0, 1, 1), -1);
            rule.Add(new Vector3Int(0, 1, -1), 1);
            rule.Add(new Vector3Int(0, 1, 0), 1);
        }
        if(type == RuleType.TopInnerCorner)
        {
            rule.Add(new Vector3Int(0, 0, 1), 1);
            rule.Add(new Vector3Int(1, 0, 0), 1);
            rule.Add(new Vector3Int(1, 0, 1), -1);
        }
        
    }

    public void Rotate90()
    {
        Dictionary<Vector3Int, int> rotatedRule = new Dictionary<Vector3Int, int>();
        foreach(KeyValuePair<Vector3Int, int> c in rule)
        {
            rotatedRule.Add(new Vector3Int(c.Key.z, c.Key.y, -c.Key.x), c.Value);
        }
        rule = rotatedRule;
    }

    public enum RuleType
    {
        TopCornerUpLeft = 2,
        TopSideUp= 3,
        Fill = 5,
        MiddleCornerUpLeft =0,
        MiddleSideUp = 1,
        TopInnerCorner =4
    }
}
