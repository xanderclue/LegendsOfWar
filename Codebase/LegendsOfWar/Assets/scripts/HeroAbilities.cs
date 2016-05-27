using UnityEngine;
public class HeroAbilities : MonoBehaviour
{
	[SerializeField]
	private AbilityQBase q = null;
	[SerializeField]
	private AbilityWBase w = null;
	[SerializeField]
	private AbilityEBase e = null;
	[SerializeField]
	private AbilityRBase r = null;
	public AbilityQBase abilityQ { get { return q; } }
	public AbilityWBase abilityW { get { return w; } }
	public AbilityEBase abilityE { get { return e; } }
	public AbilityRBase abilityR { get { return r; } }
	void Start()
	{
		GameManager.abilities.Add( this );
	}
	void OnDestroy()
	{
		GameManager.abilities.Remove( this );
	}
	[SerializeField]
	public string heroClassEn = "", heroClassJp = "";
	public string abilityInfo
	{
		get
		{
			if ( Options.Japanese )
				return heroClassJp + "\n\n" + q.abilityNameJp + ": " + q.abilityDescJp + "\n\n" + w.abilityNameJp + ": " + w.abilityDescJp + "\n\n" + e.abilityNameJp + ": " + e.abilityDescJp;
			return heroClassEn + "\n\n" + q.abilityNameEn + ": " + q.abilityDescEn + "\n\n" + w.abilityNameEn + ": " + w.abilityDescEn + "\n\n" + e.abilityNameEn + ": " + e.abilityDescEn;
		}
	}
}