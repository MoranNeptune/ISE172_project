   M S B A M L   `   `   `  ����J  FMileStoneClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MileStoneClientMileStoneClient  5   	   3    -MileStoneClient.PresentationLayer.LoginWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:MileStoneClient     5   	   -   5          �Name$  Login��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Title$ LoginWindow��5	   	   $	��600��6   $	��450��6*   .��5
      � $	��450��6   $	��600��6   ��5   
   �� 5       �� ImageSource$0 */MileStoneClient;component/media/login.jpg��6    � ColumnDefinitions 5   
   �� 5      .+�5   
   �� -   5        $��Login��5      $
��Left=�6!   $	��Top=�6<   $	��120��6T   $��
92,437,0,0q�6`   $��35��6t   $��24(�6�   $
��Bold"�6�   {�6�   �� *BtnLoginIsEnabled+ �� Mode$ TwoWay=��� 5   
   -   5        $
��Back��5      $
��Left=�6    $	��Top=�6;   $	��120��6S   $��269,437,0,0q�6_   $��35��6t   $��24(�6�   $
��Bold"�6�   6�   �� 5   
   -   5        $
��Left=�5   \   $��35��6w    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��100��6�   $��220,314,0,0q�6�   $��24(�6�   ��6   ��  �� Path$ GroupIdContent� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5   
   -   5        $
��Left=�5   ]   $��35��6x   $
 Wrap=�6�   $	��Top=�6�   $	��200��6�   $��220,367,0,0q�6�   $��24(�6�   ��6   �� $ NicknameContent�$ PropertyChanged=�6�   �� 5   
   $
��Left=�6A   $��
59,477,0,0q�6\   $	��Top=�6p   $	��200��6�   ���	#FF00B7E0$��20(�6�   $��MV Boli)�6�   $��	ExtraBold"�6�   $��50��6�   ��6   �� *LblAddRegContent+$ OneWay=�y�6�   �� *LblAddRegVisibility+$ OneWay=��� 5   
   -   5        $��Register��5      $
��Left=�6$   $	��Top=�6?   $	��120��6W   $��269,491,0,0q�6c   $��36��6x   $��24(�6�   $
��Bold"�6�   y�6�   �� *BtnRegisterVisibility+$ OneWay=�5      5      