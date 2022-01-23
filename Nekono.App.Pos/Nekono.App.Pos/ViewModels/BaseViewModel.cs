using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using Nekono.App.Pos.Models;
using Nekono.App.Pos.Services;

namespace Nekono.App.Pos.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public ICollectionReceiptServices CollectionReceiptServices => DependencyService.Get<ICollectionReceiptServices>();
        
        public IItemsServices ItemsServices => DependencyService.Get<IItemsServices>();

        public ILoginServices LoginServices => DependencyService.Get<ILoginServices>();
        
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        public IDataStore<ItemInPOSDetails> ItemsStore => DependencyService.Get<IDataStore<ItemInPOSDetails>>();

        public IDataStore<InventoryDetails> ItemSalesStore => DependencyService.Get<IDataStore<InventoryDetails>>();

        public IDataStore<CollectionReceiptDetails> CollectionReceiptsStore => DependencyService.Get<IDataStore<CollectionReceiptDetails>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        bool isNotBusy = true;
        public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                if (SetProperty(ref isNotBusy, value))
                    IsBusy = !isNotBusy;
            }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
