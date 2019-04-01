﻿using NTMiner.Vms;
using System.Windows.Controls;

namespace NTMiner.Views.Ucs {
    public partial class GroupPage : UserControl {
        public static string ViewId = nameof(GroupPage);

        public static void ShowWindow() {
            ContainerWindow.ShowWindow("组", new ContainerWindowViewModel {
                IconName = "Icon_Group",
                CloseVisible = System.Windows.Visibility.Visible,
                FooterVisible = System.Windows.Visibility.Collapsed,
                Width = 660,
                Height = 420
            }, ucFactory: (window) => new GroupPage(), fixedSize: false);
        }

        private GroupPageViewModel Vm {
            get {
                return (GroupPageViewModel)this.DataContext;
            }
        }

        public GroupPage() {
            InitializeComponent();
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Wpf.Util.DataGrid_MouseDoubleClick<GroupViewModel>(sender, e);
        }
    }
}
