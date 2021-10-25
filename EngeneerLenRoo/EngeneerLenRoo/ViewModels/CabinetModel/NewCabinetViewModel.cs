using System;
using System.Collections.Generic;
using Xamarin.Forms;
using EngeneerLenRoo.Models;

namespace EngeneerLenRoo.ViewModels.CabinetModel
{
    public class NewCabinetViewModel: BaseViewModel
    {
        private string text;
        private string description;

        public NewCabinetViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                   && !String.IsNullOrWhiteSpace(description);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Cabinet newItem = new Cabinet()
            {
                Id = 32.ToString(),
                Name = Text,
                Sotrudniks = new List<Sotrudnik>()
            };

            await DataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}