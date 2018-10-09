using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

enum TRANS_OPTIONS 
{
	Simple = 0,
	TwoPasses = 1,
	Cutout = 2
}

public class TSM2 : MaterialEditor {

	public MaterialProperty[] props;
	int trans;
	public override void OnInspectorGUI ()
	{
		if (!isVisible)
			return;
		props = MaterialEditor.GetMaterialProperties(targets);


		EditorGUIUtility.fieldWidth = 64.0f;


		
		if (!props[0].hasMixedValue && !props[14].hasMixedValue && !props[15].hasMixedValue)
		{
			if( props[0].floatValue == 1)
			{
				if( props[14].floatValue == 0 )
				{
					SetShader(Shader.Find("TSM2/BaseOutline1"));
					props = MaterialEditor.GetMaterialProperties(targets);
					props[0].floatValue = 1;
					props[14].floatValue = 0;
					PropertiesChanged();
				}
				else
				{
					trans = (int)props[15].floatValue;
					switch(trans)
					{
						case 2:
						//{
						SetShader(Shader.Find("TSM2/CutoutOutline1"));
						props = MaterialEditor.GetMaterialProperties(targets);
						props[0].floatValue = 1;
						props[14].floatValue = 1;
						props[15].floatValue = 2;
						PropertiesChanged();
						//}
						break;
						case 1:
						//{
							SetShader(Shader.Find("TSM2/Transparent2POutline1"));
							props = MaterialEditor.GetMaterialProperties(targets);
							props[0].floatValue = 1;
							props[14].floatValue = 1;
							props[15].floatValue = 1;
							PropertiesChanged();
						//}
						break;

						default:
						//{
							SetShader(Shader.Find("TSM2/TransparentOutline1"));
							props = MaterialEditor.GetMaterialProperties(targets);
							props[0].floatValue = 1;
							props[14].floatValue = 1;
							props[15].floatValue = 0;
							PropertiesChanged();
						//}
						break;
					}
				}
			}
			

			if ( props[0].floatValue == 0 )
			{
				if( props[14].floatValue == 0 )
				{
					SetShader(Shader.Find("TSM2/Base1"));
					props = MaterialEditor.GetMaterialProperties(targets);
					props[0].floatValue = 0;
					props[14].floatValue = 0;
					PropertiesChanged();
				}
				else
				{
					trans = (int)props[15].floatValue;
					switch(trans)
					{
					case 2:
						//{
						SetShader(Shader.Find("TSM2/Cutout1"));
						props = MaterialEditor.GetMaterialProperties(targets);
						props[0].floatValue = 0;
						props[14].floatValue = 1;
						props[15].floatValue = 2;
						PropertiesChanged();
						//}
						break;
					case 1:
						//{
						SetShader(Shader.Find("TSM2/Transparent2P1"));
						props = MaterialEditor.GetMaterialProperties(targets);
						props[0].floatValue = 0;
						props[14].floatValue = 1;
						props[15].floatValue = 1;
						PropertiesChanged();
						//}
						break;
						
					default:
						//{
						SetShader(Shader.Find("TSM2/Transparent1"));
						props = MaterialEditor.GetMaterialProperties(targets);
						props[0].floatValue = 0;
						props[14].floatValue = 1;
						props[15].floatValue = 0;
						PropertiesChanged();
						//}
						break;
					}
				}
			}
		}



		ShaderProperty(props[3], "Shade Tex");

		ShaderProperty(props[1], "Enable Detail Tex");
		
		if(!props[15].hasMixedValue)
		{
			if(props[1].floatValue == 1 || (props[15].floatValue == 2 && props[14].floatValue == 1) )
			{
				ShaderProperty(props[2], "Detail Tex");
			}
		}

		ShaderProperty(props[4], "Enable Color Tint");
		
		if(props[4].floatValue == 1)
		{
			ShaderProperty(props[5], "Color Tint");
		}

		if (!props[14].hasMixedValue)
		ShaderProperty(props[14], "Enable Transparency");


		if (!props[14].hasMixedValue)
		if(props[14].floatValue == 1)
		{
			if (!props[15].hasMixedValue)
			{
				ShaderProperty(props[15], "Transparency Mode");
				if(props[15].floatValue == 2)
				{
					ShaderProperty(props[16], "Cutoff");
				}
			}
		}
//
		ShaderProperty(props[6], "Enable Vertex Color");
		ShaderProperty(props[7], "Brightness");




		ShaderProperty(props[8], "Double Sided");
		
		if(props[8].floatValue == 1)
		{
			props[9].floatValue = 0f;
		}
		else
		{
			props[9].floatValue = 2f;
		}

		if (!props[0].hasMixedValue)
			ShaderProperty(props[0], "Outline");

		if(props[0].floatValue == 1f)
		{
			ShaderProperty(props[10], "Outline Color");
			ShaderProperty(props[11], "Outline Thickness");
			ShaderProperty(props[12], "Enable Outline Asymmetry");
			if(props[12].floatValue == 1)
			{
				ShaderProperty(props[13], "Outline Assymetry xyz");
			}
		}
	}
}