﻿using NTMiner.Vms;
using System.Windows.Controls;

namespace NTMiner.Views.Ucs {
    public partial class KernelOutputEdit : UserControl {
        public static string ViewId = nameof(KernelOutputEdit);

        public static void ShowWindow(FormType formType, KernelOutputViewModel source) {
            ContainerWindow.ShowWindow("内核输出", new ContainerWindowViewModel {
                FormType = formType,
                IsDialogWindow = true,
                CloseVisible = System.Windows.Visibility.Visible,
                IconName = "Icon_KernelOutput"
            }, ucFactory: (window) =>
            {
                KernelOutputViewModel vm = new KernelOutputViewModel(source) {
                    CloseWindow = () => window.Close()
                };
                return new KernelOutputEdit(vm);
            }, fixedSize: true);
        }

        private KernelOutputViewModel Vm {
            get {
                return (KernelOutputViewModel)this.DataContext;
            }
        }

        public KernelOutputEdit(KernelOutputViewModel vm) {
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
