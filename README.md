# What is BuilderWizard?
Wizard dialog for creating objects in Builder patterns. Users can create and add different views to create objects in the Builder pattern.

# How to Use?

### 1. Builder
You define a builder object. This inherits the `IJayBuilder` interface.
```
public class Builder : IJayBuilder
{
    public object Build()
    {
        return default;
    }
}
```

### 2. Configuration View & ViewModel
You create configuration views with each view model. The number of views can be multiple at this time. The created view inherits the `IJayBuilderConfigView` and the viewmodel inherits the `IJayBuilderConfigViewModel`.

#### ConfigView behind
```
public partial class ConfigView : UserControl, IJayBuilderConfigView
{
    public IJayBuilderConfigViewModel ViewModel => DataContext as IJayBuilderConfigViewModel;

    public ConfigView()
    {
        InitializeComponent();
    }
}
```

#### ConfigViewModel
```
public class ConfigViewModel : ReactiveObject, IJayBuilderConfigViewModel
{
    public IJayBuilder Builder { get; set; }
}
```

### 3. Use Wizard Dialog
```
var builder = new Builder();

var configView1 = new ConfigView1();
var configView2 = new ConfigView2();
var configViewModel1 = new ConfigViewModel1();
var configViewModel2 = new ConfigViewModel2();

configView1.DataContext = configViewModel1;
configView2.DataContext = configViewModel2;

var wizard = new JayBuilderWizard("Test", builder, configView1, configView2);
if (wizard.ShowDialog() != true)
    return;

var result = wizard.Builder.Build();
```
