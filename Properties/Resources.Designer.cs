﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PushCommand.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PushCommand.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] AdmPwd_PS {
            get {
                object obj = ResourceManager.GetObject("AdmPwd_PS", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] AdmPwd_PS_format {
            get {
                object obj = ResourceManager.GetObject("AdmPwd_PS_format", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] AdmPwd_PS1 {
            get {
                object obj = ResourceManager.GetObject("AdmPwd_PS1", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] AdmPwd_Utils {
            get {
                object obj = ResourceManager.GetObject("AdmPwd_Utils", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;Objs Version=&quot;1.1.0.1&quot; xmlns=&quot;http://schemas.microsoft.com/powershell/2004/04&quot;&gt;
        ///  &lt;Obj RefId=&quot;0&quot;&gt;
        ///    &lt;TN RefId=&quot;0&quot;&gt;
        ///      &lt;T&gt;Microsoft.PowerShell.Commands.PSRepositoryItemInfo&lt;/T&gt;
        ///      &lt;T&gt;System.Management.Automation.PSCustomObject&lt;/T&gt;
        ///      &lt;T&gt;System.Object&lt;/T&gt;
        ///    &lt;/TN&gt;
        ///    &lt;MS&gt;
        ///      &lt;S N=&quot;Name&quot;&gt;AdmPwd.PS&lt;/S&gt;
        ///      &lt;Version N=&quot;Version&quot;&gt;6.3.1.0&lt;/Version&gt;
        ///      &lt;S N=&quot;Type&quot;&gt;Module&lt;/S&gt;
        ///      &lt;S N=&quot;Description&quot;&gt;Provides cmdlets for configuration and usage of Local admin password management solut [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PSGetModuleInfo {
            get {
                return ResourceManager.GetString("PSGetModuleInfo", resourceCulture);
            }
        }
    }
}
