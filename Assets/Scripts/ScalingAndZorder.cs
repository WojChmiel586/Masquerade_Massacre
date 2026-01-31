using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ScalingAndZorder : MonoBehaviour
{
	public Vector2 m_MinMaxYPositions;
	public Vector2 m_MinMaxScales;

	[SerializeField] Transform m_ParentTransform;
	float m_CurrentYValue;

	SpriteRenderer[] m_Sprites;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		m_Sprites = GetComponentsInChildren<SpriteRenderer>();

		ScaleAndZOrder();
	}
	
	// Update is called once per frame
	void Update()
	{
		ScaleAndZOrder();
	}

	private void ScaleAndZOrder()
	{
		m_CurrentYValue = m_ParentTransform.transform.position.y;

		float t = Mathf.InverseLerp( m_MinMaxYPositions.x, m_MinMaxYPositions.y, m_CurrentYValue );
		float scaledB = Mathf.Lerp( m_MinMaxScales.x, m_MinMaxScales.y, t );

		transform.localScale = Vector3.one * scaledB;

		int fZOrder = ( int ) ( Mathf.Lerp( 1, 10000, t ) );
		for ( int i = 0; i < m_Sprites.Length; i++ )
		{
			m_Sprites[ i ].sortingOrder = fZOrder + i;
		}
	}
}
