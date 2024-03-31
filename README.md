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