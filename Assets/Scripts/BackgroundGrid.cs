using UnityEngine;
using System.Collections;
public class BackgroundGrid : MonoBehaviour {
	#region Fields
	//Common fields
	private GameObject myGo;
	private Transform myTr;
	private float divisorToDetermineGridSize = 64f;
	//Rendering Fields
	public float m_MainSpacing = 10.0f;
	public int m_SubSplit = 5;
	public int m_LineCount = 10;
	public Color m_CenterLineColor = new Color(1,1,1, 1);
	public Color m_MainLineColor = new Color(1,1,1, 0.5f);
	public Color m_SubLineColor = new Color(1,1,1, 0.25f);
	private Material lineMaterial;
	#endregion
	private void Start(){
		myGo = gameObject;
		myTr = transform;
		CreateLineMaterial();
	}
	public void ShowGrid(bool state){
		if (myGo != null){
			myGo.SetActive(state);
		}
	}
	private void CreateLineMaterial () {
		lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
		                            "SubShader { Tags { \"Queue\" = \"Geometry\" \"RenderType\"=\"Opaque\"}" +
		                            "Pass {" +
		                            "   BindChannels { Bind \"Color\",color }" +
		                           // "   Blend SrcAlpha OneMinusSrcAlpha" +
		                           // "   ZWrite Off Cull Off Fog { Mode Off }" +
		                            "} } }");
		lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
	}
	private void OnRenderObject()
	{
		GL.PushMatrix();
		GL.MultMatrix(transform.localToWorldMatrix);
		float fMinDist = -m_MainSpacing * m_LineCount;
		float fMaxDist = m_MainSpacing * m_LineCount;
		float fMain;
		
		int m;
		int s;
		lineMaterial.SetPass(0);
		
		//////////////////////////////////////////
		GL.Begin(GL.LINES);
		GL.Color(m_CenterLineColor);
		
		GL.Vertex3(fMinDist, 0, 0);
		GL.Vertex3(fMaxDist, 0, 0);
		
		GL.Vertex3(0, 0, fMinDist);
		GL.Vertex3(0, 0, fMaxDist);
		
		GL.End();
		//////////////////////////////////////////
		
		//////////////////////////////////////////
		GL.Begin(GL.LINES);
		GL.Color(m_MainLineColor);
		
		for (m = 1; m <= m_LineCount; m++)
		{
			fMain = m*m_MainSpacing;
			
			GL.Vertex3(fMinDist, 0, fMain);
			GL.Vertex3(fMaxDist, 0, fMain);
			GL.Vertex3(fMinDist, 0, -fMain);
			GL.Vertex3(fMaxDist, 0, -fMain);
			
			GL.Vertex3(fMain, 0, fMinDist);
			GL.Vertex3(fMain, 0, fMaxDist);
			GL.Vertex3(-fMain, 0, fMinDist);
			GL.Vertex3(-fMain, 0, fMaxDist);
		}
		
		GL.End();
		//////////////////////////////////////////
		
		//////////////////////////////////////////
		GL.Begin(GL.LINES);
		GL.Color(m_SubLineColor);
		
		for (m = 0; m < m_LineCount; m++)
		{
			fMain = m*m_MainSpacing;
			
			for (s = 1; s < m_SubSplit; s++)
			{
				float fSub =  fMain + (m_MainSpacing/m_SubSplit*s);
				
				GL.Vertex3(fMinDist, 0, fSub);
				GL.Vertex3(fMaxDist, 0, fSub);
				GL.Vertex3(fMinDist, 0, -fSub);
				GL.Vertex3(fMaxDist, 0, -fSub);
				
				GL.Vertex3(fSub, 0, fMinDist);
				GL.Vertex3(fSub, 0, fMaxDist);
				GL.Vertex3(-fSub, 0, fMinDist);
				GL.Vertex3(-fSub, 0, fMaxDist);
			}
		}
		
		GL.End();
		//////////////////////////////////////////
		
		GL.PopMatrix();
		
	}
}
