   M S B A M L   `   `   `  ����P  LHandwritingCompressor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:HandwritingCompressorHandwritingCompressor  5   	   &     HandwritingCompressor.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:HandwritingCompressor     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $	��200 �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      -   5         ��Name$ demoNotificationBar��5   (   $��24 �6   6(   .+�5   
   �� $��3��6   $��2��5       �� Orientation$ 
Horizontal=�5      $��Right=�5      .+�5      �� $L��FYou have a demo version. Enter the product key to get full access. -> ��6   �� 5      -   5         ��Name$ enterProductKeyBtn��5      .��6R   Enter product key6e   5      � 5   
    5      �� 5      $��* �6    �� 5      $��22 �6    .+�5       �� -   5         ��Name$ filesListView��5       5!      �� 5#      $��1��6   $ 
Horizontal=�6&   .+�5$      �� -   5        $ 
addFileBtn��5$   $   $��60��6   .��6Q   + Add6X   �� 5%      -   5        $ removeFileBtn��5%   $   $��60��6   .��6W   
- Remove6a   5&      5'      �� 5)   
   $��1��6   .+�5*      �� -   5         ��Name$ brightnessLabel��5*      $��Brightness (0%)��6-   �� 5+      -   5         ��Name$ brightnessSlider��5+      	 � Minimum$
	 -100Q�6/   
 � Maximum$	
 100Q�6>    � Value$ 0Q�6L   6V   �� 5,      -   5        $ contrastLabel��5,      $��Contrast (0%)��6+   �� 5-      -	   5        $ contrastSlider��5-      $
	 -100Q�6-   $	
 100Q�6<   $ 0Q�6J   6T   �� 5.      -
   5         ��Name$ negativeCheckBox��5.      $��Negative��61    d� 	IsChecked$ FalseY�6D   �� 5/      -   5        $��Update preview��5/      6.   �� 50      -   5        $��Export selected��50      6/   �� 51      -   5        $ exportAllBtn��51   *   $��
Export all��6   6@   52      � 53   
   $��1��6   $��2��6    54      �� 55      $��24 �6    �� 56      $��* �6    .+�58      �� -   5        $ previewScale��58      $	
 200Q�59      $	 10Q�6#   $	 100Q�60   $	��200��5;      �� 5<      $��1��6   $��Visible=�6(   .��5=      �� -   5         ��Name$ previewImage��5=      5>      5?      5@      5A      