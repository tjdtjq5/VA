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
using OPS.Obfuscator.Editor.Assembly.Mono.Member.Attribute.Helper;

namespace OPS.Obfuscator.Editor.Project.PostAssemblyBuild.Compatibility.Component
{
    public class PhotonSdkCompatibilityComponent : AObfuscationCompatibilityComponent, INamespaceCompatibility, ITypeCompatibility, IMethodCompatibility
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
                return "Photon - Compatibility";
            }
        }

        /// <summary>
        /// Description, descriping what this component does.
        /// </summary>
        public override String Description
        {
            get
            {
                return "Controls the Obfuscator compatibility to Photon Sdks.";
            }
        }

        /// <summary>
        /// Short description, descriping short what this component does.
        /// </summary>
        public override String ShortDescription
        {
            get
            {
                return "Controls the Obfuscator compatibility to Photon Sdks.";
            }
        }

        #endregion

        //Namespace
        #region Namespace

        private List<String> namespaceToSkip = new List<string>()
        {
            "Photon",
        };

        public bool SkipWholeNamespace(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, TypeDefinition _TypeDefinition, out string _Cause)
        {
            for (int i = 0; i < this.namespaceToSkip.Count; i++)
            {
                if (_TypeDefinition.Namespace.StartsWith(this.namespaceToSkip[i]))
                {
                    _Cause = "Inside Photon Namespace";
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
            "Photon.MonoBehaviour",
            "Photon.PunBehaviour",
            "SupportLogging"
        };

        public bool SkipWholeType(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, TypeDefinition _TypeDefinition, out string _Cause)
        {
            _Cause = "";
            return false;
        }

        public bool IsTypeRenamingAllowed(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, TypeDefinition _TypeDefinition, out string _Cause)
        {
            if (this.typesToSkip.Contains(_TypeDefinition.Namespace + "." + _TypeDefinition.Name))
            {
                _Cause = "Is Photon Class.";
                return true;
            }

            if (this.typesToSkip.Contains(_TypeDefinition.Name))
            {
                _Cause = "Is Photon Class.";
                return true;
            }

            _Cause = null;
            return true;
        }

        public string ApplyTypeRenamingFilter(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, TypeDefinition _TypeDefinition, string _OriginalName, string _CurrentName)
        {
            return _CurrentName;
        }

        #endregion

        //Method
        #region Method

        private HashSet<String> methodsToSkip = new HashSet<string>()
        {
            "OnConnectedToPhoton",
            "OnLeftRoom",
            "OnMasterClientSwitched",
            "OnPhotonCreateRoomFailed",
            "OnPhotonJoinRoomFailed",
            "OnCreatedRoom",
            "OnJoinedLobby",
            "OnLeftLobby",
            "OnDisconnectedFromPhoton",
            "OnConnectionFail",
            "OnFailedToConnectToPhoton",
            "OnReceivedRoomListUpdate",
            "OnJoinedRoom",
            "OnPhotonPlayerConnected",
            "OnPhotonPlayerDisconnected",
            "OnPhotonRandomJoinFailed",
            "OnConnectedToMaster",
            "OnPhotonSerializeView",
            "OnPhotonInstantiate",
            "OnPhotonMaxCccuReached",
            "OnPhotonCustomRoomPropertiesChanged",
            "OnPhotonPlayerPropertiesChanged",
            "OnUpdatedFriendList",
            "OnCustomAuthenticationFailed",
            "OnCustomAuthenticationResponse",
            "OnWebRpcResponse",
            "OnOwnershipRequest",
            "OnLobbyStatisticsUpdate",
        };

        public bool IsMethodRenamingAllowed(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, MethodDefinition _MethodDefinition, out string _Cause)
        {
            if (AttributeHelper.HasCustomAttribute(_MethodDefinition, "PunRPC"))
            {
                _Cause = "Has PunRPCAttribute.";
                return false;
            }

            if (this.methodsToSkip.Contains(_MethodDefinition.Name))
            {
                _Cause = "Is Photon Method.";
                return false;
            }

            _Cause = null;
            return true;
        }

        public string ApplyMethodRenamingFilter(PostAssemblyBuildStep _Step, AssemblyInfo _AssemblyInfo, MethodDefinition _MethodDefinition, string _OriginalName, string _CurrentName)
        {
            return _CurrentName;
        }

        #endregion

        // Settings
        #region Settings

        // Constants - Component
        public const String CSettingsKey = "Default_Compatibility_Component_Photon_Sdk";

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
