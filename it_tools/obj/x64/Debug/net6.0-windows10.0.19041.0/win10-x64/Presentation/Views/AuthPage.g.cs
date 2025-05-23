﻿#pragma checksum "C:\Users\dient\source\repos\it_tools\it_tools\Presentation\Views\AuthPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "AAF5E2A5E018E630B4ACA04706ECC0182EE26CA8EFB319A5DB416443C000345B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace it_tools.Presentation.Views
{
    partial class AuthPage : 
        global::Microsoft.UI.Xaml.Controls.Page, 
        global::Microsoft.UI.Xaml.Markup.IComponentConnector
    {

        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2309")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Presentation\Views\AuthPage.xaml line 48
                {
                    this.ErrorTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 3: // Presentation\Views\AuthPage.xaml line 56
                {
                    this.SuccessTextBlock = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 4: // Presentation\Views\AuthPage.xaml line 65
                {
                    this.RegisterForm = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.StackPanel>(target);
                }
                break;
            case 5: // Presentation\Views\AuthPage.xaml line 123
                {
                    this.LoginForm = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.StackPanel>(target);
                }
                break;
            case 6: // Presentation\Views\AuthPage.xaml line 134
                {
                    this.EmailLoginBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                }
                break;
            case 7: // Presentation\Views\AuthPage.xaml line 195
                {
                    global::Microsoft.UI.Xaml.Documents.Hyperlink element7 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Documents.Hyperlink>(target);
                    ((global::Microsoft.UI.Xaml.Documents.Hyperlink)element7).Click += this.SignUpLink_Click;
                }
                break;
            case 8: // Presentation\Views\AuthPage.xaml line 157
                {
                    this.LoginButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.LoginButton).Click += this.LoginButton_Click;
                }
                break;
            case 9: // Presentation\Views\AuthPage.xaml line 171
                {
                    this.GuessButton = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)this.GuessButton).Click += this.GuessButton_Click;
                }
                break;
            case 10: // Presentation\Views\AuthPage.xaml line 180
                {
                    this.GuessButtonText = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 11: // Presentation\Views\AuthPage.xaml line 183
                {
                    this.GuessProgressRing = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ProgressRing>(target);
                }
                break;
            case 12: // Presentation\Views\AuthPage.xaml line 166
                {
                    this.LoginButtonText = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBlock>(target);
                }
                break;
            case 13: // Presentation\Views\AuthPage.xaml line 168
                {
                    this.LoginProgressRing = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.ProgressRing>(target);
                }
                break;
            case 14: // Presentation\Views\AuthPage.xaml line 145
                {
                    this.PasswordLoginBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.PasswordBox>(target);
                }
                break;
            case 15: // Presentation\Views\AuthPage.xaml line 82
                {
                    this.EmailRegisterBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.TextBox>(target);
                }
                break;
            case 16: // Presentation\Views\AuthPage.xaml line 92
                {
                    this.PasswordRegisterBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.PasswordBox>(target);
                }
                break;
            case 17: // Presentation\Views\AuthPage.xaml line 102
                {
                    this.ConfirmPasswordBox = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.PasswordBox>(target);
                }
                break;
            case 18: // Presentation\Views\AuthPage.xaml line 110
                {
                    global::Microsoft.UI.Xaml.Controls.Button element18 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Button>(target);
                    ((global::Microsoft.UI.Xaml.Controls.Button)element18).Click += this.RegisterButton_Click;
                }
                break;
            case 19: // Presentation\Views\AuthPage.xaml line 76
                {
                    global::Microsoft.UI.Xaml.Documents.Hyperlink element19 = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Documents.Hyperlink>(target);
                    ((global::Microsoft.UI.Xaml.Documents.Hyperlink)element19).Click += this.LoginLink_Click;
                }
                break;
            case 20: // Presentation\Views\AuthPage.xaml line 16
                {
                    this.BackgroundImage = global::WinRT.CastExtensions.As<global::Microsoft.UI.Xaml.Controls.Image>(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler"," 3.0.0.2309")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Microsoft.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Microsoft.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

