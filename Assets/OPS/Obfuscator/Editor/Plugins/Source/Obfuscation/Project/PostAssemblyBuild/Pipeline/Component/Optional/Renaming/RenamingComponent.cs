using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// OPS - Mono - Cecil
using OPS.Mono.Cecil;
using OPS.Mono.Cecil.Cil;

// OPS - Settings
using OPS.Editor.Settings.File;

// OPS - Pipeling
using OPS.Editor.Project.Pipeline;

// OPS - Obfuscator - Assembly
using OPS.Obfuscator.Editor.Assembly;
using OPS.Obfuscator.Editor.Assembly.Mono.Member;
using OPS.Obfuscator.Editor.Assembly.Mono.Member.Cache;
using OPS.Obfuscator.Editor.Assembly.Mono.Member.Extension;
using OPS.Obfuscator.Editor.Assembly.Mono.Member.Helper;
using OPS.Obfuscator.Editor.Assembly.Mono.Member.Key;
using OPS.Obfuscator.Editor.Assembly.Mono.Member.Key.Helper;

// OPS - Obfuscator - Components
using OPS.Obfuscator.Editor.Component.Gui;

// OPS - Gui
using OPS.Editor.Gui;

// OPS - Obfuscator - Gui
using OPS.Obfuscator.Editor.Gui;

// OPS - Obfuscator - PostAssemblyBuild
using OPS.Obfuscator.Editor.Project.PostAssemblyBuild.Renaming;
using OPS.Obfuscator.Editor.Project.PostAssemblyBuild.Renaming.Charset;
using OPS.Editor.Project.Step;

namespace OPS.Obfuscator.Editor.Project.PostAssemblyBuild.Pipeline.Component.Optional
{
    public class RenamingComponent : APostAssemblyBuildComponent, IGuiComponent
    {
        // Name
        #region Name

        /// <summary>
        /// Name of the component.
        /// </summary>
        public override String Name
        {
            get
            {
                return "Renaming - Settings";
            }
        }

        #endregion

        // Description
        #region Description

        /// <summary>
        /// Description, descriping what this component does.
        /// </summary>
        public override string Description
        {
            get
            {
                return "Manage here the renaming settings.";
            }
        }

        #endregion

        // Short Description
        #region Short Description

        /// <summary>
        /// Short description, descriping short what this component does.
        /// </summary>
        public override String ShortDescription
        {
            get
            {
                return "Manage here the renaming settings.";
            }
        }

        #endregion

        // Settings
        #region Settings

        // Constants - Component
        public const String CSettingsKey = "Default_Obfuscation_Component_Renaming";

        /// <summary>
        /// The settings key for this component in the obfuscator settings.
        /// </summary>
        public String SettingsKey { get; } = CSettingsKey;

        // Constants - Component - Elements

        // Constants - Component - Elements - Pattern
        public const String CActive_Renaming_Pattern = "Active_Renaming_Pattern";
        public const String CCustom_Renaming_Pattern = "Custom_Renaming_Pattern";

        //Constants - Component - Elements - Mapping
        public const String CEnable_Load_Mapping = "Enable_Load_Mapping";
        public const String CLoad_Mapping_FilePath = "Load_Mapping_FilePath";
        public const String CEnable_Save_Mapping = "Enable_Save_Mapping";
        public const String CSave_Mapping_FilePath = "Save_Mapping_FilePath";

        #endregion

        //Gui
        #region Gui

        /// <summary>
        /// Category this Component belongs too.
        /// </summary>
        public EObfuscatorCategory ObfuscatorCategory
        {
            get
            {
                return EObfuscatorCategory.Optional;
            }
        }

        // Rows
        private Row_DropDown_Enum<ECharset> active_RenamingMapping_Row;
        private Row_TextBox custom_RenamingMapping_Row;

