   M S B A M L   `   `   `  ����?  ;MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MVVM.ViewModelsMVVM.ViewModels  5   	   'clr-namespace:MVVM.Views
MVVM.Views  5   	       MVVM.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   4
viewModelsclr-namespace:MVVM.ViewModels     5   	   *viewsclr-namespace:MVVM.Views     5   	     � Title$  
GDA client��5   	   $
��1068��5   	   $	��600��6    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5	   	    � DataContext 5      #   MVVM.ViewModels.MainViewModel  5   
    � 	Resources 5       �� DataType#   MVVM.ViewModels.HomeViewModel   MVVM.Views.HomeView(  "MVVM.ViewModels.GetValuesViewModel   MVVM.Views.GetValuesView+   %MVVM.ViewModels.AvailableGIDViewModel!   MVVM.Views.AvailableGIDView/   )MVVM.ViewModels.GetRelatedValuesViewModel!	   MVVM.Views.GetRelatedValues.
   (MVVM.ViewModels.GetExtentValuesViewModel$   MVVM.Views.GetExtentValuesView��%w  (��       5      *, +)(�� 2     5      *, +)(�� d     5      *, +)(�� �     5      *, +)(�� �     5!      *,
 +)�� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5      	  5      �� 5!   
   " 
 6   .R�5"        5#      5%      .��5'      � .+�5)   
   �� {�6   �� *DataContext.IsCimXmlSelected+ �� RelativeSource ��  �� AncestorType$ WindowL���5*      �� *Views+ �� SelectedItem 63   �� *SelectedTab+��5,      �� 5-      .R�5.      �� ��6    �� *Title+5/      	 �� ContentTemplate	 52      �� 53      .R�54      �� ��6'   �� *ContentViewModel+55      58      5:      5<      