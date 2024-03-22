using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UI;

namespace OverlayFilters
{
    public class OverlayShaderGUI : ShaderGUI
    {
        static bool showProperties;

        //Categories:
        public static bool showPresets = true;
        public static bool showInfos;
        public static bool showMainTexture = true;
        public static bool showUIMasking;
        public static bool showShaderTime;
        public static bool showShaderSpace;
        public static bool showNoise;
        public static bool showShaderFade;
        public static bool showReflect;

        //Reset:
        static Shader currentDefaultShader;
        static float[] defaultFloats;
        static Vector4[] defaultVectors;
        static Color[] defaultColors;

        //Toggle Individual Properties:
        static float lastShaderSpace;
        static float lastShaderFading;
        static bool lastFadeWithAlpha;
        static float lastReflectLocalMirror;

        //Color:
        Color hueColor;

        //Shader Count:
        static int enabledShaders;
        static int maxShaders;

        //Check Texture:
        double lastCheckTime = 0;
        TextureImporter textureImporter;
        bool hasTextureIssue;
        bool hasAlphaIssue;
        static bool ignoreTextureIssue;

        //Presets:
        static string folderPath = "";

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material mat = (Material)materialEditor.target;

            Shader shader = mat.shader;

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.richText = true;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.richText = true;

            GUIStyle iconStyle = new GUIStyle(GUI.skin.label);
            iconStyle.contentOffset = new Vector2(0, 1);

