using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Unity
using UnityEngine;
using UnityEditor;

// OPS - Pipeline
using OPS.Editor.Project.Pipeline.Component;

// OPS - Obfuscator - Assembly
using OPS.Obfuscator.Editor.Assembly;

namespace OPS.Obfuscator.Editor.Project.PreBuild.Pipeline.Component
{
    /// <summary>
    /// Analyse or manipulate a Unity Component pre build.
    /// </summary>
    public interface IComponentProcessingComponent : IPreBuildComponent
    {
        /// <summary>
        /// Analyse a Unity Component before the build starts.
        /// </summary>
        /// <returns></returns>
        bool OnAnalyse_Component(UnityEngine.Component _Component);

        /// <summary>
        /// Process a Unity Component before the build starts but directly after 'OnAnalyse_Component(...)'.
        /// </summary>
        /// <returns></returns>
        bool OnProcess_Component(UnityEngine.Component _Component);
    }
}
