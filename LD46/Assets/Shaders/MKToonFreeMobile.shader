﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Toon/FreeMobile"
{
	Properties
	{
		//Main
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Color (RGB)", 2D) = "white"{}

		//Normalmap
		_BumpMap ("Normalmap", 2D) = "bump" {}

		//Light
		_LightThreshold("LightThreshold", Range (0.01, 1)) = 0.3

		//Render
		_LightSmoothness ("Light Smoothness", Range(0,1)) = 0
		_RimSmoothness ("Rim Smoothness", Range(0,1)) = 0.5

		//Custom shadow
		_ShadowColor ("Shadow Color", Color) = (0.0,0.0,0.0,1.0)
		_HighlightColor ("Highlight Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_ShadowIntensity("Shadow Intensity", Range (0.0, 2.0)) = 1.0

		//Outline
		_OutlineColor ("Outline Color", Color) = (0,0,0,1.0)
		_OutlineSize ("Outline Size", Float) = 0.02

		//Rim
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimSize ("Rim Size", Range(0.0,3.0)) = 1.5
		_RimIntensity("Intensity", Range (0, 1)) = 0.5

		//Specular
		_Shininess ("Shininess",  Range (0.01, 1)) = 0.275
		_SpecColor ("Specular Color", Color) = (1,1,1,0.5)
		_SpecularIntensity("Intensity", Range (0, 1)) = 0.5

		//Emission
		_EmissionColor("Emission Color", Color) = (0,0,0)

		//Editor
		[HideInInspector] _MKEditorShowMainBehavior ("Main Behavior", int) = 1
		[HideInInspector] _MKEditorShowDetailBehavior ("Detail Behavior", int) = 0
		[HideInInspector] _MKEditorShowLightBehavior ("Light Behavior", int) = 0
		[HideInInspector] _MKEditorShowShadowBehavior ("Shadow Behavior", int) = 0
		[HideInInspector] _MKEditorShowRenderBehavior ("Render Behavior", int) = 0
		[HideInInspector] _MKEditorShowSpecularBehavior ("Specular Behavior", int) = 0
		[HideInInspector] _MKEditorShowTranslucentBehavior ("Translucent Behavior", int) = 0
		[HideInInspector] _MKEditorShowRimBehavior ("Rim Behavior", int) = 0
		[HideInInspector] _MKEditorShowReflectionBehavior ("Reflection Behavior", int) = 0
		[HideInInspector] _MKEditorShowDissolveBehavior ("Dissolve Behavior", int) = 0
		[HideInInspector] _MKEditorShowOutlineBehavior ("Outline Behavior", int) = 0
		[HideInInspector] _MKEditorShowSketchBehavior ("Sketch Behavior", int) = 0
	}
	SubShader
	{
		LOD 150
		Tags {"RenderType"="Opaque" "PerformanceChecks"="False"}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD BASE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardBase" } 
			Name "FORWARDBASE" 
			Cull Back
			Blend One Zero
			ZWrite On
			ZTest LEqual

			CGPROGRAM
			#pragma target 2.5

			#pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE

			#pragma vertex vertfwd
			#pragma fragment fragfwd
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#include "Inc/Forward/MKToonForwardBaseSetup.cginc"
			#include "Inc/Forward/MKToonForward.cginc"
			
			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// FORWARD ADD
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			Tags { "LightMode" = "ForwardAdd" } 
			Name "FORWARDADD"
			Cull Back
			Blend One One
			ZWrite Off
			ZTest LEqual
			Fog { Color (0,0,0,0) }

			CGPROGRAM
			#pragma target 2.5

			#pragma skip_variants SHADOWS_SOFT

			#pragma vertex vertfwd
			#pragma fragment fragfwd
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma multi_compile_fog
			#pragma multi_compile_fwdadd_fullshadows

			#pragma skip_variants SHADOWS_SOFT

			#include "Inc/Forward/MKToonForwardAddSetup.cginc"
			#include "Inc/Forward/MKToonForward.cginc"
			
			ENDCG
		}
		Pass 
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			ZWrite On ZTest LEqual Cull Off
			Offset 1, 1

			CGPROGRAM
			#pragma target 2.5
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "Inc/ShadowCaster/MKToonShadowCasterSetup.cginc"
			#include "Inc/ShadowCaster/MKToonShadowCaster.cginc"

			ENDCG
		}

		/////////////////////////////////////////////////////////////////////////////////////////////
		// META
		/////////////////////////////////////////////////////////////////////////////////////////////
		UsePass "Standard/META"

		/////////////////////////////////////////////////////////////////////////////////////////////
		// OUTLINE
		/////////////////////////////////////////////////////////////////////////////////////////////
		Pass
		{
			LOD 150
			Tags {"LightMode" = "Always"}
			Name "OUTLINE_SM_2_5"
			Cull Front
			Blend One Zero
			ZWrite On
			ZTest LEqual

			CGPROGRAM 
			#pragma target 2.5
			#pragma vertex outlinevert
			#pragma fragment outlinefrag
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma multi_compile_fog

			#include "./Inc/Outline/MKToonOutlineOnlySetup.cginc"

			// OUTLINE START
					#ifndef MK_TOON_OUTLINE_ONLY_BASE
					#define MK_TOON_OUTLINE_ONLY_BASE
					/////////////////////////////////////////////////////////////////////////////////////////////
					// VERTEX SHADER
					/////////////////////////////////////////////////////////////////////////////////////////////
					VertexOutputOutlineOnly outlinevert(VertexInputOutlineOnly v)
					{
						UNITY_SETUP_INSTANCE_ID(v);
						VertexOutputOutlineOnly o;
						UNITY_INITIALIZE_OUTPUT(VertexOutputOutlineOnly, o);
						UNITY_TRANSFER_INSTANCE_ID(v,o);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

						v.vertex.xyz += v.normal * _OutlineSize;
						//o.pos = UnityObjectToClipPos(v.vertex);
						o.pos = UnityObjectToClipPos(v.vertex);
						o.color = _OutlineColor;

						UNITY_TRANSFER_FOG(o,o.pos);
						return o;
					}

					/////////////////////////////////////////////////////////////////////////////////////////////
					// FRAGMENT SHADER
					/////////////////////////////////////////////////////////////////////////////////////////////
					fixed4 outlinefrag(VertexOutputOutlineOnly o) : SV_Target
					{
						UNITY_SETUP_INSTANCE_ID(o);
						UNITY_APPLY_FOG(o.fogCoord, o.color);
						return o.color;
					}
					#endif

		// OUTLINE END
			ENDCG 
		}
    }
	FallBack "Diffuse"
	CustomEditor "MK.Toon.MKToonFreeEditor"
}
