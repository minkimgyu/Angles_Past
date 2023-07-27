using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SynthesisWay
{
    public string possessingSkill;
    public string getSkill;
    public string synthesizedSkill;
}

[System.Serializable]
public class SkillSynthesisComponent
{
    [SerializeField]
    List<SynthesisWay> synthesisWays = new List<SynthesisWay>();

   public SkillData SynthesisSkill(List<SkillData> PossessingSkills, SkillData getSkill)
   {

        for (int i = 0; i < synthesisWays.Count; i++)
        {
            for (int j = 0; j < PossessingSkills.Count; j++)
            {
                if(synthesisWays[i].possessingSkill == PossessingSkills[j].Name && synthesisWays[i].getSkill == getSkill.Name)
                {
                    PossessingSkills.Remove(PossessingSkills[j]);
                    return DatabaseManager.Instance.UtilizationDB.ReturnSkillData(synthesisWays[i].synthesizedSkill);
                }
            }
        }

        return null;
   }
}
