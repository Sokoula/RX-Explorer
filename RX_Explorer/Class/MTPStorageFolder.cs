﻿using Microsoft.Win32.SafeHandles;
using RX_Explorer.Interface;
using ShareClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Portable;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace RX_Explorer.Class
{
    public class MTPStorageFolder : FileSystemStorageFolder, IMTPStorageItem
    {
        private readonly MTPStorageFolder ParentFolder;

        public string DeviceId => @$"\\?\{new string(Path.Skip(4).ToArray()).Split(@"\", StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()}";

        protected override Task<BitmapImage> GetThumbnailCoreAsync(ThumbnailMode Mode)
        {
            return Task.FromResult<BitmapImage>(null);
        }

        protected override Task<IRandomAccessStream> GetThumbnailRawStreamCoreAsync(ThumbnailMode Mode)
        {
            return Task.FromResult<IRandomAccessStream>(null);
        }

        protected override Task LoadCoreAsync(bool ForceUpdate)
        {
            return Task.CompletedTask;
        }

        public override async Task<IStorageItem> GetStorageItemAsync()
        {
            try
            {
                if (StorageItem != null)
                {
                    return StorageItem;
                }
                else
                {
                    if (Path.Equals(DeviceId, StringComparison.OrdinalIgnoreCase))
                    {
                        return StorageItem = await Task.Run(() => StorageDevice.FromId(DeviceId));
                    }
                    else if (ParentFolder != null)
                    {
                        if (await ParentFolder.GetStorageItemAsync() is StorageFolder Folder)
                        {
                            if (await Folder.TryGetItemAsync(Name) is StorageFolder Item)
                            {
                                return StorageItem = Item;
                            }
                        }
                    }
                    else if (await Task.Run(() => StorageDevice.FromId(DeviceId)) is StorageFolder RootFolder)
                    {
                        return StorageItem = await RootFolder.GetStorageItemByTraverse<StorageFolder>(new PathAnalysis(Path, DeviceId));
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"Could not get StorageFile, Path: {Path}");
            }

            return null;
        }

        public override Task<IReadOnlyDictionary<string, string>> GetPropertiesAsync(IEnumerable<string> Properties)
        {
            return Task.FromResult<IReadOnlyDictionary<string, string>>(new Dictionary<string, string>(Properties.Select((Prop) => new KeyValuePair<string, string>(Prop, string.Empty))));
        }

        public override async IAsyncEnumerable<FileSystemStorageItemBase> GetChildItemsAsync(bool IncludeHiddenItems = false,
                                                                                             bool IncludeSystemItems = false,
                                                                                             bool IncludeAllSubItems = false,
                                                                                             [EnumeratorCancellation] CancellationToken CancelToken = default,
                                                                                             BasicFilters Filter = BasicFilters.File | BasicFilters.Folder,
                                                                                             Func<string, bool> AdvanceFilter = null)
        {
            using (FullTrustProcessController.ExclusiveUsage Exclusive = await FullTrustProcessController.GetAvailableControllerAsync())
            {
                foreach (MTPFileData Data in await Exclusive.Controller.GetMTPChildItemsDataAsync(Path, IncludeHiddenItems, IncludeSystemItems, IncludeAllSubItems, Filter, CancelToken))
                {
                    if (Data.Attributes.HasFlag(System.IO.FileAttributes.Directory))
                    {
                        yield return new MTPStorageFolder(Data, this);
                    }
                    else
                    {
                        yield return new MTPStorageFile(Data, this);
                    }
                }
            }
        }

        public override IAsyncEnumerable<FileSystemStorageItemBase> SearchAsync(string SearchWord,
                                                                                bool SearchInSubFolders = false,
                                                                                bool IncludeHiddenItems = false,
                                                                                bool IncludeSystemItems = false,
                                                                                bool IsRegexExpression = false,
                                                                                bool IsAQSExpression = false,
                                                                                bool UseIndexerOnly = false,
                                                                                bool IgnoreCase = true,
                                                                                CancellationToken CancelToken = default)
        {
            if (IsAQSExpression)
            {
                throw new ArgumentException($"{nameof(IsAQSExpression)} is not supported");
            }

            IAsyncEnumerable<FileSystemStorageItemBase> Result = GetChildItemsAsync(IncludeHiddenItems, IncludeSystemItems, SearchInSubFolders, CancelToken: CancelToken);

            return IsRegexExpression ? Result.Where((Item) => Regex.IsMatch(Item.Name, SearchWord, IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None))
                                     : Result.Where((Item) => Item.Name.Contains(SearchWord, IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
        }

        public override async Task<FileSystemStorageItemBase> CreateNewSubItemAsync(string Name, CreateType ItemType, CreateOption Option)
        {
            using (FullTrustProcessController.ExclusiveUsage Exclusive = await FullTrustProcessController.GetAvailableControllerAsync())
            {
                MTPFileData Data = await Exclusive.Controller.MTPCreateSubItemAsync(Path, Name, ItemType, Option);

                if (Data != null)
                {
                    switch (ItemType)
                    {
                        case CreateType.File:
                            {
                                return new MTPStorageFile(Data, this);
                            }
                        case CreateType.Folder:
                            {
                                return new MTPStorageFolder(Data, this);
                            }
                    }
                }

                return null;
            }
        }

        public override async Task<bool> CheckContainsAnyItemAsync(bool IncludeHiddenItems = false,
                                                                   bool IncludeSystemItems = false,
                                                                   BasicFilters Filter = BasicFilters.File | BasicFilters.Folder)
        {
            using (FullTrustProcessController.ExclusiveUsage Exclusive = await FullTrustProcessController.GetAvailableControllerAsync())
            {
                return await Exclusive.Controller.MTPCheckContainersAnyItemsAsync(Path, IncludeHiddenItems, IncludeSystemItems, Filter);
            }
        }

        public override Task<SafeFileHandle> GetNativeHandleAsync(AccessMode Mode, OptimizeOption Option)
        {
            return Task.FromResult(new SafeFileHandle(IntPtr.Zero, true));
        }

        protected override Task<BitmapImage> GetThumbnailOverlayAsync()
        {
            return Task.FromResult<BitmapImage>(null);
        }

        public async Task<MTPFileData> GetRawDataAsync()
        {
            try
            {
                if (GetBulkAccessSharedController(out var ControllerRef))
                {
                    using (ControllerRef)
                    {
                        return await ControllerRef.Value.Controller.GetMTPItemDataAsync(Path);
                    }
                }
                else
                {
                    using (FullTrustProcessController.ExclusiveUsage Exclusive = await FullTrustProcessController.GetAvailableControllerAsync())
                    {
                        return await Exclusive.Controller.GetMTPItemDataAsync(Path);
                    }
                }
            }
            catch (Exception ex)
            {
                LogTracer.Log(ex, $"An unexpected exception was threw in {nameof(GetRawDataAsync)}");
            }

            return null;
        }

        public MTPStorageFolder(MTPFileData Data) : this(Data, null)
        {

        }

        private MTPStorageFolder(MTPFileData Data, MTPStorageFolder Parent) : base(Data)
        {
            ParentFolder = Parent;
        }
    }
}
