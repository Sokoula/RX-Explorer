﻿using ComputerVision;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.UI.Xaml.Controls;
using RX_Explorer.Class;
using RX_Explorer.Dialog;
using ShareClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Services.Store;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using AnimationController = RX_Explorer.Class.AnimationController;
using AnimationDirection = Windows.UI.Composition.AnimationDirection;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;
using NavigationViewPaneDisplayMode = Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode;

namespace RX_Explorer.View
{
    public sealed partial class SettingPage : UserControl
    {
        private readonly ObservableCollection<BackgroundPicture> PictureList = new ObservableCollection<BackgroundPicture>();
        private readonly ObservableCollection<TerminalProfile> TerminalList = new ObservableCollection<TerminalProfile>();
        private int AnimationLocker = 0;

        public static bool AllowTaskParalledExecution
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["TaskListParalledExecution"] is bool IsParalled)
                {
                    return IsParalled;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["TaskListParalledExecution"] = true;
                    return true;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["TaskListParalledExecution"] = value;
        }

        public static bool OpenPanelWhenTaskIsCreated
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["TaskListOpenPanelWhenNewTaskCreated"] is bool IsPanelOpened)
                {
                    return IsPanelOpened;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["TaskListOpenPanelWhenNewTaskCreated"] = true;
                    return true;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["TaskListOpenPanelWhenNewTaskCreated"] = value;
        }

        public static bool IsTaskListPinned
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["ShouldPinTaskList"] is bool ShouldPin)
                {
                    return ShouldPin;
                }
                else
                {
                    return false;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["ShouldPinTaskList"] = value;
        }

        public static bool IsDisplayProtectedSystemItems
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["DisplayProtectedSystemItems"] is bool IsDisplaySystemItems)
                {
                    return IsDisplaySystemItems;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["DisplayProtectedSystemItems"] = false;
                    return false;
                }
            }
            private set
            {
                ApplicationData.Current.LocalSettings.Values["DisplayProtectedSystemItems"] = value;
            }
        }

        public static bool IsDoubleClickEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["IsDoubleClickEnable"] is bool IsDoubleClick)
                {
                    return IsDoubleClick;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["IsDoubleClickEnable"] = true;
                    return true;
                }
            }
            private set
            {
                ApplicationData.Current.LocalSettings.Values["IsDoubleClickEnable"] = value;
            }
        }

        public static bool IsDetachTreeViewAndPresenter
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["DetachTreeViewAndPresenter"] is bool IsDetach)
                {
                    return IsDetach;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["DetachTreeViewAndPresenter"] = false;
                    return false;
                }
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values["DetachTreeViewAndPresenter"] = value;
            }
        }

        public static bool IsQuicklookEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["EnableQuicklook"] is bool Enable)
                {
                    return Enable;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["EnableQuicklook"] = true;
                    return true;
                }
            }
            private set
            {
                ApplicationData.Current.LocalSettings.Values["EnableQuicklook"] = value;
            }
        }

        public static bool IsShowHiddenFilesEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["DisplayHiddenItem"] is bool Display)
                {
                    return Display;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["DisplayHiddenItem"] = false;
                    return false;
                }
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values["DisplayHiddenItem"] = value;
            }
        }

        public static bool IsParallelShowContextMenu
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["ParallelShowContextMenu"] is bool IsParallel)
                {
                    return IsParallel;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["ParallelShowContextMenu"] = true;
                    return true;
                }
            }
            private set
            {
                ApplicationData.Current.LocalSettings.Values["ParallelShowContextMenu"] = value;
            }
        }

        public static bool IsTabPreviewEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["EnableTabPreview"] is bool Enabled)
                {
                    return Enabled;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["EnableTabPreview"] = true;
                    return true;
                }
            }
            private set
            {
                ApplicationData.Current.LocalSettings.Values["EnableTabPreview"] = value;
            }
        }

        public static bool IsShowFileExtensionsEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["EnableFileExtensions"] is bool Enabled)
                {
                    return Enabled;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["EnableFileExtensions"] = true;
                    return true;
                }
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values["EnableFileExtensions"] = value;
            }
        }

        public static bool IsPathHistoryEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["EnablePathHistory"] is bool Enabled)
                {
                    return Enabled;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["EnablePathHistory"] = true;
                    return true;
                }
            }
            private set
            {
                ApplicationData.Current.LocalSettings.Values["EnablePathHistory"] = value;

                if (!value)
                {
                    SQLite.Current.ClearTable("PathHistory");
                }
            }
        }

        public static bool IsSearchHistoryEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["EnableSearchHistory"] is bool Enabled)
                {
                    return Enabled;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["EnableSearchHistory"] = true;
                    return true;
                }
            }
            private set
            {
                ApplicationData.Current.LocalSettings.Values["EnableSearchHistory"] = value;

                if (!value)
                {
                    SQLite.Current.ClearTable("SearchHistory");
                }
            }
        }



        public static NavigationViewPaneDisplayMode LayoutMode
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["NavigationViewLayout"] is string Layout)
                {
                    return Enum.Parse<NavigationViewPaneDisplayMode>(Layout);
                }
                else
                {
                    return NavigationViewPaneDisplayMode.Top;
                }
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values["NavigationViewLayout"] = Enum.GetName(typeof(NavigationViewPaneDisplayMode), value);
            }
        }

        public static LoadMode ContentLoadMode
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["FileLoadMode"] is int SelectedIndex)
                {
                    switch (SelectedIndex)
                    {
                        case 0:
                            {
                                return LoadMode.None;
                            }
                        case 1:
                            {
                                return LoadMode.OnlyFile;
                            }
                        case 2:
                            {
                                return LoadMode.All;
                            }
                        default:
                            {
                                return LoadMode.Unknown;
                            }
                    }
                }
                else
                {
                    return LoadMode.OnlyFile;
                }
            }
        }

        public static SearchEngineFlyoutMode SearchEngineMode
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["SearchEngineFlyoutMode"] is int FlyoutModeIndex)
                {
                    switch (FlyoutModeIndex)
                    {
                        case 0:
                            {
                                return SearchEngineFlyoutMode.AlwaysPopup;
                            }
                        case 1:
                            {
                                return SearchEngineFlyoutMode.UseBuildInEngineAsDefault;
                            }
                        case 2:
                            {
                                return SearchEngineFlyoutMode.UseEverythingEngineAsDefault;
                            }
                        default:
                            {
                                return SearchEngineFlyoutMode.AlwaysPopup;
                            }
                    }
                }
                else
                {
                    return SearchEngineFlyoutMode.AlwaysPopup;
                }
            }
        }

        public static bool IsLibraryExpanderExpanded
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["LibraryExpanderIsExpand"] is bool IsExpand)
                {
                    return IsExpand;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["LibraryExpanderIsExpand"] = true;
                    return true;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["LibraryExpanderIsExpand"] = value;
        }

        public static bool IsDeviceExpanderExpanded
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["DeviceExpanderIsExpand"] is bool IsExpand)
                {
                    return IsExpand;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["DeviceExpanderIsExpand"] = true;
                    return true;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["DeviceExpanderIsExpand"] = value;
        }

        public static bool IsAlwaysLaunchNewProcess
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["AlwaysStartNew"] is bool AlwaysStartNew)
                {
                    return AlwaysStartNew;
                }
                else
                {
                    return true;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["AlwaysStartNew"] = value;
        }

        public static bool IsWindowAlwaysOnTop
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["AlwaysOnTop"] is bool IsAlwayOnTop)
                {
                    return IsAlwayOnTop;
                }
                else
                {
                    return false;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["AlwaysOnTop"] = value;
        }

        public static bool IsWindowsExplorerContextMenuIntegrated
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["IntegrateWithWindowsExplorerContextMenu"] is bool IsEnabled)
                {
                    return IsEnabled;
                }
                else
                {
                    return true;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["IntegrateWithWindowsExplorerContextMenu"] = value;
        }

        public static string DefaultTerminalName
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["DefaultTerminal"] is string Terminal)
                {
                    return Terminal;
                }
                else
                {
                    return SQLite.Current.GetAllTerminalProfile().FirstOrDefault()?.Name ?? string.Empty;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["DefaultTerminal"] = value;
        }

        public static bool IsPreventAcrylicFallbackEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["PreventFallBack"] is bool IsPrevent)
                {
                    return IsPrevent;
                }
                else
                {
                    return false;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["PreventFallBack"] = value;
        }

        public static bool IsContextMenuExtensionEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["ContextMenuExtSwitch"] is bool IsEnabled)
                {
                    return IsEnabled;
                }
                else
                {
                    return true;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["ContextMenuExtSwitch"] = value;
        }

        public static BackgroundBrushType CustomModeSubType
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["CustomModeSubType"] is string SubType)
                {
                    return Enum.Parse<BackgroundBrushType>(SubType);
                }
                else
                {
                    return BackgroundBrushType.CustomAcrylic;
                }
            }
            set
            {
                if (value is BackgroundBrushType.CustomAcrylic or BackgroundBrushType.Mica or BackgroundBrushType.Picture or BackgroundBrushType.BingPicture)
                {
                    ApplicationData.Current.LocalSettings.Values["CustomModeSubType"] = Enum.GetName(typeof(BackgroundBrushType), value);
                }
                else
                {
                    throw new ArgumentException("CustomModeSubType is not a valid value");
                }
            }
        }

        public static bool IsLoadWSLFolderOnStartupEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["LoadWSLFolderOnStartupEnabled"] is bool IsEnabled)
                {
                    return IsEnabled;
                }
                else
                {
                    return false;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["LoadWSLFolderOnStartupEnabled"] = value;
        }

        public static bool IsAvoidRecycleBinEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["AvoidRecycleBin"] is bool IsEnabled)
                {
                    return IsEnabled;
                }
                else
                {
                    return false;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["AvoidRecycleBin"] = value;
        }

        public static bool IsDoubleConfirmOnDeletionEnabled
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["DeleteConfirmSwitch"] is bool IsEnabled)
                {
                    return IsEnabled;
                }
                else
                {
                    return true;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["DeleteConfirmSwitch"] = value;
        }

        public static int DefaultDisplayModeIndex
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["DefaultDisplayMode"] is int Index)
                {
                    return Index;
                }
                else
                {
                    return 1;
                }
            }
            set => ApplicationData.Current.LocalSettings.Values["DefaultDisplayMode"] = value;
        }

        public static bool IsOpened { get; private set; }

        private string Version => $"{Globalization.GetString("SettingVersion/Text")}: {Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}.{Package.Current.Id.Version.Revision}";

        private bool HasInit;

        private readonly SemaphoreSlim SyncLocker = new SemaphoreSlim(1, 1);

        private SettingPage()
        {
            InitializeComponent();

            PictureGirdView.ItemsSource = PictureList;
            CloseButton.Content = Globalization.GetString("Common_Dialog_ConfirmButton");

            Loading += SettingDialog_Loading;
            AnimationController.Current.AnimationStateChanged += Current_AnimationStateChanged;
            BackgroundController.Current.BackgroundTypeChanged += Current_BackgroundTypeChanged;

            if (Globalization.CurrentLanguage == LanguageEnum.Chinese_Simplified)
            {
                if (FindName(nameof(CopyQQ)) is Button Btn)
                {
                    Btn.Visibility = Visibility.Visible;
                }
            }
        }

        private void Current_BackgroundTypeChanged(object sender, BackgroundBrushType Type)
        {
            PreventFallBack.IsEnabled = false;
            TintOpacitySlider.IsEnabled = false;
            AcrylicColor.IsEnabled = false;
            TintLuminositySlider.IsEnabled = false;
            PictureGirdView.IsEnabled = false;
            TintOpacitySliderLabel.Foreground = new SolidColorBrush(Colors.Gray);
            TintOpacitySliderValueText.Foreground = new SolidColorBrush(Colors.Gray);
            AccentColorLabel.Foreground = new SolidColorBrush(Colors.Gray);
            TintLuminositySliderLabel.Foreground = new SolidColorBrush(Colors.Gray);
            TintLuminositySliderValueText.Foreground = new SolidColorBrush(Colors.Gray);
            OtherEffectArea.Visibility = Visibility.Collapsed;
            GetBingPhotoState.Visibility = Visibility.Collapsed;

            switch (Type)
            {
                case BackgroundBrushType.DefaultAcrylic:
                    {
                        break;
                    }
                case BackgroundBrushType.CustomAcrylic:
                    {
                        PreventFallBack.IsEnabled = true;
                        TintOpacitySlider.IsEnabled = true;
                        AcrylicColor.IsEnabled = true;
                        TintLuminositySlider.IsEnabled = true;
                        TintOpacitySliderLabel.Foreground = Application.Current.Resources["DefaultTextForegroundThemeBrush"] as Brush;
                        TintOpacitySliderValueText.Foreground = Application.Current.Resources["DefaultTextForegroundThemeBrush"] as Brush;
                        AccentColorLabel.Foreground = Application.Current.Resources["DefaultTextForegroundThemeBrush"] as Brush;
                        TintLuminositySliderLabel.Foreground = Application.Current.Resources["DefaultTextForegroundThemeBrush"] as Brush;
                        TintLuminositySliderValueText.Foreground = Application.Current.Resources["DefaultTextForegroundThemeBrush"] as Brush;
                        break;
                    }
                case BackgroundBrushType.BingPicture:
                    {
                        OtherEffectArea.Visibility = Visibility.Visible;
                        break;
                    }
                case BackgroundBrushType.Picture:
                    {
                        PictureGirdView.IsEnabled = true;
                        OtherEffectArea.Visibility = Visibility.Visible;
                        break;
                    }
            }
        }

        public async Task ShowAsync()
        {
            if (Interlocked.CompareExchange(ref AnimationLocker, 1, 0) == 0)
            {
                try
                {
                    IsOpened = true;
                    Visibility = Visibility.Visible;

                    if (AnimationController.Current.IsEnableAnimation)
                    {
                        await Task.WhenAll(ActivateAnimation(RootGrid, TimeSpan.FromMilliseconds(500), TimeSpan.Zero, 250, false),
                                           ActivateAnimation(SettingNavigation, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(300), 350, false));
                    }
                }
                catch (Exception ex)
                {
                    LogTracer.Log(ex, $"An exception was threw in {nameof(ShowAsync)}");
                }
                finally
                {
                    Interlocked.Exchange(ref AnimationLocker, 0);
                }
            }
        }

        public async Task HideAsync()
        {
            if (Interlocked.CompareExchange(ref AnimationLocker, 1, 0) == 0)
            {
                try
                {
                    if (AnimationController.Current.IsEnableAnimation)
                    {
                        await Task.WhenAll(ActivateAnimation(RootGrid, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(300), 250, true),
                                           ActivateAnimation(SettingNavigation, TimeSpan.FromMilliseconds(500), TimeSpan.Zero, 350, true));
                    }

                    IsOpened = false;
                    Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    LogTracer.Log(ex, $"An exception was threw in {nameof(HideAsync)}");
                }
                finally
                {
                    Interlocked.Exchange(ref AnimationLocker, 0);
                }
            }
        }

        private async void SettingDialog_Loading(FrameworkElement sender, object args)
        {
            try
            {
                await InitializeAsync();

                using (FullTrustProcessController.ExclusiveUsage Exclusive = await FullTrustProcessController.GetAvailableControllerAsync())
                {
                    EnableQuicklook.IsEnabled = await Exclusive.Controller.CheckIfQuicklookIsAvaliableAsync();
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, "An exception was threw in initialize the setting page");
            }
        }

        private async void Current_AnimationStateChanged(object sender, bool e)
        {
            await Task.WhenAll(TabViewContainer.Current.TabCollection.Select((Tab) => Tab.Content)
                                                                     .Cast<Frame>()
                                                                     .Select((Frame) => Frame.Content)
                                                                     .Cast<TabItemContentRenderer>()
                                                                     .Select((Renderer) => Renderer.RefreshPresentersAsync()));
        }

        public static async Task<SettingPage> CreateAsync()
        {
            SettingPage Page = new SettingPage();
            await Page.InitializeAsync();
            return Page;
        }

        private async Task InitializeAsync()
        {
            if (!HasInit)
            {
                HasInit = true;

                UIMode.Items.Add(Globalization.GetString("Setting_UIMode_Recommend"));
                UIMode.Items.Add(Globalization.GetString("Setting_UIMode_SolidColor"));
                UIMode.Items.Add(Globalization.GetString("Setting_UIMode_Custom"));

                FolderOpenMethod.Items.Add(Globalization.GetString("Folder_Open_Method_2"));
                FolderOpenMethod.Items.Add(Globalization.GetString("Folder_Open_Method_1"));

                ThemeColor.Items.Add(Globalization.GetString("Font_Color_Black"));
                ThemeColor.Items.Add(Globalization.GetString("Font_Color_White"));

                FileLoadMode.Items.Add(Globalization.GetString("LoadMode_None_Text"));
                FileLoadMode.Items.Add(Globalization.GetString("LoadMode_OnlyFile_Text"));
                FileLoadMode.Items.Add(Globalization.GetString("LoadMode_FileAndFolder_Text"));

                SearchEngineConfig.Items.Add(Globalization.GetString("SearchEngineConfig_AlwaysPopup"));
                SearchEngineConfig.Items.Add(Globalization.GetString("SearchEngineConfig_UseBuildInAsDefault"));
                SearchEngineConfig.Items.Add(Globalization.GetString("SearchEngineConfig_UseEverythingAsDefault"));

                TerminalList.AddRange(SQLite.Current.GetAllTerminalProfile());

                await ApplyLocalSettingsAsync();

                DisplayHiddenItem.Toggled += DisplayHiddenItem_Toggled;
                FileExtensionSwitch.Toggled += FileExtensionSwitch_Toggled;
                TreeViewDetach.Toggled += TreeViewDetach_Toggled;
                FileLoadMode.SelectionChanged += FileLoadMode_SelectionChanged;
                LanguageComboBox.SelectionChanged += LanguageComboBox_SelectionChanged;
                FontFamilyComboBox.SelectionChanged += FontFamilyComboBox_SelectionChanged;

                if (WindowsVersionChecker.IsOlderOrEqual(Class.Version.Windows10_2004))
                {
                    DisableSelectionAnimation.Checked += DisableSelectionAnimation_Changed;
                    DisableSelectionAnimation.Unchecked += DisableSelectionAnimation_Changed;
                }

                ApplicationData.Current.DataChanged += Current_DataChanged;

                if (await MSStoreHelper.Current.CheckPurchaseStatusAsync())
                {
                    PurchaseApp.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (SearchEngineConfig.SelectedIndex == 2)
                    {
                        SearchEngineConfig.SelectedIndex = 0;
                    }

                    SearchEngineConfig.Items.Remove(Globalization.GetString("SearchEngineConfig_UseEverythingAsDefault"));
                }

                if (await MSStoreHelper.Current.CheckHasUpdateAsync())
                {
                    VersionTip.Text = Globalization.GetString("UpdateAvailable");
                }

                if (PictureList.Count == 0)
                {
                    PictureList.AddRange(await GetCustomPictureAsync());
                }
            }
        }

        private async void Current_DataChanged(ApplicationData sender, object args)
        {
            await SyncLocker.WaitAsync();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
            {
                try
                {
                    IEnumerable<TerminalProfile> CurrentTerminalProfiles = SQLite.Current.GetAllTerminalProfile();

                    foreach (TerminalProfile NewProfile in CurrentTerminalProfiles.Except(TerminalList).ToArray())
                    {
                        TerminalList.Add(NewProfile);
                    }

                    foreach (TerminalProfile RemoveProfile in TerminalList.Except(CurrentTerminalProfiles).ToArray())
                    {
                        TerminalList.Remove(RemoveProfile);
                    }

                    await ApplyLocalSettingsAsync();

                    if (UIMode.SelectedIndex == BackgroundController.Current.CurrentType switch
                    {
                        BackgroundBrushType.DefaultAcrylic => 0,
                        BackgroundBrushType.SolidColor => 1,
                        _ => 2
                    })
                    {
                        switch (BackgroundController.Current.CurrentType)
                        {
                            case BackgroundBrushType.SolidColor:
                                {
                                    if (ApplicationData.Current.LocalSettings.Values["SolidColorType"] is string ColorType)
                                    {
                                        if (ColorType == Colors.White.ToString())
                                        {
                                            SolidColor_White.IsChecked = true;
                                        }
                                        else
                                        {
                                            SolidColor_Black.IsChecked = true;
                                        }
                                    }
                                    else
                                    {
                                        SolidColor_FollowSystem.IsChecked = true;
                                    }

                                    break;
                                }
                            case BackgroundBrushType.CustomAcrylic:
                                {
                                    if (AcrylicMode.IsChecked.GetValueOrDefault())
                                    {
                                        PreventFallBack.IsChecked = IsPreventAcrylicFallbackEnabled;
                                    }
                                    else
                                    {
                                        AcrylicMode.IsChecked = true;
                                    }

                                    break;
                                }
                            case BackgroundBrushType.BingPicture:
                                {
                                    BingPictureMode.IsChecked = true;
                                    break;
                                }
                            case BackgroundBrushType.Mica:
                                {
                                    MicaMode.IsChecked = true;
                                    break;
                                }
                            case BackgroundBrushType.Picture:
                                {
                                    if (PictureMode.IsChecked.GetValueOrDefault())
                                    {
                                        if (ApplicationData.Current.LocalSettings.Values["PictureBackgroundUri"] is string Uri)
                                        {
                                            if (PictureList.FirstOrDefault((Picture) => Picture.PictureUri.ToString() == Uri) is BackgroundPicture PictureItem)
                                            {
                                                PictureGirdView.SelectedItem = PictureItem;
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    BackgroundPicture Picture = await BackgroundPicture.CreateAsync(new Uri(Uri));

                                                    if (!PictureList.Contains(Picture))
                                                    {
                                                        PictureList.Add(Picture);
                                                        PictureGirdView.UpdateLayout();
                                                        PictureGirdView.SelectedItem = Picture;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogTracer.Log(ex, "Sync setting failure, background picture could not be found");
                                                }
                                            }
                                        }
                                        else if (PictureList.Count > 0)
                                        {
                                            PictureGirdView.SelectedIndex = 0;
                                        }
                                        else
                                        {
                                            PictureGirdView.SelectedIndex = -1;
                                        }
                                    }
                                    else
                                    {
                                        PictureMode.IsChecked = true;
                                    }

                                    break;
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogTracer.Log(ex, $"An exception was threw in Current_DataChanged");
                }
                finally
                {
                    SyncLocker.Release();
                }
            });
        }

        private Task ActivateAnimation(UIElement Element, TimeSpan Duration, TimeSpan DelayTime, float VerticalOffset, bool IsReverse)
        {
            Visual Visual = ElementCompositionPreview.GetElementVisual(Element);

            CompositionScopedBatch Batch = Visual.Compositor.CreateScopedBatch(CompositionBatchTypes.Animation);

            Vector3KeyFrameAnimation EntranceAnimation = Visual.Compositor.CreateVector3KeyFrameAnimation();
            ScalarKeyFrameAnimation FadeAnimation = Visual.Compositor.CreateScalarKeyFrameAnimation();

            EntranceAnimation.Target = nameof(Visual.Offset);
            EntranceAnimation.InsertKeyFrame(0, new Vector3(Visual.Offset.X, VerticalOffset, Visual.Offset.Z));
            EntranceAnimation.InsertKeyFrame(1, new Vector3(Visual.Offset.X, 0, Visual.Offset.Z), Visual.Compositor.CreateCubicBezierEasingFunction(new Vector2(.1f, .9f), new Vector2(.2f, 1)));
            EntranceAnimation.Duration = Duration;
            EntranceAnimation.DelayTime = DelayTime;
            EntranceAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;

            FadeAnimation.Target = nameof(Visual.Opacity);
            FadeAnimation.InsertKeyFrame(0, 0);
            FadeAnimation.InsertKeyFrame(1, 1);
            FadeAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
            FadeAnimation.DelayTime = DelayTime;
            FadeAnimation.Duration = Duration;

            if (IsReverse)
            {
                EntranceAnimation.Direction = AnimationDirection.Reverse;
                FadeAnimation.Direction = AnimationDirection.Reverse;
            }
            else
            {
                EntranceAnimation.Direction = AnimationDirection.Normal;
                FadeAnimation.Direction = AnimationDirection.Normal;
            }

            CompositionAnimationGroup AnimationGroup = Visual.Compositor.CreateAnimationGroup();
            AnimationGroup.Add(EntranceAnimation);
            AnimationGroup.Add(FadeAnimation);

            Visual.StartAnimationGroup(AnimationGroup);

            TaskCompletionSource<bool> CompletionTask = new TaskCompletionSource<bool>();

            Batch.End();
            Batch.Completed += (s, e) =>
            {
                CompletionTask.SetResult(true);
            };

            return CompletionTask.Task;
        }

        private async Task ApplyLocalSettingsAsync()
        {
            DefaultTerminal.SelectionChanged -= DefaultTerminal_SelectionChanged;
            UseWinAndEActivate.Toggled -= UseWinAndEActivate_Toggled;
            InterceptFolderSwitch.Toggled -= InterceptFolder_Toggled;
            AutoBoot.Toggled -= AutoBoot_Toggled;
            WindowsExplorerContextMenu.Toggled -= WindowsExplorerContextMenu_Toggled;
            HideProtectedSystemItems.Checked -= HideProtectedSystemItems_Checked;
            HideProtectedSystemItems.Unchecked -= HideProtectedSystemItems_Unchecked;
            DefaultDisplayMode.SelectionChanged -= DefaultDisplayMode_SelectionChanged;

            BuiltInEngineIgnoreCase.Checked -= SeachEngineOptionSave_Checked;
            BuiltInEngineIgnoreCase.Unchecked -= SeachEngineOptionSave_UnChecked;
            BuiltInEngineIncludeRegex.Checked -= SeachEngineOptionSave_Checked;
            BuiltInEngineIncludeRegex.Unchecked -= SeachEngineOptionSave_UnChecked;
            BuiltInSearchAllSubFolders.Checked -= SeachEngineOptionSave_Checked;
            BuiltInSearchAllSubFolders.Unchecked -= SeachEngineOptionSave_UnChecked;
            BuiltInEngineIncludeAQS.Checked -= SeachEngineOptionSave_Checked;
            BuiltInEngineIncludeAQS.Unchecked -= SeachEngineOptionSave_UnChecked;
            BuiltInSearchUseIndexer.Checked -= SeachEngineOptionSave_Checked;
            BuiltInSearchUseIndexer.Unchecked -= SeachEngineOptionSave_UnChecked;
            EverythingEngineIgnoreCase.Checked -= SeachEngineOptionSave_Checked;
            EverythingEngineIgnoreCase.Unchecked -= SeachEngineOptionSave_UnChecked;
            EverythingEngineIncludeRegex.Checked -= SeachEngineOptionSave_Checked;
            EverythingEngineIncludeRegex.Unchecked -= SeachEngineOptionSave_UnChecked;
            EverythingEngineSearchGloble.Checked -= SeachEngineOptionSave_Checked;
            EverythingEngineSearchGloble.Unchecked -= SeachEngineOptionSave_UnChecked;
            ShowContextMenuWhenLoading.Checked -= ShowContextMenuWhenLoading_Checked;
            ShowContextMenuWhenLoading.Unchecked -= ShowContextMenuWhenLoading_Unchecked;

            LanguageComboBox.SelectedIndex = Convert.ToInt32(ApplicationData.Current.LocalSettings.Values["LanguageOverride"]);

            FontFamilyComboBox.SelectedIndex = ApplicationData.Current.LocalSettings.Values["DefaultFontFamilyOverride"] is string OverrideString
                                                  ? Array.IndexOf(FontFamilyController.GetInstalledFontFamily().ToArray(), JsonSerializer.Deserialize<InstalledFonts>(OverrideString))
                                                  : Array.IndexOf(FontFamilyController.GetInstalledFontFamily().ToArray(), FontFamilyController.Default);

            BackgroundBlurSlider.Value = Convert.ToSingle(ApplicationData.Current.LocalSettings.Values["BackgroundBlurValue"]);
            BackgroundLightSlider.Value = Convert.ToSingle(ApplicationData.Current.LocalSettings.Values["BackgroundLightValue"]);

            AutoBoot.IsOn = (await StartupTask.GetAsync("RXExplorer")).State switch
            {
                StartupTaskState.DisabledByPolicy
                or StartupTaskState.DisabledByUser
                or StartupTaskState.Disabled => false,
                _ => true
            };

            FolderOpenMethod.SelectedIndex = IsDoubleClickEnabled ? 1 : 0;
            TreeViewDetach.IsOn = !IsDetachTreeViewAndPresenter;
            EnableQuicklook.IsOn = IsQuicklookEnabled;
            DisplayHiddenItem.IsOn = IsShowHiddenFilesEnabled;
            HideProtectedSystemItems.IsChecked = !IsDisplayProtectedSystemItems;
            TabPreviewSwitch.IsOn = IsTabPreviewEnabled;
            SearchHistory.IsOn = IsSearchHistoryEnabled;
            PathHistory.IsOn = IsPathHistoryEnabled;
            NavigationViewLayout.IsOn = LayoutMode == NavigationViewPaneDisplayMode.LeftCompact;
            AlwaysLaunchNew.IsChecked = IsAlwaysLaunchNewProcess;
            AlwaysOnTop.IsOn = IsWindowAlwaysOnTop;
            WindowsExplorerContextMenu.IsOn = IsWindowsExplorerContextMenuIntegrated;
            ContextMenuExtSwitch.IsOn = IsContextMenuExtensionEnabled;
            FileExtensionSwitch.IsOn = IsShowFileExtensionsEnabled;
            ShowContextMenuWhenLoading.IsChecked = !IsParallelShowContextMenu;
            LoadWSLOnStartup.IsOn = IsLoadWSLFolderOnStartupEnabled;
            AvoidRecycleBin.IsChecked = IsAvoidRecycleBinEnabled;
            DeleteConfirmSwitch.IsOn = IsDoubleConfirmOnDeletionEnabled;
            DefaultDisplayMode.SelectedIndex = DefaultDisplayModeIndex;

#if DEBUG
            SettingShareData.IsOn = false;
#else
            SettingShareData.IsOn = await Microsoft.AppCenter.AppCenter.IsEnabledAsync();
#endif

            UIMode.SelectedIndex = BackgroundController.Current.CurrentType switch
            {
                BackgroundBrushType.DefaultAcrylic => 0,
                BackgroundBrushType.SolidColor => 1,
                _ => 2
            };

            if (TerminalList.FirstOrDefault((Profile) => Profile.Name == DefaultTerminalName) is TerminalProfile Profile)
            {
                DefaultTerminal.SelectedItem = Profile;
            }
            else if (TerminalList.Count > 0)
            {
                DefaultTerminal.SelectedIndex = 0;
            }

            if (ApplicationData.Current.LocalSettings.Values["InterceptWindowsE"] is bool IsInterceptedWinE)
            {
                UseWinAndEActivate.IsOn = IsInterceptedWinE;
            }

            if (ApplicationData.Current.LocalSettings.Values["InterceptDesktopFolder"] is bool IsInterceptedDesktopFolder)
            {
                InterceptFolderSwitch.IsOn = IsInterceptedDesktopFolder;
            }

            if (ApplicationData.Current.LocalSettings.Values["FileLoadMode"] is int SelectedIndex)
            {
                FileLoadMode.SelectedIndex = SelectedIndex;
            }
            else
            {
                FileLoadMode.SelectedIndex = 1;
            }

            if (ApplicationData.Current.LocalSettings.Values["SearchEngineFlyoutMode"] is int FlyoutModeIndex)
            {
                if (FlyoutModeIndex >= SearchEngineConfig.Items.Count)
                {
                    SearchEngineConfig.SelectedIndex = 0;
                }
                else
                {
                    SearchEngineConfig.SelectedIndex = FlyoutModeIndex;
                }
            }
            else
            {
                SearchEngineConfig.SelectedIndex = 0;
            }

            switch (SearchEngineConfig.SelectedIndex)
            {
                case 1:
                    {
                        SearchOptions Options = SearchOptions.LoadSavedConfiguration(SearchCategory.BuiltInEngine);
                        BuiltInEngineIgnoreCase.IsChecked = Options.IgnoreCase;
                        BuiltInEngineIncludeRegex.IsChecked = Options.UseRegexExpression;
                        BuiltInSearchAllSubFolders.IsChecked = Options.DeepSearch;
                        BuiltInEngineIncludeAQS.IsChecked = Options.UseAQSExpression;
                        BuiltInSearchUseIndexer.IsChecked = Options.UseIndexerOnly;
                        break;
                    }
                case 2:
                    {
                        SearchOptions Options = SearchOptions.LoadSavedConfiguration(SearchCategory.EverythingEngine);
                        EverythingEngineIgnoreCase.IsChecked = Options.IgnoreCase;
                        EverythingEngineIncludeRegex.IsChecked = Options.UseRegexExpression;
                        EverythingEngineSearchGloble.IsChecked = Options.DeepSearch;
                        break;
                    }
            }

            switch (StartupModeController.Mode)
            {
                case StartupMode.CreateNewTab:
                    {
                        StartupWithNewTab.IsChecked = true;
                        break;
                    }
                case StartupMode.SpecificTab:
                    {
                        StartupSpecificTab.IsChecked = true;

                        IEnumerable<string> PathArray = await StartupModeController.GetAllPathAsync(StartupMode.SpecificTab).Select((Item) => Item.FirstOrDefault()).OfType<string>().ToArrayAsync();

                        foreach (string AddItem in PathArray.Except(SpecificTabListView.Items.Cast<string>()))
                        {
                            SpecificTabListView.Items.Add(AddItem);
                        }

                        foreach (string RemoveItem in SpecificTabListView.Items.Cast<string>().Except(PathArray))
                        {
                            SpecificTabListView.Items.Remove(RemoveItem);
                        }

                        break;
                    }
                case StartupMode.LastOpenedTab:
                    {
                        StartupWithLastTab.IsChecked = true;
                        break;
                    }
            }

            UseWinAndEActivate.Toggled += UseWinAndEActivate_Toggled;
            InterceptFolderSwitch.Toggled += InterceptFolder_Toggled;
            DefaultTerminal.SelectionChanged += DefaultTerminal_SelectionChanged;
            AutoBoot.Toggled += AutoBoot_Toggled;
            WindowsExplorerContextMenu.Toggled += WindowsExplorerContextMenu_Toggled;
            HideProtectedSystemItems.Checked += HideProtectedSystemItems_Checked;
            HideProtectedSystemItems.Unchecked += HideProtectedSystemItems_Unchecked;
            DefaultDisplayMode.SelectionChanged += DefaultDisplayMode_SelectionChanged;

            BuiltInEngineIgnoreCase.Checked += SeachEngineOptionSave_Checked;
            BuiltInEngineIgnoreCase.Unchecked += SeachEngineOptionSave_UnChecked;
            BuiltInEngineIncludeRegex.Checked += SeachEngineOptionSave_Checked;
            BuiltInEngineIncludeRegex.Unchecked += SeachEngineOptionSave_UnChecked;
            BuiltInSearchAllSubFolders.Checked += SeachEngineOptionSave_Checked;
            BuiltInSearchAllSubFolders.Unchecked += SeachEngineOptionSave_UnChecked;
            BuiltInEngineIncludeAQS.Checked += SeachEngineOptionSave_Checked;
            BuiltInEngineIncludeAQS.Unchecked += SeachEngineOptionSave_UnChecked;
            BuiltInSearchUseIndexer.Checked += SeachEngineOptionSave_Checked;
            BuiltInSearchUseIndexer.Unchecked += SeachEngineOptionSave_UnChecked;
            EverythingEngineIgnoreCase.Checked += SeachEngineOptionSave_Checked;
            EverythingEngineIgnoreCase.Unchecked += SeachEngineOptionSave_UnChecked;
            EverythingEngineIncludeRegex.Checked += SeachEngineOptionSave_Checked;
            EverythingEngineIncludeRegex.Unchecked += SeachEngineOptionSave_UnChecked;
            EverythingEngineSearchGloble.Checked += SeachEngineOptionSave_Checked;
            EverythingEngineSearchGloble.Unchecked += SeachEngineOptionSave_UnChecked;
            ShowContextMenuWhenLoading.Checked += ShowContextMenuWhenLoading_Checked;
            ShowContextMenuWhenLoading.Unchecked += ShowContextMenuWhenLoading_Unchecked;
        }

        private void WindowsExplorerContextMenu_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                IsWindowsExplorerContextMenuIntegrated = WindowsExplorerContextMenu.IsOn;
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw in {nameof(WindowsExplorerContextMenu_Toggled)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (FontFamilyComboBox.SelectedItem is InstalledFonts NewFont)
                {
                    if (FontFamilyController.SwitchTo(NewFont))
                    {
                        if (!InfoTipController.Current.CheckIfAlreadyOpened(InfoTipType.FontFamilyRestartRequired))
                        {
                            InfoTipController.Current.Show(InfoTipType.FontFamilyRestartRequired);
                        }
                    }
                    else
                    {
                        InfoTipController.Current.Hide(InfoTipType.FontFamilyRestartRequired);
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw in {nameof(FontFamilyComboBox_SelectionChanged)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void DefaultDisplayMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DefaultDisplayModeIndex = DefaultDisplayMode.SelectedIndex;
                SQLite.Current.SetDefaultDisplayMode(DefaultDisplayMode.SelectedIndex);
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw in {nameof(DefaultDisplayMode_SelectionChanged)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void NavigationViewLayout_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NavigationViewLayout.IsOn)
                {
                    LayoutMode = NavigationViewPaneDisplayMode.LeftCompact;
                    MainPage.Current.NavView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftCompact;
                }
                else
                {
                    LayoutMode = NavigationViewPaneDisplayMode.Top;
                    MainPage.Current.NavView.PaneDisplayMode = NavigationViewPaneDisplayMode.Top;
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw in {nameof(NavigationViewLayout_Toggled)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void AlwaysOnTop_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                IsWindowAlwaysOnTop = AlwaysOnTop.IsOn;

                using (FullTrustProcessController.ExclusiveUsage Exclusive = await FullTrustProcessController.GetAvailableControllerAsync())
                {
                    using Process CurrentProcess = Process.GetCurrentProcess();

                    if (AlwaysOnTop.IsOn)
                    {
                        if (!await Exclusive.Controller.SetAsTopMostWindowAsync(Package.Current.Id.FamilyName, Convert.ToUInt32(CurrentProcess.Id)))
                        {
                            QueueContentDialog Dialog = new QueueContentDialog
                            {
                                Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                Content = Globalization.GetString("QueueDialog_SetTopMostFailed_Content"),
                                CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                            };

                            await Dialog.ShowAsync();
                        }
                    }
                    else
                    {
                        if (!await Exclusive.Controller.RemoveTopMostWindowAsync(Package.Current.Id.FamilyName, Convert.ToUInt32(CurrentProcess.Id)))
                        {
                            QueueContentDialog Dialog = new QueueContentDialog
                            {
                                Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                Content = Globalization.GetString("QueueDialog_RemoveTopMostFailed_Content"),
                                CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                            };

                            await Dialog.ShowAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw in {nameof(AlwaysOnTop_Toggled)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void FileLoadMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ApplicationData.Current.LocalSettings.Values["FileLoadMode"] = FileLoadMode.SelectedIndex;

                foreach (TabItemContentRenderer Renderer in TabViewContainer.Current.TabCollection.Select((Tab) => Tab.Content)
                                                                                                  .Cast<Frame>()
                                                                                                  .Select((Frame) => Frame.Content)
                                                                                                  .Cast<TabItemContentRenderer>())
                {
                    await Task.WhenAll(Renderer.ClearAndRebuildTreeViewAsync(), Renderer.RefreshPresentersAsync());
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw in {nameof(FileLoadMode_SelectionChanged)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void DefaultTerminal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DefaultTerminal.SelectedItem is TerminalProfile Profile)
            {
                DefaultTerminalName = Convert.ToString(Profile.Name);
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void ClearUp_Click(object sender, RoutedEventArgs e)
        {
            ResetDialog Dialog = new ResetDialog();

            if ((await Dialog.ShowAsync()) == ContentDialogResult.Primary)
            {
                if (!Dialog.IsClearSecureFolder)
                {
                    LoadingText.Text = Globalization.GetString("Progress_Tip_Exporting");
                    LoadingControl.IsLoading = true;

                    if (await FileSystemStorageItemBase.CreateNewAsync(Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "SecureFolder"), CreateType.Folder, CreateOption.OpenIfExist) is FileSystemStorageFolder SecureFolder)
                    {
                        try
                        {
                            await foreach (FileSystemStorageFile Item in SecureFolder.GetChildItemsAsync(false, false, Filter: BasicFilters.File))
                            {
                                string DecryptedFilePath = Path.Combine(Dialog.ExportFolder.Path, Path.GetRandomFileName());

                                if (await FileSystemStorageItemBase.CreateNewAsync(DecryptedFilePath, CreateType.File, CreateOption.GenerateUniqueName) is FileSystemStorageFile DecryptedFile)
                                {
                                    using (Stream EncryptedFStream = await Item.GetStreamFromFileAsync(AccessMode.Read, OptimizeOption.RandomAccess))
                                    using (SLEInputStream SLEStream = new SLEInputStream(EncryptedFStream, SecureArea.AESKey))
                                    {
                                        using (Stream DecryptedFStream = await DecryptedFile.GetStreamFromFileAsync(AccessMode.Write, OptimizeOption.Sequential))
                                        {
                                            await SLEStream.CopyToAsync(DecryptedFStream, 2048);
                                            await DecryptedFStream.FlushAsync();
                                        }

                                        await DecryptedFile.RenameAsync(SLEStream.Header.FileName);
                                    }
                                }
                            }
                        }
                        catch (PasswordErrorException)
                        {
                            QueueContentDialog Dialog1 = new QueueContentDialog
                            {
                                Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                Content = Globalization.GetString("QueueDialog_DecryptPasswordError_Content"),
                                CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                            };

                            await Dialog1.ShowAsync();
                        }
                        catch (FileDamagedException)
                        {
                            QueueContentDialog Dialog1 = new QueueContentDialog
                            {
                                Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                Content = Globalization.GetString("QueueDialog_FileDamageError_Content"),
                                CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                            };

                            await Dialog1.ShowAsync();
                        }
                        catch (Exception)
                        {
                            QueueContentDialog Dialog1 = new QueueContentDialog
                            {
                                Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                Content = Globalization.GetString("QueueDialog_EncryptError_Content"),
                                CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                            };

                            await Dialog.ShowAsync();
                        }
                    }

                    await Task.Delay(1000);
                }

                try
                {
                    SQLite.Current.ClearAllData();
                    ApplicationData.Current.LocalSettings.Values.Clear();

                    Window.Current.Activate();

                    switch (await CoreApplication.RequestRestartAsync("Restart"))
                    {
                        case AppRestartFailureReason.InvalidUser:
                        case AppRestartFailureReason.NotInForeground:
                        case AppRestartFailureReason.Other:
                            {
                                QueueContentDialog Dialog1 = new QueueContentDialog
                                {
                                    Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                    Content = Globalization.GetString("QueueDialog_RestartFail_Content"),
                                    CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                                };

                                await Dialog1.ShowAsync();

                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    LogTracer.Log(ex, $"An exception was threw in {nameof(ClearUp_Click)}");
                }
            }
        }

        private void UIMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UIModeExpander.IsExpanded = true;

                switch (UIMode.SelectedIndex)
                {
                    case 0:
                        {
                            AcrylicMode.IsChecked = false;
                            PictureMode.IsChecked = false;
                            BingPictureMode.IsChecked = false;
                            SolidColor_White.IsChecked = false;
                            SolidColor_FollowSystem.IsChecked = false;
                            SolidColor_Black.IsChecked = false;
                            PreventFallBack.IsChecked = null;

                            BackgroundController.Current.SwitchTo(BackgroundBrushType.DefaultAcrylic);

                            ApplicationData.Current.SignalDataChanged();
                            break;
                        }
                    case 1:
                        {
                            AcrylicMode.IsChecked = false;
                            PictureMode.IsChecked = false;
                            BingPictureMode.IsChecked = false;
                            PreventFallBack.IsChecked = null;

                            if (ApplicationData.Current.LocalSettings.Values["SolidColorType"] is string ColorType)
                            {
                                if (ColorType == Colors.White.ToString())
                                {
                                    SolidColor_White.IsChecked = true;
                                }
                                else
                                {
                                    SolidColor_Black.IsChecked = true;
                                }
                            }
                            else
                            {
                                SolidColor_FollowSystem.IsChecked = true;
                            }

                            break;
                        }
                    case 2:
                        {
                            SolidColor_White.IsChecked = false;
                            SolidColor_Black.IsChecked = false;
                            SolidColor_FollowSystem.IsChecked = false;

                            switch (CustomModeSubType)
                            {
                                case BackgroundBrushType.BingPicture:
                                    {
                                        BingPictureMode.IsChecked = true;
                                        break;
                                    }
                                case BackgroundBrushType.Picture:
                                    {
                                        PictureMode.IsChecked = true;
                                        break;
                                    }
                                case BackgroundBrushType.Mica:
                                    {
                                        MicaMode.IsChecked = true;
                                        break;
                                    }
                                case BackgroundBrushType.CustomAcrylic:
                                    {
                                        AcrylicMode.IsChecked = true;
                                        break;
                                    }
                            }

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw in {nameof(UIMode_SelectionChanged)}");
            }
        }

        private async void ShowUpdateLog_Click(object sender, RoutedEventArgs e)
        {
            await new WhatIsNew().ShowAsync();
        }

        private async void SystemInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (Package.Current.Id.Architecture is ProcessorArchitecture.X64 or ProcessorArchitecture.X86 or ProcessorArchitecture.X86OnArm64)
            {
                await new SystemInfoDialog().ShowAsync();
            }
            else
            {
                QueueContentDialog Dialog = new QueueContentDialog
                {
                    Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                    Content = Globalization.GetString("QueueDialog_NotSupportARM_Content"),
                    CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                };

                await Dialog.ShowAsync();
            }

        }

        private void AcrylicMode_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                AcrylicModeExpander.IsExpanded = true;
                PreventFallBack.IsChecked = IsPreventAcrylicFallbackEnabled;
                BackgroundController.Current.SwitchTo(BackgroundBrushType.CustomAcrylic);
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw in {nameof(AcrylicMode_Checked)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void PictureMode_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                PictureModeExpander.IsExpanded = true;
                PreventFallBack.IsChecked = null;

                if (ApplicationData.Current.LocalSettings.Values["PictureBackgroundUri"] is string Uri)
                {
                    if (PictureList.FirstOrDefault((Picture) => Picture.PictureUri.ToString() == Uri) is BackgroundPicture PictureItem)
                    {
                        if (await PictureItem.GetFullSizeBitmapImageAsync() is BitmapImage Bitmap)
                        {
                            BackgroundController.Current.SwitchTo(BackgroundBrushType.Picture, Bitmap, PictureItem.PictureUri);
                            PictureGirdView.SelectedItem = PictureItem;
                        }
                        else
                        {
                            LogTracer.Log($"Could not switch to \"{PictureItem.PictureUri}\"");
                        }
                    }
                    else if (PictureList.Count > 0)
                    {
                        if (await PictureList[0].GetFullSizeBitmapImageAsync() is BitmapImage Bitmap)
                        {
                            BackgroundController.Current.SwitchTo(BackgroundBrushType.Picture, Bitmap, PictureList[0].PictureUri);
                            PictureGirdView.SelectedIndex = 0;
                        }
                        else
                        {
                            LogTracer.Log($"Could not switch to \"{PictureList[0].PictureUri}\"");
                        }
                    }
                }
                else if (PictureList.Count > 0)
                {
                    if (await PictureList[0].GetFullSizeBitmapImageAsync() is BitmapImage Bitmap)
                    {
                        BackgroundController.Current.SwitchTo(BackgroundBrushType.Picture, Bitmap, PictureList[0].PictureUri);
                        PictureGirdView.SelectedIndex = 0;
                    }
                    else
                    {
                        LogTracer.Log($"Could not switch to \"{PictureList[0].PictureUri}\"");
                    }
                }
                else
                {
                    PictureGirdView.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"Error in {nameof(PictureMode_Checked)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void BingPictureMode_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                PreventFallBack.IsChecked = null;
                GetBingPhotoState.Visibility = Visibility.Visible;

                bool DetectBrightnessNeeded = await BingPictureDownloader.CheckIfNeedToUpdateAsync();

                if (await BingPictureDownloader.GetBingPictureAsync() is FileSystemStorageFile File)
                {
                    using (Stream FileStream = await File.GetStreamFromFileAsync(AccessMode.Read, OptimizeOption.RandomAccess))
                    {
                        BitmapImage Bitmap = new BitmapImage();

                        await Bitmap.SetSourceAsync(FileStream.AsRandomAccessStream());

                        BackgroundController.Current.SwitchTo(BackgroundBrushType.BingPicture, Bitmap);

                        if (DetectBrightnessNeeded)
                        {
                            BitmapDecoder Decoder = await BitmapDecoder.CreateAsync(FileStream.AsRandomAccessStream());

                            using (SoftwareBitmap SBitmap = await Decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied))
                            {
                                float Brightness = ComputerVisionProvider.DetectAvgBrightness(SBitmap);

                                if (Brightness <= 100 && ThemeColor.SelectedIndex == 1)
                                {
                                    QueueContentDialog Dialog = new QueueContentDialog
                                    {
                                        Title = Globalization.GetString("Common_Dialog_TipTitle"),
                                        Content = Globalization.GetString("QueueDialog_AutoDetectBlackColor_Content"),
                                        PrimaryButtonText = Globalization.GetString("Common_Dialog_SwitchButton"),
                                        CloseButtonText = Globalization.GetString("Common_Dialog_CancelButton")
                                    };

                                    if (await Dialog.ShowAsync() == ContentDialogResult.Primary)
                                    {
                                        ThemeColor.SelectedIndex = 0;
                                    }
                                }
                                else if (Brightness > 156 && ThemeColor.SelectedIndex == 0)
                                {
                                    QueueContentDialog Dialog = new QueueContentDialog
                                    {
                                        Title = Globalization.GetString("Common_Dialog_TipTitle"),
                                        Content = Globalization.GetString("QueueDialog_AutoDetectWhiteColor_Content"),
                                        PrimaryButtonText = Globalization.GetString("Common_Dialog_SwitchButton"),
                                        CloseButtonText = Globalization.GetString("Common_Dialog_CancelButton")
                                    };

                                    if (await Dialog.ShowAsync() == ContentDialogResult.Primary)
                                    {
                                        ThemeColor.SelectedIndex = 1;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    QueueContentDialog Dialog = new QueueContentDialog
                    {
                        Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                        Content = Globalization.GetString("QueueDialog_BingDownloadError_Content"),
                        CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                    };

                    await Dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"Error in {nameof(BingPictureMode_Checked)}");

                QueueContentDialog Dialog = new QueueContentDialog
                {
                    Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                    Content = Globalization.GetString("QueueDialog_BingDownloadError_Content"),
                    CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                };

                await Dialog.ShowAsync();
            }
            finally
            {
                GetBingPhotoState.Visibility = Visibility.Collapsed;

                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void PictureGirdView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PictureGirdView.SelectedItem is BackgroundPicture PictureItem)
            {
                try
                {
                    if (await PictureItem.GetFullSizeBitmapImageAsync() is BitmapImage Bitmap)
                    {
                        BackgroundController.Current.SwitchTo(BackgroundBrushType.Picture, Bitmap, PictureItem.PictureUri);

                        if (PictureGirdView.IsLoaded)
                        {
                            await PictureGirdView.SmoothScrollIntoViewWithIndexAsync(PictureGirdView.SelectedIndex, ScrollItemPlacement.Center);
                        }
                        else
                        {
                            PictureGirdView.ScrollIntoView(PictureItem, ScrollIntoViewAlignment.Leading);
                        }

                        if (e.RemovedItems.Count > 0)
                        {
                            StorageFile ImageFile = await StorageFile.GetFileFromApplicationUriAsync(PictureItem.PictureUri);

                            using (IRandomAccessStream Stream = await ImageFile.OpenAsync(FileAccessMode.Read))
                            {
                                BitmapDecoder Decoder = await BitmapDecoder.CreateAsync(Stream);

                                using (SoftwareBitmap SBitmap = await Decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied))
                                {
                                    float Brightness = ComputerVisionProvider.DetectAvgBrightness(SBitmap);

                                    if (Brightness <= 100 && ThemeColor.SelectedIndex == 1)
                                    {
                                        QueueContentDialog Dialog = new QueueContentDialog
                                        {
                                            Title = Globalization.GetString("Common_Dialog_TipTitle"),
                                            Content = Globalization.GetString("QueueDialog_AutoDetectBlackColor_Content"),
                                            PrimaryButtonText = Globalization.GetString("Common_Dialog_SwitchButton"),
                                            CloseButtonText = Globalization.GetString("Common_Dialog_CancelButton")
                                        };

                                        if (await Dialog.ShowAsync() == ContentDialogResult.Primary)
                                        {
                                            ThemeColor.SelectedIndex = 0;
                                        }
                                    }
                                    else if (Brightness > 156 && ThemeColor.SelectedIndex == 0)
                                    {
                                        QueueContentDialog Dialog = new QueueContentDialog
                                        {
                                            Title = Globalization.GetString("Common_Dialog_TipTitle"),
                                            Content = Globalization.GetString("QueueDialog_AutoDetectWhiteColor_Content"),
                                            PrimaryButtonText = Globalization.GetString("Common_Dialog_SwitchButton"),
                                            CloseButtonText = Globalization.GetString("Common_Dialog_CancelButton")
                                        };

                                        if (await Dialog.ShowAsync() == ContentDialogResult.Primary)
                                        {
                                            ThemeColor.SelectedIndex = 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        LogTracer.Log($"Could not switch to \"{PictureItem.PictureUri}\"");
                    }
                }
                catch (Exception ex)
                {
                    LogTracer.Log(ex, $"An exception was threw in {nameof(PictureGirdView_SelectionChanged)}");
                }
                finally
                {
                    ApplicationData.Current.SignalDataChanged();
                }
            }
            else
            {
                BackgroundController.Current.SwitchTo(BackgroundBrushType.Picture, new BitmapImage());
            }
        }

        private async void AddImageToPictureButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker Picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                ViewMode = PickerViewMode.Thumbnail
            };
            Picker.FileTypeFilter.Add(".png");
            Picker.FileTypeFilter.Add(".jpg");
            Picker.FileTypeFilter.Add(".jpeg");
            Picker.FileTypeFilter.Add(".jpeg");
            Picker.FileTypeFilter.Add(".bmp");

            if (await Picker.PickSingleFileAsync() is StorageFile File)
            {
                StorageFolder ImageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("CustomImageFolder", CreationCollisionOption.OpenIfExists);
                StorageFile CopyedFile = await File.CopyAsync(ImageFolder, $"BackgroundPicture_{Guid.NewGuid():N}{File.FileType}", NameCollisionOption.GenerateUniqueName);

                BackgroundPicture Picture = await BackgroundPicture.CreateAsync(new Uri($"ms-appdata:///local/CustomImageFolder/{CopyedFile.Name}"));

                PictureList.Add(Picture);
                PictureGirdView.UpdateLayout();
                PictureGirdView.SelectedItem = Picture;

                SQLite.Current.SetBackgroundPicture(Picture.PictureUri);
            }
        }

        private async void DeletePictureButton_Click(object sender, RoutedEventArgs e)
        {
            if (PictureGirdView.SelectedItem is BackgroundPicture Picture)
            {
                try
                {
                    if (!Picture.PictureUri.ToString().StartsWith("ms-appx://"))
                    {
                        StorageFile ImageFile = await StorageFile.GetFileFromApplicationUriAsync(Picture.PictureUri);
                        await ImageFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    }

                    SQLite.Current.DeleteBackgroundPicture(Picture.PictureUri);

                    PictureList.Remove(Picture);
                    PictureGirdView.UpdateLayout();
                    PictureGirdView.SelectedIndex = PictureList.Count - 1;
                }
                catch (Exception ex)
                {
                    LogTracer.Log(ex, $"An exception was threw in {nameof(DeletePictureButton_Click)}");
                }
            }
        }

        private void PictureGirdView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if ((e.OriginalSource as FrameworkElement)?.DataContext is BackgroundPicture Picture)
            {
                PictureGirdView.SelectedItem = Picture;
                PictureGirdView.ContextFlyout = PictureFlyout;
            }
            else
            {
                PictureGirdView.ContextFlyout = null;
            }
        }

        private async void AutoBoot_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                StartupTask BootTask = await StartupTask.GetAsync("RXExplorer");

                if (AutoBoot.IsOn)
                {
                    switch (await BootTask.RequestEnableAsync())
                    {
                        case StartupTaskState.Disabled:
                        case StartupTaskState.DisabledByPolicy:
                        case StartupTaskState.DisabledByUser:
                            {
                                AutoBoot.Toggled -= AutoBoot_Toggled;
                                AutoBoot.IsOn = false;
                                AutoBoot.Toggled += AutoBoot_Toggled;

                                QueueContentDialog Dialog = new QueueContentDialog
                                {
                                    Title = Globalization.GetString("Common_Dialog_TipTitle"),
                                    Content = Globalization.GetString("QueueDialog_BootAtStart_Content"),
                                    PrimaryButtonText = Globalization.GetString("Common_Dialog_NowButton"),
                                    CloseButtonText = Globalization.GetString("Common_Dialog_LaterButton")
                                };

                                if ((await Dialog.ShowAsync()) == ContentDialogResult.Primary)
                                {
                                    await Launcher.LaunchUriAsync(new Uri("ms-settings:appsfeatures-app"));
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                else
                {
                    BootTask.Disable();
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"Error in {nameof(AutoBoot_Toggled)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void SolidColor_FollowSystem_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                SolidColor_Black.IsChecked = false;
                SolidColor_White.IsChecked = false;

                BackgroundController.Current.SwitchTo(BackgroundBrushType.SolidColor);
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"Error in {nameof(SolidColor_FollowSystem_Checked)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void SolidColor_White_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                SolidColor_Black.IsChecked = false;
                SolidColor_FollowSystem.IsChecked = false;

                BackgroundController.Current.SwitchTo(BackgroundBrushType.SolidColor, Color: BackgroundController.SolidColor_WhiteTheme);
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"Error in {nameof(SolidColor_White_Checked)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void SolidColor_Black_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                SolidColor_White.IsChecked = false;
                SolidColor_FollowSystem.IsChecked = false;

                BackgroundController.Current.SwitchTo(BackgroundBrushType.SolidColor, Color: BackgroundController.SolidColor_BlackTheme);
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"Error in {nameof(SolidColor_Black_Checked)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void TreeViewDetach_Toggled(object sender, RoutedEventArgs e)
        {
            IsDetachTreeViewAndPresenter = !TreeViewDetach.IsOn;

            try
            {
                await Task.WhenAll(TabViewContainer.Current.TabCollection.Select((Tab) => Tab.Content)
                                                                         .Cast<Frame>()
                                                                         .Select((Frame) => Frame.Content)
                                                                         .Cast<TabItemContentRenderer>()
                                                                         .Select((Renderer) => Renderer.SetTreeViewStatusAsync(TreeViewDetach.IsOn)));
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"Error in {nameof(TreeViewDetach_Toggled)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void EnableQuicklook_Toggled(object sender, RoutedEventArgs e)
        {
            IsQuicklookEnabled = EnableQuicklook.IsOn;
            ApplicationData.Current.SignalDataChanged();
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            try
            {
                bool ShouldDisplayTips = LanguageComboBox.SelectedIndex switch
                {
                    0 => Globalization.SwitchTo(LanguageEnum.Chinese_Simplified),
                    1 => Globalization.SwitchTo(LanguageEnum.English),
                    2 => Globalization.SwitchTo(LanguageEnum.French),
                    3 => Globalization.SwitchTo(LanguageEnum.Chinese_Traditional),
                    4 => Globalization.SwitchTo(LanguageEnum.Spanish),
                    5 => Globalization.SwitchTo(LanguageEnum.German),
                    _ => throw new NotSupportedException()
                };

                if (ShouldDisplayTips)
                {
                    if (!InfoTipController.Current.CheckIfAlreadyOpened(InfoTipType.LanguageRestartRequired))
                    {
                        InfoTipController.Current.Show(InfoTipType.LanguageRestartRequired);
                    }
                }
                else
                {
                    InfoTipController.Current.Hide(InfoTipType.LanguageRestartRequired);
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw in {nameof(LanguageComboBox_SelectionChanged)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void DisplayHiddenItem_Toggled(object sender, RoutedEventArgs e)
        {
            IsShowHiddenFilesEnabled = DisplayHiddenItem.IsOn;

            try
            {
                if (!IsDetachTreeViewAndPresenter)
                {
                    await Task.WhenAll(TabViewContainer.Current.TabCollection.Select((Tab) => Tab.Content)
                                                                             .Cast<Frame>()
                                                                             .Select((Frame) => Frame.Content)
                                                                             .Cast<TabItemContentRenderer>()
                                                                             .Select((Renderer) => Renderer.RefreshTreeViewAsync()));
                }

                await Task.WhenAll(TabViewContainer.Current.TabCollection.Select((Tab) => Tab.Content)
                                                                         .Cast<Frame>()
                                                                         .Select((Frame) => Frame.Content)
                                                                         .Cast<TabItemContentRenderer>()
                                                                         .Select((Renderer) => Renderer.RefreshPresentersAsync()));
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"Error in {nameof(DisplayHiddenItem_Toggled)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void FolderOpenMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (FolderOpenMethod.SelectedIndex)
            {
                case 0:
                    {
                        IsDoubleClickEnabled = false;
                        break;
                    }
                case 1:
                    {
                        IsDoubleClickEnabled = true;
                        break;
                    }
            }

            ApplicationData.Current.SignalDataChanged();
        }

        private async void UseWinAndEActivate_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingControl.IsLoading = true;
                LoadingText.Text = Globalization.GetString("Progress_Tip_WaitingForAction");

                if (UseWinAndEActivate.IsOn)
                {
                    ModifySystemWarningDialog Dialog = new ModifySystemWarningDialog();

                    if (await Dialog.ShowAsync() == ContentDialogResult.Primary)
                    {
                        using (FullTrustProcessController.ExclusiveUsage Exclusive = await FullTrustProcessController.GetAvailableControllerAsync())
                        {
                            if (await Exclusive.Controller.InterceptWindowsPlusEAsync())
                            {
                                ApplicationData.Current.LocalSettings.Values["InterceptWindowsE"] = true;
                            }
                            else
                            {
                                QueueContentDialog dialog = new QueueContentDialog
                                {
                                    Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                    Content = Globalization.GetString("QueueDialog_ActionFailed_Content"),
                                    CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                                };

                                await dialog.ShowAsync();

                                UseWinAndEActivate.Toggled -= UseWinAndEActivate_Toggled;
                                UseWinAndEActivate.IsOn = false;
                                UseWinAndEActivate.Toggled += UseWinAndEActivate_Toggled;
                            }
                        }
                    }
                    else
                    {
                        UseWinAndEActivate.Toggled -= UseWinAndEActivate_Toggled;
                        UseWinAndEActivate.IsOn = false;
                        UseWinAndEActivate.Toggled += UseWinAndEActivate_Toggled;
                    }
                }
                else
                {
                    using (FullTrustProcessController.ExclusiveUsage Exclusive = await FullTrustProcessController.GetAvailableControllerAsync())
                    {
                        if (await Exclusive.Controller.RestoreWindowsPlusEInterceptionAsync())
                        {
                            ApplicationData.Current.LocalSettings.Values["InterceptWindowsE"] = false;
                        }
                        else
                        {
                            QueueContentDialog dialog = new QueueContentDialog
                            {
                                Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                Content = Globalization.GetString("QueueDialog_ActionFailed_Content"),
                                CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                            };

                            await dialog.ShowAsync();

                            UseWinAndEActivate.Toggled -= UseWinAndEActivate_Toggled;
                            UseWinAndEActivate.IsOn = true;
                            UseWinAndEActivate.Toggled += UseWinAndEActivate_Toggled;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, "Error happend when Enable/Disable Win+E");
            }
            finally
            {
                await Task.Delay(1000);
                LoadingControl.IsLoading = false;
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void AlwaysLaunchNew_Checked(object sender, RoutedEventArgs e)
        {
            IsAlwaysLaunchNewProcess = true;
            ApplicationData.Current.SignalDataChanged();
        }

        private void AlwaysLaunchNew_Unchecked(object sender, RoutedEventArgs e)
        {
            IsAlwaysLaunchNewProcess = false;
            ApplicationData.Current.SignalDataChanged();
        }

        private async void ExportLog_Click(object sender, RoutedEventArgs e)
        {
            if (await LogTracer.CheckHasAnyLogAvailableAsync())
            {
                FileSavePicker Picker = new FileSavePicker
                {
                    SuggestedFileName = "Export_All_Error_Log.txt",
                    SuggestedStartLocation = PickerLocationId.Desktop
                };
                Picker.FileTypeChoices.Add(Globalization.GetString("File_Type_TXT_Description"), new List<string> { ".txt" });

                if (await Picker.PickSaveFileAsync() is StorageFile PickedFile)
                {
                    await LogTracer.ExportAllLogAsync(PickedFile).ConfigureAwait(false);
                }
            }
            else
            {
                QueueContentDialog Dialog = new QueueContentDialog
                {
                    Title = Globalization.GetString("Common_Dialog_TipTitle"),
                    Content = Globalization.GetString("QueueDialog_NoLogTip_Content"),
                    CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                };

                await Dialog.ShowAsync();
            }
        }

        private void PreventFallBack_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                IsPreventAcrylicFallbackEnabled = true;
                BackgroundController.Current.IsCompositionAcrylicBackgroundEnabled = true;
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw when checking {PreventFallBack}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void PreventFallBack_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                IsPreventAcrylicFallbackEnabled = false;
                BackgroundController.Current.IsCompositionAcrylicBackgroundEnabled = false;
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw when unchecking {PreventFallBack}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void ImportConfiguration_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker Picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.Desktop
            };

            Picker.FileTypeFilter.Add(".json");

            if (await Picker.PickSingleFileAsync() is StorageFile ImportFile)
            {
                try
                {
                    string JsonContent = await FileIO.ReadTextAsync(ImportFile, UnicodeEncoding.Utf16LE);

                    if (JsonSerializer.Deserialize<Dictionary<string, string>>(JsonContent) is Dictionary<string, string> Dic)
                    {
                        if (Dic.TryGetValue("Identitifier", out string Id)
                            && Id == "RX_Explorer_Export_Configuration"
                            && Dic.TryGetValue("HardwareUUID", out string HardwareId)
                            && Dic.TryGetValue("Configuration", out string Configuration)
                            && Dic.TryGetValue("ConfigHash", out string ConfigHash)
                            && Dic.TryGetValue("Database", out string Database)
                            && Dic.TryGetValue("DatabaseHash", out string DatabaseHash))
                        {
                            if (HardwareId == new EasClientDeviceInformation().Id.ToString("D"))
                            {
                                using (MD5 MD5Alg = MD5.Create())
                                {
                                    string ConfigDecryptedString = Configuration.Decrypt(Package.Current.Id.FamilyName);

                                    if (MD5Alg.GetHash(ConfigDecryptedString).Equals(ConfigHash, StringComparison.OrdinalIgnoreCase))
                                    {
                                        Dictionary<string, JsonElement> ConfigDic = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(ConfigDecryptedString);

                                        ApplicationData.Current.LocalSettings.Values.Clear();

                                        foreach (KeyValuePair<string, JsonElement> Pair in ConfigDic)
                                        {
                                            if (Pair.Key != "LicenseGrant")
                                            {
                                                switch (Pair.Value.ValueKind)
                                                {
                                                    case JsonValueKind.Number:
                                                        {
                                                            if (Pair.Value.TryGetInt32(out int INT32))
                                                            {
                                                                ApplicationData.Current.LocalSettings.Values.Add(Pair.Key, INT32);
                                                            }
                                                            else if (Pair.Value.TryGetInt64(out long INT64))
                                                            {
                                                                ApplicationData.Current.LocalSettings.Values.Add(Pair.Key, INT64);
                                                            }
                                                            else if (Pair.Value.TryGetSingle(out float FL32))
                                                            {
                                                                ApplicationData.Current.LocalSettings.Values.Add(Pair.Key, FL32);
                                                            }
                                                            else if (Pair.Value.TryGetDouble(out double FL64))
                                                            {
                                                                ApplicationData.Current.LocalSettings.Values.Add(Pair.Key, FL64);
                                                            }

                                                            break;
                                                        }
                                                    case JsonValueKind.String:
                                                        {
                                                            ApplicationData.Current.LocalSettings.Values.Add(Pair.Key, Pair.Value.GetString());
                                                            break;
                                                        }
                                                    case JsonValueKind.True:
                                                    case JsonValueKind.False:
                                                        {
                                                            ApplicationData.Current.LocalSettings.Values.Add(Pair.Key, Pair.Value.GetBoolean());
                                                            break;
                                                        }
                                                }
                                            }
                                        }

                                        string DatabaseDecryptedString = Database.Decrypt(Package.Current.Id.FamilyName);

                                        if (MD5Alg.GetHash(DatabaseDecryptedString).Equals(DatabaseHash, StringComparison.OrdinalIgnoreCase))
                                        {
                                            Dictionary<string, string> DatabaseDic = JsonSerializer.Deserialize<Dictionary<string, string>>(DatabaseDecryptedString);
                                            List<(string TableName, IEnumerable<object[]> Data)> DatabaseFormattedArray = new List<(string TableName, IEnumerable<object[]> Data)>(DatabaseDic.Count);

                                            foreach (KeyValuePair<string, string> TableDic in DatabaseDic)
                                            {
                                                if (JsonSerializer.Deserialize<IReadOnlyList<JsonElement[]>>(TableDic.Value) is IReadOnlyList<JsonElement[]> RowData)
                                                {
                                                    List<object[]> RowFormattedArray = new List<object[]>(RowData.Count);

                                                    foreach (JsonElement[] Data in RowData)
                                                    {
                                                        object[] ColumnFormattedArray = new object[Data.Length];

                                                        for (int Index = 0; Index < Data.Length; Index++)
                                                        {
                                                            JsonElement InnerElement = Data[Index];

                                                            switch (InnerElement.ValueKind)
                                                            {
                                                                case JsonValueKind.Number:
                                                                    {
                                                                        if (InnerElement.TryGetInt32(out int INT32))
                                                                        {
                                                                            ColumnFormattedArray[Index] = INT32;
                                                                        }
                                                                        else if (InnerElement.TryGetInt64(out long INT64))
                                                                        {
                                                                            ColumnFormattedArray[Index] = INT64;
                                                                        }
                                                                        else if (InnerElement.TryGetSingle(out float FL32))
                                                                        {
                                                                            ColumnFormattedArray[Index] = FL32;
                                                                        }
                                                                        else if (InnerElement.TryGetDouble(out double FL64))
                                                                        {
                                                                            ColumnFormattedArray[Index] = FL64;
                                                                        }

                                                                        break;
                                                                    }
                                                                case JsonValueKind.String:
                                                                    {
                                                                        ColumnFormattedArray[Index] = InnerElement.GetString();
                                                                        break;
                                                                    }
                                                                case JsonValueKind.True:
                                                                case JsonValueKind.False:
                                                                    {
                                                                        ColumnFormattedArray[Index] = InnerElement.GetBoolean();
                                                                        break;
                                                                    }
                                                            }
                                                        }

                                                        RowFormattedArray.Add(ColumnFormattedArray);
                                                    }

                                                    DatabaseFormattedArray.Add((TableDic.Key, RowFormattedArray));
                                                }
                                            }

                                            SQLite.Current.ImportData(DatabaseFormattedArray);

                                            if (Dic.TryGetValue("CustomImageDataPackageArray", out string CustomImageData)
                                                && Dic.TryGetValue("CustomImageDataPackageArrayHash", out string CustomImageDataHash))
                                            {
                                                string CustomImageDataDecryptedString = CustomImageData.Decrypt(Package.Current.Id.FamilyName);

                                                if (MD5Alg.GetHash(CustomImageDataDecryptedString).Equals(CustomImageDataHash, StringComparison.OrdinalIgnoreCase))
                                                {
                                                    StorageFolder CustomImageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("CustomImageFolder", CreationCollisionOption.OpenIfExists);

                                                    foreach (PortableImageDataPackage DataPackage in JsonSerializer.Deserialize<List<PortableImageDataPackage>>(CustomImageDataDecryptedString))
                                                    {
                                                        StorageFile NewImageFile = await CustomImageFolder.CreateFileAsync(DataPackage.Name, CreationCollisionOption.ReplaceExisting);

                                                        using (Stream ImageFileStream = await NewImageFile.OpenStreamForWriteAsync())
                                                        {
                                                            await ImageFileStream.WriteAsync(DataPackage.Data, 0, DataPackage.Data.Length);
                                                            await ImageFileStream.FlushAsync();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    LogTracer.Log("Import custom image data failed because database hash is incorrect");
                                                }
                                            }

                                            ApplicationData.Current.SignalDataChanged();

                                            await CommonAccessCollection.LoadLibraryFoldersAsync(true);

                                            await new QueueContentDialog
                                            {
                                                Title = Globalization.GetString("Common_Dialog_TipTitle"),
                                                Content = Globalization.GetString("QueueDialog_ImportConfigurationSuccess_Content"),
                                                CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                                            }.ShowAsync();

                                            InfoTipController.Current.Show(InfoTipType.ConfigRestartRequired);
                                        }
                                        else
                                        {
                                            LogTracer.Log("Import configuration failed because database hash is incorrect");

                                            await new QueueContentDialog
                                            {
                                                Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                                Content = Globalization.GetString("QueueDialog_ImportConfigurationDataIncorrect_Content"),
                                                CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                                            }.ShowAsync();
                                        }
                                    }
                                    else
                                    {
                                        LogTracer.Log("Import configuration failed because config hash is incorrect");

                                        await new QueueContentDialog
                                        {
                                            Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                            Content = Globalization.GetString("QueueDialog_ImportConfigurationDataIncorrect_Content"),
                                            CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                                        }.ShowAsync();
                                    }
                                }
                            }
                            else
                            {
                                LogTracer.Log("Import configuration failed because hardware id is not match");

                                await new QueueContentDialog
                                {
                                    Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                    Content = Globalization.GetString("QueueDialog_ImportConfigurationDataIncorrect_Content"),
                                    CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                                }.ShowAsync();
                            }
                        }
                        else
                        {
                            LogTracer.Log("Import configuration failed because format is incorrect");

                            await new QueueContentDialog
                            {
                                Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                Content = Globalization.GetString("QueueDialog_ImportConfigurationDataIncorrect_Content"),
                                CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                            }.ShowAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogTracer.Log(ex, "Import configuration function threw an exception");

                    QueueContentDialog Dialog = new QueueContentDialog
                    {
                        Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                        Content = Globalization.GetString("QueueDialog_ImportConfigurationFailed_Content"),
                        CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                    };

                    await Dialog.ShowAsync();
                }
            }
        }

        private async void ExportConfiguration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileSavePicker Picker = new FileSavePicker
                {
                    SuggestedStartLocation = PickerLocationId.Desktop,
                    SuggestedFileName = "RX_Configuration"
                };

                Picker.FileTypeChoices.Add("JSON", new List<string> { ".json" });

                if (await Picker.PickSaveFileAsync() is StorageFile SaveFile)
                {
                    Dictionary<string, string> DataBaseDic = new Dictionary<string, string>();

                    foreach ((string TableName, IReadOnlyList<object[]> Data) in SQLite.Current.ExportData())
                    {
                        DataBaseDic.Add(TableName, JsonSerializer.Serialize(Data));
                    }

                    List<PortableImageDataPackage> CustomImageDataPackageList = new List<PortableImageDataPackage>();

                    if (await ApplicationData.Current.LocalFolder.TryGetItemAsync("CustomImageFolder") is StorageFolder ImageFolder)
                    {
                        foreach (StorageFile ImageFile in await ImageFolder.GetFilesAsync())
                        {
                            using (Stream ImageStream = await ImageFile.OpenStreamForReadAsync())
                            using (BinaryReader Reader = new BinaryReader(ImageStream))
                            {
                                CustomImageDataPackageList.Add(new PortableImageDataPackage
                                {
                                    Name = ImageFile.Name,
                                    Data = Reader.ReadBytes((int)ImageStream.Length)
                                });
                            }
                        }
                    }

                    Dictionary<string, object> ConfigDic = new Dictionary<string, object>(ApplicationData.Current.LocalSettings.Values.ToArray());
                    ConfigDic.Remove("LicenseGrant");

                    string DatabaseString = JsonSerializer.Serialize(DataBaseDic);
                    string ConfigurationString = JsonSerializer.Serialize(ConfigDic);
                    string CustomImageString = JsonSerializer.Serialize(CustomImageDataPackageList);

                    using (MD5 MD5Alg = MD5.Create())
                    {
                        Dictionary<string, string> BaseDic = new Dictionary<string, string>
                        {
                            { "Identitifier", "RX_Explorer_Export_Configuration" },
                            { "HardwareUUID", new EasClientDeviceInformation().Id.ToString("D") },
                            { "Configuration",  ConfigurationString.Encrypt(Package.Current.Id.FamilyName) },
                            { "ConfigHash", MD5Alg.GetHash(ConfigurationString) },
                            { "Database", DatabaseString.Encrypt(Package.Current.Id.FamilyName) },
                            { "DatabaseHash", MD5Alg.GetHash(DatabaseString) },
                            { "CustomImageDataPackageArray", CustomImageString.Encrypt(Package.Current.Id.FamilyName) },
                            { "CustomImageDataPackageArrayHash", MD5Alg.GetHash(CustomImageString) }
                        };

                        await FileIO.WriteTextAsync(SaveFile, JsonSerializer.Serialize(BaseDic), UnicodeEncoding.Utf16LE);
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex);
            }
        }

        private void ContextMenuExtSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            IsContextMenuExtensionEnabled = ContextMenuExtSwitch.IsOn;
            ApplicationData.Current.SignalDataChanged();
        }

        private void SearchEngineConfig_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ApplicationData.Current.LocalSettings.Values["SearchEngineFlyoutMode"] = SearchEngineConfig.SelectedIndex;
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An exception was threw in {nameof(SearchEngineConfig_SelectionChanged)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void AddSpecificTab_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker Picker = new FolderPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.ComputerFolder
            };

            Picker.FileTypeFilter.Add("*");

            if (await Picker.PickSingleFolderAsync() is StorageFolder Folder)
            {
                SpecificTabListView.Items.Add(Folder.Path);
                StartupModeController.SetSpecificPath(SpecificTabListView.Items.Cast<string>());
            }
        }

        private void DeleteSpecificTabButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is string Path)
            {
                SpecificTabListView.Items.Remove(Path);
                StartupModeController.SetSpecificPath(SpecificTabListView.Items.Cast<string>());
            }
        }

        private void StartupWithNewTab_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                StartupModeController.Mode = StartupMode.CreateNewTab;
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, "Could not set StartupMode");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void StartupWithLastTab_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                StartupModeController.Mode = StartupMode.LastOpenedTab;
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, "Could not set StartupMode");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void StartupSpecificTab_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                StartupModeController.Mode = StartupMode.SpecificTab;
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, "Could not set StartupMode");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void DeleteConfirmSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                IsDoubleConfirmOnDeletionEnabled = DeleteConfirmSwitch.IsOn;
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, "Could not set delete confirm");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void AvoidRecycleBin_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                IsAvoidRecycleBinEnabled = true;
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, "Could not avoid recycle bin");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void AvoidRecycleBin_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                IsAvoidRecycleBinEnabled = false;
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, "Could not avoid recycle bin");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void HideProtectedSystemItems_Unchecked(object sender, RoutedEventArgs e)
        {
            QueueContentDialog Dialog = new QueueContentDialog
            {
                Title = Globalization.GetString("Common_Dialog_WarningTitle"),
                Content = Globalization.GetString("QueueDialog_DisplayProtectedSystemItemsWarning_Content"),
                PrimaryButtonText = Globalization.GetString("Common_Dialog_ContinueButton"),
                CloseButtonText = Globalization.GetString("Common_Dialog_CancelButton")
            };

            if (await Dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                IsDisplayProtectedSystemItems = true;

                try
                {
                    if (!IsDetachTreeViewAndPresenter)
                    {
                        await Task.WhenAll(TabViewContainer.Current.TabCollection.Select((Tab) => Tab.Content)
                                                                                 .Cast<Frame>()
                                                                                 .Select((Frame) => Frame.Content)
                                                                                 .Cast<TabItemContentRenderer>()
                                                                                 .Select((Renderer) => Renderer.RefreshTreeViewAsync()));
                    }

                    await Task.WhenAll(TabViewContainer.Current.TabCollection.Select((Tab) => Tab.Content)
                                                                             .Cast<Frame>()
                                                                             .Select((Frame) => Frame.Content)
                                                                             .Cast<TabItemContentRenderer>()
                                                                             .Select((Renderer) => Renderer.RefreshPresentersAsync()));
                }
                catch (Exception ex)
                {
                    LogTracer.Log(ex, $"Error in {nameof(HideProtectedSystemItems_Unchecked)}");
                }
                finally
                {
                    ApplicationData.Current.SignalDataChanged();
                }
            }
            else
            {
                HideProtectedSystemItems.Checked -= HideProtectedSystemItems_Checked;
                HideProtectedSystemItems.IsChecked = true;
                HideProtectedSystemItems.Checked += HideProtectedSystemItems_Checked;
            }
        }

        private async void HideProtectedSystemItems_Checked(object sender, RoutedEventArgs e)
        {
            IsDisplayProtectedSystemItems = false;

            try
            {
                if (!IsDetachTreeViewAndPresenter)
                {
                    await Task.WhenAll(TabViewContainer.Current.TabCollection.Select((Tab) => Tab.Content)
                                                                             .Cast<Frame>()
                                                                             .Select((Frame) => Frame.Content)
                                                                             .Cast<TabItemContentRenderer>()
                                                                             .Select((Renderer) => Renderer.RefreshTreeViewAsync()));
                }

                await Task.WhenAll(TabViewContainer.Current.TabCollection.Select((Tab) => Tab.Content)
                                                                         .Cast<Frame>()
                                                                         .Select((Frame) => Frame.Content)
                                                                         .Cast<TabItemContentRenderer>()
                                                                         .Select((Renderer) => Renderer.RefreshPresentersAsync()));
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"Error in {nameof(HideProtectedSystemItems_Checked)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private async void WinEManualGuidence_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/zhuxb711/RX-Explorer/issues/27#issuecomment-692418152"));
        }

        private async void WinEExportRestoreFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileSavePicker Picker = new FileSavePicker
                {
                    SuggestedStartLocation = PickerLocationId.Desktop,
                    SuggestedFileName = "Restore_Win_E.reg"
                };

                Picker.FileTypeChoices.Add("REG", new string[] { ".reg" });

                if (await Picker.PickSaveFileAsync() is StorageFile ExportFile)
                {
                    StorageFile File = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Restore_WIN_E.reg"));
                    await File.CopyAndReplaceAsync(ExportFile);
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, "Could not export restore file");
            }
        }

        private async void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            await SystemInformation.LaunchStoreForReviewAsync();
        }

        private async void ShortcutGuide_Click(object sender, RoutedEventArgs e)
        {
            StorageFile File = Globalization.CurrentLanguage switch
            {
                LanguageEnum.Chinese_Simplified => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/KeyboardShortcut_cn_s.txt")),
                LanguageEnum.English => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/KeyboardShortcut_en.txt")),
                LanguageEnum.French => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/KeyboardShortcut_fr.txt")),
                LanguageEnum.Chinese_Traditional => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/KeyboardShortcut_cn_t.txt")),
                LanguageEnum.Spanish => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/KeyboardShortcut_es.txt")),
                LanguageEnum.German => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/KeyboardShortcut_de.txt")),
                _ => throw new Exception("Unsupported language")
            };

            await new KeyboardShortcutGuideDialog(await FileIO.ReadTextAsync(File)).ShowAsync();
        }

        private async void PurchaseApp_Click(object sender, RoutedEventArgs e)
        {
            switch (await MSStoreHelper.Current.PurchaseAsync())
            {
                case StorePurchaseStatus.Succeeded:
                    {
                        QueueContentDialog QueueContenDialog = new QueueContentDialog
                        {
                            Title = Globalization.GetString("Common_Dialog_TipTitle"),
                            Content = Globalization.GetString("QueueDialog_Store_PurchaseSuccess_Content"),
                            CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                        };
                        await QueueContenDialog.ShowAsync();
                        break;
                    }
                case StorePurchaseStatus.AlreadyPurchased:
                    {
                        QueueContentDialog QueueContenDialog = new QueueContentDialog
                        {
                            Title = Globalization.GetString("Common_Dialog_TipTitle"),
                            Content = Globalization.GetString("QueueDialog_Store_AlreadyPurchase_Content"),
                            CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                        };
                        await QueueContenDialog.ShowAsync();
                        break;
                    }
                case StorePurchaseStatus.NotPurchased:
                    {
                        QueueContentDialog QueueContenDialog = new QueueContentDialog
                        {
                            Title = Globalization.GetString("Common_Dialog_TipTitle"),
                            Content = Globalization.GetString("QueueDialog_Store_NotPurchase_Content"),
                            CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                        };
                        await QueueContenDialog.ShowAsync();
                        break;
                    }
                default:
                    {
                        QueueContentDialog QueueContenDialog = new QueueContentDialog
                        {
                            Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                            Content = Globalization.GetString("QueueDialog_Store_NetworkError_Content"),
                            CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                        };
                        await QueueContenDialog.ShowAsync();
                        break;
                    }
            }
        }

        private async void NavigateGithub_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/zhuxb711/RX-Explorer"));
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            if (await MSStoreHelper.Current.CheckHasUpdateAsync())
            {
                await Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?productid=9N88QBQKF2RS"));
            }
        }

        private async void CopyQQ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataPackage Package = new DataPackage();
                Package.SetText("937294538");
                Clipboard.SetContent(Package);
            }
            catch
            {
                QueueContentDialog Dialog = new QueueContentDialog
                {
                    Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                    Content = Globalization.GetString("QueueDialog_UnableAccessClipboard_Content"),
                    CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                };

                await Dialog.ShowAsync();
            }
        }

        private async void AQSGuide_Click(object sender, RoutedEventArgs e)
        {
            StorageFile File = Globalization.CurrentLanguage switch
            {
                LanguageEnum.Chinese_Simplified => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/AQSGuide_cn_s.txt")),
                LanguageEnum.English => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/AQSGuide_en.txt")),
                LanguageEnum.French => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/AQSGuide_fr.txt")),
                LanguageEnum.Chinese_Traditional => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/AQSGuide_cn_t.txt")),
                LanguageEnum.Spanish => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/AQSGuide_es.txt")),
                LanguageEnum.German => await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/AQSGuide_de.txt")),
                _ => throw new Exception("Unsupported language")
            };

            await new AQSGuide(await FileIO.ReadTextAsync(File)).ShowAsync();
        }


        private void SeachEngineOptionSave_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox Box)
            {
                switch (Box.Name)
                {
                    case "EverythingEngineIgnoreCase":
                        {
                            ApplicationData.Current.LocalSettings.Values["EverythingEngineIgnoreCase"] = true;
                            break;
                        }
                    case "EverythingEngineIncludeRegex":
                        {
                            ApplicationData.Current.LocalSettings.Values["EverythingEngineIncludeRegex"] = true;
                            break;
                        }
                    case "EverythingEngineSearchGloble":
                        {
                            ApplicationData.Current.LocalSettings.Values["EverythingEngineSearchGloble"] = true;
                            break;
                        }
                    case "BuiltInEngineIgnoreCase":
                        {
                            ApplicationData.Current.LocalSettings.Values["BuiltInEngineIgnoreCase"] = true;
                            break;
                        }
                    case "BuiltInEngineIncludeRegex":
                        {
                            ApplicationData.Current.LocalSettings.Values["BuiltInEngineIncludeRegex"] = true;
                            break;
                        }
                    case "BuiltInSearchAllSubFolders":
                        {
                            ApplicationData.Current.LocalSettings.Values["BuiltInSearchAllSubFolders"] = true;
                            break;
                        }
                    case "BuiltInEngineIncludeAQS":
                        {
                            ApplicationData.Current.LocalSettings.Values["BuiltInEngineIncludeAQS"] = true;
                            break;
                        }
                    case "BuiltInSearchUseIndexer":
                        {
                            ApplicationData.Current.LocalSettings.Values["BuiltInSearchUseIndexer"] = true;
                            break;
                        }
                }

                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void SeachEngineOptionSave_UnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox Box)
            {
                switch (Box.Name)
                {
                    case "EverythingEngineIgnoreCase":
                        {
                            ApplicationData.Current.LocalSettings.Values["EverythingEngineIgnoreCase"] = false;
                            break;
                        }
                    case "EverythingEngineIncludeRegex":
                        {
                            ApplicationData.Current.LocalSettings.Values["EverythingEngineIncludeRegex"] = false;
                            break;
                        }
                    case "EverythingEngineSearchGloble":
                        {
                            ApplicationData.Current.LocalSettings.Values["EverythingEngineSearchGloble"] = false;
                            break;
                        }
                    case "BuiltInEngineIgnoreCase":
                        {
                            ApplicationData.Current.LocalSettings.Values["BuiltInEngineIgnoreCase"] = false;
                            break;
                        }
                    case "BuiltInEngineIncludeRegex":
                        {
                            ApplicationData.Current.LocalSettings.Values["BuiltInEngineIncludeRegex"] = false;
                            break;
                        }
                    case "BuiltInSearchAllSubFolders":
                        {
                            ApplicationData.Current.LocalSettings.Values["BuiltInSearchAllSubFolders"] = false;
                            break;
                        }
                    case "BuiltInEngineIncludeAQS":
                        {
                            ApplicationData.Current.LocalSettings.Values["BuiltInEngineIncludeAQS"] = false;
                            break;
                        }
                    case "BuiltInSearchUseIndexer":
                        {
                            ApplicationData.Current.LocalSettings.Values["BuiltInSearchUseIndexer"] = false;
                            break;
                        }
                }

                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void TabPreviewSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            IsTabPreviewEnabled = TabPreviewSwitch.IsOn;
            ApplicationData.Current.SignalDataChanged();
        }

        private void PathHistory_Toggled(object sender, RoutedEventArgs e)
        {
            IsPathHistoryEnabled = PathHistory.IsOn;
            ApplicationData.Current.SignalDataChanged();
        }

        private void SearchHistory_Toggled(object sender, RoutedEventArgs e)
        {
            IsSearchHistoryEnabled = SearchHistory.IsOn;
            ApplicationData.Current.SignalDataChanged();
        }

        private async void DisableSelectionAnimation_Changed(object sender, RoutedEventArgs e)
        {
            List<Task> ParallelTask = new List<Task>
            {
                CommonAccessCollection.LoadDriveAsync(true),
                CommonAccessCollection.LoadLibraryFoldersAsync(true)
            };

            await Task.WhenAll(ParallelTask.Concat(TabViewContainer.Current.TabCollection.Select((Tab) => Tab.Content)
                                                                                         .Cast<Frame>()
                                                                                         .Select((Frame) => Frame.Content)
                                                                                         .Cast<TabItemContentRenderer>()
                                                                                         .Select((Renderer) => Renderer.RefreshPresentersAsync())));
        }

        private void SettingNavigation_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem Item)
            {
                PresenterSwitcher.Value = Convert.ToString(sender.MenuItems.IndexOf(Item));
            }
        }

        private void RemoveProfile_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is TerminalProfile Profile)
            {
                bool SholdRaiseDataChangedEvent = false;

                try
                {
                    TerminalList.Remove(Profile);

                    if (SQLite.Current.GetTerminalProfileByName(Profile.Name) != null)
                    {
                        SQLite.Current.DeleteTerminalProfile(Profile);
                        SholdRaiseDataChangedEvent = true;
                    }
                }
                catch (Exception ex)
                {
                    LogTracer.Log(ex, $"Could not remove the terminal profile: {Profile}");
                }
                finally
                {
                    if (SholdRaiseDataChangedEvent)
                    {
                        ApplicationData.Current.SignalDataChanged();
                    }
                }
            }
        }

        private void AddTerminalProfile_Click(object sender, RoutedEventArgs e)
        {
            string NewProfileName = string.Empty;
            short Count = 0;

            while (true)
            {
                NewProfileName = $"{Globalization.GetString("NewTerminalProfileDefaultName")} ({++Count})";

                if (!TerminalList.Select((Profile) => Profile.Name).Contains(NewProfileName))
                {
                    break;
                }
            }

            TerminalList.Add(new TerminalProfile(NewProfileName, string.Empty, string.Empty, default));
        }

        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            bool SholdRaiseDataChangedEvent = SQLite.Current.GetAllTerminalProfile().Count != TerminalList.Count;

            foreach (TerminalProfile Profile in TerminalList.Where((Profile) => !string.IsNullOrWhiteSpace(Profile.Name) && !string.IsNullOrWhiteSpace(Profile.Path) && !string.IsNullOrWhiteSpace(Profile.Argument)))
            {
                SQLite.Current.SetTerminalProfile(Profile);
            }

            if (SholdRaiseDataChangedEvent)
            {
                ApplicationData.Current.SignalDataChanged();
            }

            await HideAsync();
        }

        private async Task<IReadOnlyList<BackgroundPicture>> GetCustomPictureAsync()
        {
            List<BackgroundPicture> PictureList = new List<BackgroundPicture>();

            foreach (Uri ImageUri in SQLite.Current.GetBackgroundPicture())
            {
                try
                {
                    PictureList.Add(await BackgroundPicture.CreateAsync(ImageUri));
                }
                catch (Exception ex)
                {
                    SQLite.Current.DeleteBackgroundPicture(ImageUri);
                    LogTracer.Log(ex, "Error when loading background pictures, the file might lost");
                }
            }

            return PictureList;
        }

        private async void NavigatePrivacyLink_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/zhuxb711/RX-Explorer/blob/master/README.md"));
        }

        private void TerminalEditName_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;

            if (sender is FrameworkElement Element)
            {
                if (Element.FindParentOfType<RelativePanel>() is RelativePanel RootPanel)
                {
                    if (RootPanel.FindChildOfName<TextBox>("TerminalNameInput") is TextBox Input)
                    {
                        Input.Visibility = Visibility.Visible;
                        Input.Focus(FocusState.Programmatic);
                    }
                }
            }
        }

        private void TerminalNameInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement Element)
            {
                Element.Visibility = Visibility.Collapsed;
            }
        }

        private async void PictureGirdView_Loaded(object sender, RoutedEventArgs e)
        {
            if (PictureGirdView.SelectedIndex >= 0)
            {
                await PictureGirdView.SmoothScrollIntoViewWithIndexAsync(PictureGirdView.SelectedIndex, ScrollItemPlacement.Center);
            }
        }

        private async void InterceptFolder_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingControl.IsLoading = true;
                LoadingText.Text = Globalization.GetString("Progress_Tip_WaitingForAction");

                if (InterceptFolderSwitch.IsOn)
                {
                    ModifySystemWarningDialog Dialog = new ModifySystemWarningDialog();

                    if (await Dialog.ShowAsync() == ContentDialogResult.Primary)
                    {
                        using (FullTrustProcessController.ExclusiveUsage Exclusive = await FullTrustProcessController.GetAvailableControllerAsync())
                        {
                            if (await Exclusive.Controller.InterceptDesktopFolderAsync())
                            {
                                ApplicationData.Current.LocalSettings.Values["InterceptDesktopFolder"] = true;
                            }
                            else
                            {
                                QueueContentDialog dialog = new QueueContentDialog
                                {
                                    Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                    Content = Globalization.GetString("QueueDialog_ActionFailed_Content"),
                                    CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                                };

                                await dialog.ShowAsync();

                                InterceptFolderSwitch.Toggled -= InterceptFolder_Toggled;
                                InterceptFolderSwitch.IsOn = false;
                                InterceptFolderSwitch.Toggled += InterceptFolder_Toggled;
                            }
                        }
                    }
                    else
                    {
                        InterceptFolderSwitch.Toggled -= InterceptFolder_Toggled;
                        InterceptFolderSwitch.IsOn = false;
                        InterceptFolderSwitch.Toggled += InterceptFolder_Toggled;
                    }
                }
                else
                {
                    using (FullTrustProcessController.ExclusiveUsage Exclusive = await FullTrustProcessController.GetAvailableControllerAsync())
                    {
                        if (await Exclusive.Controller.RestoreFolderInterceptionAsync())
                        {
                            ApplicationData.Current.LocalSettings.Values["InterceptDesktopFolder"] = false;
                        }
                        else
                        {
                            QueueContentDialog dialog = new QueueContentDialog
                            {
                                Title = Globalization.GetString("Common_Dialog_ErrorTitle"),
                                Content = Globalization.GetString("QueueDialog_ActionFailed_Content"),
                                CloseButtonText = Globalization.GetString("Common_Dialog_CloseButton")
                            };

                            await dialog.ShowAsync();

                            InterceptFolderSwitch.Toggled -= InterceptFolder_Toggled;
                            InterceptFolderSwitch.IsOn = false;
                            InterceptFolderSwitch.Toggled += InterceptFolder_Toggled;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, "Error happend when Enable/Disable Win+E");
            }
            finally
            {
                await Task.Delay(1000);
                LoadingControl.IsLoading = false;
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void CommonSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ApplicationData.Current.SignalDataChanged();
        }

        private void AcrylicColorPicker_ColorChanged(Microsoft.UI.Xaml.Controls.ColorPicker sender, Microsoft.UI.Xaml.Controls.ColorChangedEventArgs args)
        {
            ApplicationData.Current.SignalDataChanged();
        }

        private async void FolderExportRestoreFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileSavePicker Picker = new FileSavePicker
                {
                    SuggestedStartLocation = PickerLocationId.Desktop,
                    SuggestedFileName = "Restore_Folder.reg"
                };

                Picker.FileTypeChoices.Add("REG", new string[] { ".reg" });

                if (await Picker.PickSaveFileAsync() is StorageFile ExportFile)
                {
                    StorageFile File = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Restore_Folder.reg"));
                    await File.CopyAndReplaceAsync(ExportFile);
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, "Could not export restore file");
            }
        }

        private async void ContactAuthor_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("mailto:zrfcfgs@outlook.com"));
        }

        private async void FileExtensionSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            IsShowFileExtensionsEnabled = FileExtensionSwitch.IsOn;

            try
            {
                await Task.WhenAll(TabViewContainer.Current.TabCollection.Select((Tab) => Tab.Content)
                                                                         .Cast<Frame>()
                                                                         .Select((Frame) => Frame.Content)
                                                                         .Cast<TabItemContentRenderer>()
                                                                         .Select((Renderer) => Renderer.RefreshPresentersAsync()));
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"Error in {nameof(FileExtensionSwitch_Toggled)}");
            }
            finally
            {
                ApplicationData.Current.SignalDataChanged();
            }
        }

        private void ShowContextMenuWhenLoading_Checked(object sender, RoutedEventArgs e)
        {
            IsParallelShowContextMenu = false;
            ApplicationData.Current.SignalDataChanged();
        }

        private void ShowContextMenuWhenLoading_Unchecked(object sender, RoutedEventArgs e)
        {
            IsParallelShowContextMenu = true;
            ApplicationData.Current.SignalDataChanged();
        }

        private void MicaMode_Checked(object sender, RoutedEventArgs e)
        {
            SolidColor_White.IsChecked = false;
            SolidColor_FollowSystem.IsChecked = false;
            SolidColor_Black.IsChecked = false;
            PreventFallBack.IsChecked = null;

            BackgroundController.Current.SwitchTo(BackgroundBrushType.Mica);

            ApplicationData.Current.SignalDataChanged();
        }

        private async void PictureModeExpander_Expanding(Expander sender, ExpanderExpandingEventArgs args)
        {
            if (PictureGirdView.SelectedIndex >= 0)
            {
                if (PictureGirdView.IsLoaded)
                {
                    await PictureGirdView.SmoothScrollIntoViewWithIndexAsync(PictureGirdView.SelectedIndex, ScrollItemPlacement.Center);
                }
                else
                {
                    PictureGirdView.ScrollIntoView(PictureGirdView.SelectedItem, ScrollIntoViewAlignment.Leading);
                }
            }
        }

        private void LoadWSLOnStartup_Toggled(object sender, RoutedEventArgs e)
        {
            IsLoadWSLFolderOnStartupEnabled = LoadWSLOnStartup.IsOn;
            ApplicationData.Current.SignalDataChanged();
        }

        private async void SettingShareData_Toggled(object sender, RoutedEventArgs e)
        {
#if DEBUG
            await Task.CompletedTask;
#else
            await Microsoft.AppCenter.AppCenter.SetEnabledAsync(SettingShareData.IsOn);
#endif
        }
    }
}
