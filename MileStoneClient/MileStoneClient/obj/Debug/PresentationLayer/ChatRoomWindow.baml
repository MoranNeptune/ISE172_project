   M S B A M L   `   `   `  ����J  FMileStoneClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MileStoneClientMileStoneClient  5   	   6    0MileStoneClient.PresentationLayer.ChatRoomWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   J FWpfAnimatedGif, Version=1.4.15.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  5   	   )localclr-namespace:MileStoneClient  5   	   -   5          �Name$  ChatRoomWin��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Title$ ChatRoom��5
   	   $	��700��6   $	��950��6'   4  .System.Windows.Automation.AutomationProperties   Name$ ChatRoomWin��63   ��6[   Z� ��6q   Z� .��5      � $
��Left=�6   $	��670��6&   $��	0,6,-8,-7q�63   $	��Top=�6F   $	��950��6^   ��5   
   �� 5       �� ImageSource$3 -/MileStoneClient;component/media/ChatRoom.jpg��6   .+�5   
   �� $
��Left=�6   $
��Wrap=�6/   $��	TextBlock��6C   $	��Top=�6T   �� 5   
   -   5        $��View Profile��5      $
��Left=�6(   $	��Top=�6C   $��75��6[   $��
68,322,0,0q�6f   $��75��6z   6�   �� 5   
   -   5        $��LogOut��5      $
��Left=�6"   $	��Top=�6=   $��75��6U   $��
68,442,0,0q�6`   $��75��6t   6�   �� 5   
   $
��Left=�6   $��35��6-   $��230,598,0,0q�69    �� TextWrapping$
 Wrap=�6N   $	��Top=�6�   $	��607��6�   ��6b   �� *TxtSendContent+6�   �� 5   
   -   5        $
��Send��5      $
��Left=�6    $��35��6;   $��840,598,0,0q�6G   $	��Top=�6\   $��68��6t   6   �� 5   
   -   5        $��Options��5      $
��Left=�6#   $	��Top=�6>   $��75��6V   $��
68,202,0,0q�6a   $��75��6u   6�   �� 5   
   -   5        $
��Left=�5   K   $	��481��6f   $��230,112,0,0q�6s   $	��Top=�6�   $	��678��6�   ��6   �� *
Messages+6�    � 5   
     � NavigationUIVisibility$ Hidden=�6   $
��Left=�60   $	��400��6K   $��148,202,0,0q�6X   $	��Top=�6m   $��320.8��6�   ��6�   �� *IsOptionVisible+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5      5      