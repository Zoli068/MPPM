   M S B A M L   `   `   `  ����  MVVM$    MVVM.Views.GetExtentValuesView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      .��5      �   � RowDefinitions  5   
   �� 5      $��2* �6   �� 5	      $��3* �6   �� 5
      $��3* �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0 5 0 0q�6"   .+�5      �� $��Center=�6   $��25(�65   Select the DMS type6X   �� 5      $��0 15 0 0q�6   $��Center=�6*   $��20(�6G   ��6U   �� *
DMSTypes+ �� SelectedItem 6v   �� *SelectedDMSType+6�   5      �� 5   
   $��1��6   $��	25 0 25 0q�6"   .+�5      �� $��Center=�6   $��0 0 0 5q�65   $��25(�6G   Select the attrbiutes6l   �� 5       $��Stretch=�6   $	��100��63   $	��100��6C   ��. $
��Auto=�5!   =   ��5    S   �� *
Attributes+ 6v   �� *SelectedAttributeToAdd+5!   =   5#      �� 5%   
   $��2��6   $��	25 0 25 0q�6"   .+�5'      �� $��Center=�6   $��0 0 0 0q�65   $��25(�6G   Selected attrbiutes6j   �� 5)      $��Stretch=�6   $	��100��62   $	��100��6B    �� SelectionMode$ Single=�6R   ��. $
��Auto=�5*   n   ��5)   j   �� *SelectedAttributes+ 5*      �� *SelectedAttributeToRemove+6n   5,      �� 5.   
   $��4��6   $��
25 5 25 10q�6   $��25(�62   ��6@   �� *GetExtentValuesCommand+.��6k   
Get Values6w   �� 50   
   $��0��6   $��0 20 0 0q�6!   $��2��63   $��1��6G   $��Center=�6W   $��30(�6t   Readed values6�   �� 52   
   $��0 35 0 0q�6   $��0��6&   $��1��63   $��2��6C   $��Center=�6X   $��20(�6u   $��30��53   p   $��70��6   ��6   �� *SelectedResultNumbers+ 6B   �� *SelectedResultNumber+6   �� 55   
   $��0��6   $��0 85 0 0q�6!   $��1��64   $��Center=�6D   $��25(�6a   Attribute name6   �� 57   
   $��0��6   $��0 85 0 0q�6!   $��2��64   $��Center=�6D   $��25(�6a   Attribute Value6�   �� 59   
   $��Hidden=�6   $��1��6D   $��4��6Q   $��1��6b   $��2��6r   $��10q�6�   ��6�   �� *ReadedValues+��5:      �� 5;      .R�5=      �� $��1q�6   
���   �._�5?      � $��6q�6    5A      �� 5B   "   $	��325 �63   �� 5C   "   $	��325 �63   .+�5F      �� $
��Wrap=�6(    �� TextAlignment$ Center=�6<   $��Center=�6S   $��0��6o   $��	-15 0 0 0q�6   $��15(�5G   (   $
��Bold"�6M   $��Center=�6`   ��66   �� *Item1+6`   �� 5I      $
��Wrap=�6(   $ Center=�6<   $��Center=�6S   $��1��6o   $��30 0 0 0q�6�   $��15(�5J   (   $��Center=�6M   ��66   �� *Item2+6M   5L      5N      5P      5R      5T      5V      