            if(mat != null && EditorApplication.timeSinceStartup > lastCheckTime + 0.5)
            {
                lastCheckTime = EditorApplication.timeSinceStartup;
                Texture texture = mat.GetTexture("_MainTex");

                if(Selection.activeGameObject != null)
                {
                    SpriteRenderer spriteRenderer = Selection.activeGameObject.GetComponent<SpriteRenderer>();
                    if(spriteRenderer != null && mat == spriteRenderer.sharedMaterial && spriteRenderer.sprite != null)
                    {
                        texture = spriteRenderer.sprite.texture;
                    }

                    Image image = Selection.activeGameObject.GetComponent<Image>();
                    if(image != null && image.material == mat && image.sprite != null)
                    {
                        texture = image.sprite.texture;
                    }
                }

                if(texture != null)
                {
                    if(AssetDatabase.Contains(texture))
                    {
                        textureImporter = (TextureImporter) AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture));

                        if (textureImporter != null)
                        {
                            TextureImporterSettings tis = new TextureImporterSettings();
                            textureImporter.ReadTextureSettings(tis);

                            if (tis.spriteMeshType == SpriteMeshType.Tight)
                            {
                                hasTextureIssue = true;

                                if(ignoreTextureIssue == false)
                                {
                                    if (EditorUtility.DisplayDialog("Fix TextureImport Settings",
                                        "Texture's import settings may cause problems.\n" +
                                        "\n" +
                                        "Please set the texture's mesh type to full rect.\n" +
                                        "The texture in question is called: " + texture.name + "\n" +
                                        "\n" +
                                        "If you want I can fix it for you :)" +
                                        "", "Fix", "Ignore"))
                                    {
                                        Undo.RecordObject(textureImporter, "Set mesh-type to full-rect.");
                                        tis.spriteMeshType = SpriteMeshType.FullRect;
                                        textureImporter.SetTextureSettings(tis);
                                        textureImporter.SaveAndReimport();
                                        hasTextureIssue = false;
                                    }
                                    else
                                    {
                                        ignoreTextureIssue = true;
                                    }
                                }
                            }
                            else
                            {
                                hasTextureIssue = false;
                            }
                        }
                    }


                }
            }

            #region Reset Preperation
            if (currentDefaultShader != shader || defaultFloats == null)
            {
                currentDefaultShader = shader;

                int propCount = ShaderUtil.GetPropertyCount(shader);

                defaultFloats = new float[propCount];
                defaultVectors = new Vector4[propCount];
                defaultColors = new Color[propCount];

                Material defaultMaterial = new Material(shader);

                for (int n = 0; n < propCount; n++)
                {
                    string propName = ShaderUtil.GetPropertyName(shader, n);

                    switch (ShaderUtil.GetPropertyType(shader, n))
                    {
                        case (ShaderUtil.ShaderPropertyType.Float):
                            defaultFloats[n] = defaultMaterial.GetFloat(propName);
                            break;
                        case (ShaderUtil.ShaderPropertyType.Range):
                            defaultFloats[n] = defaultMaterial.GetFloat(propName);
                            break;
                        case (ShaderUtil.ShaderPropertyType.Vector):
                            defaultVectors[n] = defaultMaterial.GetVector(propName);
                            break;
                        case (ShaderUtil.ShaderPropertyType.Color):
                            defaultColors[n] = defaultMaterial.GetColor(propName);
                            break;
                        default:
                            break;
                    }
                }
            }
            #endregion

            //Top:
            EditorGUILayout.BeginVertical("Helpbox");
            EditorGUILayout.LabelField("<b><size=16>Overlay Filters</size></b>", labelStyle);
            EditorGUILayout.Space();

            //Shader Switch:
            GUI.color = new Color(1, 1, 1, 0.5f);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.color = Color.white;
            EditorGUILayout.LabelField("<b><size=13>Overlay Groups</size></b>", labelStyle);
            string[] shaderName = mat.shader.name.Split('/');
            string currentShader = shaderName[shaderName.Length - 1];
            int currentIndex = -1;
            switch(currentShader)
            {
                case ("Overlay"):
                    currentIndex = 0;
                    break;
                case ("Overlay G1"):
                    currentIndex = 1;
                    break;
                case ("Overlay G2"):
                    currentIndex = 2;
                    break;
                case ("Overlay G3"):
                    currentIndex = 3;
                    break;
                case ("Overlay G4"):
                    currentIndex = 4;
                    break;
                case ("Overlay G5"):
                    currentIndex = 5;
                    break;
            }
            int newIndex = GUILayout.Toolbar(currentIndex, new string[] { "Ungrouped", "Group 1", "Group 2", "Group 3", "Group 4", "Group 5" });
            if(newIndex != currentIndex)
            {
                if(newIndex < 1)
                {
                    mat.shader = Shader.Find("Overlay Filters/Overlay");
                }
                else
                {
                    mat.shader = Shader.Find("Overlay Filters/Overlay G" + newIndex);
                }
            }
            Lines();
            hasAlphaIssue = false;
            GUI.color = new Color(1, 1, 1, 0.7f);
            if (newIndex < 1)
            {
                EditorGUILayout.LabelField("Every overlay creates a new <b>grab-pass</b>.", labelStyle);
                EditorGUILayout.LabelField("Not very <b>efficient</b> but overlays can be layered properly.", labelStyle);
                EditorGUILayout.LabelField("Not recommended for <b>mobile</b>, <b>particles</b> or <b>frequent</b> usage.", labelStyle);
            }
            else
            {
                if(lastFadeWithAlpha)
                {
                    hasAlphaIssue = true;
                }

                EditorGUILayout.LabelField("The first overlay of <b>group " + newIndex + "</b> creates a <b>grab-pass</b>.", labelStyle);
                EditorGUILayout.LabelField("Very <b>efficient</b> but overlays do not stack when layered.", labelStyle);
                EditorGUILayout.LabelField("Recommended for <b>mobile</b>, <b>particles</b> and <b>frequent</b> usage.", labelStyle);
            }
            GUI.color = Color.white;
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();
            CreateCategory("Presets", ref showPresets, ref labelStyle, ref buttonStyle, "Box");
            if(showPresets)
            {
                //Prepare:
                if (folderPath == null || folderPath == "")
                {
                    GetFolderPath(shader);
                }

                EditorGUILayout.BeginHorizontal();
                PresetButton("Painting", mat);
                PresetButton("Screen", mat);
                PresetButton("Neon", mat);
                PresetButton("Poison", mat);
                PresetButton("GameBoy", mat, "Game Boy");
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                PresetButton("Underwater", mat);
                PresetButton("Drunk", mat);
                PresetButton("Sepia", mat);
                PresetButton("Rage", mat);
                PresetButton("FancyGlitch", mat, "Fancy Glitch");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            CreateCategory("Information", ref showInfos, ref labelStyle, ref buttonStyle, "Box");
            if (showInfos)
            {
                GUI.color = new Color(1, 1, 1, 0.7f);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("<b>Usage:</b>", labelStyle);
                EditorGUILayout.LabelField("Simply assign this material to a <b>UI Image</b>, <b>Sprite</b> or <b>Particle</b>.", labelStyle);
                EditorGUILayout.LabelField("Everything rendered <b>behind</b> will be filtered by the enabled shaders.", labelStyle);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("<b>Performance:</b>", labelStyle);
                EditorGUILayout.LabelField("Only <b>enabled filters</b> and <b>features</b> use gpu performance.", labelStyle);
                EditorGUILayout.LabelField("Using <b>overlay-groups</b> significantly <b>boosts performance</b>.", labelStyle);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("<b>Stacking:</b>", labelStyle);
                EditorGUILayout.LabelField("Shaders can be stacked in the same <b>material</b> or <b>overlay</b>.", labelStyle);
                EditorGUILayout.LabelField("Overlays can be stacked by being layered on top of each other.", labelStyle);
                EditorGUILayout.LabelField("Sprite overlays won't stack if their <b>render order</b> is exactly the same.", labelStyle);
                EditorGUILayout.LabelField("They also won't stack if they are on the same <b>overlay-group</b>.", labelStyle);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("<b>Fading:</b>", labelStyle);
                EditorGUILayout.LabelField("There are several options in the <b>Fade Settings</b> section.", labelStyle);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("<b>Particles:</b>", labelStyle);
                EditorGUILayout.LabelField("Make sure to use an <b>overlay-group</b> for your particles.", labelStyle);
                EditorGUILayout.LabelField("Also don't use <b>Fade With Alpha</b>.", labelStyle);

                GUI.color = Color.white;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Enabled <b>" + enabledShaders + "</b> out of <b>" + maxShaders + "</b> shaders.", labelStyle);

            //Count Shaders:
            enabledShaders = maxShaders = 0;

            EditorGUILayout.EndVertical();

            //Warnings:
            if (hasTextureIssue)
            {
                EditorGUILayout.Space();
                GUI.color = new Color(1, 0.9f, 0.5f);
                EditorGUILayout.BeginVertical("Helpbox");
                EditorGUILayout.LabelField("Texture's <b>Mesh Type</b> should be set to <b>Full Rect</b>.", labelStyle);
                EditorGUILayout.LabelField("Fix the issue in the texture's inspector or click the button below.", labelStyle);
                EditorGUILayout.Space();
                if(GUILayout.Button("Fix Issue"))
                {
                    TextureImporterSettings tis = new TextureImporterSettings();
                    textureImporter.ReadTextureSettings(tis);
                    Undo.RecordObject(textureImporter, "Set mesh-type to full-rect.");
                    tis.spriteMeshType = SpriteMeshType.FullRect;
                    textureImporter.SetTextureSettings(tis);
                    textureImporter.SaveAndReimport();
                    hasTextureIssue = false;
                }
                EditorGUILayout.EndVertical();
                GUI.color = Color.white;
            }
            if(hasAlphaIssue)
            {
                EditorGUILayout.Space();
                GUI.color = new Color(1, 0.9f, 0.5f);
                EditorGUILayout.BeginVertical("Helpbox");
                EditorGUILayout.LabelField("Overlay groups do not support <b>Fade With Alpha</b>.", labelStyle);
                EditorGUILayout.LabelField("Please disable <b>Fade With Alpha</b>.", labelStyle);
                EditorGUILayout.EndVertical();
                GUI.color = Color.white;
            }

            EditorGUILayout.BeginVertical();

            //Properties:
            for (int p = 0; p < properties.Length; p++)
            {
                //Get Property
                MaterialProperty prop = properties[p];

                #region Name and Tooltip
                string propName = prop.name;
                string displayName = prop.displayName;
                
                #region Remove Prefix
                string[] splitNames = displayName.Split(':');
                if(splitNames.Length > 1)
                {
                    displayName = splitNames[1].Substring(1);
                }
                #endregion

                GUIContent displayContent = new GUIContent(displayName, propName + "  (C#)");
                #endregion

                #region Skip Property
                switch (propName)
                {
                    case ("_texcoord"):
                        continue;
                    case ("_PixelsPerUnit"):
                        if ((lastShaderSpace > 0.9f && lastShaderSpace < 4.9f) || lastShaderSpace > 5.9f)
                        {
                            continue;
                        }
                        break;
                    case ("_RectWidth"):
                        if (lastShaderSpace < 4.9f || lastShaderSpace > 5.9f)
                        {
                            continue;
                        }
                        break;
                    case ("_RectHeight"):
                        if (lastShaderSpace < 4.9f || lastShaderSpace > 5.9f)
                        {
                            showProperties = false; //To still display suffix.
                        }
                        break;
                    case ("_ScreenWidthUnits"):
                        if (lastShaderSpace < 5.9f)
                        {
                            continue;
                        }
                        break;
                    case ("_FadingMask"):
                        if (lastShaderFading < 1.5f || lastShaderFading > 2.5f)
                        {
                            showProperties = false; //To still display suffix.
                        }
                        break;
                    case ("_FullFade"):
                        if (lastShaderFading < 0.5f)
                        {
                            continue;
                        }
                        break;
                    case ("_FadingNoiseScale"):
                        if (lastShaderFading < 2.5f)
                        {
                            continue;
                        }
                        break;
                    case ("_FadingWidth"):
                        if (lastShaderFading < 2.5f)
                        {
                            continue;
                        }
                        break;
                    case ("_FadingPosition"):
                        if (lastShaderFading < 3.5f)
                        {
                            continue;
                        }
                        break;
                    case ("_FadingNoiseFactor"):
                        if (lastShaderFading < 3.5f)
                        {
                            continue;
                        }
                        break;
                    case ("_ReflectLocalMirror"):
                        lastReflectLocalMirror = prop.floatValue;
                        break;
                    case ("_ReflectMirrorHeight"):
                        if (lastReflectLocalMirror > 0.5f)
                        {
                            showProperties = false;
                        }
                        break;
                }
                #endregion

                #region Categories
                //Custom Categories:
                switch (propName)
                {
                    case ("_MainTex"):
                        CreateCategory("Main Texture", ref showMainTexture, ref labelStyle, ref buttonStyle);
                        displayContent.text = "Texture";
                        break;
                    case ("_StencilComp"):
                        CreateCategory("UI Masking", ref showUIMasking, ref labelStyle, ref buttonStyle);
                        break;
                    case ("_EnableTimeUnscaled"):
                        CreateCategory("Time Settings", ref showShaderTime, ref labelStyle, ref buttonStyle);
                        break;
                    case ("_EnableFadeWithAlpha"):
                        lastFadeWithAlpha = prop.floatValue > 0.5f;
                        CreateCategory("Fade Settings", ref showShaderFade, ref labelStyle, ref buttonStyle);
                        break;
                    case ("_ShaderSpace"):
                        CreateCategory("Space Settings", ref showShaderSpace, ref labelStyle, ref buttonStyle);
                        lastShaderSpace = prop.floatValue;
                        break;
                    case ("_NoiseTexture"):
                        CreateCategory("Noise Texture", ref showNoise, ref labelStyle, ref buttonStyle);
                        break;
                    case ("_EnableReflect"):
                        showReflect = prop.floatValue > 0.5f;
                        break;
                }
                //Shader Categories:
                if (propName.StartsWith("_Enable"))
                {
                    if (propName.StartsWith("_EnableTime"))
                    {
                        if (showShaderTime)
                        {
                            Lines();

                            showProperties = true;
                        }
                    }
                    else if(propName.StartsWith("_EnableFade"))
                    {
                        if (showShaderFade)
                        {
                            Lines();

                            showProperties = true;
                        }
                    }
                    else
                    {
                        showProperties = true;

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();

                        float brightness = prop.floatValue > 0.5f ? (EditorGUIUtility.isProSkin ? 0f : 0.5f) : 1f;
                        GUI.color = new Color(brightness, brightness, brightness, 1);
                        EditorGUILayout.BeginVertical("Helpbox");
                    }
                }
                #endregion

                #region Display Property

                //Property Type:
                ShaderUtil.ShaderPropertyType propType = ShaderUtil.ShaderPropertyType.TexEnv;
                if (ShaderUtil.GetPropertyCount(shader) >= properties.Length && prop == properties[p])
                {
                    propType = ShaderUtil.GetPropertyType(shader, p);
                }

                //Display:
                EditorGUILayout.BeginHorizontal();
                if (showProperties)
                {
                    if (prop.hasMixedValue)
                    {
                        EditorGUI.showMixedValue = true;
                    }

                    //Modify Name:
                    if (propName.StartsWith("_Time"))
                    {
                        displayContent.text = displayContent.text.Substring(5);
                    }

                    if (propName.StartsWith("_Enable")) //Shader Toggles:
                    {
                        if (!propName.StartsWith("_EnableFade") && !propName.StartsWith("_EnableTime"))
                        {
                            maxShaders++;
                        }

                        GUI.color = new Color(1, 1, 1, prop.floatValue > 0.5f ? 1f : 0.7f);
                        EditorGUILayout.PrefixLabel("<b><size=14>" + (maxShaders > 0 ? maxShaders + ". " : "") + displayName.Replace("Enable ","") + "</size></b>", labelStyle, labelStyle);
                        float old = EditorGUIUtility.labelWidth;
                        EditorGUIUtility.labelWidth = 40;
                        materialEditor.ShaderProperty(prop, GUIContent.none);
                        GUI.color = Color.white;
                        EditorGUIUtility.labelWidth = old;

                        if (prop.floatValue > 0.5f)
                        {
                            bool expanded = prop.floatValue != 1.1f;

                            if (propName.StartsWith("_EnableFade") || propName.StartsWith("_EnableTime"))
                            {
                                expanded = true;
                            }
                            else
                            {
                                enabledShaders++;

                                if (GUILayout.Button("<size=10>" + (expanded ? "▼" : "▲") + "</size>", buttonStyle, GUILayout.Width(20)))
                                {
                                    expanded = !expanded;

                                    if (expanded)
                                    {
                                        prop.floatValue = 1f;
                                    }
                                    else
                                    {
                                        prop.floatValue = 1.1f;
                                    }
                                }
                            }

                            showProperties = expanded;
                        }
                        else
                        {
                            showProperties = false;
                        }
                    } else if (displayName == "Flip") //Flips:
                    {
                        EditorGUILayout.PrefixLabel("Flip");
                        prop.floatValue = EditorGUILayout.IntPopup((int)(prop.floatValue), new string[] { "Default", "Flipped" }, new int[] { 0, -1 });
                    }
                    else if (propType == ShaderUtil.ShaderPropertyType.Vector) //Vectors:
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel(displayContent);

                        Vector4 value = prop.vectorValue;
                        float oldWidth = EditorGUIUtility.labelWidth;
                        EditorGUIUtility.labelWidth = 15f;

                        value.x = EditorGUILayout.FloatField(" X", value.x);
                        value.y = EditorGUILayout.FloatField(" Y", value.y);
                        value.w = value.z = 0;

                        EditorGUILayout.EndHorizontal();
                        EditorGUIUtility.labelWidth = oldWidth;

                        if (prop.vectorValue != value)
                        {
                            prop.vectorValue = value;
                        }
                    }
                    else if (propType == ShaderUtil.ShaderPropertyType.TexEnv) //Textures:
                    {
                        Texture newTexture = (Texture)EditorGUILayout.ObjectField(displayContent, prop.textureValue, typeof(Texture), false);

                        if(newTexture != prop.textureValue)
                        {
                            prop.textureValue = newTexture;
                        }
                    }else if (propType == ShaderUtil.ShaderPropertyType.Color)
                    {
                        if(propName == "_Color" || propName.StartsWith("_Outline"))
                        {
                            prop.colorValue = EditorGUILayout.ColorField(displayContent, prop.colorValue, true, true, true);
                        }
                        else
                        {
                            prop.colorValue = EditorGUILayout.ColorField(displayContent, prop.colorValue, true, false, true);
                        }
                    }
                    else if (displayName == "Hue") //Textures:
                    {
                        materialEditor.ShaderProperty(prop, displayContent);

                        hueColor = EditorGUILayout.ColorField(GUIContent.none, Color.HSVToRGB(prop.floatValue, 1, 1), false, false, false, GUILayout.Width(20));
                        float outHue;
                        float trash;
                        Color.RGBToHSV(hueColor, out outHue, out trash, out trash);
                        if (outHue != 0 || prop.floatValue != 1)
                        {
                            prop.floatValue = outHue;
                        }
                    }else if(propName == "_FullFade" && lastShaderFading > 3.5f)
                    {
                        float newValue = EditorGUILayout.FloatField(displayContent, prop.floatValue);

                        if(prop.floatValue != newValue)
                        {
                            prop.floatValue = newValue;
                        }
                    }
                    else //Default:
                    {
                        materialEditor.ShaderProperty(prop, displayContent);
                    }

                    EditorGUI.showMixedValue = false;
                }
                #endregion

                #region Fix Values
                if (showProperties)
                {
                    switch (propName)
                    {
                        case ("_GaussianBlurOffset"):
                            if (prop.floatValue < 0f) prop.floatValue = 0f;
                            break;
                        case ("_DirectionalBlurOffset"):
                            if (prop.floatValue < 0f) prop.floatValue = 0f;
                            break;
                    }
                }
                #endregion

                #region Reset
                if (showProperties && propType != ShaderUtil.ShaderPropertyType.TexEnv && IsNotKeyword(propName,displayName))
                {
                    //Prevent Error:
                    if(p >= defaultFloats.Length)
                    {
                        currentDefaultShader = null;
                        continue;
                    }

                    GUIContent resetButton = new GUIContent();
                    resetButton.text = "R";
                    resetButton.tooltip = "Resets the property.";

                    if (propType == ShaderUtil.ShaderPropertyType.Vector) //Vector:
                    {
                        Vector4 defaultValue = defaultVectors[p];

                        if (prop.vectorValue == defaultValue)
                        {
                            GUI.color = new Color(1, 1, 1, 0.5f);
                            GUILayout.Toolbar(0, new GUIContent[] { resetButton }, GUILayout.Width(20));
                        }
                        else
                        {
                            if (GUILayout.Button(resetButton, GUILayout.Width(20)))
                            {
                                prop.vectorValue = defaultValue;
                            }
                        }
                    }
                    else if (propType == ShaderUtil.ShaderPropertyType.Color) //Color:
                    {
                        Color defaultValue = defaultColors[p];

                        if (prop.colorValue == defaultValue)
                        {
                            GUI.color = new Color(1, 1, 1, 0.5f);
                            GUILayout.Toolbar(0, new GUIContent[] { resetButton }, GUILayout.Width(20));
                        }
                        else
                        {
                            if (GUILayout.Button(resetButton, GUILayout.Width(20)))
                            {
                                prop.colorValue = defaultValue;
                            }
                        }
                    }
                    else //Float:
                    {
                        float defaultValue = defaultFloats[p];

                        if (prop.floatValue == defaultValue)
                        {
                            GUI.color = new Color(1, 1, 1, 0.5f);
                            GUILayout.Toolbar(0, new GUIContent[] { resetButton }, GUILayout.Width(20));
                        }
                        else
                        {
                            if (GUILayout.Button(resetButton, GUILayout.Width(20)))
                            {
                                prop.floatValue = defaultValue;
                            }
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                GUI.color = new Color(1, 1, 1, 1f);
                #endregion

                #region Suffix Information
                if(showProperties)
                {
                    //Lines:
                    if (propName.EndsWith("Fade") && propName != "_FullFade" && propName != "_NegativeFade" && !propName.StartsWith("_Fade"))
                    {
                        Lines();
                    }
                    switch (propName)
                    {
                        case ("_SplitToneContrast"):
                            Lines();
                            break;
                        case ("_ZoomBlurPower"):
                            Lines();
                            break;
                        case ("_DistortOffset"):
                            Lines();
                            break;
                        case ("_SnapDistortOffset"):
                            Lines();
                            break;
                        case ("_SnapDistortSnapLevels"):
                            Lines();
                            break;
                        case ("_GlitchMaskSpeed"):
                            Lines();
                            break;
                        case ("_GlitchBrightness"):
                            Lines();
                            break;
                        case ("_GlitchNoiseSpeed"):
                            Lines();
                            break;
                        case ("_FadingMethod"):
                            if(lastShaderFading > 0.5f)
                            {
                                Lines();
                            }
                            break;
                        case ("_OutlineSaturationTolerance"):
                            Lines();
                            break;
                        case ("_OutlineLineColor"):
                            Lines();
                            break;
                        case ("_ColorStepsColor0"):
                            Lines();
                            break;
                        case ("_ColorStepsColor1"):
                            Lines();
                            break;
                        case ("_ColorStepsColor2"):
                            Lines();
                            break;
                        case ("_RainbowContrast"):
                            Lines();
                            break;
                        case ("_RainbowCenter"):
                            Lines();
                            break;
                        case ("_GaussianBlurOffset"):
                            Lines();
                            GUI.color = new Color(1, 1, 1, 0.7f);
                            EditorGUILayout.LabelField("Has to be stacked for a <b>smoother</b> blur effect.", labelStyle);
                            EditorGUILayout.LabelField("Look into the <b>demo</b> scene for a demonstration.", labelStyle);
                            GUI.color = Color.white;
                            break;
                        case ("_DirectionalBlurDirection"):
                            Lines();
                            GUI.color = new Color(1, 1, 1, 0.7f);
                            EditorGUILayout.LabelField("Has to be stacked for a <b>smoother</b> blur effect.", labelStyle);
                            EditorGUILayout.LabelField("Look into the <b>demo</b> scene for a demonstration.", labelStyle);
                            GUI.color = Color.white;
                            break;
                        case ("_ZoomBlurMaxRadius"):
                            Lines();
                            GUI.color = new Color(1, 1, 1, 0.7f);
                            EditorGUILayout.LabelField("Has to be stacked for a <b>smoother</b> blur effect.", labelStyle);
                            EditorGUILayout.LabelField("Look into the <b>demo</b> scene for a demonstration.", labelStyle);
                            GUI.color = Color.white;
                            break;
                        case ("_EnableTimeUnscaled"):
                            if (prop.floatValue > 0.5f)
                            {
                                GUI.color = new Color(1, 1, 1, 0.7f);
                                EditorGUILayout.LabelField("Requires a single active <b>UnscaledTime</b> component in your scene.", labelStyle);
                                GUI.color = Color.white;
                            }
                            break;
                    }

                    //Group Information:
                    switch (propName)
                    {
                        case ("_UseUIAlphaClip"):
                            GroupInformation(
                                "These properties are used for <b>UI Masking</b>.",
                                "You can leave them at their default values."
                                );
                            break;
                        case ("_NoiseTexture"):
                            GroupInformation(
                                "This texture is used by all noise-based shaders."
                                );
                            break;
                        case ("_Brightness"):
                            GroupInformation(
                                "Changes the brightness."
                                );
                            break;
                        case ("_Hue"):
                            GroupInformation(
                                "Replaces the hue."
                                );
                            break;
                        case ("_Saturation"):
                            GroupInformation(
                                "Changes the saturation."
                                );
                            break;
                        case ("_Contrast"):
                            GroupInformation(
                                "Changes the contrast."
                                );
                            break;
                        case ("_SingleToneContrast"):
                            GroupInformation(
                                "Replaces the color with a single tone."
                                );
                            break;
                        case ("_SplitToneShift"):
                            GroupInformation(
                                "Replaces brighter and darker colors separately."
                                );
                            break;
                        case ("_DistortOppositeFlow"):
                            GroupInformation(
                                "Distorts using a noise texture."
                                );
                            break;
                        case ("_SnapDistortOppositeFlow"):
                            GroupInformation(
                                "Distorts using a noise texture but also snaps distortion level.",
                                "Was created for glitch-like distortion effects."
                                );
                            break;
                        case ("_PixelatePixels"):
                            int pixelCount = Mathf.RoundToInt(prop.floatValue);
                            GroupInformation(
                                "Renders in <b>" + pixelCount + "</b> pixels of screen height.",
                                "At an aspect ratio of 16:9 this results in <b>" + Mathf.RoundToInt(1.77777f * prop.floatValue) + "x" + pixelCount + "</b> pixels."
                                );
                            break;
                        case ("_GlitchDistortionSpeed"):
                            GroupInformation(
                                "Distorts and recolors to create a glitch-like effect.",
                                "Combine with other shaders to get a <b>fancier</b> glitch effect."
                                );
                            break;
                        case ("_RippleCenter"):
                            GroupInformation(
                                "Intended for a ripple effect when used with the <b>ring</b> shape.",
                                (Mathf.Abs(lastShaderSpace - 1) > 0.2f) ? "<color=#FF1111>I recommend using the <b>UV_Raw</b> shader space !</color>" : "You can find an example in the demo scene."
                                );
                            break;
                        case ("_OutlineFillColor"):
                            GroupInformation(
                                "Tries to draw outlines by detecting color differences."
                                );
                            break;
                        case ("_NegativeFade"):
                            GroupInformation(
                                "Negates the color."
                                );
                            break;
                        case ("_ColorStepsColor3"):
                            GroupInformation(
                                "Replaces color with 4 different colors based on brightness."
                                );
                            break;
                        case ("_LimitColorsSaturationSteps"):
                            GroupInformation(
                                "Snaps color to a limited set of tones.",
                                "Can be used to create a <b>painting</b> effect."
                                );
                            break;
                        case ("_LightSaturation"):
                            GroupInformation(
                                "A more realistic light simulation than a simple additive shader."
                                );
                            break;
                        case ("_RainbowNoiseFactor"):
                            GroupInformation(
                                "Zooms a rainbow towards the center position.",
                                "Set <b>Zoom Density</b> to <b>0</b> to disable the zoom effect."
                                );
                            break;
                        case ("_SharpenIntensity"):
                            GroupInformation(
                                "Sharpens the image using the <b>Unsharp Masking</b> technique."
                                );
                            break;

                    }
                }
                if(showShaderSpace && propName == "_RectHeight")
                {
                    GroupInformation(
                        "The space for shader <b>patterns</b> and <b>noise</b>."
                        );
                }
                if (showShaderTime && propName == "_TimeValue")
                {
                    GroupInformation(
                        "Controls the time for all shader <b>animations</b>."
                        );
                }
                if(showShaderFade && propName == "_FadingMask")
                {
                    if(lastShaderFading < 0.5f)
                    {
                        GroupInformation(
                            "Select a fading method to fade all shaders together."
                            );
                    }else if(lastShaderFading < 1.5f)
                    {
                        GroupInformation(
                            "Fades everything uniformly."
                            );
                    }
                    else if (lastShaderFading < 2.5f)
                    {
                        GroupInformation(
                            "Fades everything using a grayscale mask texture."
                            );
                    }
                    else if (lastShaderFading < 3.5f)
                    {
                        GroupInformation(
                            "Fades everything using a dissolve pattern."
                            );
                    }
                    else
                    {
                        GroupInformation(
                            "Fades everything by spreading from a source position."
                            );
                    }
                }
                if(showReflect && propName == "_ReflectMirrorHeight")
                {
                    Lines();

                    GUI.color = new Color(1, 1, 1, 0.7f);
                    EditorGUILayout.LabelField("Vertically reflects screen-pixels.", labelStyle);
                    EditorGUILayout.LabelField("Should not get <b>too close</b> to the <b>top</b> of the screen.", labelStyle);
                    EditorGUILayout.LabelField("", labelStyle);

                    if (lastReflectLocalMirror > 0.5f)
                    {
                        EditorGUILayout.LabelField("<b>Local:</b>", labelStyle);
                        EditorGUILayout.LabelField("Use <b>HardReflection.png</b> or <b>SoftReflection.png</b> for the sprite.", labelStyle);
                    }
                    else
                    {
                        EditorGUILayout.LabelField("<b>World:</b>", labelStyle); 
                            EditorGUILayout.LabelField("Input the height in world-space that we mirror at.", labelStyle);
                    }

                    GUI.color = Color.white;
                }
                #endregion
            }

            //Finish:
            EditorGUILayout.EndVertical();
            DisplayFinalInformation();
        }

        /*
        static void CategoryIcon(string fileName, GUIStyle style, bool enabled)
        {
            Color oldColor = GUI.color;

            GUI.color = new Color(1, 1, 1, enabled ? 1f : 0.6f);

            GUILayout.Label(AssetDatabase.LoadAssetAtPath<Texture>(folderPath + "GUI/" + fileName + ".png"), style, GUILayout.Height(18), GUILayout.Width(18));

            GUI.color = oldColor;
        }
        */

        static void PresetButton(string presetName, Material material, string customName = "")
        {
            if(GUILayout.Button(customName != "" ? customName : presetName))
            {
                GetPreset(presetName, material);
            }
        }
        static void GetFolderPath(Shader shader)
        {
            if(shader != null)
            {
                string shaderPath = AssetDatabase.GetAssetPath(shader);
                string[] split = shaderPath.Split('/');

                int goBack = shader.name.EndsWith("URP") ? 3 : 2;

                folderPath = "";
                for(int n = 0; n < split.Length - goBack; n++)
                {
                    folderPath += split[n] + "/";
                }
            }
        }
        static void GetPreset(string presetName, Material material)
        {
            if (material == null) return;

            Material preset = AssetDatabase.LoadAssetAtPath<Material>(folderPath + "Presets/Preset" + presetName + ".mat");

            bool retry = false;
            if(preset == null)
            {
                retry = true;
            }

            if(retry)
            {
                if (material.shader == null) return;

                GetFolderPath(material.shader);
                preset = AssetDatabase.LoadAssetAtPath<Material>(folderPath + "Presets/Preset" + presetName + ".mat");
            }

            if(preset == null)
            {
                Debug.Log("Preset was not found.\nThis can happen if the folder structure of the asset has been changed.");
            }
            else
            {
                //Copy:
                Undo.RecordObject(material, "Loaded the " + presetName + " Preset");
                material.CopyPropertiesFromMaterial(preset);
                InternalEditorUtility.RepaintAllViews();
            }
        }

        static void Lines()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.richText = true;
            labelStyle.alignment = TextAnchor.MiddleCenter;

            GUI.color = new Color(1, 1, 1, 0.5f);

            EditorGUILayout.LabelField("<b><size=15> - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - </size></b>", labelStyle, GUILayout.Height(7));

            GUI.color = Color.white;
        }

        static void GroupInformation(params string[] lines)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;

            Lines();

            GUI.color = new Color(1, 1, 1, 0.7f);

            for (int l = 0; l < lines.Length; l++)
            {
                EditorGUILayout.LabelField(lines[l], style);
            }

            GUI.color = Color.white;
        }

        static void CreateCategory(string categoryTitle, ref bool toggleVariable, ref GUIStyle labelStyle, ref GUIStyle buttonStyle, string verticalInput = "Helpbox")
        {
            buttonStyle.richText = true;

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            if(verticalInput == "Box")
            {
                GUI.color = new Color(1, 1, 1, 0.5f);
            }
            EditorGUILayout.BeginVertical(verticalInput);
            EditorGUILayout.BeginHorizontal();
            GUI.color = Color.white;
            EditorGUILayout.LabelField("<b><size=13>" + categoryTitle + "</size></b>", labelStyle);
            if (GUILayout.Button("<size=10>" + (toggleVariable ? "▼" : "▲") + "</size>", buttonStyle, GUILayout.Width(20)))
            {
                toggleVariable = !toggleVariable;
            }

            showProperties = toggleVariable;

            EditorGUILayout.EndHorizontal();
        }

        static bool IsNotKeyword(string propName, string displayName)
        {
            return propName != "_UseUIAlphaClip" && propName != "_FadingMethod" && !displayName.EndsWith("_Toggle") && displayName != "Shader Space" && displayName != "Uber Fading" && displayName != "Time Settings" && !propName.StartsWith("_Enable") && propName != "_OutlineFillInside" && propName != "_OutlineFillLines" && propName != "_DistortOppositeFlow" && propName != "_SnapDistortOppositeFlow" && propName != "_EnableFadeWithAlpha" && propName != "_ReflectLocalMirror";
        }

        public static void DisplayFinalInformation()
        {
            EditorGUILayout.Space();
            GUI.color = new Color(0.9f, 1, 0.9f, 1);
            EditorGUILayout.BeginVertical("Helpbox");

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;
            style.alignment = TextAnchor.MiddleLeft;

            GUIStyle linkStyle = new GUIStyle(GUI.skin.label);
            linkStyle.richText = true;
            linkStyle.alignment = TextAnchor.MiddleLeft;
            linkStyle.normal.textColor = linkStyle.focused.textColor = linkStyle.hover.textColor = EditorGUIUtility.isProSkin ? new Color(0.8f, 0.9f, 1f, 1) : new Color(0.1f, 0.2f, 0.4f, 1);
            linkStyle.active.textColor = EditorGUIUtility.isProSkin ? new Color(0.6f, 0.8f, 1f, 1) : new Color(0.15f, 0.4f, 0.6f, 1);

            GUI.color = new Color(1, 1, 1f, 0.75f);
            EditorGUILayout.LabelField("<b>Contact me if you have any issues or questions.</b>", style);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUI.color = new Color(1, 1, 1, 1);
            EditorGUILayout.LabelField("<b>Email:</b>", style, GUILayout.Width(100));
            EditorGUILayout.SelectableLabel("<b>ekincantascontact@gmail.com</b>", linkStyle, GUILayout.Height(16));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUI.color = new Color(1, 1, 1, 1);
            EditorGUILayout.LabelField("<b>Discord:</b>", style, GUILayout.Width(100));
            if (GUILayout.Button("<b><size=11>https://discord.gg/nWbRkN8Zxr</size></b>", linkStyle))
            {
                Application.OpenURL("https://discord.gg/nWbRkN8Zxr");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<b>Documentation:</b>", style, GUILayout.Width(100));
            if (GUILayout.Button("<b><size=11>https://ekincantas.com/overlay-filters/</size></b>", linkStyle))
            {
                Application.OpenURL("https://ekincantas.com/overlay-filters");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUI.color = new Color(1, 1, 1, 1);
        }
    }
}