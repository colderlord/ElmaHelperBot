[assembly: System.Runtime.InteropServices.Guid("bc6aec35-6d5c-46e6-b749-3d5b02d80885")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Reflection.AssemblyTitle("Elma Bot")]
[assembly: EleWise.ELMA.ComponentModel.ComponentAssembly()]
[assembly: EleWise.ELMA.Model.Attributes.ModelAssembly()]

namespace EleWise.ELMA.ELMAHelperBot
{
    using System;
    
    
    [global::EleWise.ELMA.Model.Attributes.MetadataType(typeof(global::EleWise.ELMA.Model.Metadata.AssemblyInfoMetadata))]
    [global::EleWise.ELMA.Model.Attributes.Uid("bc6aec35-6d5c-46e6-b749-3d5b02d80885")]
    [global::EleWise.ELMA.Model.Attributes.AssemblySummary(typeof(@__Resources__AssemblyInfo), "Summary")]
    internal class @__AssemblyInfo
    {
        
        /// <summary>
        /// UID of this class
        /// </summary>
        public const string UID_S = "bc6aec35-6d5c-46e6-b749-3d5b02d80885";
        
        private static global::System.Guid _UID = new global::System.Guid(UID_S);
        
        /// <summary>
        /// UID of this class
        /// </summary>
        public static global::System.Guid UID
        {
            get
            {
                return _UID;
            }
        }
    }
    
    internal class @__Resources__AssemblyInfo
    {
        
        public static string DisplayName
        {
            get
            {
                return global::EleWise.ELMA.SR.T("Elma Bot");
            }
        }
        
        public static string Summary
        {
            get
            {
                return global::EleWise.ELMA.SR.T("Elma Bot");
            }
        }
    }
}
