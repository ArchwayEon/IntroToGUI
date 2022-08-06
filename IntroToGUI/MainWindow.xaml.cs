using IntoToGUILib;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace IntroToGUI;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private AppWindow _appWindow;
    private ObservableCollection<Thing> _things;

    public MainWindow()
    {
        this.InitializeComponent();
        _appWindow = GetAppWindowForCurrentWindow();
        _appWindow.Title = "Introduction to GUI Programming";
        _appWindow.Resize(new SizeInt32(800, 600));
        CreateThings();
    }

    private AppWindow GetAppWindowForCurrentWindow()
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(this);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }

    private void CreateThings()
    {
        _things = new ObservableCollection<Thing>() 
        {
            new Thing() {Name = "Thing1", Age = 3},
            new Thing() {Name = "Thing2", Age = 13},
            new Thing() {Name = "Thing3", Age = 34},
            new Thing() {Name = "Thing4", Age = 9}
        };
        ThingsListView.ItemsSource = _things;
    }

    private async void OnFileNewThing(object sender, RoutedEventArgs e)
    {
        ContentDialog createNewThingDialog = new()
        {
            XamlRoot = MainPanel.XamlRoot,
            Title = "Creating a new Thing",
            Content = new CreateThingDialog(),
            PrimaryButtonText = "Create",
            SecondaryButtonText = "Don't Create",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary
        };

        ContentDialogResult result = await createNewThingDialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            CreateThingDialog content = 
                (CreateThingDialog)createNewThingDialog.Content;
            Thing thing = new()
            {
                Name = content.ThingName,
                Age = content.ThingAge
            };
            _things.Add(thing);
        }
    }

    private async void OnFileExit(object sender, RoutedEventArgs e)
    {
        ContentDialog confirmDialog = new()
        {
            XamlRoot = MainPanel.XamlRoot,
            Title = "Do you want to exit?",
            Content = "Please confirm that you want to exit.",
            CloseButtonText = "No, don't exit",
            PrimaryButtonText = "Yes, exit"
   
        };

        ContentDialogResult result = await confirmDialog.ShowAsync();
        if(result == ContentDialogResult.Primary)
        {
            this.Close();
        }
    }

    private async void OnThingClicked(object sender, ItemClickEventArgs e)
    {
        Thing thing = (Thing)e.ClickedItem;
        ContentDialog confirmDialog = new()
        {
            XamlRoot = MainPanel.XamlRoot,
            Title = $"Name: {thing.Name}",
            Content = $"Age: {thing.Age}",
            CloseButtonText = "OK"
        };
        await confirmDialog.ShowAsync();
    }
}