        public ObfuscatorContainer GetGuiContainer(ComponentSettings _ComponentSettings)
        {
            //Header
            ObfuscatorHeader var_Header = new ObfuscatorHeader(this.Name);

            //Description
            ObfuscatorDescription var_Description = new ObfuscatorDescription(this.ShortDescription);

            //Content
            ObfuscatorContent var_Content = new ObfuscatorContent();

            // Renaming Pattern
            this.active_RenamingMapping_Row = new Row_DropDown_Enum<ECharset>("Active renaming pattern:", _ComponentSettings, CActive_Renaming_Pattern, this.OnActiveRenamingMappingChanged);
            this.active_RenamingMapping_Row.Notification_Info = "Select one of the predefined renaming pattern, or use a custom one.";
            this.active_RenamingMapping_Row.Notification_Warning = "Some characters may not be valid for some build targets. For example building to IOS creates a XCode Project with file names based one class files. If some of the characters are no valid characters on the Mac Platform you receive errors. Same might be if you build to IL2CPP. The Default renaming pattern will always work.";
            var_Content.AddRow(this.active_RenamingMapping_Row, true);

            this.custom_RenamingMapping_Row = new Row_TextBox("Custom renaming pattern: ", _ComponentSettings, CCustom_Renaming_Pattern);
            this.custom_RenamingMapping_Row.Notification_Info = "Enter here characters you want to use as custom renaming pattern. Do not add delimiters between the characters! The default renaming pattern for example is: abcdefghijklmnopqrstuvwxyz. You can switch the pattern as often as you want, it is also independent from the renaming mapping.";
            var_Content.AddRow(this.custom_RenamingMapping_Row, true);

            //Use Load Mapping
            Row_Boolean var_UseLoadMapping_Row = new Row_Boolean("Load an obfuscation mapping file: ", _ComponentSettings, CEnable_Load_Mapping);
            var_UseLoadMapping_Row.Notification_Info = "Activate this setting to load an obfuscation mapping from a file. Define a file path below. Recommended if you obfuscate serializeable classes/fields/... .";
            var_Content.AddRow(var_UseLoadMapping_Row);

            Row_OpenFileSelect var_LoadMapping_Row = new Row_OpenFileSelect("Load mapping file path: ", _ComponentSettings, CLoad_Mapping_FilePath);
            var_LoadMapping_Row.Notification_Info = "Enter here a file path you want to load the mapping from.";
            var_Content.AddRow(var_LoadMapping_Row);

            //Use Save Mapping
            Row_Boolean var_UseSaveMapping_Row = new Row_Boolean("Save an obfuscation mapping file: ", _ComponentSettings, CEnable_Save_Mapping);
            var_UseSaveMapping_Row.Notification_Info = "Activate this setting to save the obfuscation mapping to a file. Define a file path below. Recommended if you obfuscate serializeable classes/fields/... .";
            var_Content.AddRow(var_UseSaveMapping_Row);

            Row_SaveFileSelect var_SaveMapping_Row = new Row_SaveFileSelect("Save mapping file path: ", _ComponentSettings, CSave_Mapping_FilePath);
            var_SaveMapping_Row.Notification_Info = "Enter here a file path you want to save the mapping to.";
            var_Content.AddRow(var_SaveMapping_Row);

            //Container
            ObfuscatorContainer var_ObfuscatorContainer = new ObfuscatorContainer(var_Header, var_Description, var_Content, false);

            return var_ObfuscatorContainer;
        }

        private void OnActiveRenamingMappingChanged()
        {
            this.custom_RenamingMapping_Row.Enabled = (ECharset)this.active_RenamingMapping_Row.RowContent == ECharset.Custom;
        }

        #endregion

        // On Pre Pipeline Process
        #region On Pre Pipeline Process

        public override bool OnPrePipelineProcess(IStepInput _StepInput)
        {
            // Load renaming pattern!
            ACharset var_Charset = new DefaultCharset();

#if Obfuscator_Free
#else
            String var_Enum_Charset_String_Value = this.Step.Settings.Get_ComponentSettings_As_String(this.SettingsKey, CActive_Renaming_Pattern);

            try
            {
                ECharset var_Enum_Charset_Value = (ECharset)Enum.Parse(typeof(ECharset), var_Enum_Charset_String_Value);

                switch (var_Enum_Charset_Value)
                {
                    case ECharset.Default:
                        {
                            break;
                        }
                    case ECharset.Unicode:
                        {
                            var_Charset = new UnicodeCharset();
                            break;
                        }
                    case ECharset.Common_Chinese:
                        {
                            var_Charset = new ChineseCharset();
                            break;
                        }
                    case ECharset.Common_Korean:
                        {
                            var_Charset = new KoreanCharset();
                            break;
                        }
                    case ECharset.Custom:
                        {
                            String var_Custom_Pattern_String_Value = this.Step.Settings.Get_ComponentSettings_As_String(this.SettingsKey, CCustom_Renaming_Pattern);

                            var_Charset = new CustomCharset(var_Custom_Pattern_String_Value);
                            break;
                        }
                }
            }
            catch(Exception e)
            {
                Obfuscator.Report.Append(OPS.Editor.Report.EReportLevel.Warning, "Failed to load Charset: " + var_Enum_Charset_String_Value + ". Using default! Exception: " + e.ToString());

                // Failed and so use default.
                var_Charset = new DefaultCharset();
            }
#endif

            // Set Charset.
            this.DataContainer.RenameManager.NameGenerator.UseCharSet(var_Charset);

            // Check if should load a mapping.
            if (!this.Step.Settings.Get_ComponentSettings_As_Bool(this.SettingsKey, CEnable_Load_Mapping))
            {
                return true;
            }

            // Mapping file path.
            this.DataContainer.RenameManager.LoadMapping(this.Step.Settings.Get_ComponentSettings_As_String(this.SettingsKey, CLoad_Mapping_FilePath));

            return true;
        }

        #endregion

        // On Post Pipeline Proces
        #region On Post Pipeline Proces

        public override bool OnPostPipelineProcess(IStepOutput _StepOutput)
        {
            //Check if should save a mapping.
            if (!this.Step.Settings.Get_ComponentSettings_As_Bool(this.SettingsKey, CEnable_Save_Mapping))
            {
                return true;
            }

            //Mapping file path.
            this.DataContainer.RenameManager.SaveMapping(this.Step.Settings.Get_ComponentSettings_As_String(this.SettingsKey, CSave_Mapping_FilePath));

            return true;
        }

        #endregion
    }
}
