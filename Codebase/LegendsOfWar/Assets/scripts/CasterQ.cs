using UnityEngine;
public class CasterQ : AbilityQBase
{
	[SerializeField]
	private GameObject m_Engulf = null, m_targetingSystem = null;
	private ParticleSystem m_targetingEffect;
	private RaycastHit m_targetHit;
	private bool aimingSkill = false;
	protected override void Start()
	{
		base.Start();
		m_Engulf.GetComponent<ParticleSystem>().Stop();
		m_Engulf.GetComponent<ParticleSystem>().Clear();
		m_targetingEffect = m_targetingSystem.GetComponent<ParticleSystem>();
		m_targetingEffect.Stop();
		m_targetingEffect.Clear();
		cooldownTime = 0.8f;
	}
	protected override void Update()
	{
		if ( Input.GetMouseButtonDown( 0 ) && aimingSkill )
		{
			aimingSkill = false;
			TryCast();
			ResetSystem();
		}
		else if ( Input.GetMouseButtonDown( 1 ) && aimingSkill )
			aimingSkill = false;
		else if ( ( ( Input.GetKeyDown( KeyCode.Q ) && !HeroCamScript.onHero ) || Input.GetKeyDown(
			KeyCode.Alpha1 ) || Input.GetKeyDown( KeyCode.Keypad1 ) ) && !aimingSkill &&
			cooldownTimer <= 0.0f )
			aimingSkill = true;
		if ( !EnoughMana )
			aimingSkill = false;
	}
	protected override void AbilityActivate()
	{
		if ( m_targetHit.collider )
		{
			GameObject tmp = Instantiate( m_Engulf, m_targetHit.transform.position, m_targetHit.
				transform.rotation ) as GameObject;
			tmp.GetComponent<CasterEBehavior>().Activate = true;
			tmp.transform.parent = m_targetHit.transform;
			StatusEffects.Inflict( m_targetHit.collider.gameObject, Effect.CreateEffect() );
			m_targetHit.collider.GetComponentInParent<Info>().TakeDamage( m_effect.m_damage );
			base.AbilityActivate();
		}
	}
	private void FixedUpdate()
	{
		if ( aimingSkill )
			if ( Physics.SphereCast( transform.parent.position, 5.0f, transform.forward, out
				m_targetHit, 150.0f, 1 ) )
				if ( "Minion" == m_targetHit.collider.gameObject.tag && Team.RED_TEAM == m_targetHit
					.collider.GetComponentInParent<Info>().team )
				{
					m_targetingSystem.transform.position = m_targetHit.collider.transform.position;
					m_targetingEffect.Play();
					return;
				}
		if ( m_targetingEffect.isPlaying )
			ResetSystem();
	}
	private void ResetSystem()
	{
		if ( m_targetingEffect.isPlaying )
			m_targetingEffect.Stop();
		m_targetingEffect.Clear();
		m_targetingSystem.transform.localPosition = Vector3.zero;
	}
}
#region OLD_CODE
#if false
#endif
#endregion //OLD_CODE