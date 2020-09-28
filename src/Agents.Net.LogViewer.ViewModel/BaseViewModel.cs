using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Agents.Net.LogViewer.ViewModel.Annotations;

namespace Agents.Net.LogViewer.ViewModel
{
    public abstract class BaseViewModel
    {
        protected BaseViewModel(string name, string fullName, Guid id)
        {
            Name = name;
            FullName = fullName;
            Id = id;
            SelectCommand = new RelayCommand(Select);
        }

        public string Name { get; }
        public string FullName { get; }
        public Guid Id { get; }
        public ICommand SelectCommand { get; }

        public void Select(object parameter)
        {
            OnSelectRequested(new SelectRequestedEventArgs((BaseViewModel) parameter));
        }

        public event EventHandler<SelectRequestedEventArgs> SelectRequested; 

        public override string ToString()
        {
            return $"{nameof(FullName)}: {FullName}, {nameof(Id)}: {Id}";
        }

        protected virtual void OnSelectRequested(SelectRequestedEventArgs e)
        {
            SelectRequested?.Invoke(this, e);
        }
    }
}