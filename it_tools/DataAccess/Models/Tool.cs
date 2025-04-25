using it_tools.BusinessLogic.Services;
using it_tools.Helper;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ToolLib;

namespace it_tools.DataAccess.Models
{
    public class Tool : INotifyPropertyChanged
    {
        public string idTool { get; set; }
        public string name { get; set; }
        public string descript { get; set; }
        private string _status;

        public string status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(status));
                }
            }
        }
        private string _accessLevel;
        public string access_level
        {
            get => _accessLevel;
            set
            {
                if (_accessLevel != value)
                {
                    _accessLevel = value;
                    OnPropertyChanged();
                }
            }
        }
        public string iconURL { get; set; }
        public string idToolType { get; set; }
        public string? dllPath { get; set; }

        private bool _isFavourite;
        public bool isFavourite
        {
            get => _isFavourite;
            set
            {
                if (_isFavourite != value)
                {
                    _isFavourite = value;
                    OnPropertyChanged();
                }
            }
        }

        public ITool? LoadedPlugin { get; set; }

        public void LoadPlugin()
        {
            if (!string.IsNullOrEmpty(dllPath))
            {
                LoadedPlugin = ToolHelper.LoadToolFromDll(dllPath);
            }
        }
        private bool _isDelete;
        public bool isDelete
        {
            get => _isDelete;
            set
            {
                if (_isDelete != value)
                {
                    _isDelete = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // fix lỗi hiern thị thông báo khi recreate itemTool trong combo box
        private string _originalAccessLevel;
        public string OriginalAccessLevel
        {
            get => _originalAccessLevel;
            set
            {
                _originalAccessLevel = value;
                OnPropertyChanged(nameof(OriginalAccessLevel));
            }
        }
    }
}