// System
using System;
using System.Collections.Generic;

// OPS - Mono - Cecil
using OPS.Mono.Cecil;

// OPS - Settings
using OPS.Editor.Settings.File;

// OPS - Gui
using OPS.Editor.Gui;

// OPS - Obfuscator - Gui
using OPS.Obfuscator.Editor.Gui;

//OPS - Obfuscator - Assembly
using OPS.Obfuscator.Editor.Assembly;

namespace OPS.Obfuscator.Editor.Project.PostAssemblyBuild.Compatibility.Component
{
    public class MetaCompatibilityComponent : AObfuscationCompatibilityComponent, INamespaceCompatibility
    {
        // Info
        #region Info

        /// <summary>
        /// Name of the component.
        /// </summary>
        public override String Name
        {
            get
            {
                return "Meta - Compatibility";
            }
        }

        /// <summary>
        /// Description, descriping what this component does.
        /// </summary>
        public override String Description
        {
            get
            {
                return "Controls the Obfuscator compatibility to Meta Sdks.";
            }
        }

        /// <summary>
        /// Short description, descriping short what this component does.
        /// </summary>
        public override String ShortDescription
        {
            get
            {
                return "Controls the Obfuscator compatibility to Meta Sdks.";
            }
        }

        #endregion

        // Assembly
        #region Assembly

        private List<String> metaAssemblyToSkip = new List<string>()
        {
            "Meta",
        };

        private List<String> facebookAssemblyToSkip = new List<string>()
        {
            "Facebook",
        };

        private List<String> ooculusAssemblyToSkip = new List<string>()
        {
            "Oculus",
        };

        #endregion

        // Namespace
        #region Namespace

        private List<String> namespaceToSkip = new List<string>()
        {
            "AudienceNetwork",
            "FaceBook",
            "Facebook",
        };

        public bool SkipWholeNamespace(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, TypeDefinition _TypeDefinition, out string _Cause)
        {
            // Check the assemblies.
            if(this.Helper_Has_SkipMetaAssembliesActivated(_Step))
            {
                for (int i = 0; i < this.metaAssemblyToSkip.Count; i++)
                {
                    if (_AssemblyInfo.Name.StartsWith(this.metaAssemblyToSkip[i]))
                    {
                        _Cause = "Inside Meta Assembly";
                        return true;
                    }
                }
            }

            if (this.Helper_Has_SkipFacebookAssembliesActivated(_Step))
            {
                for (int i = 0; i < this.facebookAssemblyToSkip.Count; i++)
                {
                    if (_AssemblyInfo.Name.StartsWith(this.facebookAssemblyToSkip[i]))
                    {
                        _Cause = "Inside Facebook Assembly";
                        return true;
                    }
                }
            }

            if (this.Helper_Has_OculusFacebookAssembliesActivated(_Step))
            {
                for (int i = 0; i < this.ooculusAssemblyToSkip.Count; i++)
                {
                    if (_AssemblyInfo.Name.StartsWith(this.ooculusAssemblyToSkip[i]))
                    {
                        _Cause = "Inside Oculus Assembly";
                        return true;
                    }
                }
            }

            // Check the namespaces.
            for (int i = 0; i < this.namespaceToSkip.Count; i++)
            {
                if (_TypeDefinition.Namespace.StartsWith(this.namespaceToSkip[i]))
                {
                    _Cause = "Inside Facebook Namespace";
                    return true;
                }
            }

            _Cause = "";
            return false;
        }

        public bool IsNamespaceRenamingAllowed(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, TypeDefinition _TypeDefinition, out string _Cause)
        {
            _Cause = "";
            return true;
        }

        public string ApplyNamespaceRenamingFilter(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, TypeDefinition _TypeDefinition, string _OriginalName, string _CurrentName)
        {
            return _CurrentName;
        }

        #endregion

        // Settings
        #region Settings

        // Constants - Component
        public const String CSettingsKey = "Default_Compatibility_Component_Meta";

        /// <summary>
        /// The settings key for this component in the obfuscator settings.
        /// </summary>
        public override String SettingsKey { get; } = CSettingsKey;

        // Constants - Component - Elements
        public const String CSkip_Meta_Assemblies = "Skip_Meta_Assemblies";
        public const String CSkip_Facebook_Assemblies = "Skip_Facebook_Assemblies";
        public const String CSkip_Oculus_Assemblies = "Skip_Oculus_Assemblies";

        /// <summary>
        /// Helper method to check if the user has activated the skip Meta assemblies setting.
        /// </summary>
        /// <param name="_Step"></param>
        /// <returns></returns>
        private bool Helper_Has_SkipMetaAssembliesActivated(PostAssemblyBuildStep _Step)
        {
            return _Step.Settings.Get_ComponentSettings_As_Bool(this.SettingsKey, CSkip_Meta_Assemblies);
        }

        /// <summary>
        /// Helper method to check if the user has activated the skip Facebook assemblies setting.
        /// </summary>
        /// <param name="_Step"></param>
        /// <returns></returns>
        private bool Helper_Has_SkipFacebookAssembliesActivated(PostAssemblyBuildStep _Step)
        {
            return _Step.Settings.Get_ComponentSettings_As_Bool(this.SettingsKey, CSkip_Facebook_Assemblies);
        }

        /// <summary>
        /// Helper method to check if the user has activated the skip Facebook assemblies setting.
        /// </summary>
        /// <param name="_Step"></param>
        /// <returns></returns>
        private bool Helper_Has_OculusFacebookAssembliesActivated(PostAssemblyBuildStep _Step)
        {
            return _Step.Settings.Get_ComponentSettings_As_Bool(this.SettingsKey, CSkip_Oculus_Assemblies);
        }

        #endregion

        //Gui
        #region Gui

        public override ObfuscatorContainer GetGuiContainer(ComponentSettings _ComponentSettings)
        {
            //Header
            ObfuscatorHeader var_Header = new ObfuscatorHeader(this.Name);

            //Description
            ObfuscatorDescription var_Description = new ObfuscatorDescription(this.ShortDescription);

            //Content
            ObfuscatorContent var_Content = new ObfuscatorContent();

            //Skip Meta Assemblies
            Row_Boolean var_SkipMetaAssemblies = new Row_Boolean("Skip Meta assemblies: ", _ComponentSettings, CSkip_Meta_Assemblies);
            var_SkipMetaAssemblies.Notification_Info = "Activate this setting to skip Meta assemblies from obfuscation (Recommended).";
            var_Content.AddRow(var_SkipMetaAssemblies);

            //Skip Facebook Assemblies
            Row_Boolean var_SkipFacebookAssemblies = new Row_Boolean("Skip Facebook assemblies: ", _ComponentSettings, CSkip_Facebook_Assemblies);
            var_SkipFacebookAssemblies.Notification_Info = "Activate this setting to skip Facebook assemblies from obfuscation (Recommended).";
            var_Content.AddRow(var_SkipFacebookAssemblies);

            //Skip Oculus Assemblies
            Row_Boolean var_SkipOculusAssemblies = new Row_Boolean("Skip Oculus assemblies: ", _ComponentSettings, CSkip_Oculus_Assemblies);
            var_SkipOculusAssemblies.Notification_Info = "Activate this setting to skip Oculus assemblies from obfuscation (Recommended).";
            var_Content.AddRow(var_SkipOculusAssemblies);

            //Container
            ObfuscatorContainer var_ObfuscatorContainer = new ObfuscatorContainer(var_Header, var_Description, var_Content, false);

            return var_ObfuscatorContainer;
        }

        #endregion
    }
}
