# Toolkits

### 1. XAML Converters
1. StringConverters
2. ObjectConverters
3. MediaConverters
4. EnumerableConverters
5. BooleanConverters
6. ***CompositeConverter***

``` XAML
<Window.Resources>
    <CompositeConverter x:Key="converter">
        <StringLengthConverter />
        <CompareConverter
            Compare="{Int32 18}"
            Matched="{Double 11.11}"
            Mode="GreaterThan"
            Unmatched="{Double 600}" />
    </CompositeConverter>
</Window.Resources>

<StackPanel>
    <TextBox x:Name="text" />
    <TextBlock
        Text="{Binding ElementName=text, Path=Text, Converter={StaticResource converter}}" />
</StackPanel>
```



### 2. Animation

``` XAML
  <Button
      Width="{Double 200}"
      Background="Transparent"
      BorderBrush="{SolidColorBrush Red}"
      Click="Button_Click"
      Content="Open Popup"
      Tag="{SolidColorBrush Color=Aqua}">

      <toolkit:Interaction.Animations>

          <toolkit:BrushPropertyAnimation
              Play="{Binding Play}"
              Property="{x:Static Button.BackgroundProperty}"
              From="Transparent"
              To="Green"
              Duration="0:0:01" />
          <toolkit:ThicknessPropertyAnimation
              Play="{Binding Play}"
              Property="{x:Static Button.MarginProperty}"
              From="0,0,0,0"
              To="100"
              Duration="0:0:01" />

          <toolkit:SlideXAnimation
              Play="{Binding Play}"
              SlideMode="Left"
              To="0"
              Duration="0:0:01" />
          <toolkit:SlideYAnimation
              Play="{Binding Play}"
              SlideMode="Top"
              From="200"
              To="0"
              Duration="0:0:01" />

      </toolkit:Interaction.Animations>

  </Button>
```


### 3. Popup

``` XAML
<AdornerDecorator 
    PopupManager.ContainerName="MainPopupContainer"
    PopupManager.IsMainContainer="True">
    <Button
        Width="100"
        Height="20"
        Click="Button_Click"
        Content="Open Popup" />
</AdornerDecorator>
```



``` csharp
private async void Button_Click(object sender, RoutedEventArgs e)
{
    PopupManager popupManager = new PopupManager();

    for (int i = 0; i < 5; i++)
    {
        var resu = await popupManager.PopupAsync(() => new PopupView());
    }
}
```