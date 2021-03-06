﻿using CodeSniffer.Interfaces;
using System.Collections.ObjectModel;

namespace CodeSniffer.ViewModels
{
    
    //TODO: MOVE TO SEPARATE CLASS
    public class CodeFragmentViewModel : ViewModelBase
    {
        private bool _isSelected;

        public string Name { get; set; }
        public string Content { get; set; }
        
        public ICodeFragment Model { get; set; }

        public ObservableCollection<CodeFragmentViewModel> Children { get; set; }

        public CodeFragmentViewModel()
        {
            Name = "";
            Content = "";
        }

        public CodeFragmentViewModel(ICodeFragment model)
        {
            Name = model.Name;
            Content = model.Content;
            Model = model;
            Children = new ObservableCollection<CodeFragmentViewModel>();
        }

        public void AddChild(CodeFragmentViewModel codeItem)
        {
            Children.Add(codeItem);
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set{
                _isSelected = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsActive
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged();
            }
        }
    }
}
