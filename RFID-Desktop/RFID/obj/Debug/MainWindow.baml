   M S B A M L   `   `   `  ����H  DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	   ? ;RFID, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:RFIDRFID 5   	      RFID.MainWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   $localclr-namespace:RFID     5   	   -   5          �� ClearTypeHint$
  Auto=�5   	   &   System.Windows.Media.TextOptions   TextHintingMode$ Animated=�6,   (   "MahApps.Metro.Controls.MetroWindow   ShowTitleBar .    ShowIconOnTitleBar . $��1q�5	   H      SaveWindowPosition .  � 
ResizeMode$ CanResizeWithGrip=�5
   	      AccentBaseColorBrush   	GlowBrush# �   6(    � WindowStartupLocation$ CenterScreen=�6[    � Title$ DRFID��5   	   $	��600��6   $	��800��6'   	 � WindowStyle$	 SingleBorderWindow=�6R   
   RightWindowCommands
 5      +   %MahApps.Metro.Controls.WindowCommands  5   
   .@�5      � $��Right=�6   $
��10,0q�64   .+�5      �� $��Tema :��6   $��10(�6@   $��Segoe UI)�6N   $	��5,0q�6d   $��Center=�6q   ��6*   Z� 6q   )   #MahApps.Metro.Controls.ToggleSwitch  5      -   5          Name$ toogle��5   (   $��Center=�68   $
��Auto��6S    + 'MahApps.Metro.Styles.ToggleSwitch.Win10#��[ 5   %   $��Segoe UI)�6f    � Cursor$
 Hand��6|      OnLabel$
 Dark��5   %      OffLabel$ Light��64      	IsChecked$ FalseY�6E   $��Center=�6W   $��9(�6t   $��0,0,10,0q�6�   �� 5      $��Renk :��6   $��10(�6@   $��Segoe UI)�6N   $	��5,0q�6d   $��Center=�6q   ��6*   Z� 6q   �� 5      -   5         ��Name$ ColorsSelector��5      $��0q�6�   $��0,2,10,0q�6�   $
 Hand��6�   ��6g   �� *Colors+��6�   Z� ��5      �� 5      .R�5      �� $��Segoe UI)�6S   ��6(   �� *Key+��6=   Z� 6S   5      5      5      5      .��5      �  � RowDefinitions 5   
   �� 5       $��3* �6   �� 5!      $��5* �6   .+�5$   
   � $��0��6    5%      �� 5&      $��3* �6    .+�5(      � $��1��6   .+�5)      �� -   5         ��Name$ IPtxt��5)   u   *   $MahApps.Metro.Controls.TextBoxHelper   	Watermark$ 	IP ADRESS��6      ClearTextButton . $	��150��6�   $��Center=�6�   $
��Left=�6�   $	��Top=�6�   $��30,10q�6�   d� 5*      -   5         d�Name$ connectionButton��5*      $��Connect��69   $��80��6K   $��20��6V   $	��Top=�6b   $
��Left=�6z   $��200,10q�6�   6�   �� 5+      #���   6   $	��200��6O   $	��120��6[   $��Bottom=�6h   $
��Left=�6�   $��30,20q�6�   .��5,      �  5-      �� 5.      $��3* �6,   �� 5/      $��2* �6,   .+�51        -   5        $ start��51   0   $��Center=�6?   $��Center=�6a   $��Center=�6|   $��Center=�6�   #��[ 52   e   $��Segoe UI)�6�   $
 Hand��6�   $
 STOP��53   0   $ START��6?   $ FalseY�6P   $��20(�6b   $��40��6p   $
��AUTO��6|   �� 54      -   5         ��Name$ 	tagsfound��54   $   $��1��67   $��TAGS FOUND : ��6D   $
��13,0q�6Y   $��80��6g   $
��Left=�6r   $��Center=�6�   �� 55      -   5        $ tagstxt��55   "   $��1��63   $��50��6@   $��Center=�6K   $��Right=�6o    �� 
IsReadOnly . $��Center=�6�   $��	15,3,20,2q�6�   56      57      58      � 59      .+�5:      �� #���   6   $��Stretch=�6O   $��Stretch=�6k   $��
330,0,30,0q�6�   .��5;      � .+�5<      �� -	   5         ��Name$ loglst��5<   "   5>      5?      5@      5A      5B      &   System.Windows.Controls.DataGrid  5C   
   -
   5          Name$ datagrd��5C      $��2��6$      AutoGenerateColumns .     
IsReadOnly . $��Visible=�6u   $��Visible=�6�   ��6_   Z�    Columns 5D      0  *System.Windows.Controls.DataGridTextColumn  5E      -   5        	EPCdt��,  &System.Windows.Controls.DataGridColumn   Header$	 EPC��5E   4      Width5	  /System.Windows.Controls.DataGridLengthConverter$ 2*	 6A   1
  +System.Windows.Controls.DataGridBoundColumn 
  Binding 6L   �� *EPC+  5F      -   5        	Antennadt��$ Antenna��5F   8   $ *	 6I    6S   �� *	Antenna+  5G      -   5        Countdt��$ Count��5G   6    6E   �� *Count+  5H      -   5        Rssiddt��$
 RSSI��5H   6    6D   �� *RSSI+  5I      -   5        Timestampdt��$ 	TimeStamp��5I   :   $ *	 6M    6W   �� *	TimeStamp+    ColumnHeaderStyle  5K      �� 5L        MetroDataGridColumnHeader! �� BasedOn#! [ 6   " �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader""  6M   .�5M      �� # �� Property# � ��$ �� Value#$ �   63   % �� Triggers% 5N      ]� 5O      & ]� 
Conditions& 5P      �� 5Q   "   ' �� Property' �@J�IsMouseOver( �� Value( . 6C   �� 5R   "   ' �@ SortDirection( 6E   Z� ./�5T      �� # � ��  HighlightBrush#$ �  6;   5U      5W      5Y      5Z      5[      