﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace UWPMidTerm
{
    public partial class App : global::Windows.UI.Xaml.Markup.IXamlMetadataProvider
    {
        private global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlMetaDataProvider __appProvider;
        private global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlMetaDataProvider _AppProvider
        {
            get
            {
                if (__appProvider == null)
                {
                    __appProvider = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlMetaDataProvider();
                }
                return __appProvider;
            }
        }

        /// <summary>
        /// GetXamlType(Type)
        /// </summary>
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(global::System.Type type)
        {
            return _AppProvider.GetXamlType(type);
        }

        /// <summary>
        /// GetXamlType(String)
        /// </summary>
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(string fullName)
        {
            return _AppProvider.GetXamlType(fullName);
        }

        /// <summary>
        /// GetXmlnsDefinitions()
        /// </summary>
        public global::Windows.UI.Xaml.Markup.XmlnsDefinition[] GetXmlnsDefinitions()
        {
            return _AppProvider.GetXmlnsDefinitions();
        }
    }
}

namespace UWPMidTerm.UWPMidTerm_XamlTypeInfo
{
    /// <summary>
    /// Main class for providing metadata for the app or library
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public sealed class XamlMetaDataProvider : global::Windows.UI.Xaml.Markup.IXamlMetadataProvider
    {
        private global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlTypeInfoProvider _provider = null;

        private global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlTypeInfoProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlTypeInfoProvider();
                }
                return _provider;
            }
        }

        /// <summary>
        /// GetXamlType(Type)
        /// </summary>
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(global::System.Type type)
        {
            return Provider.GetXamlTypeByType(type);
        }

        /// <summary>
        /// GetXamlType(String)
        /// </summary>
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(string fullName)
        {
            return Provider.GetXamlTypeByName(fullName);
        }

        /// <summary>
        /// GetXmlnsDefinitions()
        /// </summary>
        public global::Windows.UI.Xaml.Markup.XmlnsDefinition[] GetXmlnsDefinitions()
        {
            return new global::Windows.UI.Xaml.Markup.XmlnsDefinition[0];
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal partial class XamlTypeInfoProvider
    {
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByType(global::System.Type type)
        {
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByType.TryGetValue(type, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByType(type);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByName.TryGetValue(typeName, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByName(typeName);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlMember GetMemberByLongName(string longMemberName)
        {
            if (string.IsNullOrEmpty(longMemberName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlMember xamlMember;
            if (_xamlMembers.TryGetValue(longMemberName, out xamlMember))
            {
                return xamlMember;
            }
            xamlMember = CreateXamlMember(longMemberName);
            if (xamlMember != null)
            {
                _xamlMembers.Add(longMemberName, xamlMember);
            }
            return xamlMember;
        }

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByName = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByType = new global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>
                _xamlMembers = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>();

        string[] _typeNameTable = null;
        global::System.Type[] _typeTable = null;

        private void InitTypeTables()
        {
            _typeNameTable = new string[18];
            _typeNameTable[0] = "UWPMidTerm.MainPage";
            _typeNameTable[1] = "Windows.UI.Xaml.Controls.Page";
            _typeNameTable[2] = "Windows.UI.Xaml.Controls.UserControl";
            _typeNameTable[3] = "UWPMidTerm.Converters.SingersConvert";
            _typeNameTable[4] = "Object";
            _typeNameTable[5] = "UWPMidTerm.Pages.AlbumRelease";
            _typeNameTable[6] = "UWPMidTerm.Pages.DownloadManager";
            _typeNameTable[7] = "UWPMidTerm.Converters.RectConvert";
            _typeNameTable[8] = "UWPMidTerm.Converters.LeftConvert";
            _typeNameTable[9] = "UWPMidTerm.Pages.FindMusic";
            _typeNameTable[10] = "UWPMidTerm.Pages.MediaPlayer";
            _typeNameTable[11] = "UWPMidTerm.Pages.MyCollection";
            _typeNameTable[12] = "UWPMidTerm.Pages.NativeMusic";
            _typeNameTable[13] = "UWPMidTerm.Pages.NewMusicRecommend";
            _typeNameTable[14] = "UWPMidTerm.Pages.SongHistory";
            _typeNameTable[15] = "UWPMidTerm.Pages.SongSheetRecommend";
            _typeNameTable[16] = "UWPMidTerm.Converters.LeftConvert2";
            _typeNameTable[17] = "UWPMidTerm.Pages.TopList";

            _typeTable = new global::System.Type[18];
            _typeTable[0] = typeof(global::UWPMidTerm.MainPage);
            _typeTable[1] = typeof(global::Windows.UI.Xaml.Controls.Page);
            _typeTable[2] = typeof(global::Windows.UI.Xaml.Controls.UserControl);
            _typeTable[3] = typeof(global::UWPMidTerm.Converters.SingersConvert);
            _typeTable[4] = typeof(global::System.Object);
            _typeTable[5] = typeof(global::UWPMidTerm.Pages.AlbumRelease);
            _typeTable[6] = typeof(global::UWPMidTerm.Pages.DownloadManager);
            _typeTable[7] = typeof(global::UWPMidTerm.Converters.RectConvert);
            _typeTable[8] = typeof(global::UWPMidTerm.Converters.LeftConvert);
            _typeTable[9] = typeof(global::UWPMidTerm.Pages.FindMusic);
            _typeTable[10] = typeof(global::UWPMidTerm.Pages.MediaPlayer);
            _typeTable[11] = typeof(global::UWPMidTerm.Pages.MyCollection);
            _typeTable[12] = typeof(global::UWPMidTerm.Pages.NativeMusic);
            _typeTable[13] = typeof(global::UWPMidTerm.Pages.NewMusicRecommend);
            _typeTable[14] = typeof(global::UWPMidTerm.Pages.SongHistory);
            _typeTable[15] = typeof(global::UWPMidTerm.Pages.SongSheetRecommend);
            _typeTable[16] = typeof(global::UWPMidTerm.Converters.LeftConvert2);
            _typeTable[17] = typeof(global::UWPMidTerm.Pages.TopList);
        }

        private int LookupTypeIndexByName(string typeName)
        {
            if (_typeNameTable == null)
            {
                InitTypeTables();
            }
            for (int i=0; i<_typeNameTable.Length; i++)
            {
                if(0 == string.CompareOrdinal(_typeNameTable[i], typeName))
                {
                    return i;
                }
            }
            return -1;
        }

        private int LookupTypeIndexByType(global::System.Type type)
        {
            if (_typeTable == null)
            {
                InitTypeTables();
            }
            for(int i=0; i<_typeTable.Length; i++)
            {
                if(type == _typeTable[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private object Activate_0_MainPage() { return new global::UWPMidTerm.MainPage(); }
        private object Activate_3_SingersConvert() { return new global::UWPMidTerm.Converters.SingersConvert(); }
        private object Activate_5_AlbumRelease() { return new global::UWPMidTerm.Pages.AlbumRelease(); }
        private object Activate_6_DownloadManager() { return new global::UWPMidTerm.Pages.DownloadManager(); }
        private object Activate_7_RectConvert() { return new global::UWPMidTerm.Converters.RectConvert(); }
        private object Activate_8_LeftConvert() { return new global::UWPMidTerm.Converters.LeftConvert(); }
        private object Activate_9_FindMusic() { return new global::UWPMidTerm.Pages.FindMusic(); }
        private object Activate_10_MediaPlayer() { return new global::UWPMidTerm.Pages.MediaPlayer(); }
        private object Activate_11_MyCollection() { return new global::UWPMidTerm.Pages.MyCollection(); }
        private object Activate_12_NativeMusic() { return new global::UWPMidTerm.Pages.NativeMusic(); }
        private object Activate_13_NewMusicRecommend() { return new global::UWPMidTerm.Pages.NewMusicRecommend(); }
        private object Activate_14_SongHistory() { return new global::UWPMidTerm.Pages.SongHistory(); }
        private object Activate_15_SongSheetRecommend() { return new global::UWPMidTerm.Pages.SongSheetRecommend(); }
        private object Activate_16_LeftConvert2() { return new global::UWPMidTerm.Converters.LeftConvert2(); }
        private object Activate_17_TopList() { return new global::UWPMidTerm.Pages.TopList(); }

        private global::Windows.UI.Xaml.Markup.IXamlType CreateXamlType(int typeIndex)
        {
            global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlSystemBaseType xamlType = null;
            global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType userType;
            string typeName = _typeNameTable[typeIndex];
            global::System.Type type = _typeTable[typeIndex];

            switch (typeIndex)
            {

            case 0:   //  UWPMidTerm.MainPage
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_0_MainPage;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 1:   //  Windows.UI.Xaml.Controls.Page
                xamlType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 2:   //  Windows.UI.Xaml.Controls.UserControl
                xamlType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 3:   //  UWPMidTerm.Converters.SingersConvert
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_3_SingersConvert;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 4:   //  Object
                xamlType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 5:   //  UWPMidTerm.Pages.AlbumRelease
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_5_AlbumRelease;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 6:   //  UWPMidTerm.Pages.DownloadManager
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_6_DownloadManager;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 7:   //  UWPMidTerm.Converters.RectConvert
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_7_RectConvert;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 8:   //  UWPMidTerm.Converters.LeftConvert
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_8_LeftConvert;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 9:   //  UWPMidTerm.Pages.FindMusic
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_9_FindMusic;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 10:   //  UWPMidTerm.Pages.MediaPlayer
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_10_MediaPlayer;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 11:   //  UWPMidTerm.Pages.MyCollection
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_11_MyCollection;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 12:   //  UWPMidTerm.Pages.NativeMusic
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_12_NativeMusic;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 13:   //  UWPMidTerm.Pages.NewMusicRecommend
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_13_NewMusicRecommend;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 14:   //  UWPMidTerm.Pages.SongHistory
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_14_SongHistory;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 15:   //  UWPMidTerm.Pages.SongSheetRecommend
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_15_SongSheetRecommend;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 16:   //  UWPMidTerm.Converters.LeftConvert2
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_16_LeftConvert2;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 17:   //  UWPMidTerm.Pages.TopList
                userType = new global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_17_TopList;
                userType.SetIsLocalType();
                xamlType = userType;
                break;
            }
            return xamlType;
        }



        private global::Windows.UI.Xaml.Markup.IXamlMember CreateXamlMember(string longMemberName)
        {
            global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlMember xamlMember = null;
            // No Local Properties
            return xamlMember;
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlSystemBaseType : global::Windows.UI.Xaml.Markup.IXamlType
    {
        string _fullName;
        global::System.Type _underlyingType;

        public XamlSystemBaseType(string fullName, global::System.Type underlyingType)
        {
            _fullName = fullName;
            _underlyingType = underlyingType;
        }

        public string FullName { get { return _fullName; } }

        public global::System.Type UnderlyingType
        {
            get
            {
                return _underlyingType;
            }
        }

        virtual public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name) { throw new global::System.NotImplementedException(); }
        virtual public bool IsArray { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsCollection { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsConstructible { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsDictionary { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsMarkupExtension { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsBindable { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsReturnTypeStub { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsLocalType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType ItemType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType KeyType { get { throw new global::System.NotImplementedException(); } }
        virtual public object ActivateInstance() { throw new global::System.NotImplementedException(); }
        virtual public void AddToMap(object instance, object key, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void AddToVector(object instance, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void RunInitializer()   { throw new global::System.NotImplementedException(); }
        virtual public object CreateFromString(string input)   { throw new global::System.NotImplementedException(); }
    }
    
    internal delegate object Activator();
    internal delegate void AddToCollection(object instance, object item);
    internal delegate void AddToDictionary(object instance, object key, object item);
    internal delegate object CreateFromStringMethod(string args);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlUserType : global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlSystemBaseType
    {
        global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlTypeInfoProvider _provider;
        global::Windows.UI.Xaml.Markup.IXamlType _baseType;
        bool _isArray;
        bool _isMarkupExtension;
        bool _isBindable;
        bool _isReturnTypeStub;
        bool _isLocalType;

        string _contentPropertyName;
        string _itemTypeName;
        string _keyTypeName;
        global::System.Collections.Generic.Dictionary<string, string> _memberNames;
        global::System.Collections.Generic.Dictionary<string, object> _enumValues;

        public XamlUserType(global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlTypeInfoProvider provider, string fullName, global::System.Type fullType, global::Windows.UI.Xaml.Markup.IXamlType baseType)
            :base(fullName, fullType)
        {
            _provider = provider;
            _baseType = baseType;
        }

        // --- Interface methods ----

        override public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { return _baseType; } }
        override public bool IsArray { get { return _isArray; } }
        override public bool IsCollection { get { return (CollectionAdd != null); } }
        override public bool IsConstructible { get { return (Activator != null); } }
        override public bool IsDictionary { get { return (DictionaryAdd != null); } }
        override public bool IsMarkupExtension { get { return _isMarkupExtension; } }
        override public bool IsBindable { get { return _isBindable; } }
        override public bool IsReturnTypeStub { get { return _isReturnTypeStub; } }
        override public bool IsLocalType { get { return _isLocalType; } }

        override public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty
        {
            get { return _provider.GetMemberByLongName(_contentPropertyName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType ItemType
        {
            get { return _provider.GetXamlTypeByName(_itemTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType KeyType
        {
            get { return _provider.GetXamlTypeByName(_keyTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name)
        {
            if (_memberNames == null)
            {
                return null;
            }
            string longName;
            if (_memberNames.TryGetValue(name, out longName))
            {
                return _provider.GetMemberByLongName(longName);
            }
            return null;
        }

        override public object ActivateInstance()
        {
            return Activator(); 
        }

        override public void AddToMap(object instance, object key, object item) 
        {
            DictionaryAdd(instance, key, item);
        }

        override public void AddToVector(object instance, object item)
        {
            CollectionAdd(instance, item);
        }

        override public void RunInitializer() 
        {
            global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(UnderlyingType.TypeHandle);
        }

        override public object CreateFromString(string input)
        {
            if (CreateFromStringMethod != null)
            {
                return this.CreateFromStringMethod(input);
            }
            else if (_enumValues != null)
            {
                int value = 0;

                string[] valueParts = input.Split(',');

                foreach (string valuePart in valueParts) 
                {
                    object partValue;
                    int enumFieldValue = 0;
                    try
                    {
                        if (_enumValues.TryGetValue(valuePart.Trim(), out partValue))
                        {
                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                        }
                        else
                        {
                            try
                            {
                                enumFieldValue = global::System.Convert.ToInt32(valuePart.Trim());
                            }
                            catch( global::System.FormatException )
                            {
                                foreach( string key in _enumValues.Keys )
                                {
                                    if( string.Compare(valuePart.Trim(), key, global::System.StringComparison.OrdinalIgnoreCase) == 0 )
                                    {
                                        if( _enumValues.TryGetValue(key.Trim(), out partValue) )
                                        {
                                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        value |= enumFieldValue; 
                    }
                    catch( global::System.FormatException )
                    {
                        throw new global::System.ArgumentException(input, FullName);
                    }
                }

                return value; 
            }
            throw new global::System.ArgumentException(input, FullName);
        }

        // --- End of Interface methods

        public Activator Activator { get; set; }
        public AddToCollection CollectionAdd { get; set; }
        public AddToDictionary DictionaryAdd { get; set; }
        public CreateFromStringMethod CreateFromStringMethod {get; set; }

        public void SetContentPropertyName(string contentPropertyName)
        {
            _contentPropertyName = contentPropertyName;
        }

        public void SetIsArray()
        {
            _isArray = true; 
        }

        public void SetIsMarkupExtension()
        {
            _isMarkupExtension = true;
        }

        public void SetIsBindable()
        {
            _isBindable = true;
        }

        public void SetIsReturnTypeStub()
        {
            _isReturnTypeStub = true;
        }

        public void SetIsLocalType()
        {
            _isLocalType = true;
        }

        public void SetItemTypeName(string itemTypeName)
        {
            _itemTypeName = itemTypeName;
        }

        public void SetKeyTypeName(string keyTypeName)
        {
            _keyTypeName = keyTypeName;
        }

        public void AddMemberName(string shortName)
        {
            if(_memberNames == null)
            {
                _memberNames =  new global::System.Collections.Generic.Dictionary<string,string>();
            }
            _memberNames.Add(shortName, FullName + "." + shortName);
        }

        public void AddEnumValue(string name, object value)
        {
            if (_enumValues == null)
            {
                _enumValues = new global::System.Collections.Generic.Dictionary<string, object>();
            }
            _enumValues.Add(name, value);
        }
    }

    internal delegate object Getter(object instance);
    internal delegate void Setter(object instance, object value);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.16.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlMember : global::Windows.UI.Xaml.Markup.IXamlMember
    {
        global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlTypeInfoProvider _provider;
        string _name;
        bool _isAttachable;
        bool _isDependencyProperty;
        bool _isReadOnly;

        string _typeName;
        string _targetTypeName;

        public XamlMember(global::UWPMidTerm.UWPMidTerm_XamlTypeInfo.XamlTypeInfoProvider provider, string name, string typeName)
        {
            _name = name;
            _typeName = typeName;
            _provider = provider;
        }

        public string Name { get { return _name; } }

        public global::Windows.UI.Xaml.Markup.IXamlType Type
        {
            get { return _provider.GetXamlTypeByName(_typeName); }
        }

        public void SetTargetTypeName(string targetTypeName)
        {
            _targetTypeName = targetTypeName;
        }
        public global::Windows.UI.Xaml.Markup.IXamlType TargetType
        {
            get { return _provider.GetXamlTypeByName(_targetTypeName); }
        }

        public void SetIsAttachable() { _isAttachable = true; }
        public bool IsAttachable { get { return _isAttachable; } }

        public void SetIsDependencyProperty() { _isDependencyProperty = true; }
        public bool IsDependencyProperty { get { return _isDependencyProperty; } }

        public void SetIsReadOnly() { _isReadOnly = true; }
        public bool IsReadOnly { get { return _isReadOnly; } }

        public Getter Getter { get; set; }
        public object GetValue(object instance)
        {
            if (Getter != null)
                return Getter(instance);
            else
                throw new global::System.InvalidOperationException("GetValue");
        }

        public Setter Setter { get; set; }
        public void SetValue(object instance, object value)
        {
            if (Setter != null)
                Setter(instance, value);
            else
                throw new global::System.InvalidOperationException("SetValue");
        }
    }
}
