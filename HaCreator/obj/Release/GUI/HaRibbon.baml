   M S B A M L   `   `   `  ���� D  @HaCreator, Version=2.1.1.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:HaCreator.CustomControlsHaCreator.CustomControls  -clr-namespace:HaCreator.GUIHaCreator.GUI  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem     HaCreator.GUI.HaRibbon   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5cc&clr-namespace:HaCreator.CustomControls     *nsclr-namespace:HaCreator.GUI     6sys&clr-namespace:System;assembly=mscorlib     .��� .+�+  %System.Windows.Controls.Ribbon.Ribbon  -      Name$  ribbon��$	��140��|�.   J� CommandBindings )  #System.Windows.Input.CommandBinding  -      Command$ ns:HaRibbon.New��  -   $ ns:HaRibbon.Open��  -   $ ns:HaRibbon.Save��  -   $ ns:HaRibbon.Repack��  -   $ ns:HaRibbon.About��  -   $ ns:HaRibbon.Help��  -   $ ns:HaRibbon.Settings��  -	   $ ns:HaRibbon.Exit��  -
   $ ns:HaRibbon.ViewBoxes��  -   $ ns:HaRibbon.Minimap��  -   $ ns:HaRibbon.Parallax��  -   $ ns:HaRibbon.Finalize��  -   $ ns:HaRibbon.AllLayerView��  -   $ ns:HaRibbon.MapSim��  -   $ ns:HaRibbon.RegenMinimap��  -   $ ns:HaRibbon.Snapping��  -   $ ns:HaRibbon.Random��  -   $ ns:HaRibbon.InfoMode��  -   $ ns:HaRibbon.HaRepacker��  -   $ ns:HaRibbon.LayerUp��  -   $ ns:HaRibbon.LayerDown��  -   $! ns:HaRibbon.AllPlatformView��  -   $ ns:HaRibbon.PlatformUp��  -   $ ns:HaRibbon.PlatformDown��  -   $ ns:HaRibbon.NewPlatform��  -   $ ns:HaRibbon.UserObjs��  -   $ ns:HaRibbon.Export��   ApplicationMenu :  4System.Windows.Controls.Ribbon.RibbonApplicationMenu  5  /System.Windows.Controls.Ribbon.RibbonMenuButton   KeyTip$ M��|�.  .@�>  8System.Windows.Controls.Ribbon.RibbonApplicationMenuItem  -     Name$ newBtn��$��New...�� m� Command$ ns:HaRibbon.New��3  -System.Windows.Controls.Ribbon.RibbonMenuItem   ImageSource$D >pack://application:,,,/HaCreator;component/Images/new_icon.png��   KeyTip$ N��|�.    -   $ openBtn��$��Open...��$ ns:HaRibbon.Open��$B <pack://application:,,,/HaCreator;component/Images/Folder.png��$ O��|�.    -   $ 	exportBtn��$��Save...��$ ns:HaRibbon.Export��$@ :pack://application:,,,/HaCreator;component/Images/Save.png��$ S��|�.  4  .System.Windows.Controls.Ribbon.RibbonSeparator    -    $ saveBtn��$��Save to WZ...��$ ns:HaRibbon.Save��$G Apack://application:,,,/HaCreator;component/Images/export-icon.png��|�.    -!   $ 	repackBtn��$��Repack WZ Files...��$ ns:HaRibbon.Repack��$> 8pack://application:,,,/HaCreator;component/Images/wz.ico��|�.      -"   $ hrBtn��$��Open HaRepacker��$ ns:HaRibbon.HaRepacker��$D >pack://application:,,,/HaCreator;component/Images/HaLogo32.png��|�.      -#   $ aboutBtn��$��About��$ ns:HaRibbon.About��$A ;pack://application:,,,/HaCreator;component/Images/About.png��|�.    -$   $ helpBtn��$
��Help��$ ns:HaRibbon.Help��$E ?pack://application:,,,/HaCreator;component/Images/help_icon.png��$ H��|�.  	   FooterPaneContent	 � 
 � ColumnDefinitions
 �� $
��Auto ��� $
��Auto �.+�1  +System.Windows.Controls.Ribbon.RibbonButton     Label$ Settings��$��ns:HaRibbon.Settings��   SmallImageSource$D >pack://application:,,,/HaCreator;component/Images/Settings.png��$��0��|�.    $
 Exit��$��ns:HaRibbon.Exit��$@ :pack://application:,,,/HaCreator;component/Images/Exit.png��$��1��|�.  .@�.	  (System.Windows.Controls.Ribbon.RibbonTab	  -%    	 Name$ viewTab��$
��View�� 	  KeyTip$ V��|�.  .@�0
  *System.Windows.Controls.Ribbon.RibbonGroup
  -&    
 Name$ MapObj��$��Map Objects��.@�� 
 �� $��70 ��� $��70 ��� $��80 ��� $	��100 ��� $��* � � RowDefinitions �� $��20 ��� $��20 ��� $��20 �.+��� -'    ��Name$ 
tilesCheck��$��Tiles��$��0��$��0��$
��Left=�$��Center=� d� IsThreeState . $��ns:HaRibbon.ViewBoxes��|�.  �� -(   $ 	objsCheck��$��Objects��$��0��$��1��$
��Left=�$��Center=� . $��ns:HaRibbon.ViewBoxes��|�.  �� -)   $ 	npcsCheck��$
��NPCs��$��0��$��2��$
��Left=�$��Center=� . $��ns:HaRibbon.ViewBoxes��|�.  �� -*   $ 	mobsCheck��$
��Mobs��$��1��$��0��$
��Left=�$��Center=� . $��ns:HaRibbon.ViewBoxes��|�.  �� -+   $ 
reactCheck��$��Reactors��$��1��$��1��$
��Left=�$��Center=� . $��ns:HaRibbon.ViewBoxes��|�.  �� -,   $ portalCheck��$��Portals��$��1��$��2��$
��Left=�$��Center=� . $��ns:HaRibbon.ViewBoxes��|�.  �� --   $ fhCheck��$��	Footholds��$��2��$��0��$
��Left=�$��Center=� . $��ns:HaRibbon.ViewBoxes��|�.  �� -.   $ 	ropeCheck��$��Ropes��$��2��$��1��$
��Left=�$��Center=� . $��ns:HaRibbon.ViewBoxes��|�.  �� -/   $ 
chairCheck��$��Chairs��$��2��$��2��$
��Left=�$��Center=� . $��ns:HaRibbon.ViewBoxes��|�.  �� -0   $ tooltipCheck��$��Tooltips��$��3��$��0��$
��Left=�$��Center=� . $��ns:HaRibbon.ViewBoxes��|�.  �� -1   $ bgCheck��$��Backgrounds��$��3��$��1��$
��Left=�$��Center=� . $��ns:HaRibbon.ViewBoxes��|�.  �� -2   $ 	miscCheck��$��Other��$��3��$��2��$
��Left=�$��Center=� . $��ns:HaRibbon.ViewBoxes��|�.  -   'HaCreator.CustomControls.CheckboxButton  -3     Name$ allFullCheck��-   'HaCreator.CustomControls.CheckboxButton   Clicked$ allFullCheck_Click��$��All Objects��$��4��$��0��$
��Left=�$��Center=� .  d� 	IsChecked$
 TrueY�|�.    -4   $ allHalfCheck��$ allHalfCheck_Click��$��All Objects��$��4��$��1��$
��Left=�$��Center=� . |�.   Z�   -5   $ allClearCheck��$ allClearCheck_Click��$��All Objects��$��4��$��2��$
��Left=�$��Center=� . $ FalseY�|�.  
  -6   $ Layers��$��Layers��.@�� 
 �� $
��Auto ��� $
��Auto �.+�%   HaCreator.CustomControls.HaList  -7     Name$ layerBox��%   HaCreator.CustomControls.HaList   SelectionChanged$ layerBox_SelectionChanged��$��0��$	��100��$��65��|�.  � $��1��$��Center=� �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -8   $ layerCheckbox��$��1��$��Show All��$��Center=�$��Center=�$��ns:HaRibbon.AllLayerView��|�.  � $��2��$��Center=�
 �� $
��Auto ��� $
��Auto �.+�  -9     Name$ 
layerUpBtn��$D >pack://application:,,,/HaCreator;component/Images/arrow_up.png��$��ns:HaRibbon.LayerUp��$��0��|�.    -:   $ layerDownBtn��$F @pack://application:,,,/HaCreator;component/Images/arrow_down.png��$��ns:HaRibbon.LayerDown��$��1��|�.  
  -;   $ 	Platforms��$��	Platforms��|�.  .@�� 
 �� $
��Auto ��� $
��Auto ��� $
��Auto �.+�  -<   $ platformBox��$" platformBox_SelectionChanged��$��0��$��50��$��65��� $��1��$��Center=� �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -=   $ platformCheckbox��$��1��$��Show All��$��Center=�$��Center=�$!��ns:HaRibbon.AllPlatformView��|�.  � $��2��$��Center=�
 �� $
��Auto ��� $
��Auto �.+�  ->   $ platformUpBtn��$D >pack://application:,,,/HaCreator;component/Images/arrow_up.png��$��ns:HaRibbon.PlatformUp��$��0��|�.    -?   $ platformDownBtn��$F @pack://application:,,,/HaCreator;component/Images/arrow_down.png��$��ns:HaRibbon.PlatformDown��$��1��|�.    -@   $ newPlatformBtn��$ New Platform��   LargeImageSource$H Bpack://application:,,,/HaCreator;component/Images/Settings_big.png��$��2��$��ns:HaRibbon.NewPlatform��	  -A   $ toolsTab��$��Tools��$ T��.@�
  -B   $ Tools��$��Tools��.@�  $ Settings��$H Bpack://application:,,,/HaCreator;component/Images/Settings_big.png��$��ns:HaRibbon.Settings��|�.    $ Map Simulation��$@ :pack://application:,,,/HaCreator;component/Images/Play.png��$��ns:HaRibbon.MapSim��|�.    $ Manage User Objects��$D >pack://application:,,,/HaCreator;component/Images/UserObjs.png��$��ns:HaRibbon.UserObjs��|�.  
  -C   $ Options��$��Options��.@�7  1System.Windows.Controls.Ribbon.RibbonToggleButton  -D     Name$ 
minimapBtn��   Label$ Show Minimap��   LargeImageSource$C =pack://application:,,,/HaCreator;component/Images/Minimap.png��$��ns:HaRibbon.Minimap��|�.    -E   $ parallaxBtn��$ Parallax BGs��$L Fpack://application:,,,/HaCreator;component/Images/diagram_parallax.png��$��ns:HaRibbon.Parallax��|�.    -F   $ snapBtn��$ Snapping��$E ?pack://application:,,,/HaCreator;component/Images/magnet_32.png��$��ns:HaRibbon.Snapping��|�.    -G   $ 	randomBtn��$ Random Tiles��$E ?pack://application:,,,/HaCreator;component/Images/Untitled1.png��$��ns:HaRibbon.Random��|�.    -H   $ infomodeBtn��$ 	Info Mode��$A ;pack://application:,,,/HaCreator;component/Images/About.png��$��ns:HaRibbon.InfoMode��|�.  
  -I   $ Saving��$��Saving��.@�  $ Regenerate Minimap��$H Bpack://application:,,,/HaCreator;component/Images/RegenMinimap.png��$��ns:HaRibbon.RegenMinimap��|�.    $ Finalize Footholds��$D >pack://application:,,,/HaCreator;component/Images/Foothold.png��$��ns:HaRibbon.Finalize��|�.  	  -J   $ statTab��$��Stats��$ S��.@�
  -K   $ 	mouseVirt��$��Mouse Virtual��.@�6  0System.Windows.Controls.Ribbon.RibbonTwoLineText     Text$ Virtual
��  -L     Name$ 
virtualPos��$ mousepos��
  -M   $ 	mousePhys��$��Mouse Physical��.@�  $ 	Physical
��  -N   $ physicalPos��$ mousepos��
  -O   $ itemSel��$��Selected Item��.@�  -P   $ itemDesc��$ itemdesc��