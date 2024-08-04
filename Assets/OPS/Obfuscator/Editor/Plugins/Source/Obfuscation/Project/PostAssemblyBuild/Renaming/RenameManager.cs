using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// OPS - Mono - Cecil
using OPS.Mono.Cecil;

// OPS - Obfuscator - Assembly
using OPS.Obfuscator.Editor.Assembly;
using OPS.Obfuscator.Editor.Assembly.Mono.Member;
using OPS.Obfuscator.Editor.Assembly.Mono.Member.Attribute.Helper;
using OPS.Obfuscator.Editor.Assembly.Mono.Member.Cache;
using OPS.Obfuscator.Editor.Assembly.Mono.Member.Key;

namespace OPS.Obfuscator.Editor.Project.PostAssemblyBuild.Renaming
{
    /// <summary>
    /// Contains the mappings, and the shall rename/not rename stuff.
    /// </summary>
    public class RenameManager
    {
        // Constructor
        #region Constructor

        /// <summary>
        /// The rename manager belongs to a PostAssemblyBuildStep.
        /// </summary>
        /// <param name="_Step"></param>
        public RenameManager(PostAssemblyBuildStep _Step)
        {
            this.step = _Step;
        }

        #endregion

        // Step
        #region Step

        /// <summary>
        /// The used project.
        /// </summary>
        private readonly PostAssemblyBuildStep step;

        #endregion

        // Load
        #region Load

        /// <summary>
        /// Load the RenameManager.
        /// </summary>
        public bool Load()
        {
            // Already loaded?
            if (this.doNotObfuscateDictionary != null)
            {
                return true;
            }

            //Init - Do not obfuscate dictionaries.
            this.doNotObfuscateDictionary = new Dictionary<EMemberType, Dictionary<IMemberKey, List<string>>>();

            //Init - The NameGenerator
            this.NameGenerator = new NameGenerator();

            //Init - The Mappings
            this.OriginalMapping = new ObfuscatorV4Mapping();
            this.CurrentMapping = new ObfuscatorV4Mapping();
            this.LoadedMapping = new ObfuscatorV4Mapping();

            // Load Original Mapping
            this.SetupOriginalMapping();

            return true;
        }

        #endregion

        // Unload
        #region Unload

        /// <summary>
        /// Unload the Renamemanager.
        /// </summary>
        public bool Unload()
        {
            // Already unloaded.
            if (this.doNotObfuscateDictionary == null)
            {
                return true;
            }

            //Clear - Do not obfuscate dictionaries.
            this.doNotObfuscateDictionary = null;

            //Clear - The NameGenerator
            this.NameGenerator = null;

            //Clear - The Mappings
            this.OriginalMapping = null;
            this.CurrentMapping = null;
            this.LoadedMapping = null;

            return true;
        }

        #endregion

        //Do not rename
        #region Do not rename

        /// <summary>
        /// Member Type, Dictionary of Original Keys and a List of not obfuscation cause.
        /// </summary>
        private Dictionary<EMemberType, Dictionary<IMemberKey, List<String>>> doNotObfuscateDictionary;

