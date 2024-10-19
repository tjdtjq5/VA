using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
using OPS.Obfuscator.Editor.Assembly.Mono.Member.Helper;

namespace OPS.Obfuscator.Editor.Project.PostAssemblyBuild.Compatibility.Component
{
    public class PlayMakerCompatibilityComponent : AObfuscationCompatibilityComponent, INamespaceCompatibility, ITypeCompatibility
    {
        //Info
        #region Info

        /// <summary>
        /// Name of the component.
        /// </summary>
        public override String Name
        {
            get
            {
                return "PlayMaker - Compatibility";
            }
        }

        /// <summary>
        /// Description, descriping what this component does.
        /// </summary>
        public override String Description
        {
            get
            {
                return "Controls the Obfuscator compatibility to PlayMaker Sdks.";
            }
        }

        /// <summary>
        /// Short description, descriping short what this component does.
        /// </summary>
        public override String ShortDescription
        {
            get
            {
                return "Controls the Obfuscator compatibility to PlayMaker Sdks.";
            }
        }

        #endregion

        //Namespace
        #region Namespace

        private List<String> namespaceToSkip = new List<string>()
        {
            "HutongGames",
        };

        public bool SkipWholeNamespace(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, TypeDefinition _TypeDefinition, out string _Cause)
        {
            for (int i = 0; i < this.namespaceToSkip.Count; i++)
            {
                if (_TypeDefinition.Namespace.StartsWith(this.namespaceToSkip[i]))
                {
                    _Cause = "Is inside HutongGames namespace.";
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

        //Type
        #region Type

        private HashSet<String> typesToSkip = new HashSet<string>()
        {
            "iTween",
            "iTweenFSMEvents",
            "iTweenFSMType",
            "PlayMakerRPCProxy",
        };

        public bool SkipWholeType(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, TypeDefinition _TypeDefinition, out string _Cause)
        {
            if (this.typesToSkip.Contains(_TypeDefinition.Name))
            {
                _Cause = "Is a PlayMaker Sdk Class.";
                return true;
            }

            _Cause = "";
            return false;
        }

        public bool IsTypeRenamingAllowed(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, TypeDefinition _TypeDefinition, out string _Cause)
        {
            _Cause = null;
            return true;
        }

        public string ApplyTypeRenamingFilter(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, TypeDefinition _TypeDefinition, string _OriginalName, string _CurrentName)
        {
            return _CurrentName;
        }

        #endregion

        // Settings
        #region Settings

        // Constants - Component
        public const String CSettingsKey = "Default_Compatibility_Component_PlayMaker_Sdk";

        /// <summary>
        /// The settings key for this component in the obfuscator settings.
        /// </summary>
        public override String SettingsKey { get; } = CSettingsKey;

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

            //Container
            ObfuscatorContainer var_ObfuscatorContainer = new ObfuscatorContainer(var_Header, var_Description, var_Content, false);

            return var_ObfuscatorContainer;
        }

        #endregion
    }
}
