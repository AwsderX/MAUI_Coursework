﻿using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace MAUI_Coursework;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new Views.LoginPage());
	}
}