        /// <summary>
        /// Do not obfuscate _MemberDefinition of _MemberType with _Cause of not obfuscation.
        /// </summary>
        /// <param name="_MemberType"></param>
        /// <param name="_MemberDefinition"></param>
        /// <param name="_Cause"></param>
        public void AddDoNotObfuscate(EMemberType _MemberType, IMemberDefinition _MemberDefinition, String _Cause)
        {
            if (_MemberDefinition == null)
            {
                throw new ArgumentNullException("_MemberDefinition");
            }
            if (String.IsNullOrEmpty(_Cause))
            {
                _Cause = "Non cause given.";
            }

            // Got not loaded yet!
            if (this.doNotObfuscateDictionary == null)
            {
                return;
            }

            IMemberKey var_OriginalKey = null;

            switch (_MemberType)
            {
                case EMemberType.Namespace:
                    {
                        var_OriginalKey = this.step.GetCache<TypeCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as TypeDefinition);
                        break;
                    }
                case EMemberType.Type:
                    {
                        var_OriginalKey = this.step.GetCache<TypeCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as TypeDefinition);
                        break;
                    }
                case EMemberType.Method:
                    {
                        var_OriginalKey = this.step.GetCache<MethodCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as MethodDefinition);
                        break;
                    }
                case EMemberType.Field:
                    {
                        var_OriginalKey = this.step.GetCache<FieldCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as FieldDefinition);
                        break;
                    }
                case EMemberType.Property:
                    {
                        var_OriginalKey = this.step.GetCache<PropertyCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as PropertyDefinition);
                        break;
                    }
                case EMemberType.Event:
                    {
                        var_OriginalKey = this.step.GetCache<EventCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as EventDefinition);
                        break;
                    }
            }

            if (var_OriginalKey == null)
            {
                // Is not in cache.
                return;
            }

            // Get belonging fullname to cause list dictionary.
            Dictionary<IMemberKey, List<String>> var_FullNameToCauseListDictionary = null;

            // Or if does not exist, create one.
            if (!this.doNotObfuscateDictionary.TryGetValue(_MemberType, out var_FullNameToCauseListDictionary))
            {
                var_FullNameToCauseListDictionary = new Dictionary<IMemberKey, List<string>>();
                this.doNotObfuscateDictionary.Add(_MemberType, var_FullNameToCauseListDictionary);
            }

            // Add _Cause to the cause list.
            List<String> var_CauseList;
            if (!var_FullNameToCauseListDictionary.TryGetValue(var_OriginalKey, out var_CauseList))
            {
                var_CauseList = new List<string>();
                var_FullNameToCauseListDictionary.Add(var_OriginalKey, var_CauseList);
            }
            var_CauseList.Add(_Cause);
        }

        ////////////////////////////

        /// <summary>
        /// Returns if the _MemberDefinition of _MemberType will be not obfuscated.
        /// True: Getting NOT obfuscated.
        /// False: Getting obfuscated.
        /// </summary>
        /// <param name="_MemberType"></param>
        /// <param name="_MemberDefinition"></param>
        /// <returns></returns>
        public bool GetDoNotObfuscate(EMemberType _MemberType, IMemberDefinition _MemberDefinition)
        {
            if (_MemberDefinition == null)
            {
                throw new ArgumentNullException("_MemberDefinition");
            }

            // Has the ObfuscateAnywayAttribute, so just force obfuscate!
            if (AttributeHelper.HasCustomAttribute(_MemberDefinition, typeof(OPS.Obfuscator.Attribute.ObfuscateAnywayAttribute).Name))
            {
                return false;
            }

            IMemberKey var_OriginalKey = null;

            switch (_MemberType)
            {
                case EMemberType.Namespace:
                    {
                        var_OriginalKey = this.step.GetCache<TypeCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as TypeDefinition);
                        break;
                    }
                case EMemberType.Type:
                    {
                        var_OriginalKey = this.step.GetCache<TypeCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as TypeDefinition);
                        break;
                    }
                case EMemberType.Method:
                    {
                        var_OriginalKey = this.step.GetCache<MethodCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as MethodDefinition);
                        break;
                    }
                case EMemberType.Field:
                    {
                        var_OriginalKey = this.step.GetCache<FieldCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as FieldDefinition);
                        break;
                    }
                case EMemberType.Property:
                    {
                        var_OriginalKey = this.step.GetCache<PropertyCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as PropertyDefinition);
                        break;
                    }
                case EMemberType.Event:
                    {
                        var_OriginalKey = this.step.GetCache<EventCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as EventDefinition);
                        break;
                    }
            }

            if (var_OriginalKey == null)
            {
                // Is not in cache.
                return false;
            }

            // Get belonging fullname to cause list dictionary.
            Dictionary<IMemberKey, List<String>> var_FullNameToCauseListDictionary = null;

            // Or if does not exist, return.
            if (!this.doNotObfuscateDictionary.TryGetValue(_MemberType, out var_FullNameToCauseListDictionary))
            {
                return false;
            }

            // Return if contained in do not obfuscate dictionary.
            return var_FullNameToCauseListDictionary.ContainsKey(var_OriginalKey);
        }

        ////////////////////////////

        /// <summary>
        /// Returns the list of not obfuscation cause.
        /// If there is no cause null will be returned.
        /// </summary>
        /// <param name="_MemberType"></param>
        /// <param name="_MemberDefinition"></param>
        /// <returns></returns>
        public List<String> GetDoNotObfuscateCause(EMemberType _MemberType, IMemberDefinition _MemberDefinition)
        {
            if (_MemberDefinition == null)
            {
                throw new ArgumentNullException("_MemberDefinition");
            }

            // Has the ObfuscateAnywayAttribute, so just force obfuscate!
            if (AttributeHelper.HasCustomAttribute(_MemberDefinition, typeof(OPS.Obfuscator.Attribute.ObfuscateAnywayAttribute).Name))
            {
                return null;
            }

            IMemberKey var_OriginalKey = null;

            switch (_MemberType)
            {
                case EMemberType.Namespace:
                    {
                        var_OriginalKey = this.step.GetCache<TypeCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as TypeDefinition);
                        break;
                    }
                case EMemberType.Type:
                    {
                        var_OriginalKey = this.step.GetCache<TypeCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as TypeDefinition);
                        break;
                    }
                case EMemberType.Method:
                    {
                        var_OriginalKey = this.step.GetCache<MethodCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as MethodDefinition);
                        break;
                    }
                case EMemberType.Field:
                    {
                        var_OriginalKey = this.step.GetCache<FieldCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as FieldDefinition);
                        break;
                    }
                case EMemberType.Property:
                    {
                        var_OriginalKey = this.step.GetCache<PropertyCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as PropertyDefinition);
                        break;
                    }
                case EMemberType.Event:
                    {
                        var_OriginalKey = this.step.GetCache<EventCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as EventDefinition);
                        break;
                    }
            }

            if (var_OriginalKey == null)
            {
                // Is not in cache.
                return null;
            }

            // Get belonging fullname to cause list dictionary.
            Dictionary<IMemberKey, List<String>> var_FullNameToCauseListDictionary = null;

            // Or if does not exist return.
            if (!this.doNotObfuscateDictionary.TryGetValue(_MemberType, out var_FullNameToCauseListDictionary))
            {
                return null;
            }

            // Get and return the list of causes.
            List<String> var_CauseList;
            if (var_FullNameToCauseListDictionary.TryGetValue(var_OriginalKey, out var_CauseList))
            {
                return var_CauseList;
            }

            return null;
        }

        /// <summary>
        /// Returns the count of the to not obfuscate member per type.
        /// </summary>
        /// <param name="_MemberType"></param>
        /// <returns></returns>
        public int GetCountOfDoNotObfuscate(EMemberType _MemberType)
        {
            if (this.doNotObfuscateDictionary.TryGetValue(_MemberType, out var var_Dictionary))
            {
                return var_Dictionary.Count;
            }

            return 0;
        }

        #endregion

        //Renaming Mapping
        #region Renaming Mapping

        /// <summary>
        /// The original mapping (from original key to name) of the assembly members.
        /// </summary>
        public IRenameMapping OriginalMapping { get; private set; }

        /// <summary>
        /// The current obfuscation mapping.
        /// </summary>
        public IRenameMapping CurrentMapping { get; private set; }

        /// <summary>
        /// The loaded obfuscation mapping.
        /// </summary>
        public IRenameMapping LoadedMapping { get; private set; }

        /// <summary>
        /// Setup the original mapping.
        /// </summary>
        private void SetupOriginalMapping()
        {
            //Iterate all assemblies and members and add those to the original mapping.
            foreach (AssemblyInfo var_AssemblyInfo in this.step.DataContainer.ObfuscateAssemblyList)
            {
                foreach (TypeDefinition var_TypeDefinition in var_AssemblyInfo.GetAllTypeDefinitions())
                {
                    this.OriginalMapping.GetMapping(EMemberType.Namespace).Add(new TypeKey(var_TypeDefinition), var_TypeDefinition.Namespace);
                    this.OriginalMapping.GetMapping(EMemberType.Type).Add(new TypeKey(var_TypeDefinition), var_TypeDefinition.Name);

                    foreach (var var_Member in var_TypeDefinition.Methods)
                    {
                        this.OriginalMapping.GetMapping(EMemberType.Method).Add(new MethodKey(var_Member), var_Member.Name);
                    }

                    foreach (var var_Member in var_TypeDefinition.Fields)
                    {
                        this.OriginalMapping.GetMapping(EMemberType.Field).Add(new FieldKey(var_Member), var_Member.Name);
                    }

                    foreach (var var_Member in var_TypeDefinition.Properties)
                    {
                        this.OriginalMapping.GetMapping(EMemberType.Property).Add(new PropertyKey(var_Member), var_Member.Name);
                    }

                    foreach (var var_Member in var_TypeDefinition.Events)
                    {
                        this.OriginalMapping.GetMapping(EMemberType.Event).Add(new EventKey(var_Member), var_Member.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Get a unique name for _MemberDefinition of _MemberType.
        /// Will check the loaded, current or original mapping with _Tries for a unique name.
        /// </summary>
        /// <param name="_MemberType"></param>
        /// <param name="_MemberDefinition"></param>
        /// <param name="_Tries"></param>
        /// <returns></returns>
        public String GetUniqueObfuscatedName(EMemberType _MemberType, IMemberDefinition _MemberDefinition, int _Tries = Int32.MaxValue)
        {
            String var_ObfuscatedName = null;

            for (int i = 0; i < _Tries; i++)
            {
                var_ObfuscatedName = this.NameGenerator.GetNextName(_MemberType, _MemberDefinition);

                if (!this.IsNameFree(_MemberDefinition, _MemberType, var_ObfuscatedName))
                {
                    var_ObfuscatedName = null;
                    continue;
                }

                break;
            }

            return var_ObfuscatedName;
        }

        /// <summary>
        /// Add _ObfuscatedName for _MemberDefinition of _MemberType to the current mapping. 
        /// </summary>
        /// <param name="_MemberType"></param>
        /// <param name="_MemberDefinition"></param>
        /// <param name="_ObfuscatedName"></param>
        public void AddObfuscated(EMemberType _MemberType, IMemberDefinition _MemberDefinition, String _ObfuscatedName)
        {
            if (_MemberDefinition == null)
            {
                throw new ArgumentNullException("_MemberDefinition");
            }

            IMemberKey var_OriginalKey = null;

            switch (_MemberType)
            {
                case EMemberType.Namespace:
                    {
                        var_OriginalKey = this.step.GetCache<TypeCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as TypeDefinition);
                        break;
                    }
                case EMemberType.Type:
                    {
                        var_OriginalKey = this.step.GetCache<TypeCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as TypeDefinition);
                        break;
                    }
                case EMemberType.Method:
                    {
                        var_OriginalKey = this.step.GetCache<MethodCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as MethodDefinition);
                        break;
                    }
                case EMemberType.Field:
                    {
                        var_OriginalKey = this.step.GetCache<FieldCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as FieldDefinition);
                        break;
                    }
                case EMemberType.Property:
                    {
                        var_OriginalKey = this.step.GetCache<PropertyCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as PropertyDefinition);
                        break;
                    }
                case EMemberType.Event:
                    {
                        var_OriginalKey = this.step.GetCache<EventCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as EventDefinition);
                        break;
                    }
            }

            if (var_OriginalKey == null)
            {
                // Is not in cache.
                return;
            }

            // Add to current mapping.
            this.CurrentMapping.GetMapping(_MemberType).Add(var_OriginalKey, _ObfuscatedName);
        }

        /// <summary>
        /// Returns the obfuscated name of _MemberDefinition of _MemberType.
        /// The method first checks in the current mapping, if there is a obfuscated name, and returns it.
        /// If there is none, it checks the loaded mapping and returns it.
        /// If non found, returns null.
        /// </summary>
        /// <param name="_MemberType"></param>
        /// <param name="_MemberDefinition"></param>
        /// <returns></returns>
        public String GetObfuscated(EMemberType _MemberType, IMemberDefinition _MemberDefinition)
        {
            if (_MemberDefinition == null)
            {
                throw new ArgumentNullException("_MemberDefinition");
            }

            // Has the ObfuscateAnywayAttribute, so return its value!
            if (AttributeHelper.TryGetCustomAttribute(_MemberDefinition, typeof(OPS.Obfuscator.Attribute.ObfuscateAnywayAttribute).Name, out CustomAttribute var_ObfuscateAnywayAttribute))
            {
                return (String)var_ObfuscateAnywayAttribute.ConstructorArguments[0].Value;
            }

            IMemberKey var_OriginalKey = null;

            switch (_MemberType)
            {
                case EMemberType.Namespace:
                    {
                        var_OriginalKey = this.step.GetCache<TypeCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as TypeDefinition);
                        break;
                    }
                case EMemberType.Type:
                    {
                        var_OriginalKey = this.step.GetCache<TypeCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as TypeDefinition);
                        break;
                    }
                case EMemberType.Method:
                    {
                        var_OriginalKey = this.step.GetCache<MethodCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as MethodDefinition);
                        break;
                    }
                case EMemberType.Field:
                    {
                        var_OriginalKey = this.step.GetCache<FieldCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as FieldDefinition);
                        break;
                    }
                case EMemberType.Property:
                    {
                        var_OriginalKey = this.step.GetCache<PropertyCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as PropertyDefinition);
                        break;
                    }
                case EMemberType.Event:
                    {
                        var_OriginalKey = this.step.GetCache<EventCache>().GetOriginalMemberKeyByMemberDefinition(_MemberDefinition as EventDefinition);
                        break;
                    }
            }

            if (var_OriginalKey == null)
            {
                // Is not in cache.
                return null;
            }

            // First try to get from current mapping.
            String var_ObfuscatedName = this.CurrentMapping.GetMapping(_MemberType).Get(var_OriginalKey) as String;

            // Second try from loaded mapping.
            if (var_ObfuscatedName == null)
            {
                var_ObfuscatedName = this.LoadedMapping.GetMapping(_MemberType).Get(var_OriginalKey) as String;
            }

            return var_ObfuscatedName;
        }

        /// <summary>
        /// Load a mapping used as reference.
        /// </summary>
        /// <param name="_FilePath"></param>
        public void LoadMapping(String _FilePath)
        {
            if (!System.IO.File.Exists(_FilePath))
            {
                Obfuscator.Report.Append(OPS.Editor.Report.EReportLevel.Warning, "There is no mapping file at path: " + _FilePath, true);
                return;
            }

            // Check Version
            bool var_Pre_4_0 = false;

            try
            {
                // Read mapping.
                using (System.IO.StreamReader var_Reader = new System.IO.StreamReader(new System.IO.FileStream(_FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read)))
                {
                    String var_First_Line = var_Reader.ReadLine();

                    // If is a json object, is past 4.0.
                    // If not a json object, is pre 4.0.
                    if (!OPS.Serialization.Json.JsonSerializer.IsJson(var_First_Line))
                    {
                        var_Pre_4_0 = true;
                    }
                }
            }
            catch (Exception e)
            {
                Obfuscator.Report.Append(OPS.Editor.Report.EReportLevel.Warning, "Failed to read mapping file at path: " + _FilePath + ". Exception: " + e.ToString(), true);
            }

            if(var_Pre_4_0)
            {
                try
                {
                    // Create a v3 mapping.
                    ObfuscatorV3Mapping var_ObfuscatorV3Mapping = new ObfuscatorV3Mapping();

                    // Deserialize from file path.
                    var_ObfuscatorV3Mapping.Deserialize(_FilePath);

                    // Parse to v4.
                    this.LoadedMapping.LoadFromVersion(var_ObfuscatorV3Mapping);
                }
                catch (Exception e)
                {
                    Obfuscator.Report.Append(OPS.Editor.Report.EReportLevel.Warning, "Failed to read mapping file at path: " + _FilePath + ". Exception: " + e.ToString(), true);
                }
            }
            else
            {
                try
                {
                    // Read mapping.
                    using (System.IO.StreamReader var_Reader = new System.IO.StreamReader(new System.IO.FileStream(_FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read)))
                    {
                        this.LoadedMapping = OPS.Serialization.Json.JsonSerializer.ParseStringToObject<ObfuscatorV4Mapping>(var_Reader.ReadToEnd());
                    }
                }
                catch (Exception e)
                {
                    Obfuscator.Report.Append(OPS.Editor.Report.EReportLevel.Warning, "Failed to read mapping file at path: " + _FilePath + ". Exception: " + e.ToString(), true);
                }
            }
        }

        /// <summary>
        /// Save the current mapping.
        /// </summary>
        /// <param name="_FilePath"></param>
        public void SaveMapping(String _FilePath)
        {
            // Saves the mapping.
            using (System.IO.StreamWriter var_Writer = new System.IO.StreamWriter(new System.IO.FileStream(_FilePath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite)))
            {
                var_Writer.Write(OPS.Serialization.Json.JsonSerializer.ParseObjectToJson(this.CurrentMapping, OPS.Serialization.Json.EJsonTextMode.Compact));
            }
        }

        #endregion

        // Name Free
        #region Name Free

        /// <summary>
        /// Returns true if the _Name is free and not used either in the loaded, current or original mapping.
        /// </summary>
        /// <param name="_MemberDefinition"></param>
        /// <param name="_MemberType"></param>
        /// <param name="_Name"></param>
        /// <returns></returns>
        public bool IsNameFree(IMemberDefinition _MemberDefinition, EMemberType _MemberType, String _Name)
        {
            if (this.NameGenerator.UseGlobalIndexer)
            {
                // Any loaded has the same obfuscated name as _Name.
                if (this.LoadedMapping.GetAllMemberKeys(_Name).Count > 0)
                {
                    return false;
                }

                // Any current assigned has the same obfuscated name as _Name.
                if (this.CurrentMapping.GetAllMemberKeys(_Name).Count > 0)
                {
                    return false;
                }

                // Any has the original name of _Name.
                if (this.OriginalMapping.GetAllMemberKeys(_Name).Count > 0)
                {
                    return false;
                }

                return true;
            }
            else
            {
                // Any loaded has the same obfuscated name as _Name.
                if (this.LoadedMapping.GetMapping(_MemberType).GetAllMemberKeys(_Name).Count > 0)
                {
                    return false;
                }

                // Any current assigned has the same obfuscated name as _Name.
                if (this.CurrentMapping.GetMapping(_MemberType).GetAllMemberKeys(_Name).Count > 0)
                {
                    return false;
                }

                // Any has the original name of _Name.
                if (this.OriginalMapping.GetMapping(_MemberType).GetAllMemberKeys(_Name).Count > 0)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion

        //Name Generator
        #region Name Generator

        /// <summary>
        /// Used to generate obfuscated names.
        /// </summary>
        public NameGenerator NameGenerator { get; private set; }

        #endregion
    }
}
