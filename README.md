# Toolkits

### 1. XAML Converters
1. StringConverters
2. ObjectConverters
3. MediaConverters
4. EnumerableConverters
5. BooleanConverters
6. **CompositeConverter**

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
<Grid> 
    <Button
        Width="100"
        Height="25"
        Animation.FadeFrom="{FadeFrom From=0,
                                      Duration=0:0:1}"
        Animation.ScaleXFrom="{ScaleXFrom From=-200,
                                          Duration=0:0:1}"
        Animation.ScaleYFrom="{ScaleYFrom From=-200,
                                          Duration=0:0:1}"
        Click="Button_Click"
        Content="Close" /> 
</Grid>
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