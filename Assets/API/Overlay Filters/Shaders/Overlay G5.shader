// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Overlay Filters/Overlay G5"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		[KeywordEnum(UV,UV_Raw,Object,Object_Scaled,World,UI_Element,Screen)] _ShaderSpace("Shader Space", Float) = 4
		_PixelsPerUnit("Pixels Per Unit", Float) = 100
		_ScreenWidthUnits("Screen Width Units", Float) = 10
		_RectWidth("Rect Width", Float) = 100
		_RectHeight("Rect Height", Float) = 100
		[Toggle(_ENABLEFADEWITHALPHA_ON)] _EnableFadeWithAlpha("Enable Fade With Alpha", Float) = 0
		_Transparency("Transparency", Range( 0 , 1)) = 1
		[Toggle(_ENABLEFADESPREAD_ON)] _EnableFadeSpread("Enable Fade Spread", Float) = 0
		_FadeSpreadFade("Fade Spread: Fade", Float) = 2
		_FadeSpreadWidth("Fade Spread: Width", Float) = 0.5
		_FadeSpreadPosition("Fade Spread: Position", Vector) = (0,0,0,0)
		_FadeSpreadNoiseFactor("Fade Spread: Noise Factor", Float) = 0.2
		_FadeSpreadNoiseScale("Fade Spread: Noise Scale", Vector) = (0.2,0.2,0,0)
		[Toggle(_ENABLEFADESINE_ON)] _EnableFadeSine("Enable Fade Sine", Float) = 0
		_FadeSineFrom("Fade Sine: From", Float) = 0
		_FadeSineTo("Fade Sine: To", Float) = 1
		_FadeSineFrequency("Fade Sine: Frequency", Float) = 2
		[Toggle(_ENABLEFADEDISSOLVE_ON)] _EnableFadeDissolve("Enable Fade Dissolve", Float) = 0
		_FadeDissolveFade("Fade Dissolve: Fade", Range( 0 , 1)) = 1
		_FadeDissolveWidth("Fade Dissolve: Width", Float) = 0.5
		_FadeDissolveNoiseScale("Fade Dissolve: Noise Scale", Vector) = (0.04,0.04,0,0)
		[Toggle(_ENABLEFADENOISESCROLL_ON)] _EnableFadeNoiseScroll("Enable Fade Noise Scroll", Float) = 0
		_FadeNoiseScrollFrom("Fade Noise Scroll: From", Float) = 0
		_FadeNoiseScrollTo("Fade Noise Scroll: To", Float) = 1
		_FadeNoiseScrollSnapLevels("Fade Noise Scroll: Snap Levels", Float) = 10000
		_FadeNoiseScrollScale("Fade Noise Scroll: Scale", Vector) = (0,0.02,0,0)
		_FadeNoiseScrollSpeed("Fade Noise Scroll: Speed", Vector) = (0,5,0,0)
		[Toggle(_FADENOISESCROLLOPPOSITEFLOW_ON)] _FadeNoiseScrollOppositeFlow("Fade Noise Scroll: Opposite Flow", Float) = 0
		[Toggle(_ENABLETIMEUNSCALED_ON)] _EnableTimeUnscaled("Enable Time Unscaled", Float) = 0
		[Toggle(_ENABLETIMESPEED_ON)] _EnableTimeSpeed("Enable Time Speed", Float) = 0
		_TimeSpeed("Time Speed", Float) = 1
		[Toggle(_ENABLETIMEFPS_ON)] _EnableTimeFPS("Enable Time FPS", Float) = 0
		_TimeFPS("Time FPS", Float) = 5
		[Toggle(_ENABLETIMEFREQUENCY_ON)] _EnableTimeFrequency("Enable Time Frequency", Float) = 0
		_TimeRange("Time Range", Float) = 0.5
		_NoiseTexture("Noise Texture", 2D) = "white" {}
		[Toggle(_ENABLEBRIGHTNESS_ON)] _EnableBrightness("Enable Brightness", Float) = 0
		_Brightness("Brightness", Float) = 1
		[Toggle(_ENABLEHUE_ON)] _EnableHue("Enable Hue", Float) = 0
		_Hue("Hue", Range( 0 , 1)) = 0
		[Toggle(_ENABLESATURATION_ON)] _EnableSaturation("Enable Saturation", Float) = 0
		_Saturation("Saturation", Float) = 1
		[Toggle(_ENABLECONTRAST_ON)] _EnableContrast("Enable Contrast", Float) = 0
		_Contrast("Contrast", Float) = 1
		[Toggle(_ENABLELIGHT_ON)] _EnableLight("Enable Light", Float) = 0
		_LightIntensity("Light: Intensity", Float) = 0.2
		_LightContrast("Light: Contrast", Float) = 1
		_LightSaturation("Light: Saturation", Range( 0 , 1)) = 0.1
		[Toggle(_ENABLESINGLETONE_ON)] _EnableSingleTone("Enable Single Tone", Float) = 0
		_SingleToneFade("Single Tone: Fade", Range( 0 , 1)) = 1
		[HDR]_SingleToneColor("Single Tone: Color", Color) = (1,0,0,0)
		_SingleToneContrast("Single Tone: Contrast", Float) = 1
		[Toggle(_ENABLESPLITTONE_ON)] _EnableSplitTone("Enable Split Tone", Float) = 0
		_SplitToneFade("Split Tone: Fade", Range( 0 , 1)) = 1
		[HDR]_SplitToneHighlightsColor("Split Tone: Highlights Color", Color) = (1,0.1,0.1,0)
		[HDR]_SplitToneShadowsColor("Split Tone: Shadows Color", Color) = (0.1,0.4000002,1,0)
		_SplitToneContrast("Split Tone: Contrast", Float) = 1
		_SplitTonePolarize("Split Tone: Polarize", Float) = 1
		_SplitToneShift("Split Tone: Shift", Range( -1 , 1)) = 0
		[Toggle(_ENABLECOLORSTEPS_ON)] _EnableColorSteps("Enable Color Steps", Float) = 0
		_ColorStepsFade("Color Steps: Fade", Range( 0 , 1)) = 1
		_ColorStepsColor0("Color Steps: Color 0", Color) = (0.05882353,0.2196078,0.05882353,0)
		_ColorStepsStep1("Color Steps: Step 1", Float) = 0.25
		_ColorStepsColor1("Color Steps: Color 1", Color) = (0.1882353,0.3843137,0.1882353,0)
		_ColorStepsStep2("Color Steps: Step 2", Float) = 0.5
		_ColorStepsColor2("Color Steps: Color 2", Color) = (0.5450981,0.6745098,0.05882353,0)
		_ColorStepsStep3("Color Steps: Step 3", Float) = 0.75
		_ColorStepsColor3("Color Steps: Color 3", Color) = (0.6078432,0.7372549,0.05882353,0)
		[Toggle(_ENABLELIMITCOLORS_ON)] _EnableLimitColors("Enable Limit Colors", Float) = 0
		_LimitColorsFade("Limit Colors: Fade", Range( 0 , 1)) = 1
		_LimitColorsBrightnessSteps("Limit Colors: Brightness Steps", Float) = 10
		_LimitColorsHueSteps("Limit Colors: Hue Steps", Float) = 15
		_LimitColorsSaturationSteps("Limit Colors: Saturation Steps", Float) = 15
		[Toggle(_ENABLENEGATIVE_ON)] _EnableNegative("Enable Negative", Float) = 0
		_NegativeFade("Negative: Fade", Range( 0 , 1)) = 1
		[Toggle(_ENABLERAINBOW_ON)] _EnableRainbow("Enable Rainbow", Float) = 0
		_RainbowFade("Rainbow: Fade", Range( 0 , 1)) = 0.5
		_RainbowBrightness("Rainbow: Brightness", Float) = 1.5
		_RainbowSaturation("Rainbow: Saturation", Range( 0 , 1)) = 1
		_RainbowContrast("Rainbow: Contrast", Float) = 1
		_RainbowSpeed("Rainbow: Speed", Float) = 0.5
		_RainbowZoomDensity("Rainbow: Zoom Density", Float) = 0.2
		_RainbowCenter("Rainbow: Center", Vector) = (0,0,0,0)
		_RainbowNoiseScale("Rainbow: Noise Scale", Vector) = (0.2,0.2,0,0)
		_RainbowNoiseFactor("Rainbow: Noise Factor", Float) = 0.1
		[Toggle(_ENABLESTATICNOISE_ON)] _EnableStaticNoise("Enable Static Noise", Float) = 0
		_StaticNoiseFade("Static Noise: Fade", Range( 0 , 1)) = 1
		_StaticNoiseSpeed("Static Noise: Speed", Float) = 5
		_StaticNoiseScale("Static Noise: Scale", Vector) = (1,1,0,0)
		[Toggle(_ENABLESHARPEN_ON)] _EnableSharpen("Enable Sharpen", Float) = 0
		_SharpenIntensity("Sharpen: Intensity", Float) = 1.5
		[Toggle(_ENABLEOUTLINE_ON)] _EnableOutline("Enable Outline", Float) = 0
		_OutlineFade("Outline: Fade", Range( 0 , 1)) = 1
		_OutlineBias("Outline: Bias", Range( 0 , 1)) = 0.5
		_OutlineBrightnessTolerance("Outline: Brightness Tolerance", Float) = 0.04
		_OutlineHueTolerance("Outline: Hue Tolerance", Float) = 0.02
		_OutlineSaturationTolerance("Outline: Saturation Tolerance", Float) = 0.04
		_OutlineWidth("Outline: Width", Float) = 0.002
		[Toggle(_OUTLINEFILLLINES_ON)] _OutlineFillLines("Outline: Fill Lines", Float) = 1
		[HDR]_OutlineLineColor("Outline: Line Color", Color) = (1,1,1,1)
		[Toggle(_OUTLINEFILLINSIDE_ON)] _OutlineFillInside("Outline: Fill Inside", Float) = 1
		[HDR]_OutlineFillColor("Outline: Fill Color", Color) = (0,0,0,1)
		[Toggle(_ENABLEGAUSSIANBLUR_ON)] _EnableGaussianBlur("Enable Gaussian Blur", Float) = 0
		_GaussianBlurOffset("Gaussian Blur: Offset", Float) = 1
		[Toggle(_ENABLEDIRECTIONALBLUR_ON)] _EnableDirectionalBlur("Enable Directional Blur", Float) = 0
		_DirectionalBlurOffset("Directional Blur: Offset", Float) = 1
		_DirectionalBlurDirection("Directional Blur: Direction", Range( 0 , 360)) = 0
		[Toggle(_ENABLEZOOMBLUR_ON)] _EnableZoomBlur("Enable Zoom Blur", Float) = 0
		_ZoomBlurOffset("Zoom Blur: Offset", Float) = 1
		_ZoomBlurPower("Zoom Blur: Power", Float) = 2
		_ZoomBlurPosition("Zoom Blur: Position", Vector) = (0,0,0,0)
		_ZoomBlurMinRadius("Zoom Blur: Min Radius", Float) = 0.5
		_ZoomBlurMaxRadius("Zoom Blur: Max Radius", Float) = 10
		[Toggle(_ENABLEDISTORT_ON)] _EnableDistort("Enable Distort", Float) = 0
		_DistortFade("Distort: Fade", Range( 0 , 1)) = 1
		_DistortOffset("Distort: Offset", Vector) = (0.02,0.02,0,0)
		_DistortScale("Distort: Scale", Vector) = (0.5,0.5,0,0)
		_DistortSpeed("Distort: Speed", Vector) = (0,0,0,0)
		[Toggle(_DISTORTOPPOSITEFLOW_ON)] _DistortOppositeFlow("Distort: Opposite Flow", Float) = 0
		[Toggle(_ENABLEREFLECT_ON)] _EnableReflect("Enable: Reflect", Float) = 0
		[Toggle(_REFLECTLOCALMIRROR_ON)] _ReflectLocalMirror("Reflect: Local Mirror", Float) = 1
		_ReflectMirrorHeight("Reflect: Mirror Height", Float) = 0
		[Toggle(_ENABLERIPPLE_ON)] _EnableRipple("Enable Ripple", Float) = 0
		_RippleOffset("Ripple: Offset", Float) = 1
		_RippleCenter("Ripple: Center", Vector) = (0.5,0.5,0,0)
		[Toggle(_ENABLEPIXELATE_ON)] _EnablePixelate("Enable Pixelate", Float) = 0
		_PixelateFade("Pixelate: Fade", Float) = 1
		_PixelatePixels("Pixelate: Pixels", Float) = 100
		[Toggle(_ENABLESNAPDISTORT_ON)] _EnableSnapDistort("Enable Snap Distort", Float) = 0
		_SnapDistortFade("Snap Distort: Fade", Range( 0 , 1)) = 1
		_SnapDistortSnapLevels("Snap Distort: Snap Levels", Float) = 3
		_SnapDistortOffset("Snap Distort: Offset", Vector) = (0.1,0,0,0)
		_SnapDistortScale("Snap Distort: Scale", Vector) = (0,0.01,0,0)
		_SnapDistortSpeed("Snap Distort: Speed", Vector) = (0,50,0,0)
		[Toggle(_SNAPDISTORTOPPOSITEFLOW_ON)] _SnapDistortOppositeFlow("Snap Distort: Opposite Flow", Float) = 0
		[Toggle(_ENABLEGLITCH_ON)] _EnableGlitch("Enable Glitch", Float) = 0
		_GlitchFade("Glitch: Fade", Range( 0 , 1)) = 1
		_GlitchMaskMin("Glitch: Mask Min", Range( 0 , 1)) = 0.4
		_GlitchMaskScale("Glitch: Mask Scale", Vector) = (0,0.1,0,0)
		_GlitchMaskSpeed("Glitch: Mask Speed", Vector) = (0,4,0,0)
		_GlitchHueSpeed("Glitch: Hue Speed", Float) = 1
		_GlitchBrightness("Glitch: Brightness", Float) = 4
		_GlitchNoiseScale("Glitch: Noise Scale", Vector) = (0,3,0,0)
		_GlitchNoiseSpeed("Glitch: Noise Speed", Vector) = (0,1,0,0)
		_GlitchDistortion("Glitch: Distortion", Vector) = (0.1,0,0,0)
		_GlitchDistortionScale("Glitch: Distortion Scale", Vector) = (0,3,0,0)
		[ASEEnd]_GlitchDistortionSpeed("Glitch: Distortion Speed", Vector) = (0,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		GrabPass{ "_ScreenGrab5" }

		Pass
		{
			Name "Default"
		CGPROGRAM
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
			#else
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
			#endif

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _ENABLEOUTLINE_ON
			#pragma shader_feature_local _ENABLESHARPEN_ON
			#pragma shader_feature_local _ENABLEGAUSSIANBLUR_ON
			#pragma shader_feature_local _ENABLEDIRECTIONALBLUR_ON
			#pragma shader_feature_local _ENABLEZOOMBLUR_ON
			#pragma shader_feature_local _ENABLEGLITCH_ON
			#pragma shader_feature_local _ENABLEREFLECT_ON
			#pragma shader_feature_local _ENABLESNAPDISTORT_ON
			#pragma shader_feature_local _ENABLERIPPLE_ON
			#pragma shader_feature_local _ENABLEPIXELATE_ON
			#pragma shader_feature_local _ENABLEDISTORT_ON
			#pragma shader_feature_local _DISTORTOPPOSITEFLOW_ON
			#pragma shader_feature _SHADERSPACE_UV _SHADERSPACE_UV_RAW _SHADERSPACE_OBJECT _SHADERSPACE_OBJECT_SCALED _SHADERSPACE_WORLD _SHADERSPACE_UI_ELEMENT _SHADERSPACE_SCREEN
			#pragma shader_feature_local _ENABLETIMEFREQUENCY_ON
			#pragma shader_feature_local _ENABLETIMESPEED_ON
			#pragma shader_feature_local _ENABLETIMEFPS_ON
			#pragma shader_feature_local _ENABLETIMEUNSCALED_ON
			#pragma shader_feature_local _SNAPDISTORTOPPOSITEFLOW_ON
			#pragma shader_feature_local _REFLECTLOCALMIRROR_ON
			#pragma shader_feature_local _ENABLEFADEWITHALPHA_ON
			#pragma shader_feature_local _ENABLEFADENOISESCROLL_ON
			#pragma shader_feature_local _ENABLEFADEDISSOLVE_ON
			#pragma shader_feature_local _ENABLEFADESPREAD_ON
			#pragma shader_feature_local _ENABLEFADESINE_ON
			#pragma shader_feature_local _FADENOISESCROLLOPPOSITEFLOW_ON
			#pragma shader_feature_local _OUTLINEFILLINSIDE_ON
			#pragma shader_feature_local _OUTLINEFILLLINES_ON
			#pragma shader_feature_local _ENABLEBRIGHTNESS_ON
			#pragma shader_feature_local _ENABLESATURATION_ON
			#pragma shader_feature_local _ENABLEHUE_ON
			#pragma shader_feature_local _ENABLECONTRAST_ON
			#pragma shader_feature_local _ENABLESPLITTONE_ON
			#pragma shader_feature_local _ENABLESINGLETONE_ON
			#pragma shader_feature_local _ENABLENEGATIVE_ON
			#pragma shader_feature_local _ENABLECOLORSTEPS_ON
			#pragma shader_feature_local _ENABLELIMITCOLORS_ON
			#pragma shader_feature_local _ENABLERAINBOW_ON
			#pragma shader_feature_local _ENABLESTATICNOISE_ON
			#pragma shader_feature_local _ENABLELIGHT_ON

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			ASE_DECLARE_SCREENSPACE_TEXTURE( _ScreenGrab5 )
			uniform float2 _DistortOffset;
			uniform sampler2D _NoiseTexture;
			uniform float _PixelsPerUnit;
			float4 _MainTex_TexelSize;
			uniform float _RectWidth;
			uniform float _RectHeight;
			uniform float _ScreenWidthUnits;
			uniform float UnscaledTime;
			uniform float _TimeFPS;
			uniform float _TimeSpeed;
			uniform float _TimeRange;
			uniform float2 _DistortSpeed;
			uniform float2 _DistortScale;
			uniform float _DistortFade;
			uniform float _PixelatePixels;
			uniform float _PixelateFade;
			uniform float2 _RippleCenter;
			uniform float _RippleOffset;
			uniform float2 _SnapDistortOffset;
			uniform float _SnapDistortSnapLevels;
			uniform float2 _SnapDistortSpeed;
			uniform float2 _SnapDistortScale;
			uniform float _SnapDistortFade;
			uniform float _ReflectMirrorHeight;
			uniform float2 _GlitchDistortionSpeed;
			uniform float2 _GlitchDistortionScale;
			uniform float2 _GlitchDistortion;
			uniform float2 _GlitchMaskSpeed;
			uniform float2 _GlitchMaskScale;
			uniform float _GlitchMaskMin;
			uniform float _GlitchFade;
			uniform float _FadeSineFrom;
			uniform float _FadeSineTo;
			uniform float _FadeSineFrequency;
			uniform float _FadeSpreadFade;
			uniform float2 _FadeSpreadPosition;
			uniform float2 _FadeSpreadNoiseScale;
			uniform float _FadeSpreadNoiseFactor;
			uniform float _FadeSpreadWidth;
			uniform float _FadeDissolveFade;
			uniform float _FadeDissolveWidth;
			uniform float2 _FadeDissolveNoiseScale;
			uniform float _FadeNoiseScrollFrom;
			uniform float _FadeNoiseScrollTo;
			uniform float2 _FadeNoiseScrollSpeed;
			uniform float2 _FadeNoiseScrollScale;
			uniform float _FadeNoiseScrollSnapLevels;
			uniform float4 _MainTex_ST;
			uniform float2 _ZoomBlurPosition;
			uniform float _ZoomBlurMinRadius;
			uniform float _ZoomBlurMaxRadius;
			uniform float _ZoomBlurPower;
			uniform float _ZoomBlurOffset;
			uniform float _DirectionalBlurDirection;
			uniform float _DirectionalBlurOffset;
			uniform float _GaussianBlurOffset;
			uniform float _SharpenIntensity;
			uniform float4 _OutlineFillColor;
			uniform float4 _OutlineLineColor;
			uniform float _OutlineWidth;
			uniform float _OutlineHueTolerance;
			uniform float _OutlineSaturationTolerance;
			uniform float _OutlineBrightnessTolerance;
			uniform float _OutlineBias;
			uniform float _OutlineFade;
			uniform float _GlitchBrightness;
			uniform float2 _GlitchNoiseSpeed;
			uniform float2 _GlitchNoiseScale;
			uniform float _GlitchHueSpeed;
			uniform float _LightIntensity;
			uniform float _LightSaturation;
			uniform float _LightContrast;
			uniform float _StaticNoiseSpeed;
			uniform float2 _StaticNoiseScale;
			uniform float _StaticNoiseFade;
			uniform float _RainbowZoomDensity;
			uniform float2 _RainbowCenter;
			uniform float2 _RainbowNoiseScale;
			uniform float _RainbowNoiseFactor;
			uniform float _RainbowSpeed;
			uniform float _RainbowSaturation;
			uniform float _RainbowBrightness;
			uniform float _RainbowContrast;
			uniform float _RainbowFade;
			uniform float _LimitColorsHueSteps;
			uniform float _LimitColorsSaturationSteps;
			uniform float _LimitColorsBrightnessSteps;
			uniform float _LimitColorsFade;
			uniform float4 _ColorStepsColor0;
			uniform float _ColorStepsStep1;
			uniform float4 _ColorStepsColor1;
			uniform float _ColorStepsStep2;
			uniform float _ColorStepsStep3;
			uniform float4 _ColorStepsColor2;
			uniform float4 _ColorStepsColor3;
			uniform float _ColorStepsFade;
			uniform float _NegativeFade;
			uniform float _SingleToneContrast;
			uniform float4 _SingleToneColor;
			uniform float _SingleToneFade;
			uniform float4 _SplitToneShadowsColor;
			uniform float4 _SplitToneHighlightsColor;
			uniform float _SplitToneShift;
			uniform float _SplitTonePolarize;
			uniform float _SplitToneContrast;
			uniform float _SplitToneFade;
			uniform float _Contrast;
			uniform float _Hue;
			uniform float _Saturation;
			uniform float _Brightness;
			uniform float _Transparency;
			inline float4 ASE_ComputeGrabScreenPos( float4 pos )
			{
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				float4 o = pos;
				o.y = pos.w * 0.5f;
				o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
				return o;
			}
			
			float FlipY2_g7861( float ScreenOffsetY )
			{
				#if UNITY_UV_STARTS_AT_TOP
				return -ScreenOffsetY * _ProjectionParams.x;
				#else
				return ScreenOffsetY;
				#endif
			}
			
			float FlipY2_g7863( float ScreenOffsetY )
			{
				#if UNITY_UV_STARTS_AT_TOP
				return -ScreenOffsetY * _ProjectionParams.x;
				#else
				return ScreenOffsetY;
				#endif
			}
			
			float FlipY2_g7866( float ScreenOffsetY )
			{
				#if UNITY_UV_STARTS_AT_TOP
				return -ScreenOffsetY * _ProjectionParams.x;
				#else
				return ScreenOffsetY;
				#endif
			}
			
			float2 BranchFading2( float2 InputA, float2 InputB )
			{
				#if defined(_ENABLEFADEWITHALPHA_ON) || defined(_ENABLEFADENOISESCROLL_ON) || defined(_ENABLEFADEDISSOLVE_ON) || defined(_ENABLEFADESPREAD_ON) || defined(_ENABLEFADESINE_ON)
				return InputA;
				#else
				return InputB;
				#endif
			}
			
			float FlipY2_g8533( float ScreenOffsetY )
			{
				#if UNITY_UV_STARTS_AT_TOP
				return -ScreenOffsetY * _ProjectionParams.x;
				#else
				return ScreenOffsetY;
				#endif
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			float FlipY2_g8511( float ScreenOffsetY )
			{
				#if UNITY_UV_STARTS_AT_TOP
				return -ScreenOffsetY * _ProjectionParams.x;
				#else
				return ScreenOffsetY;
				#endif
			}
			
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}
			float BranchFading1( float InputA, float InputB )
			{
				#if defined(_ENABLEFADEWITHALPHA_ON) || defined(_ENABLEFADENOISESCROLL_ON) || defined(_ENABLEFADEDISSOLVE_ON) || defined(_ENABLEFADESPREAD_ON) || defined(_ENABLEFADESINE_ON)
				return InputA;
				#else
				return InputB;
				#endif
			}
			
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			float4 BranchFading4( float4 InputA, float4 InputB )
			{
				#if defined(_ENABLEFADEWITHALPHA_ON) || defined(_ENABLEFADENOISESCROLL_ON) || defined(_ENABLEFADEDISSOLVE_ON) || defined(_ENABLEFADESPREAD_ON) || defined(_ENABLEFADESINE_ON)
				return InputA;
				#else
				return InputB;
				#endif
			}
			

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				float4 ase_clipPos = UnityObjectToClipPos(IN.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				OUT.ase_texcoord2 = screenPos;
				float3 ase_worldPos = mul(unity_ObjectToWorld, IN.vertex).xyz;
				OUT.ase_texcoord3.xyz = ase_worldPos;
				
				OUT.ase_texcoord4 = IN.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				OUT.ase_texcoord3.w = 0;
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float4 screenPos = IN.ase_texcoord2;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float2 grabScreenPosition161 = (ase_grabScreenPosNorm).xy;
				float2 appendResult16_g7848 = (float2(_ScreenParams.x , _ScreenParams.y));
				float2 temp_output_19_0_g7848 = ( appendResult16_g7848 / _ScreenParams.y );
				float2 temp_output_1_0_g7855 = ( grabScreenPosition161 * temp_output_19_0_g7848 );
				float3 ase_worldPos = IN.ase_texcoord3.xyz;
				float2 texCoord2_g6440 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 texCoord22_g6440 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
				float2 texCoord23_g6440 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult28_g6440 = (float2(_RectWidth , _RectHeight));
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				#if defined(_SHADERSPACE_UV)
				float2 staticSwitch1_g6440 = ( texCoord2_g6440 / ( _PixelsPerUnit * (_MainTex_TexelSize).xy ) );
				#elif defined(_SHADERSPACE_UV_RAW)
				float2 staticSwitch1_g6440 = texCoord22_g6440;
				#elif defined(_SHADERSPACE_OBJECT)
				float2 staticSwitch1_g6440 = (IN.ase_texcoord4.xyz).xy;
				#elif defined(_SHADERSPACE_OBJECT_SCALED)
				float2 staticSwitch1_g6440 = ( (IN.ase_texcoord4.xyz).xy * (ase_objectScale).xy );
				#elif defined(_SHADERSPACE_WORLD)
				float2 staticSwitch1_g6440 = (ase_worldPos).xy;
				#elif defined(_SHADERSPACE_UI_ELEMENT)
				float2 staticSwitch1_g6440 = ( texCoord23_g6440 * ( appendResult28_g6440 / _PixelsPerUnit ) );
				#elif defined(_SHADERSPACE_SCREEN)
				float2 staticSwitch1_g6440 = ( ( (ase_screenPosNorm).xy * (_ScreenParams).xy ) / ( _ScreenParams.x / _ScreenWidthUnits ) );
				#else
				float2 staticSwitch1_g6440 = (ase_worldPos).xy;
				#endif
				float2 shaderPosition109 = staticSwitch1_g6440;
				float2 temp_output_6_0_g7848 = shaderPosition109;
				float2 temp_output_5_0_g7855 = temp_output_6_0_g7848;
				#ifdef _ENABLETIMEUNSCALED_ON
				float staticSwitch44_g6441 = UnscaledTime;
				#else
				float staticSwitch44_g6441 = _Time.y;
				#endif
				#ifdef _ENABLETIMEFPS_ON
				float staticSwitch34_g6441 = ( floor( ( staticSwitch44_g6441 * _TimeFPS ) ) / _TimeFPS );
				#else
				float staticSwitch34_g6441 = staticSwitch44_g6441;
				#endif
				#ifdef _ENABLETIMESPEED_ON
				float staticSwitch33_g6441 = ( staticSwitch34_g6441 * _TimeSpeed );
				#else
				float staticSwitch33_g6441 = staticSwitch34_g6441;
				#endif
				#ifdef _ENABLETIMEFREQUENCY_ON
				float staticSwitch38_g6441 = ( ( sin( staticSwitch33_g6441 ) * _TimeRange ) + 200.0 );
				#else
				float staticSwitch38_g6441 = staticSwitch33_g6441;
				#endif
				float temp_output_340_0 = staticSwitch38_g6441;
				float shaderTime129 = temp_output_340_0;
				float temp_output_22_0_g7848 = shaderTime129;
				float2 temp_output_21_0_g7855 = ( temp_output_22_0_g7848 * _DistortSpeed );
				float2 temp_output_19_0_g7855 = ( temp_output_5_0_g7855 + temp_output_21_0_g7855 );
				float2 appendResult16_g7855 = (float2(( tex2D( _NoiseTexture, ( ( temp_output_19_0_g7855 + float2( 1.234,5.678 ) ) * _DistortScale ) ).r - 0.5 ) , ( tex2D( _NoiseTexture, ( temp_output_19_0_g7855 * _DistortScale ) ).r - 0.5 )));
				float2 temp_output_15_0_g7855 = ( _DistortOffset * appendResult16_g7855 * _DistortFade );
				float2 temp_output_36_0_g7855 = ( temp_output_5_0_g7855 + -temp_output_21_0_g7855 );
				float2 appendResult32_g7855 = (float2(( tex2D( _NoiseTexture, ( ( ( temp_output_36_0_g7855 * float2( 0.97,0.97 ) ) + float2( 1.134,5.378 ) ) * _DistortScale ) ).r - 0.5 ) , ( tex2D( _NoiseTexture, ( ( temp_output_36_0_g7855 * float2( 0.97,0.97 ) ) * _DistortScale ) ).r - 0.5 )));
				#ifdef _DISTORTOPPOSITEFLOW_ON
				float2 staticSwitch38_g7855 = ( temp_output_15_0_g7855 + ( -_DistortOffset * appendResult32_g7855 * _DistortFade ) );
				#else
				float2 staticSwitch38_g7855 = temp_output_15_0_g7855;
				#endif
				#ifdef _ENABLEDISTORT_ON
				float2 staticSwitch2_g7855 = ( temp_output_1_0_g7855 + staticSwitch38_g7855 );
				#else
				float2 staticSwitch2_g7855 = temp_output_1_0_g7855;
				#endif
				float2 temp_output_1_0_g7849 = staticSwitch2_g7855;
				float2 lerpResult8_g7849 = lerp( temp_output_1_0_g7849 , ( round( ( temp_output_1_0_g7849 * _PixelatePixels ) ) / _PixelatePixels ) , _PixelateFade);
				#ifdef _ENABLEPIXELATE_ON
				float2 staticSwitch2_g7849 = lerpResult8_g7849;
				#else
				float2 staticSwitch2_g7849 = temp_output_1_0_g7849;
				#endif
				float2 temp_output_1_0_g7860 = staticSwitch2_g7849;
				float2 temp_output_4_0_g7860 = ( _RippleCenter - temp_output_6_0_g7848 );
				float2 normalizeResult5_g7860 = normalize( temp_output_4_0_g7860 );
				float2 break3_g7861 = ( normalizeResult5_g7860 * float2( 0.01,0.01 ) * min( length( temp_output_4_0_g7860 ) , 0.4 ) * _RippleOffset );
				float ScreenOffsetY2_g7861 = break3_g7861.y;
				float localFlipY2_g7861 = FlipY2_g7861( ScreenOffsetY2_g7861 );
				float2 appendResult4_g7861 = (float2(break3_g7861.x , localFlipY2_g7861));
				#ifdef _ENABLERIPPLE_ON
				float2 staticSwitch9_g7860 = ( appendResult4_g7861 + temp_output_1_0_g7860 );
				#else
				float2 staticSwitch9_g7860 = temp_output_1_0_g7860;
				#endif
				float2 temp_output_15_0_g7850 = staticSwitch9_g7860;
				float2 temp_output_37_0_g7850 = temp_output_6_0_g7848;
				float2 temp_output_24_0_g7850 = ( temp_output_22_0_g7848 * _SnapDistortSpeed );
				float2 temp_output_9_0_g7850 = ( temp_output_37_0_g7850 + temp_output_24_0_g7850 );
				float temp_output_28_0_g7850 = tex2D( _NoiseTexture, ( temp_output_9_0_g7850 * _SnapDistortScale ) ).r;
				float2 appendResult4_g7850 = (float2(( ( round( ( _SnapDistortSnapLevels * tex2D( _NoiseTexture, ( ( temp_output_9_0_g7850 + float2( 1.234,5.678 ) ) * _SnapDistortScale ) ).r ) ) / _SnapDistortSnapLevels ) - 0.5 ) , ( temp_output_28_0_g7850 - ( round( ( temp_output_28_0_g7850 * _SnapDistortSnapLevels ) ) / _SnapDistortSnapLevels ) )));
				float2 temp_output_22_0_g7850 = ( _SnapDistortOffset * appendResult4_g7850 * _SnapDistortFade );
				float2 temp_output_11_0_g7850 = ( temp_output_37_0_g7850 + -temp_output_24_0_g7850 );
				float2 appendResult5_g7850 = (float2(( ( round( ( tex2D( _NoiseTexture, ( ( ( temp_output_11_0_g7850 * float2( 0.97,0.97 ) ) + float2( 1.134,5.378 ) ) * _SnapDistortScale ) ).r * _SnapDistortSnapLevels ) ) / _SnapDistortSnapLevels ) - 0.5 ) , ( ( round( ( _SnapDistortSnapLevels * tex2D( _NoiseTexture, ( ( temp_output_11_0_g7850 * float2( 0.97,0.97 ) ) * _SnapDistortScale ) ).r ) ) / _SnapDistortSnapLevels ) - 0.5 )));
				#ifdef _SNAPDISTORTOPPOSITEFLOW_ON
				float2 staticSwitch12_g7850 = ( temp_output_22_0_g7850 + ( -_SnapDistortOffset * appendResult5_g7850 * _SnapDistortFade ) );
				#else
				float2 staticSwitch12_g7850 = temp_output_22_0_g7850;
				#endif
				#ifdef _ENABLESNAPDISTORT_ON
				float2 staticSwitch33_g7850 = ( temp_output_15_0_g7850 + staticSwitch12_g7850 );
				#else
				float2 staticSwitch33_g7850 = temp_output_15_0_g7850;
				#endif
				float2 temp_output_1_0_g7862 = staticSwitch33_g7850;
				float2 break13_g7862 = temp_output_1_0_g7862;
				float4 transform25_g7862 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
				#ifdef _REFLECTLOCALMIRROR_ON
				float staticSwitch26_g7862 = transform25_g7862.y;
				#else
				float staticSwitch26_g7862 = _ReflectMirrorHeight;
				#endif
				float ScreenOffsetY2_g7863 = ( ( staticSwitch26_g7862 - ase_worldPos.y ) / unity_OrthoParams.y );
				float localFlipY2_g7863 = FlipY2_g7863( ScreenOffsetY2_g7863 );
				float2 appendResult15_g7862 = (float2(break13_g7862.x , ( break13_g7862.y + localFlipY2_g7863 )));
				#ifdef _ENABLEREFLECT_ON
				float2 staticSwitch16_g7862 = appendResult15_g7862;
				#else
				float2 staticSwitch16_g7862 = temp_output_1_0_g7862;
				#endif
				float2 temp_output_396_0 = ( staticSwitch16_g7862 / temp_output_19_0_g7848 );
				float2 temp_output_18_0_g7342 = shaderPosition109;
				float2 glitchPosition117 = temp_output_18_0_g7342;
				float glitchFade116 = ( max( tex2D( _NoiseTexture, ( ( temp_output_18_0_g7342 + ( _GlitchMaskSpeed * shaderTime129 ) ) * _GlitchMaskScale ) ).r , _GlitchMaskMin ) * _GlitchFade );
				float2 break3_g7866 = ( ( tex2D( _NoiseTexture, ( ( glitchPosition117 + ( _GlitchDistortionSpeed * shaderTime129 ) ) * _GlitchDistortionScale ) ).r - 0.5 ) * _GlitchDistortion * glitchFade116 );
				float ScreenOffsetY2_g7866 = break3_g7866.y;
				float localFlipY2_g7866 = FlipY2_g7866( ScreenOffsetY2_g7866 );
				float2 appendResult4_g7866 = (float2(break3_g7866.x , localFlipY2_g7866));
				#ifdef _ENABLEGLITCH_ON
				float2 staticSwitch115 = ( temp_output_396_0 + appendResult4_g7866 );
				#else
				float2 staticSwitch115 = temp_output_396_0;
				#endif
				float temp_output_1_0_g7347 = 1.0;
				float temp_output_23_0_g7344 = temp_output_340_0;
				float lerpResult4_g7347 = lerp( _FadeSineFrom , _FadeSineTo , ( ( sin( ( temp_output_23_0_g7344 * _FadeSineFrequency ) ) * 0.5 ) + 0.5 ));
				#ifdef _ENABLEFADESINE_ON
				float staticSwitch7_g7347 = ( temp_output_1_0_g7347 * lerpResult4_g7347 );
				#else
				float staticSwitch7_g7347 = temp_output_1_0_g7347;
				#endif
				float temp_output_35_0_g7348 = staticSwitch7_g7347;
				float2 temp_output_3_0_g7344 = shaderPosition109;
				float2 temp_output_27_0_g7348 = temp_output_3_0_g7344;
				float clampResult3_g7348 = clamp( ( ( _FadeSpreadFade - ( distance( _FadeSpreadPosition , temp_output_27_0_g7348 ) + ( tex2D( _NoiseTexture, ( temp_output_27_0_g7348 * _FadeSpreadNoiseScale ) ).r * _FadeSpreadNoiseFactor ) ) ) / max( _FadeSpreadWidth , 0.001 ) ) , 0.0 , 1.0 );
				#ifdef _ENABLEFADESPREAD_ON
				float staticSwitch36_g7348 = ( temp_output_35_0_g7348 * clampResult3_g7348 );
				#else
				float staticSwitch36_g7348 = temp_output_35_0_g7348;
				#endif
				float temp_output_1_0_g7345 = staticSwitch36_g7348;
				float temp_output_4_0_g7345 = max( _FadeDissolveWidth , 0.001 );
				float clampResult14_g7345 = clamp( ( ( ( _FadeDissolveFade * ( 1.0 + temp_output_4_0_g7345 ) ) - tex2D( _NoiseTexture, ( temp_output_3_0_g7344 * _FadeDissolveNoiseScale ) ).r ) / temp_output_4_0_g7345 ) , 0.0 , 1.0 );
				#ifdef _ENABLEFADEDISSOLVE_ON
				float staticSwitch26_g7345 = ( temp_output_1_0_g7345 * clampResult14_g7345 );
				#else
				float staticSwitch26_g7345 = temp_output_1_0_g7345;
				#endif
				float temp_output_46_0_g7350 = staticSwitch26_g7345;
				float2 temp_output_5_0_g7350 = temp_output_3_0_g7344;
				float2 temp_output_21_0_g7350 = ( temp_output_23_0_g7344 * _FadeNoiseScrollSpeed );
				float temp_output_10_0_g7350 = tex2D( _NoiseTexture, ( ( ( temp_output_5_0_g7350 + temp_output_21_0_g7350 ) + float2( 1.234,5.678 ) ) * _FadeNoiseScrollScale ) ).r;
				#ifdef _FADENOISESCROLLOPPOSITEFLOW_ON
				float staticSwitch38_g7350 = ( ( temp_output_10_0_g7350 + tex2D( _NoiseTexture, ( ( ( ( temp_output_5_0_g7350 + -temp_output_21_0_g7350 ) * float2( 0.97,0.97 ) ) + float2( 1.134,5.378 ) ) * _FadeNoiseScrollScale ) ).r ) * 0.5 );
				#else
				float staticSwitch38_g7350 = temp_output_10_0_g7350;
				#endif
				float lerpResult48_g7350 = lerp( _FadeNoiseScrollFrom , _FadeNoiseScrollTo , ( round( ( staticSwitch38_g7350 * _FadeNoiseScrollSnapLevels ) ) / _FadeNoiseScrollSnapLevels ));
				#ifdef _ENABLEFADENOISESCROLL_ON
				float staticSwitch2_g7350 = ( temp_output_46_0_g7350 * max( lerpResult48_g7350 , 0.0 ) );
				#else
				float staticSwitch2_g7350 = temp_output_46_0_g7350;
				#endif
				float temp_output_359_0 = staticSwitch2_g7350;
				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float alphaColor189 = ( tex2D( _MainTex, uv_MainTex ).a * IN.color.a );
				#ifdef _ENABLEFADEWITHALPHA_ON
				float staticSwitch361 = ( temp_output_359_0 * alphaColor189 );
				#else
				float staticSwitch361 = temp_output_359_0;
				#endif
				float fullFade153 = staticSwitch361;
				float2 lerpResult160 = lerp( grabScreenPosition161 , staticSwitch115 , fullFade153);
				float2 InputA381 = lerpResult160;
				float2 InputB381 = staticSwitch115;
				float2 localBranchFading2381 = BranchFading2( InputA381 , InputB381 );
				float2 temp_output_1_0_g8506 = localBranchFading2381;
				float2 temp_output_1_0_g8527 = temp_output_1_0_g8506;
				float4 screenColor22_g8527 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8527);
				float4 appendResult32_g8527 = (float4(max( (screenColor22_g8527).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 temp_output_1_0_g8528 = temp_output_1_0_g8506;
				float2 break3_g8533 = ( _ZoomBlurPosition - shaderPosition109 );
				float ScreenOffsetY2_g8533 = break3_g8533.y;
				float localFlipY2_g8533 = FlipY2_g8533( ScreenOffsetY2_g8533 );
				float2 appendResult4_g8533 = (float2(break3_g8533.x , localFlipY2_g8533));
				float2 temp_output_66_0_g8528 = appendResult4_g8533;
				float2 normalizeResult16_g8528 = normalize( temp_output_66_0_g8528 );
				float clampResult15_g8528 = clamp( ( length( temp_output_66_0_g8528 ) - _ZoomBlurMinRadius ) , 0.0 , _ZoomBlurMaxRadius );
				float saferPower51_g8528 = max( clampResult15_g8528 , 0.0001 );
				float2 appendResult20_g8528 = (float2(_ScreenParams.y , _ScreenParams.x));
				float temp_output_50_0_g8506 = fullFade153;
				float2 temp_output_17_0_g8528 = ( normalizeResult16_g8528 * pow( saferPower51_g8528 , _ZoomBlurPower ) * _ZoomBlurOffset * ( appendResult20_g8528 / _ScreenParams.y ) * temp_output_50_0_g8506 );
				float2 temp_output_1_0_g8529 = ( temp_output_1_0_g8528 + ( temp_output_17_0_g8528 * float2( 0.0001,0.0001 ) ) );
				float4 screenColor22_g8529 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8529);
				float4 appendResult32_g8529 = (float4(max( (screenColor22_g8529).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 temp_output_1_0_g8532 = ( temp_output_1_0_g8528 + ( temp_output_17_0_g8528 * float2( 0.0002,0.0002 ) ) );
				float4 screenColor22_g8532 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8532);
				float4 appendResult32_g8532 = (float4(max( (screenColor22_g8532).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 temp_output_1_0_g8530 = ( temp_output_1_0_g8528 + ( temp_output_17_0_g8528 * float2( 0.0003,0.0003 ) ) );
				float4 screenColor22_g8530 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8530);
				float4 appendResult32_g8530 = (float4(max( (screenColor22_g8530).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 temp_output_1_0_g8531 = ( temp_output_1_0_g8528 + ( temp_output_17_0_g8528 * float2( 0.0004,0.0004 ) ) );
				float4 screenColor22_g8531 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8531);
				float4 appendResult32_g8531 = (float4(max( (screenColor22_g8531).rgb , float3( 0,0,0 ) ) , 1.0));
				float4 temp_output_63_0_g8506 = ( ( appendResult32_g8529 + appendResult32_g8532 + appendResult32_g8530 + appendResult32_g8531 ) / 4.0 );
				#ifdef _ENABLEZOOMBLUR_ON
				float4 staticSwitch33_g8506 = temp_output_63_0_g8506;
				#else
				float4 staticSwitch33_g8506 = appendResult32_g8527;
				#endif
				float2 appendResult3_g8507 = (float2(_ScreenParams.y , _ScreenParams.x));
				float2 appendResult8_g8507 = (float2(_DirectionalBlurOffset , 0.0));
				float3 rotatedValue10_g8507 = RotateAroundAxis( float3( 0,0,0 ), float3( appendResult8_g8507 ,  0.0 ), float3( 0,0,1 ), ( _DirectionalBlurDirection * 0.01745329 ) );
				float2 break3_g8511 = (rotatedValue10_g8507).xy;
				float ScreenOffsetY2_g8511 = break3_g8511.y;
				float localFlipY2_g8511 = FlipY2_g8511( ScreenOffsetY2_g8511 );
				float2 appendResult4_g8511 = (float2(break3_g8511.x , localFlipY2_g8511));
				float2 temp_output_13_0_g8507 = ( ( appendResult3_g8507 / _ScreenParams.y ) * appendResult4_g8511 * temp_output_50_0_g8506 );
				float2 temp_output_1_0_g8507 = temp_output_1_0_g8506;
				float2 temp_output_1_0_g8510 = ( ( temp_output_13_0_g8507 * float2( 0.004,0.004 ) ) + temp_output_1_0_g8507 );
				float4 screenColor22_g8510 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8510);
				float4 appendResult32_g8510 = (float4(max( (screenColor22_g8510).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 temp_output_1_0_g8508 = ( ( temp_output_13_0_g8507 * float2( 0.002,0.002 ) ) + temp_output_1_0_g8507 );
				float4 screenColor22_g8508 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8508);
				float4 appendResult32_g8508 = (float4(max( (screenColor22_g8508).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 temp_output_1_0_g8509 = ( ( temp_output_13_0_g8507 * float2( -0.002,-0.002 ) ) + temp_output_1_0_g8507 );
				float4 screenColor22_g8509 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8509);
				float4 appendResult32_g8509 = (float4(max( (screenColor22_g8509).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 temp_output_1_0_g8512 = ( ( temp_output_13_0_g8507 * float2( -0.004,-0.004 ) ) + temp_output_1_0_g8507 );
				float4 screenColor22_g8512 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8512);
				float4 appendResult32_g8512 = (float4(max( (screenColor22_g8512).rgb , float3( 0,0,0 ) ) , 1.0));
				float4 temp_output_57_0_g8506 = ( ( appendResult32_g8510 + appendResult32_g8508 + appendResult32_g8509 + appendResult32_g8512 ) * 0.25 );
				#ifdef _ENABLEZOOMBLUR_ON
				float4 staticSwitch35_g8506 = ( ( temp_output_63_0_g8506 + temp_output_57_0_g8506 ) * 0.5 );
				#else
				float4 staticSwitch35_g8506 = temp_output_57_0_g8506;
				#endif
				#ifdef _ENABLEDIRECTIONALBLUR_ON
				float4 staticSwitch16_g8506 = staticSwitch35_g8506;
				#else
				float4 staticSwitch16_g8506 = staticSwitch33_g8506;
				#endif
				float2 temp_output_1_0_g8534 = temp_output_1_0_g8506;
				float2 appendResult26_g8534 = (float2(_ScreenParams.y , _ScreenParams.x));
				float2 temp_output_27_0_g8534 = ( appendResult26_g8534 / _ScreenParams.y );
				float temp_output_32_0_g8534 = ( _GaussianBlurOffset * 0.001 * temp_output_50_0_g8506 );
				float2 appendResult9_g8534 = (float2(temp_output_32_0_g8534 , temp_output_32_0_g8534));
				float2 temp_output_1_0_g8538 = ( temp_output_1_0_g8534 + ( temp_output_27_0_g8534 * appendResult9_g8534 ) );
				float4 screenColor22_g8538 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8538);
				float4 appendResult32_g8538 = (float4(max( (screenColor22_g8538).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 appendResult11_g8534 = (float2(-temp_output_32_0_g8534 , temp_output_32_0_g8534));
				float2 temp_output_1_0_g8537 = ( temp_output_1_0_g8534 + ( temp_output_27_0_g8534 * appendResult11_g8534 ) );
				float4 screenColor22_g8537 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8537);
				float4 appendResult32_g8537 = (float4(max( (screenColor22_g8537).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 appendResult15_g8534 = (float2(temp_output_32_0_g8534 , -temp_output_32_0_g8534));
				float2 temp_output_1_0_g8536 = ( temp_output_1_0_g8534 + ( temp_output_27_0_g8534 * appendResult15_g8534 ) );
				float4 screenColor22_g8536 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8536);
				float4 appendResult32_g8536 = (float4(max( (screenColor22_g8536).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 appendResult19_g8534 = (float2(-temp_output_32_0_g8534 , -temp_output_32_0_g8534));
				float2 temp_output_1_0_g8535 = ( temp_output_1_0_g8534 + ( temp_output_27_0_g8534 * appendResult19_g8534 ) );
				float4 screenColor22_g8535 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8535);
				float4 appendResult32_g8535 = (float4(max( (screenColor22_g8535).rgb , float3( 0,0,0 ) ) , 1.0));
				float4 temp_output_58_0_g8506 = ( ( appendResult32_g8538 + appendResult32_g8537 + appendResult32_g8536 + appendResult32_g8535 ) * 0.25 );
				#ifdef _ENABLEZOOMBLUR_ON
				float4 staticSwitch39_g8506 = ( ( temp_output_63_0_g8506 + temp_output_58_0_g8506 ) * 0.5 );
				#else
				float4 staticSwitch39_g8506 = temp_output_58_0_g8506;
				#endif
				#ifdef _ENABLEZOOMBLUR_ON
				float4 staticSwitch46_g8506 = ( ( temp_output_63_0_g8506 + temp_output_57_0_g8506 + temp_output_58_0_g8506 ) * 0.3333333 );
				#else
				float4 staticSwitch46_g8506 = ( ( temp_output_57_0_g8506 + temp_output_58_0_g8506 ) * 0.5 );
				#endif
				#ifdef _ENABLEDIRECTIONALBLUR_ON
				float4 staticSwitch17_g8506 = staticSwitch46_g8506;
				#else
				float4 staticSwitch17_g8506 = staticSwitch39_g8506;
				#endif
				#ifdef _ENABLEGAUSSIANBLUR_ON
				float4 staticSwitch2_g8506 = staticSwitch17_g8506;
				#else
				float4 staticSwitch2_g8506 = staticSwitch16_g8506;
				#endif
				float4 temp_output_44_0_g8513 = staticSwitch2_g8506;
				float2 temp_output_1_0_g8513 = temp_output_1_0_g8506;
				float2 appendResult26_g8513 = (float2(_ScreenParams.y , _ScreenParams.x));
				float2 temp_output_27_0_g8513 = ( appendResult26_g8513 / _ScreenParams.y );
				float temp_output_32_0_g8513 = ( _SharpenIntensity * 0.001 * temp_output_50_0_g8506 );
				float2 appendResult9_g8513 = (float2(temp_output_32_0_g8513 , temp_output_32_0_g8513));
				float2 temp_output_1_0_g8515 = ( temp_output_1_0_g8513 + ( temp_output_27_0_g8513 * appendResult9_g8513 ) );
				float4 screenColor22_g8515 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8515);
				float4 appendResult32_g8515 = (float4(max( (screenColor22_g8515).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 appendResult11_g8513 = (float2(-temp_output_32_0_g8513 , temp_output_32_0_g8513));
				float2 temp_output_1_0_g8514 = ( temp_output_1_0_g8513 + ( temp_output_27_0_g8513 * appendResult11_g8513 ) );
				float4 screenColor22_g8514 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8514);
				float4 appendResult32_g8514 = (float4(max( (screenColor22_g8514).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 appendResult15_g8513 = (float2(temp_output_32_0_g8513 , -temp_output_32_0_g8513));
				float2 temp_output_1_0_g8516 = ( temp_output_1_0_g8513 + ( temp_output_27_0_g8513 * appendResult15_g8513 ) );
				float4 screenColor22_g8516 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8516);
				float4 appendResult32_g8516 = (float4(max( (screenColor22_g8516).rgb , float3( 0,0,0 ) ) , 1.0));
				float2 appendResult19_g8513 = (float2(-temp_output_32_0_g8513 , -temp_output_32_0_g8513));
				float2 temp_output_1_0_g8517 = ( temp_output_1_0_g8513 + ( temp_output_27_0_g8513 * appendResult19_g8513 ) );
				float4 screenColor22_g8517 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8517);
				float4 appendResult32_g8517 = (float4(max( (screenColor22_g8517).rgb , float3( 0,0,0 ) ) , 1.0));
				#ifdef _ENABLESHARPEN_ON
				float4 staticSwitch43_g8513 = ( temp_output_44_0_g8513 + ( temp_output_44_0_g8513 - ( ( appendResult32_g8515 + appendResult32_g8514 + appendResult32_g8516 + appendResult32_g8517 ) * 0.25 ) ) );
				#else
				float4 staticSwitch43_g8513 = temp_output_44_0_g8513;
				#endif
				float4 temp_output_2_0_g8518 = staticSwitch43_g8513;
				#ifdef _OUTLINEFILLINSIDE_ON
				float4 staticSwitch73_g8518 = _OutlineFillColor;
				#else
				float4 staticSwitch73_g8518 = temp_output_2_0_g8518;
				#endif
				#ifdef _OUTLINEFILLLINES_ON
				float4 staticSwitch121_g8518 = _OutlineLineColor;
				#else
				float4 staticSwitch121_g8518 = temp_output_2_0_g8518;
				#endif
				float3 hsvTorgb4_g8518 = RGBToHSV( temp_output_2_0_g8518.rgb );
				float3 colorHSV35_g8518 = hsvTorgb4_g8518;
				float3 break10_g8525 = colorHSV35_g8518;
				float2 temp_output_5_0_g8518 = temp_output_1_0_g8506;
				float2 temp_output_1_0_g8526 = ( temp_output_5_0_g8518 + ( float2( 0.01,0 ) * _OutlineWidth ) );
				float4 screenColor22_g8526 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8526);
				float4 appendResult32_g8526 = (float4(max( (screenColor22_g8526).rgb , float3( 0,0,0 ) ) , 1.0));
				float3 hsvTorgb6_g8525 = RGBToHSV( appendResult32_g8526.xyz );
				float saferPower41_g8525 = max( distance( break10_g8525.x , hsvTorgb6_g8525.x ) , 0.0001 );
				float hueT38_g8518 = _OutlineHueTolerance;
				float saferPower42_g8525 = max( distance( break10_g8525.y , hsvTorgb6_g8525.y ) , 0.0001 );
				float saturationT40_g8518 = _OutlineSaturationTolerance;
				float saferPower43_g8525 = max( distance( break10_g8525.z , hsvTorgb6_g8525.z ) , 0.0001 );
				float brightnessT42_g8518 = _OutlineBrightnessTolerance;
				float3 break10_g8519 = colorHSV35_g8518;
				float2 temp_output_1_0_g8520 = ( temp_output_5_0_g8518 + ( float2( -0.01,0 ) * _OutlineWidth ) );
				float4 screenColor22_g8520 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8520);
				float4 appendResult32_g8520 = (float4(max( (screenColor22_g8520).rgb , float3( 0,0,0 ) ) , 1.0));
				float3 hsvTorgb6_g8519 = RGBToHSV( appendResult32_g8520.xyz );
				float saferPower41_g8519 = max( distance( break10_g8519.x , hsvTorgb6_g8519.x ) , 0.0001 );
				float saferPower42_g8519 = max( distance( break10_g8519.y , hsvTorgb6_g8519.y ) , 0.0001 );
				float saferPower43_g8519 = max( distance( break10_g8519.z , hsvTorgb6_g8519.z ) , 0.0001 );
				float3 break10_g8523 = colorHSV35_g8518;
				float2 temp_output_1_0_g8524 = ( temp_output_5_0_g8518 + ( _OutlineWidth * float2( 0,-0.01 ) ) );
				float4 screenColor22_g8524 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8524);
				float4 appendResult32_g8524 = (float4(max( (screenColor22_g8524).rgb , float3( 0,0,0 ) ) , 1.0));
				float3 hsvTorgb6_g8523 = RGBToHSV( appendResult32_g8524.xyz );
				float saferPower41_g8523 = max( distance( break10_g8523.x , hsvTorgb6_g8523.x ) , 0.0001 );
				float saferPower42_g8523 = max( distance( break10_g8523.y , hsvTorgb6_g8523.y ) , 0.0001 );
				float saferPower43_g8523 = max( distance( break10_g8523.z , hsvTorgb6_g8523.z ) , 0.0001 );
				float3 break10_g8521 = colorHSV35_g8518;
				float2 temp_output_1_0_g8522 = ( temp_output_5_0_g8518 + ( _OutlineWidth * float2( 0,0.01 ) ) );
				float4 screenColor22_g8522 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_ScreenGrab5,temp_output_1_0_g8522);
				float4 appendResult32_g8522 = (float4(max( (screenColor22_g8522).rgb , float3( 0,0,0 ) ) , 1.0));
				float3 hsvTorgb6_g8521 = RGBToHSV( appendResult32_g8522.xyz );
				float saferPower41_g8521 = max( distance( break10_g8521.x , hsvTorgb6_g8521.x ) , 0.0001 );
				float saferPower42_g8521 = max( distance( break10_g8521.y , hsvTorgb6_g8521.y ) , 0.0001 );
				float saferPower43_g8521 = max( distance( break10_g8521.z , hsvTorgb6_g8521.z ) , 0.0001 );
				float4 lerpResult67_g8518 = lerp( staticSwitch73_g8518 , staticSwitch121_g8518 , step( min( min( max( ( 1.0 - ( pow( saferPower41_g8525 , hueT38_g8518 ) * pow( saferPower42_g8525 , saturationT40_g8518 ) * pow( saferPower43_g8525 , brightnessT42_g8518 ) ) ) , 0.0 ) , max( ( 1.0 - ( pow( saferPower41_g8519 , hueT38_g8518 ) * pow( saferPower42_g8519 , saturationT40_g8518 ) * pow( saferPower43_g8519 , brightnessT42_g8518 ) ) ) , 0.0 ) ) , min( max( ( 1.0 - ( pow( saferPower41_g8523 , hueT38_g8518 ) * pow( saferPower42_g8523 , saturationT40_g8518 ) * pow( saferPower43_g8523 , brightnessT42_g8518 ) ) ) , 0.0 ) , max( ( 1.0 - ( pow( saferPower41_g8521 , hueT38_g8518 ) * pow( saferPower42_g8521 , saturationT40_g8518 ) * pow( saferPower43_g8521 , brightnessT42_g8518 ) ) ) , 0.0 ) ) ) , _OutlineBias ));
				float InputA128_g8518 = ( _OutlineFade * temp_output_50_0_g8506 );
				float InputB128_g8518 = _OutlineFade;
				float localBranchFading1128_g8518 = BranchFading1( InputA128_g8518 , InputB128_g8518 );
				float4 lerpResult77_g8518 = lerp( temp_output_2_0_g8518 , lerpResult67_g8518 , localBranchFading1128_g8518);
				#ifdef _ENABLEOUTLINE_ON
				float4 staticSwitch74_g8518 = lerpResult77_g8518;
				#else
				float4 staticSwitch74_g8518 = temp_output_2_0_g8518;
				#endif
				float4 temp_output_411_0 = staticSwitch74_g8518;
				float4 temp_output_1_0_g8480 = temp_output_411_0;
				float4 break2_g8482 = temp_output_1_0_g8480;
				float temp_output_34_0_g8480 = shaderTime129;
				float3 hsvTorgb3_g8481 = HSVToRGB( float3(( tex2D( _NoiseTexture, ( ( glitchPosition117 + ( _GlitchNoiseSpeed * temp_output_34_0_g8480 ) ) * _GlitchNoiseScale ) ).r + ( temp_output_34_0_g8480 * _GlitchHueSpeed ) ),1.0,1.0) );
				float3 lerpResult23_g8480 = lerp( (temp_output_1_0_g8480).rgb , ( ( ( break2_g8482.x + break2_g8482.y + break2_g8482.z ) / 3.0 ) * _GlitchBrightness * hsvTorgb3_g8481 ) , glitchFade116);
				float4 appendResult27_g8480 = (float4(lerpResult23_g8480 , temp_output_1_0_g8480.a));
				#ifdef _ENABLEGLITCH_ON
				float4 staticSwitch123 = appendResult27_g8480;
				#else
				float4 staticSwitch123 = temp_output_411_0;
				#endif
				float4 temp_output_1_0_g8489 = staticSwitch123;
				float3 temp_output_4_0_g8489 = (temp_output_1_0_g8489).rgb;
				float4 break2_g8490 = float4( temp_output_4_0_g8489 , 0.0 );
				float3 temp_cast_11 = (max( ( ( break2_g8490.x + break2_g8490.y + break2_g8490.z ) / 3.0 ) , 0.1 )).xxx;
				float3 lerpResult8_g8489 = lerp( temp_cast_11 , temp_output_4_0_g8489 , _LightSaturation);
				float3 saferPower9_g8489 = max( lerpResult8_g8489 , 0.0001 );
				float3 temp_cast_12 = (_LightContrast).xxx;
				float4 appendResult14_g8489 = (float4(( ( _LightIntensity * pow( saferPower9_g8489 , temp_cast_12 ) ) + temp_output_4_0_g8489 ) , temp_output_1_0_g8489.a));
				#ifdef _ENABLELIGHT_ON
				float4 staticSwitch12_g8489 = appendResult14_g8489;
				#else
				float4 staticSwitch12_g8489 = temp_output_1_0_g8489;
				#endif
				float4 temp_output_1_0_g8487 = staticSwitch12_g8489;
				float temp_output_37_0_g8484 = shaderTime129;
				float2 temp_output_35_0_g8484 = shaderPosition109;
				float3 rotatedValue6_g8487 = RotateAroundAxis( float3( 1000,1000,0 ), float3( temp_output_35_0_g8484 ,  0.0 ), float3( 0,0,1 ), ( _StaticNoiseSpeed * temp_output_37_0_g8484 ) );
				float temp_output_7_0_g8487 = tex2D( _NoiseTexture, ( rotatedValue6_g8487.xy * _StaticNoiseScale ) ).r;
				float4 appendResult14_g8487 = (float4(temp_output_7_0_g8487 , temp_output_7_0_g8487 , temp_output_7_0_g8487 , temp_output_1_0_g8487.a));
				float4 lerpResult10_g8487 = lerp( temp_output_1_0_g8487 , appendResult14_g8487 , _StaticNoiseFade);
				#ifdef _ENABLESTATICNOISE_ON
				float4 staticSwitch15_g8487 = lerpResult10_g8487;
				#else
				float4 staticSwitch15_g8487 = temp_output_1_0_g8487;
				#endif
				float4 temp_output_1_0_g8502 = staticSwitch15_g8487;
				float2 temp_output_42_0_g8502 = temp_output_35_0_g8484;
				float3 hsvTorgb3_g8505 = HSVToRGB( float3(( ( ( _RainbowZoomDensity * distance( temp_output_42_0_g8502 , _RainbowCenter ) ) + ( tex2D( _NoiseTexture, ( temp_output_42_0_g8502 * _RainbowNoiseScale ) ).r * _RainbowNoiseFactor ) ) + ( _RainbowSpeed * temp_output_37_0_g8484 ) ),1.0,1.0) );
				float3 hsvTorgb36_g8502 = RGBToHSV( hsvTorgb3_g8505 );
				float3 hsvTorgb37_g8502 = HSVToRGB( float3(hsvTorgb36_g8502.x,_RainbowSaturation,( hsvTorgb36_g8502.z * _RainbowBrightness )) );
				float4 break2_g8504 = temp_output_1_0_g8502;
				float saferPower24_g8502 = max( ( ( break2_g8504.x + break2_g8504.y + break2_g8504.z ) / 3.0 ) , 0.0001 );
				float3 lerpResult46_g8502 = lerp( (temp_output_1_0_g8502).rgb , ( hsvTorgb37_g8502 * pow( saferPower24_g8502 , max( _RainbowContrast , 0.01 ) ) ) , _RainbowFade);
				float4 appendResult29_g8502 = (float4(lerpResult46_g8502 , temp_output_1_0_g8502.a));
				#ifdef _ENABLERAINBOW_ON
				float4 staticSwitch45_g8502 = appendResult29_g8502;
				#else
				float4 staticSwitch45_g8502 = temp_output_1_0_g8502;
				#endif
				float4 temp_output_1_0_g8486 = staticSwitch45_g8502;
				float3 hsvTorgb2_g8486 = RGBToHSV( temp_output_1_0_g8486.rgb );
				float3 hsvTorgb5_g8486 = HSVToRGB( float3(( floor( ( hsvTorgb2_g8486.x * _LimitColorsHueSteps ) ) / _LimitColorsHueSteps ),( floor( ( hsvTorgb2_g8486.y * _LimitColorsSaturationSteps ) ) / _LimitColorsSaturationSteps ),( floor( ( hsvTorgb2_g8486.z * _LimitColorsBrightnessSteps ) ) / _LimitColorsBrightnessSteps )) );
				float3 lerpResult8_g8486 = lerp( (temp_output_1_0_g8486).rgb , hsvTorgb5_g8486 , _LimitColorsFade);
				float4 appendResult4_g8486 = (float4(lerpResult8_g8486 , temp_output_1_0_g8486.a));
				#ifdef _ENABLELIMITCOLORS_ON
				float4 staticSwitch6_g8486 = appendResult4_g8486;
				#else
				float4 staticSwitch6_g8486 = temp_output_1_0_g8486;
				#endif
				float4 temp_output_1_0_g8497 = staticSwitch6_g8486;
				float4 break2_g8498 = temp_output_1_0_g8497;
				float temp_output_2_0_g8497 = ( ( break2_g8498.x + break2_g8498.y + break2_g8498.z ) / 3.0 );
				float temp_output_4_0_g8497 = step( _ColorStepsStep1 , temp_output_2_0_g8497 );
				float temp_output_10_0_g8497 = step( _ColorStepsStep3 , temp_output_2_0_g8497 );
				float temp_output_14_0_g8497 = max( ( step( _ColorStepsStep2 , temp_output_2_0_g8497 ) - temp_output_10_0_g8497 ) , 0.0 );
				float3 lerpResult31_g8497 = lerp( (temp_output_1_0_g8497).rgb , ( ( (_ColorStepsColor0).rgb * ( 1.0 - temp_output_4_0_g8497 ) ) + ( (_ColorStepsColor1).rgb * max( ( temp_output_4_0_g8497 - temp_output_14_0_g8497 ) , 0.0 ) ) + ( temp_output_14_0_g8497 * (_ColorStepsColor2).rgb ) + ( temp_output_10_0_g8497 * (_ColorStepsColor3).rgb ) ) , _ColorStepsFade);
				float4 appendResult34_g8497 = (float4(lerpResult31_g8497 , temp_output_1_0_g8497.a));
				#ifdef _ENABLECOLORSTEPS_ON
				float4 staticSwitch36_g8497 = appendResult34_g8497;
				#else
				float4 staticSwitch36_g8497 = temp_output_1_0_g8497;
				#endif
				float4 temp_output_1_0_g8499 = staticSwitch36_g8497;
				float3 temp_output_6_0_g8499 = (temp_output_1_0_g8499).rgb;
				float3 lerpResult3_g8499 = lerp( temp_output_6_0_g8499 , ( 1.0 - temp_output_6_0_g8499 ) , _NegativeFade);
				float4 appendResult7_g8499 = (float4(lerpResult3_g8499 , temp_output_1_0_g8499.a));
				#ifdef _ENABLENEGATIVE_ON
				float4 staticSwitch5_g8499 = appendResult7_g8499;
				#else
				float4 staticSwitch5_g8499 = temp_output_1_0_g8499;
				#endif
				float4 temp_output_1_0_g8500 = staticSwitch5_g8499;
				float4 break2_g8501 = temp_output_1_0_g8500;
				float3 lerpResult15_g8500 = lerp( (temp_output_1_0_g8500).rgb , ( pow( max( ( ( break2_g8501.x + break2_g8501.y + break2_g8501.z ) / 3.0 ) , 0.0001 ) , _SingleToneContrast ) * (_SingleToneColor).rgb ) , _SingleToneFade);
				float4 appendResult10_g8500 = (float4(lerpResult15_g8500 , temp_output_1_0_g8500.a));
				#ifdef _ENABLESINGLETONE_ON
				float4 staticSwitch2_g8500 = appendResult10_g8500;
				#else
				float4 staticSwitch2_g8500 = temp_output_1_0_g8500;
				#endif
				float4 temp_output_1_0_g8491 = staticSwitch2_g8500;
				float4 break2_g8492 = temp_output_1_0_g8491;
				float temp_output_3_0_g8491 = ( ( break2_g8492.x + break2_g8492.y + break2_g8492.z ) / 3.0 );
				float clampResult25_g8491 = clamp( ( ( ( ( temp_output_3_0_g8491 + _SplitToneShift ) - 0.5 ) * _SplitTonePolarize ) + 0.5 ) , 0.0 , 1.0 );
				float3 lerpResult6_g8491 = lerp( (_SplitToneShadowsColor).rgb , (_SplitToneHighlightsColor).rgb , clampResult25_g8491);
				float saferPower26_g8491 = max( max( temp_output_3_0_g8491 , 0.0001 ) , 0.0001 );
				float3 lerpResult11_g8491 = lerp( (temp_output_1_0_g8491).rgb , ( lerpResult6_g8491 * pow( saferPower26_g8491 , _SplitToneContrast ) ) , _SplitToneFade);
				float4 appendResult18_g8491 = (float4(lerpResult11_g8491 , temp_output_1_0_g8491.a));
				#ifdef _ENABLESPLITTONE_ON
				float4 staticSwitch27_g8491 = appendResult18_g8491;
				#else
				float4 staticSwitch27_g8491 = temp_output_1_0_g8491;
				#endif
				float4 temp_output_1_0_g8493 = staticSwitch27_g8491;
				float3 hsvTorgb8_g8493 = RGBToHSV( temp_output_1_0_g8493.rgb );
				float saferPower5_g8493 = max( hsvTorgb8_g8493.z , 0.0001 );
				float3 hsvTorgb9_g8493 = HSVToRGB( float3(hsvTorgb8_g8493.x,hsvTorgb8_g8493.y,pow( saferPower5_g8493 , _Contrast )) );
				float4 appendResult4_g8493 = (float4(hsvTorgb9_g8493 , temp_output_1_0_g8493.a));
				#ifdef _ENABLECONTRAST_ON
				float4 staticSwitch7_g8493 = appendResult4_g8493;
				#else
				float4 staticSwitch7_g8493 = temp_output_1_0_g8493;
				#endif
				float4 temp_output_2_0_g8494 = staticSwitch7_g8493;
				float3 hsvTorgb1_g8494 = RGBToHSV( temp_output_2_0_g8494.rgb );
				float3 hsvTorgb3_g8494 = HSVToRGB( float3(_Hue,hsvTorgb1_g8494.y,hsvTorgb1_g8494.z) );
				float4 appendResult8_g8494 = (float4(hsvTorgb3_g8494 , temp_output_2_0_g8494.a));
				#ifdef _ENABLEHUE_ON
				float4 staticSwitch9_g8494 = appendResult8_g8494;
				#else
				float4 staticSwitch9_g8494 = temp_output_2_0_g8494;
				#endif
				float4 temp_output_6_0_g8495 = staticSwitch9_g8494;
				float4 break2_g8496 = temp_output_6_0_g8495;
				float3 temp_cast_34 = (( ( break2_g8496.x + break2_g8496.y + break2_g8496.z ) / 3.0 )).xxx;
				float3 lerpResult7_g8495 = lerp( temp_cast_34 , (temp_output_6_0_g8495).rgb , _Saturation);
				float4 appendResult3_g8495 = (float4(lerpResult7_g8495 , temp_output_6_0_g8495.a));
				#ifdef _ENABLESATURATION_ON
				float4 staticSwitch10_g8495 = appendResult3_g8495;
				#else
				float4 staticSwitch10_g8495 = temp_output_6_0_g8495;
				#endif
				float4 temp_output_2_0_g8485 = staticSwitch10_g8495;
				float4 appendResult6_g8485 = (float4(( (temp_output_2_0_g8485).rgb * _Brightness ) , temp_output_2_0_g8485.a));
				#ifdef _ENABLEBRIGHTNESS_ON
				float4 staticSwitch8_g8485 = appendResult6_g8485;
				#else
				float4 staticSwitch8_g8485 = temp_output_2_0_g8485;
				#endif
				float4 temp_output_280_0 = staticSwitch8_g8485;
				float4 lerpResult170 = lerp( temp_output_411_0 , temp_output_280_0 , fullFade153);
				float4 InputA384 = lerpResult170;
				float4 InputB384 = temp_output_280_0;
				float4 localBranchFading4384 = BranchFading4( InputA384 , InputB384 );
				#ifdef _ENABLEFADEWITHALPHA_ON
				float staticSwitch360 = _Transparency;
				#else
				float staticSwitch360 = alphaColor189;
				#endif
				float4 appendResult353 = (float4(IN.color.r , IN.color.g , IN.color.b , staticSwitch360));
				
				half4 color = ( localBranchFading4384 * appendResult353 );
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "OverlayFilters.OverlayShaderGUI"
	
	
}
/*ASEBEGIN
Version=18900
573;34;1413;754;605.744;-331.6112;1.257559;True;False
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;996.0064,1367.198;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;21;1185.273,1362.588;Inherit;False;mainTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;38;1180.584,1876.282;Inherit;True;Property;_NoiseTexture;Noise Texture;42;0;Create;True;0;0;0;False;0;False;6b7b4a61603088549ac748de5e4e6d8c;6b7b4a61603088549ac748de5e4e6d8c;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.FunctionNode;59;1699.034,1327.284;Inherit;False;ShaderSpace;0;;6440;be729ef05db9c224caec82a3516038dc;0;1;3;SAMPLER2D;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;340;1192.502,2160.204;Inherit;False;ShaderTime;34;;6441;06a15e67904f217499045f361bad56e7;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;1592.682,1525.251;Inherit;True;Property;_SampleMainTexture;Sample Main Texture;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;374;1820.705,1735.144;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;373;2038.889,1629.904;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;1434.587,1876.564;Inherit;False;noiseTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;129;1435.83,2154.657;Inherit;False;shaderTime;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;109;1958.426,1326.89;Inherit;False;shaderPosition;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;132;-1338.428,-739.0534;Inherit;False;129;shaderTime;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;343;1177.82,2415.612;Inherit;False;109;shaderPosition;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GrabScreenPosition;157;-3152.023,-114.9025;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;112;-1347.803,-636.4209;Inherit;False;109;shaderPosition;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;344;1184.82,2515.612;Inherit;False;39;noiseTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;189;2241.682,1683.667;Inherit;False;alphaColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;113;-1341.105,-551.0184;Inherit;False;39;noiseTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FunctionNode;283;-1129.898,-626.2079;Inherit;False;OverlayGlitchPre;168;;7342;b8ad29d751d87bd4d9cbf14898be6163;0;3;19;FLOAT;0;False;18;FLOAT2;0,0;False;16;SAMPLER2D;0;False;2;FLOAT;15;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;158;-2888.881,-85.84608;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;181;1612.852,2648.918;Inherit;False;189;alphaColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;359;1468.051,2411.374;Inherit;False;ShaderFade;8;;7344;5243780ec23f1e1448708f4ce06b4c4e;0;3;23;FLOAT;0;False;3;FLOAT2;0,0;False;4;SAMPLER2D;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-2623.375,320.5648;Inherit;False;39;noiseTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;117;-781.1174,-579.2969;Inherit;False;glitchPosition;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;111;-2633.145,236.8364;Inherit;False;109;shaderPosition;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;116;-778.1174,-675.2969;Inherit;False;glitchFade;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;161;-2649.242,-10.90055;Inherit;False;grabScreenPosition;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;180;1824.073,2567.489;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;134;-2613.193,155.2851;Inherit;False;129;shaderTime;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;118;-2250.647,520.5081;Inherit;False;116;glitchFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;119;-2248,365.3318;Inherit;False;117;glitchPosition;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;130;-2244.937,276.3733;Inherit;False;129;shaderTime;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;114;-2248.435,444.3287;Inherit;False;39;noiseTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.StaticSwitch;361;1983.954,2414.367;Inherit;False;Property;_Keyword2;Keyword 2;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;360;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;396;-2368.338,110.7642;Inherit;False;_Position;139;;7848;3aeee69b69420494f962d2d06f99ddc5;0;4;27;FLOAT2;0,0;False;22;FLOAT;0;False;6;FLOAT2;0,0;False;5;SAMPLER2D;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;336;-1953.346,300.3356;Inherit;False;OverlayGlitchUV;178;;7864;2addb21417fb5d745a5abfe02cbcd453;0;5;23;FLOAT;0;False;13;FLOAT2;0,0;False;22;SAMPLER2D;0;False;3;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;153;2369.219,2507.688;Inherit;False;fullFade;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;164;-1110.174,237.4903;Inherit;False;153;fullFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;-1127.852,21.64797;Inherit;False;161;grabScreenPosition;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;115;-1569.413,106.6302;Inherit;False;Property;_EnableGlitch;Enable Glitch;167;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;160;-811.2774,87.65822;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;110;-444.8699,-64.18593;Inherit;False;109;shaderPosition;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;190;-325.6071,188.8555;Inherit;False;153;fullFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;381;-602.8113,185.4381;Inherit;False;#if defined(_ENABLEFADEWITHALPHA_ON) || defined(_ENABLEFADENOISESCROLL_ON) || defined(_ENABLEFADEDISSOLVE_ON) || defined(_ENABLEFADESPREAD_ON) || defined(_ENABLEFADESINE_ON)$return InputA@$#else$return InputB@$#endif;2;False;2;False;InputA;FLOAT2;0,0;In;;Inherit;False;True;InputB;FLOAT2;0,0;In;;Inherit;False;BranchFading2;False;False;0;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;121;-7.772345,265.2681;Inherit;False;117;glitchPosition;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;131;3.055775,187.3723;Inherit;False;129;shaderTime;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;122;-7.772398,354.1108;Inherit;False;39;noiseTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;-3.255022,435.4243;Inherit;False;116;glitchFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;411;-111.3018,15.60661;Inherit;False;_Sample;109;;8506;9b7fc20dc45a5454c991b15678891b18;0;3;31;FLOAT2;0,0;False;1;FLOAT2;0,0;False;50;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;282;258.6629,284.8721;Inherit;False;OverlayGlitch;173;;8480;97a01281f94bcc04fbb9a7c1cd328f08;0;5;34;FLOAT;0;False;31;FLOAT2;0,0;False;33;SAMPLER2D;0;False;29;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;272;712.6942,145.5141;Inherit;False;109;shaderPosition;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;271;724.6942,216.5141;Inherit;False;39;noiseTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.StaticSwitch;123;717.5644,47.79606;Inherit;False;Property;_Keyword4;Keyword 4;167;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;115;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;273;747.6942,291.5141;Inherit;False;129;shaderTime;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;280;1011.876,95.09289;Inherit;False;_Color;43;;8484;87857c725b66a6f4a93d0f2a3f185435;0;4;1;COLOR;0,0,0,0;False;35;FLOAT2;0,0;False;36;SAMPLER2D;0;False;37;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;169;1080.417,331.8592;Inherit;False;153;fullFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;362;788.1863,647.9692;Inherit;False;189;alphaColor;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;352;740.0049,776.1812;Inherit;False;Property;_Transparency;Transparency;7;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;3;762.5627,447.8728;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;170;1359.725,250.8887;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;360;1071.544,660.3877;Inherit;True;Property;_EnableFadeWithAlpha;Enable Fade With Alpha;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;384;1508.437,106.6369;Inherit;False;#if defined(_ENABLEFADEWITHALPHA_ON) || defined(_ENABLEFADENOISESCROLL_ON) || defined(_ENABLEFADEDISSOLVE_ON) || defined(_ENABLEFADESPREAD_ON) || defined(_ENABLEFADESINE_ON)$return InputA@$#else$return InputB@$#endif;4;False;2;True;InputA;FLOAT4;0,0,0,0;In;;Inherit;False;True;InputB;FLOAT4;0,0,0,0;In;;Inherit;False;BranchFading4;False;False;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;353;1400.646,510.2042;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;1681.376,303.9494;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1994.455,296.1124;Float;False;True;-1;2;OverlayFilters.OverlayShaderGUI;0;6;Overlay Filters/Overlay G5;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;True;0;True;-9;False;False;False;False;False;False;False;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;21;0;1;0
WireConnection;59;3;21;0
WireConnection;2;0;21;0
WireConnection;373;0;2;4
WireConnection;373;1;374;4
WireConnection;39;0;38;0
WireConnection;129;0;340;0
WireConnection;109;0;59;0
WireConnection;189;0;373;0
WireConnection;283;19;132;0
WireConnection;283;18;112;0
WireConnection;283;16;113;0
WireConnection;158;0;157;0
WireConnection;359;23;340;0
WireConnection;359;3;343;0
WireConnection;359;4;344;0
WireConnection;117;0;283;0
WireConnection;116;0;283;15
WireConnection;161;0;158;0
WireConnection;180;0;359;0
WireConnection;180;1;181;0
WireConnection;361;1;359;0
WireConnection;361;0;180;0
WireConnection;396;27;161;0
WireConnection;396;22;134;0
WireConnection;396;6;111;0
WireConnection;396;5;45;0
WireConnection;336;23;130;0
WireConnection;336;13;119;0
WireConnection;336;22;114;0
WireConnection;336;3;118;0
WireConnection;336;1;396;0
WireConnection;153;0;361;0
WireConnection;115;1;396;0
WireConnection;115;0;336;0
WireConnection;160;0;162;0
WireConnection;160;1;115;0
WireConnection;160;2;164;0
WireConnection;381;0;160;0
WireConnection;381;1;115;0
WireConnection;411;31;110;0
WireConnection;411;1;381;0
WireConnection;411;50;190;0
WireConnection;282;34;131;0
WireConnection;282;31;121;0
WireConnection;282;33;122;0
WireConnection;282;29;120;0
WireConnection;282;1;411;0
WireConnection;123;1;411;0
WireConnection;123;0;282;0
WireConnection;280;1;123;0
WireConnection;280;35;272;0
WireConnection;280;36;271;0
WireConnection;280;37;273;0
WireConnection;170;0;411;0
WireConnection;170;1;280;0
WireConnection;170;2;169;0
WireConnection;360;1;362;0
WireConnection;360;0;352;0
WireConnection;384;0;170;0
WireConnection;384;1;280;0
WireConnection;353;0;3;1
WireConnection;353;1;3;2
WireConnection;353;2;3;3
WireConnection;353;3;360;0
WireConnection;4;0;384;0
WireConnection;4;1;353;0
WireConnection;0;0;4;0
ASEEND*/
//CHKSM=B15891B94ABCC49518D313A43FA205546DFDA0E6