﻿#pragma checksum "D:\software\Workspaces\Visual Studio 2017\win 10 uwp\homework\homework1\HomeWork1\HomeWork1\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "39753E83009C92EDD008CF6A11B4C368"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HomeWork1
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.checkBox = (global::Windows.UI.Xaml.Controls.CheckBox)(target);
                    #line 11 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.CheckBox)this.checkBox).Click += this.checkBox_Click;
                    #line default
                }
                break;
            case 2:
                {
                    this.line = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

