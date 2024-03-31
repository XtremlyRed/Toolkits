# Toolkits

### 1. XAML Converters

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



``` csharp

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



``` csharp

```