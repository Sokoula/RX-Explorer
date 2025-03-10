﻿using ShareClassLibrary;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace RX_Explorer.Class
{
    public class MTPDriveData : NormalDriveData
    {
        public override string Name => string.IsNullOrEmpty(RawData?.Name) ? base.Name : RawData.Name;

        public override string DisplayName => Name;

        public override string Path => DeviceId;

        public override ulong TotalByte => (RawData?.TotalByte).GetValueOrDefault();

        public override ulong FreeByte => (RawData?.FreeByte).GetValueOrDefault();

        public override string FileSystem => string.IsNullOrEmpty(RawData?.FileSystem) ? base.FileSystem : RawData.FileSystem;

        private MTPDriveVolumnData RawData;

        public MTPDriveData(FileSystemStorageFolder Drive, string DriveId = null) : base(Drive, DriveType.Removable, DriveId)
        {

        }

        protected override async Task<BitmapImage> GetThumbnailCoreAsync()
        {
            if (await DriveFolder.GetStorageItemAsync() is IStorageItem Item)
            {
                return await Item.GetThumbnailBitmapAsync(ThumbnailMode.SingleItem);
            }

            return null;
        }

        protected override async Task LoadCoreAsync()
        {
            using (FullTrustProcessController.ExclusiveUsage Exclusive = await FullTrustProcessController.GetAvailableControllerAsync())
            {
                RawData = await Exclusive.Controller.GetMTPDriveVolumnDataAsync(Path);
            }
        }
    }
}
